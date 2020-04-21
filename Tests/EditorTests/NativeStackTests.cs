using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Refsa.Collections.String;
using Unity.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Refsa.Collections.Tests
{
    public class NativeStackTests
    {
        [Test]
        public void NativeStackPushTest()
        {
            int testSize = 10000;

            NativeStack<int> stack = new NativeStack<int>(testSize, Allocator.TempJob);

            for (int i = 0; i < testSize; i++)
            {
                stack.Push(i);
                Assert.AreEqual(i, stack.Peek(), "Wrong value at start of stack");
            }

            stack.Dispose();
        }

        [Test]
        public void NativeStackPopTest()
        {
            int testSize = 10000;

            NativeStack<int> stack = new NativeStack<int>(testSize, Allocator.TempJob);

            for (int i = 0; i < testSize; i++)
            {
                stack.Push(i);
            }

            for (int i = testSize - 1; i >= 0; i--)
            {
                Assert.True(stack.Pop(out int value));
                Assert.AreEqual(i, value, "Wrong value when popping stack");
            }

            stack.Dispose();
        }

        [Test]
        [Performance]
        public void NativeStackPerformanceTest()
        {
            int testSize = 10000;

            Measure
            .Method(() => 
            {
                NativeStack<int> stack = new NativeStack<int>(testSize, Allocator.TempJob);
                
                for (int i = 0; i < testSize; i++)
                {
                    stack.Push(i);
                }

                for (int i = testSize - 1; i >= 0; i--)
                {
                    stack.Pop(out int value);
                }

                stack.Dispose();
            })
            .WarmupCount(10)
            .MeasurementCount(1000)
            .GC()
            .Run();
        }
    }
}
