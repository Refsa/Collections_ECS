using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Refsa.Collections.QuadTree
{
    public unsafe struct QuadTreeNode
    {
        public int Index;
        public int Count;

        public static QuadTreeNode Null =>
            new QuadTreeNode{Index = -1, Count = -1};
        
        public static QuadTreeNode Empty =>
            new QuadTreeNode{Index = 0, Count = 0};
    }
}