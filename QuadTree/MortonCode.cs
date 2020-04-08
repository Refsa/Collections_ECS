using Unity.Mathematics;

public static class MortonCode
{
    static readonly int[] B = {0x55555555, 0x33333333, 0x0F0F0F0F, 0x00FF00FF};
    static readonly int[] S = {1, 2, 4, 8};

    public static int Encode2D(int2 point)
    {
        unchecked
        {
            point.x = (point.x | (point.x << S[3])) & B[3];
            point.x = (point.x | (point.x << S[2])) & B[2];
            point.x = (point.x | (point.x << S[1])) & B[1];
            point.x = (point.x | (point.x << S[0])) & B[0];
    
            point.y = (point.y | (point.y << S[3])) & B[3];
            point.y = (point.y | (point.y << S[2])) & B[2];
            point.y = (point.y | (point.y << S[1])) & B[1];
            point.y = (point.y | (point.y << S[0])) & B[0];
    
            return point.x | (point.y << 1);
        }
    }
}