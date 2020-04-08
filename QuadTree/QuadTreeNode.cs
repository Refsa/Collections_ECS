using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Refsa.Collections.QuadTree
{
    public struct QuadTreeNode
    {
        public Entity entity;
        public int Index;
    
        public static QuadTreeNode Null =>
            new QuadTreeNode{Index = -1};
    }
}