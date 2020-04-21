using System;
using System.Diagnostics;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Refsa.Collections
{
    [NativeContainer]
    public unsafe struct NativeStack<T> where T : struct
    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal AtomicSafetyHandle m_Safety;

        [NativeSetClassTypeToNullOnSchedule]
        DisposeSentinel m_DisposeSentinel;

        const int disposeSentinelStackDepth = 2;
#endif

        UnsafeList* data;

        public NativeStack(int capacity, Allocator allocator)
        {
            var totalSize = UnsafeUtility.SizeOf<T>() * (long)capacity;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            // Native allocation is only valid for Temp, Job and Persistent.
            if (allocator <= Allocator.None)
                throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", nameof(allocator));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be >= 0");

            CollectionHelper.CheckIsUnmanaged<T>();

            if (totalSize > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(capacity), $"Capacity * sizeof(T) cannot exceed {int.MaxValue} bytes");

            DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, disposeSentinelStackDepth, allocator);
#endif

            data = UnsafeList.Create(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), capacity, allocator);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.SetBumpSecondaryVersionOnScheduleWrite(m_Safety, true);
#endif
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif

            data->Dispose();
            data = null;
        }

        public T Peek()
        {
            return UnsafeUtility.ReadArrayElement<T>(data->Ptr, data->Length - 1);
        }

        public bool Pop(out T value)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
            int newLength = data->Length - 1;
            if (newLength >= 0)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
                data->Length = newLength;
                value = UnsafeUtility.ReadArrayElement<T>(data->Ptr, newLength);
                UnsafeUtility.WriteArrayElement<T>(data->Ptr, newLength, default(T));
                return true;
            }
                
            value = default(T);
            return false;
        }

        public void Push(T value)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_Safety);
#endif

            data->Add(value);
        }

        public ParallelReader AsParallelReader()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ParallelReader(data->Ptr, data, ref m_Safety);
#else
            return new ParallelReader(data->Ptr, data);
#endif
        }

        public ParallelWriter AsParallelWriter()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ParallelWriter(data->Ptr, data, ref m_Safety);
#else
            return new ParallelWriter(data->Ptr, data);
#endif
        }

        public unsafe struct ParallelReader
        {
            public void* Ptr;
            public UnsafeList* ListData;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            internal AtomicSafetyHandle m_Safety;

            public unsafe ParallelReader(void* ptr, UnsafeList* listData, ref AtomicSafetyHandle safety)
            {
                Ptr = ptr;
                ListData = listData;
                m_Safety = safety;
            }
#else
            public unsafe ParallelReader(void* ptr, UnsafeList* listData)
            {
                Ptr = ptr;
                ListData = listData;
            }
#endif
            public bool Pop(out T value)
            {
                var idx = Interlocked.Decrement(ref ListData->Length) - 1;
                if (idx < 0)
                {
                    value = default(T);
                    return false;
                }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                value = UnsafeUtility.ReadArrayElement<T>(Ptr, idx);
                return true;
            }
        }

        public unsafe struct ParallelWriter
        {
            public void* Ptr;
            public UnsafeList* ListData;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            internal AtomicSafetyHandle m_Safety;

            public unsafe ParallelWriter(void* ptr, UnsafeList* listData, ref AtomicSafetyHandle safety)
            {
                Ptr = ptr;
                ListData = listData;
                m_Safety = safety;
            }
#else
            public unsafe ParallelWriter(void* ptr, UnsafeList* listData)
            {
                Ptr = ptr;
                ListData = listData;
            }
#endif

            [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
            private static void CheckSufficientCapacity(int capacity, int length)
            {
                if (capacity < length)
                {
                    throw new Exception($"Length {length} exceeds capacity Capacity {capacity}");
                }
            }

            public void PushNoResize(T value)
            {
                var idx = Interlocked.Increment(ref ListData->Length) - 1;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
                CheckSufficientCapacity(ListData->Capacity, idx + 1);
#endif

                UnsafeUtility.WriteArrayElement(Ptr, idx, value);
            }
        }
    }
}