using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Refsa.Collections.String
{
    /// <summary>
    /// Natively supported char array
    /// 
    /// Currently should be assinged as a NativeContainer for safety reasons.
    /// This means that it cant be used in other NativeContainers(i.e. NativeArray)
    /// and thus it's not as useful as NativeString_32.
    /// 
    /// Missing a property drawer as there is no nice way to serialize the internal data at this time.
    /// </summary>
    [System.Serializable]
    [NativeContainer]
    public unsafe struct NativeCharArray : System.IComparable, System.IEquatable<NativeCharArray>, System.IDisposable
    {
        public int Length;
        char* buffer;
        Allocator _allocator;

        public NativeCharArray(int length, Allocator allocator = Allocator.Persistent)
        {
            _allocator = allocator;
            Length = length;
            buffer = (char*) UnsafeUtility.Malloc(Length * UnsafeUtility.SizeOf<char>(), UnsafeUtility.AlignOf<char>(), _allocator);
        }

        /// <summary>
        /// Burst compatible, but more cumbersome
        /// </summary>
        /// <param name="source">NativeArray of chars to construct char array from</param>
        public NativeCharArray(NativeArray<char> source, Allocator allocator = Allocator.Persistent) : this(source.Length, allocator)
        {
            for (int i = 0; i < source.Length; i++)
            {
                buffer[i] = source[i];
            }
        }

        /// <summary>
        /// Constructs a NativeCharArray using the supplied NativeString_32
        /// </summary>
        /// <param name="source">A NativeString_32 to copy data from</param>
        public NativeCharArray(NativeString_32 source, Allocator allocator = Allocator.Persistent) : this(NativeString_32.MaxLength, allocator)
        {
            buffer[0] = (char) source.buffer.byte0000;
            buffer[1] = (char) source.buffer.byte0001;
            buffer[2] = (char) source.buffer.byte0002;
            buffer[3] = (char) source.buffer.byte0003;
            buffer[4] = (char) source.buffer.byte0004;
            buffer[5] = (char) source.buffer.byte0005;
            buffer[6] = (char) source.buffer.byte0006;
            buffer[7] = (char) source.buffer.byte0007;
            buffer[8] = (char) source.buffer.byte0008;
            buffer[9] = (char) source.buffer.byte0009;
            buffer[10] = (char) source.buffer.byte0010;
            buffer[11] = (char) source.buffer.byte0011;
            buffer[12] = (char) source.buffer.byte0012;
            buffer[13] = (char) source.buffer.byte0013;
            buffer[14] = (char) source.buffer.byte0014.byte0000;
            buffer[15] = (char) source.buffer.byte0014.byte0001;
            buffer[16] = (char) source.buffer.byte0014.byte0002;
            buffer[17] = (char) source.buffer.byte0014.byte0003;
            buffer[18] = (char) source.buffer.byte0014.byte0004;
            buffer[19] = (char) source.buffer.byte0014.byte0005;
            buffer[20] = (char) source.buffer.byte0014.byte0006;
            buffer[21] = (char) source.buffer.byte0014.byte0007;
            buffer[22] = (char) source.buffer.byte0014.byte0008;
            buffer[23] = (char) source.buffer.byte0014.byte0009;
            buffer[24] = (char) source.buffer.byte0014.byte0010;
            buffer[25] = (char) source.buffer.byte0014.byte0011;
            buffer[26] = (char) source.buffer.byte0014.byte0012;
            buffer[27] = (char) source.buffer.byte0014.byte0013;
            buffer[28] = (char) source.buffer.byte0014.byte0014;
            buffer[29] = (char) source.buffer.byte0014.byte0015;
        }

        /// <summary>
        /// Takes a managed string reference and creates a native char array from it
        /// </summary>
        /// <param name="source">managed string reference to construct char array from</param>
        public NativeCharArray(string source, Allocator allocator = Allocator.Persistent) : this(source.Length, allocator)
        {
            for (int i = 0; i < Length; i++)
            {
                buffer[i] = source[i];
            }
        }

        /// <summary>
        /// Gets char at index, performs boundary checks
        /// </summary>
        /// <param name="index">index to get character from</param>
        /// <returns>char at index position or null character if out of bounds</returns>
        public char GetChar(int index)
        {
            if (index < 0 || index >= Length) return (char) 0b0000;

            return buffer[index];
        }

        /// <summary>
        /// UNSAFE
        /// Fetches char at index without boundary checks
        /// make sure you know the index is inside the bounds of 0 -> Length
        /// </summary>
        /// <param name="index">index to get char from</param>
        /// <returns>char at index, undefined char outside of bounds</returns>
        internal char GetCharFast(int index)
        {
            return buffer[index];
        }

        /// <summary>
        /// Frees the memory used by the char array
        /// </summary>
        public void Dispose()
        {
            UnsafeUtility.Free(buffer, _allocator);
        }   

        /// <summary>
        /// Constructs a string from the internal char array
        /// </summary>
        /// <returns>string of the internal char array</returns>
        public override string ToString()
        {
            var charArray = new char[Length];

            for (int i = 0; i < Length; i++)
            {
                charArray[i] = buffer[i];
            }

            return new string(charArray);
        }

        public int CompareTo(object obj)
        {
            return 0;
        }

        public bool Equals(NativeCharArray other)
        {
            if (other.Length != Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (GetCharFast(i) != other.GetCharFast(i)) return false;
            }

            return true;
        }

        public static implicit operator NativeCharArray(string b)
        {
            return new NativeCharArray(b);
        }

        public static implicit operator NativeCharArray(NativeString_32 b)
        {
            return new NativeCharArray(b);
        }
    }

}