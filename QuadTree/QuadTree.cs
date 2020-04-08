using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Refsa.Collections.QuadTree
{
    [NativeContainer]
    public unsafe struct QuadTree : System.IDisposable
    {
        public enum QuadTreeStatus
        {
            IncreasedCapacity,
            OutOfBounds,
            AlreadyExists,
            OK
        }

        QuadTreeNode* _data;
        Allocator _allocator;
        int _capacity;
        const int initialCapacity = 1000;
        bool _initialized;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle _safetyHandle;
        [NativeSetClassTypeToNullOnSchedule] DisposeSentinel _disposeSentinel;
        static readonly int DisposeSentinelStackDepth = 2;
#endif

        public int Capacity => _capacity;

        public QuadTree(Allocator allocator, int capacity = initialCapacity)
        {
            _allocator = allocator;
            _capacity = capacity;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out _safetyHandle, out _disposeSentinel, DisposeSentinelStackDepth, _allocator);
            AtomicSafetyHandle.SetBumpSecondaryVersionOnScheduleWrite(_safetyHandle, true);
#endif

            _data = (QuadTreeNode*) UnsafeUtility.Malloc(_capacity * UnsafeUtility.SizeOf<QuadTreeNode>(), UnsafeUtility.AlignOf<QuadTreeNode>(), _allocator);

            _initialized = true;
        }

        public void Clear()
        {
            if (_initialized)
            {
                UnsafeUtility.MemClear(_data, _capacity);
            }
        }

        public QuadTreeStatus Query(int2 point, out QuadTreeNode data)
        {
            int pos = MortonCode.Encode2D(point);
            if (pos < _capacity)
            {
                data = _data[pos];
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
                data = _data[mortonCode];
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
            AtomicSafetyHandle.CheckWriteAndThrow(_safetyHandle);
#endif

            int pos = MortonCode.Encode2D(point);
            bool increasedCapacity = EnsureCapacity(pos);

            _data[pos] = data;

            if (increasedCapacity) return QuadTreeStatus.IncreasedCapacity;
            return QuadTreeStatus.OK;
        }

        public QuadTreeStatus Insert(int mortonCode, QuadTreeNode data)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(_safetyHandle);
#endif

            bool increasedCapacity = EnsureCapacity(mortonCode);

            _data[mortonCode] = data;

            if (increasedCapacity) return QuadTreeStatus.IncreasedCapacity;
            return QuadTreeStatus.OK;
        }

        bool EnsureCapacity(int size)
        {
            if (size > _capacity)
            {
                int newSize = size * 2;
                int sizeOf = UnsafeUtility.SizeOf<QuadTreeNode>();
                QuadTreeNode* newPointer = (QuadTreeNode*) UnsafeUtility.Malloc(newSize * sizeOf, UnsafeUtility.AlignOf<QuadTreeNode>(), _allocator);
                UnsafeUtility.MemCpy(newPointer, _data, _capacity * sizeOf);

                UnsafeUtility.Free(_data, _allocator);
                _data = newPointer;

                _capacity = newSize;

                return true;
            }
            return false;
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(_safetyHandle);
            DisposeSentinel.Dispose(ref _safetyHandle, ref _disposeSentinel);
#endif

            if (_initialized)
            {
                UnsafeUtility.Free(_data, _allocator);
                _data = null;
            }
        }
    }
}