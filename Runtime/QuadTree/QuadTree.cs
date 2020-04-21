using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;

namespace Refsa.Collections.QuadTree
{
    [NativeContainer]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NativeQuadTree : System.IDisposable
    {
        public enum QuadTreeStatus
        {
            IncreasedCapacity,
            OutOfBounds,
            AlreadyExists,
            OK
        }

        QuadTreeNode* _keys;
        Entity* _values;

        Allocator _allocator;
        int _capacity;
        const int InitialCapacity = 1000;
        const int MaxNodesPerLeaf = 100;
        bool _initialized;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal AtomicSafetyHandle m_Safety;
        [NativeSetClassTypeToNullOnSchedule] DisposeSentinel _disposeSentinel;
        static readonly int DisposeSentinelStackDepth = 2;
#endif

        public int Capacity => _capacity;
        public bool IsCreated => _initialized;


        public NativeQuadTree(Allocator allocator, int capacity = InitialCapacity)
        {
            _allocator = allocator;
            _capacity = capacity;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out m_Safety, out _disposeSentinel, DisposeSentinelStackDepth, _allocator);
            AtomicSafetyHandle.SetBumpSecondaryVersionOnScheduleWrite(m_Safety, true);
#endif

            _keys = (QuadTreeNode*) UnsafeUtility.Malloc(
                    _capacity * UnsafeUtility.SizeOf<QuadTreeNode>(), 
                    UnsafeUtility.AlignOf<QuadTreeNode>(), 
                    _allocator
                );

            _values = (Entity*) UnsafeUtility.Malloc(
                    _capacity * MaxNodesPerLeaf * UnsafeUtility.SizeOf<Entity>(),
                    UnsafeUtility.AlignOf<Entity>(),
                    _allocator
                );

            _initialized = true;
        }

        public void Clear()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!_initialized) return;
#endif

            for (int i = 0; i < _capacity; i++)
            {
                _keys[i].Count = -1;
            }

            // UnsafeUtility.MemClear(_keys, _capacity);
            // UnsafeUtility.MemClear(_values, _capacity * MaxNodesPerLeaf);
        }

        public QuadTreeStatus Query(int2 point, out QuadTreeNode data)
        {
            int pos = MortonCode.Encode2D(point);
            if (pos < _capacity)
            {
                data = _keys[pos];
                return QuadTreeStatus.OK;
            }
            else
            {
                data = QuadTreeNode.Null;
                return QuadTreeStatus.OutOfBounds;
            }
        }

        public QuadTreeStatus Query(int mortonCode, out QuadTreeNode data)
        {
            if (mortonCode < _capacity)
            {
                data = _keys[mortonCode];
                return QuadTreeStatus.OK;
            }
            else
            {
                data = QuadTreeNode.Null;
                return QuadTreeStatus.OutOfBounds;
            }
        }

        public QuadTreeStatus Insert(int2 point, QuadTreeNode data)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif

            int pos = MortonCode.Encode2D(point);
            bool increasedCapacity = EnsureCapacity(pos);

            _keys[pos] = data;

            if (increasedCapacity) return QuadTreeStatus.IncreasedCapacity;
            return QuadTreeStatus.OK;
        }

        public QuadTreeStatus Insert(int mortonCode, QuadTreeNode data)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif

            bool increasedCapacity = EnsureCapacity(mortonCode);

            _keys[mortonCode] = data;

            if (increasedCapacity) return QuadTreeStatus.IncreasedCapacity;
            return QuadTreeStatus.OK;
        }

        public QuadTreeStatus InsertValue(int mortonCode, Entity value)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif

            bool increasedCapacity = EnsureCapacity(mortonCode);
            QuadTreeNode node = _keys[mortonCode];

            if (node.Equals(QuadTreeNode.Null))
            {
                node = 
                    new QuadTreeNode{
                        Index = mortonCode,
                        Count = 0
                    };
                Insert(mortonCode, node);
            }
            else
            {
                node.Count++;
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckValueBounds(node.Index, node.Count, MaxNodesPerLeaf);
#endif

            _values[node.Index + node.Count] = value;

            if (increasedCapacity) return QuadTreeStatus.IncreasedCapacity;
            return QuadTreeStatus.OK;
        }

        bool EnsureCapacity(int size)
        {
            if (size > _capacity)
            {
                int newSize = size * 2;

                IncreaseKeyCapacity(newSize);
                IncreaseValueCapacity(newSize);

                _capacity = newSize;

                return true;
            }
            return false;
        }

        private void IncreaseValueCapacity(int newSize)
        {
            int sizeOf = UnsafeUtility.SizeOf<Entity>();
            Entity* newPointer = (Entity*) UnsafeUtility.Malloc(newSize * MaxNodesPerLeaf * sizeOf, UnsafeUtility.AlignOf<Entity>(), _allocator);
            UnsafeUtility.MemCpy(newPointer, _values, _capacity * sizeOf);

            UnsafeUtility.Free(_values, _allocator);
            _values = newPointer;
        }

        void IncreaseKeyCapacity(int newSize)
        {
            int sizeOf = UnsafeUtility.SizeOf<QuadTreeNode>();
            QuadTreeNode* newPointer = (QuadTreeNode*) UnsafeUtility.Malloc(newSize * sizeOf, UnsafeUtility.AlignOf<QuadTreeNode>(), _allocator);
            UnsafeUtility.MemCpy(newPointer, _keys, _capacity * sizeOf);

            UnsafeUtility.Free(_keys, _allocator);
            _keys = newPointer;
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
            DisposeSentinel.Dispose(ref m_Safety, ref _disposeSentinel);
#endif

            if (_initialized)
            {
                UnsafeUtility.Free(_keys, _allocator);
                UnsafeUtility.Free(_values, _allocator);

                _keys = null;
                _values = null;
            }
        }

        public ParallelReader AsParallelReader()
        {
            return new ParallelReader(_keys, _values, _capacity);
        }

        public ParallelWriter AsParallelWriter()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ParallelWriter(_keys, _values, _capacity, ref m_Safety);
