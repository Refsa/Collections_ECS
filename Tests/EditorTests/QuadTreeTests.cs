using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Refsa.Collections.QuadTree;
using Unity.Collections;
using Unity.PerformanceTesting;
using Unity.Mathematics;

namespace Refsa.Collections.Tests
{
    public class QuadTreeTests
    {
        // A Test behaves as an ordinary method
        [Test]
        [Performance]
        public void QuadTreeInsertPerformance()
        {
            int testSize = 100;
            var testMortonCodes = new NativeArray<int>(testSize * testSize, Allocator.Persistent);

            int2 testPoint = int2.zero;
            for (int i = 0; i < testSize; i++)
            {
                for (int j = 0; j < testSize; j++)
                {
                    testPoint.x = i;
                    testPoint.y = j;

                    int mortonCode = MortonCode.Encode2D(testPoint);
                    testMortonCodes[i * testSize + j] = mortonCode;
                }
            }

            NativeQuadTree quadTree = new NativeQuadTree(Allocator.Persistent);
            var quadTreeNode = new QuadTreeNode();

            Measure
            .Method(() => 
            {
                quadTree.Clear();

                for (int i = 0; i < testMortonCodes.Length; i++)
                {
                    quadTreeNode.Index = testMortonCodes[i];
                    quadTree.Insert(testMortonCodes[i], quadTreeNode);
                }
            })
            .WarmupCount(10)
            .MeasurementCount(500)
            .GC()
            .Run();

            Assert.True(true);

            quadTree.Dispose(); 
            testMortonCodes.Dispose();
        }

        [Test]
        [Performance]
        public void QuadTreeQueryPerformance()
        {
            int testSize = 100;
            var testMortonCodes = new NativeArray<int>(testSize * testSize, Allocator.Persistent);

            int2 testPoint = int2.zero;
            for (int i = 0; i < testSize; i++)
            {
                for (int j = 0; j < testSize; j++)
                {
                    testPoint.x = i;
                    testPoint.y = j;

                    int mortonCode = MortonCode.Encode2D(testPoint);
                    testMortonCodes[i * testSize + j] = mortonCode;
                }
            }

            NativeQuadTree quadTree = new NativeQuadTree(Allocator.Persistent);
            var quadTreeNode = new QuadTreeNode();

            for (int i = 0; i < testMortonCodes.Length; i++)
            {   
                quadTreeNode.Index = testMortonCodes[i];
                quadTree.Insert(testMortonCodes[i], quadTreeNode);
            }

            Measure
            .Method(() => 
            {
                for (int i = 0; i < testMortonCodes.Length; i++)
                {
                    quadTree.Query(testMortonCodes[i], out var data);
                }
            })
            .WarmupCount(10)
            .MeasurementCount(500)
            .GC()
            .Run();

            quadTree.Dispose(); 
            testMortonCodes.Dispose();
        }

        [Test]
        public void QuadTreeInsert()
        {
            int testSize = 100;
            var testMortonCodes = new NativeArray<int>(testSize * testSize, Allocator.Persistent);

            int2 testPoint = int2.zero;
            for (int i = 0; i < testSize; i++)
            {
                for (int j = 0; j < testSize; j++)
                {
                    testPoint.x = i;
                    testPoint.y = j;

                    int mortonCode = MortonCode.Encode2D(testPoint);
                    testMortonCodes[i * testSize + j] = mortonCode;
                }
            }

            NativeQuadTree quadTree = new NativeQuadTree(Allocator.Persistent, 100000);
            var quadTreeNode = new QuadTreeNode();

            for (int i = 0; i < testMortonCodes.Length; i++)
            {   
                quadTreeNode.Index = testMortonCodes[i];
                var status = quadTree.Insert(testMortonCodes[i], quadTreeNode);

                Assert.AreEqual(NativeQuadTree.QuadTreeStatus.OK, status);
            }

            quadTree.Dispose();
            testMortonCodes.Dispose();
        }

        [Test]
        public void QuadTreeQuery()
        {
            int testSize = 100;
            var testMortonCodes = new NativeArray<int>(testSize * testSize, Allocator.Persistent);

            int2 testPoint = int2.zero;
            for (int i = 0; i < testSize; i++)
            {
                for (int j = 0; j < testSize; j++)
                {
                    testPoint.x = i;
                    testPoint.y = j;

                    int mortonCode = MortonCode.Encode2D(testPoint);
                    testMortonCodes[i * testSize + j] = mortonCode;
                }
            }

            NativeQuadTree quadTree = new NativeQuadTree(Allocator.Persistent, 100000);
            var quadTreeNode = new QuadTreeNode();

            for (int i = 0; i < testMortonCodes.Length; i++)
            {   
                quadTreeNode.Index = testMortonCodes[i];
                var status = quadTree.Insert(testMortonCodes[i], quadTreeNode);
            }

            for (int i = 0; i < testMortonCodes.Length; i++)
            {
                var status = quadTree.Query(testMortonCodes[i], out var data);
                Assert.AreEqual(NativeQuadTree.QuadTreeStatus.OK, status);
                Assert.AreEqual(testMortonCodes[i], data.Index);
            }

            quadTree.Dispose();
            testMortonCodes.Dispose();
        }
    }
}