#else
            return new ParallelWriter(_data, _values, _capacity);
#endif
        }

        public unsafe struct ParallelReader
        {
            [NativeDisableUnsafePtrRestriction]
            public readonly QuadTreeNode* KeyPtr;
            [NativeDisableUnsafePtrRestriction]
            public readonly Entity* ValuePtr;
            public int Capacity;

            public ParallelReader(QuadTreeNode* keyptr, Entity* valueptr, int capacity)
            {
                KeyPtr = keyptr;
                ValuePtr = valueptr;
                Capacity = capacity;
            }

            public QuadTreeNode this[int index]
            {
                get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    CheckIndexBounds(index, Capacity);
#endif
                    // return KeyPtr[index];
                    return UnsafeUtility.ReadArrayElement<QuadTreeNode>(KeyPtr, index);
                }
            }
        }

        [NativeContainer]
        [NativeContainerIsAtomicWriteOnly]
        public unsafe struct ParallelWriter
        {
            [NativeDisableUnsafePtrRestriction]
            public readonly QuadTreeNode* KeyPtr;
            [NativeDisableUnsafePtrRestriction]
            public readonly Entity* ValuePtr;
            public int Capacity;

            QuadTreeNode Empty;


#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle m_Safety;

            public ParallelWriter(QuadTreeNode* keyptr, Entity* valueptr, int capacity, ref AtomicSafetyHandle handle)
            {
                KeyPtr = keyptr;
                ValuePtr = valueptr;
                Capacity = capacity;
                m_Safety = handle;

                Empty = QuadTreeNode.Empty;
            }
#else      
            public ParallelWriter(QuadTreeNode* keyptr, Entity* valueptr, int capacity)
            {
                KeyPtr = keyptr;
                ValuePtr = valueptr
                Capacity = capacity;

                Empty = QuadTreeNode.Empty;
            }
#endif

            [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
            private static void CheckSufficientCapacity(int capacity, int length)
            {
                if (capacity < length)
                {
                    throw new System.Exception($"Length {length} exceeds capacity of {capacity}");
                }
            }

            public void AddNoResize(QuadTreeNode node)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
                CheckIndexBounds(node.Index, Capacity);
#endif

                UnsafeUtility.WriteArrayElement(KeyPtr, node.Index, node);
            }

            public void InsertNoResize(int cell, Entity entity)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
                CheckIndexBounds(cell, Capacity);
#endif

                QuadTreeNode node = UnsafeUtility.ReadArrayElement<QuadTreeNode>(KeyPtr, cell);

                if (node.Equals(QuadTreeNode.Null))
                {
                    Empty.Index = cell;
                    
                    UnsafeUtility.WriteArrayElement(KeyPtr, cell, Empty);
                }
                else
                {
                    node.Count++;
                }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                CheckValueBounds(node.Index, node.Count, MaxNodesPerLeaf);
#endif

                UnsafeUtility.WriteArrayElement(ValuePtr, node.Index + node.Count, entity);
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void CheckIndexBounds(int index, int capacity)
        {
            if (index < 0)
                throw new System.Exception($"Index {index} is negative");
            if (index > capacity)
                throw new System.Exception($"Index {index} exceeds capacity {capacity}");
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void CheckValueBounds(int index, int count, int capacity)
        {
            if (index + count > capacity)
                throw new System.Exception($"Node of QuadTree has reached its capacity {capacity}");
        }

    }
}