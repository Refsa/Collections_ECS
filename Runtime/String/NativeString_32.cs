using System.Collections.Generic;

namespace Refsa.Collections.String
{
    /// <summary>
    /// Alternative to Unity.Collections.NativeString32
    /// This version allows serialization through a property drawer and 
    /// has a property drawer to allow editing the content at design time.
    /// 
    /// Far from optimized and still makes use of a managed string to construct the internal bytes.
    /// Uses System.Text.Encoding.UTF8 to create the byte data.
    /// 
    /// Data is stored in a Bytes30 struct to avoid having to manage the memory. This should make it safe
    /// for use in containers marked with NativeCollection.
    /// </summary>
    [System.Serializable]
    public struct NativeString_32 : System.IComparable, System.IEquatable<NativeString_32>
    {
        public const int MaxLength = 30;
        public ushort LengthInBytes;
        public Bytes30 buffer;

        /// <summary>
        /// Creates a new NativeString_32 using a string as the source
        /// </summary>
        /// <param name="source">string to create the NativeString_32 from</param>
        public NativeString_32(string source)
        {
            var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(source);

            LengthInBytes = (ushort) utf8Bytes.Length;
            buffer = new Bytes30();

            SetString(source);
        }

        /// <summary>
        /// Constructs the string, strips any null(0b0000) characters.
        /// </summary>
        /// <returns>string representing the internal byte data</returns>
        public override string ToString()
        {
            List<byte> utf8Bytes = new List<byte>();
            try
            {
                utf8Bytes.Add(buffer.byte0000);
                utf8Bytes.Add(buffer.byte0001);
                utf8Bytes.Add(buffer.byte0002);
                utf8Bytes.Add(buffer.byte0003);
                utf8Bytes.Add(buffer.byte0004);
                utf8Bytes.Add(buffer.byte0005);
                utf8Bytes.Add(buffer.byte0006);
                utf8Bytes.Add(buffer.byte0007);
                utf8Bytes.Add(buffer.byte0008);
                utf8Bytes.Add(buffer.byte0009);
                utf8Bytes.Add(buffer.byte0010);
                utf8Bytes.Add(buffer.byte0011);
                utf8Bytes.Add(buffer.byte0012);
                utf8Bytes.Add(buffer.byte0013);
                utf8Bytes.Add(buffer.byte0014.byte0000);
                utf8Bytes.Add(buffer.byte0014.byte0001);
                utf8Bytes.Add(buffer.byte0014.byte0002);
                utf8Bytes.Add(buffer.byte0014.byte0003);
                utf8Bytes.Add(buffer.byte0014.byte0004);
                utf8Bytes.Add(buffer.byte0014.byte0005);
                utf8Bytes.Add(buffer.byte0014.byte0006);
                utf8Bytes.Add(buffer.byte0014.byte0007);
                utf8Bytes.Add(buffer.byte0014.byte0008);
                utf8Bytes.Add(buffer.byte0014.byte0009);
                utf8Bytes.Add(buffer.byte0014.byte0010);
                utf8Bytes.Add(buffer.byte0014.byte0011);
                utf8Bytes.Add(buffer.byte0014.byte0012);
                utf8Bytes.Add(buffer.byte0014.byte0013);
                utf8Bytes.Add(buffer.byte0014.byte0014);
                utf8Bytes.Add(buffer.byte0014.byte0015);
            }
            catch{}

            utf8Bytes.RemoveAll(b => b == 0b0000);

            return System.Text.Encoding.UTF8.GetString(utf8Bytes.ToArray());
        }

        /// <summary>
        /// Sets the internal data from the given string
        /// </summary>
        /// <param name="source">string to copy data from</param>
        public void SetString(string source)
        {
            Clear();

            var charArray = source.ToCharArray();
            var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(charArray, 0, charArray.Length);
            int currentPos = 0;

            try
            {
                buffer.byte0000 = utf8Bytes[currentPos++];
                buffer.byte0001 = utf8Bytes[currentPos++];
                buffer.byte0002 = utf8Bytes[currentPos++];
                buffer.byte0003 = utf8Bytes[currentPos++];
                buffer.byte0004 = utf8Bytes[currentPos++];
                buffer.byte0005 = utf8Bytes[currentPos++];
                buffer.byte0006 = utf8Bytes[currentPos++];
                buffer.byte0007 = utf8Bytes[currentPos++];
                buffer.byte0008 = utf8Bytes[currentPos++];
                buffer.byte0009 = utf8Bytes[currentPos++];
                buffer.byte0010 = utf8Bytes[currentPos++];
                buffer.byte0011 = utf8Bytes[currentPos++];
                buffer.byte0012 = utf8Bytes[currentPos++];
                buffer.byte0013 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0000 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0001 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0002 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0003 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0004 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0005 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0006 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0007 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0008 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0009 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0010 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0011 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0012 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0013 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0014 = utf8Bytes[currentPos++];
                buffer.byte0014.byte0015 = utf8Bytes[currentPos];
            }
            catch
            {

            }
        }

        void Clear()
        {
            try
            {
                buffer.byte0000 = 0b0000;
                buffer.byte0001 = 0b0000;
                buffer.byte0002 = 0b0000;
                buffer.byte0003 = 0b0000;
                buffer.byte0004 = 0b0000;
                buffer.byte0005 = 0b0000;
                buffer.byte0006 = 0b0000;
                buffer.byte0007 = 0b0000;
                buffer.byte0008 = 0b0000;
                buffer.byte0009 = 0b0000;
                buffer.byte0010 = 0b0000;
                buffer.byte0011 = 0b0000;
                buffer.byte0012 = 0b0000;
                buffer.byte0013 = 0b0000;
                buffer.byte0014.byte0000 = 0b0000;
                buffer.byte0014.byte0001 = 0b0000;
                buffer.byte0014.byte0002 = 0b0000;
                buffer.byte0014.byte0003 = 0b0000;
                buffer.byte0014.byte0004 = 0b0000;
                buffer.byte0014.byte0005 = 0b0000;
                buffer.byte0014.byte0006 = 0b0000;
                buffer.byte0014.byte0007 = 0b0000;
                buffer.byte0014.byte0008 = 0b0000;
                buffer.byte0014.byte0009 = 0b0000;
                buffer.byte0014.byte0010 = 0b0000;
                buffer.byte0014.byte0011 = 0b0000;
                buffer.byte0014.byte0012 = 0b0000;
                buffer.byte0014.byte0013 = 0b0000;
                buffer.byte0014.byte0014 = 0b0000;
                buffer.byte0014.byte0015 = 0b0000;
            }
            catch
            {

            }
        }

        public int CompareTo(object obj)
        {
            return 0;
        }

        public bool Equals(NativeString_32 other)
        {
            if (other.LengthInBytes != LengthInBytes) return false;
            return buffer.Equals(other.buffer);
        }

        public override int GetHashCode()
        {
            int hashCode = -1717183400;
            hashCode = hashCode * -1521134295 + LengthInBytes.GetHashCode();
            hashCode = hashCode * -1521134295 + buffer.GetHashCode();
            return hashCode;
        }

        public static implicit operator NativeString_32(string b)
        {
            return new NativeString_32(b);
        }
    }

    [System.Serializable]
    public struct Bytes16
    {
        public byte byte0000;
        public byte byte0001;
        public byte byte0002;
        public byte byte0003;
        public byte byte0004;
        public byte byte0005;
        public byte byte0006;
        public byte byte0007;
        public byte byte0008;
        public byte byte0009;
        public byte byte0010;
        public byte byte0011;
        public byte byte0012;
        public byte byte0013;
        public byte byte0014;
        public byte byte0015;

        public override bool Equals(object obj)
        {
            return obj is Bytes16 bytes &&
                   byte0000 == bytes.byte0000 &&
                   byte0001 == bytes.byte0001 &&
                   byte0002 == bytes.byte0002 &&
                   byte0003 == bytes.byte0003 &&
                   byte0004 == bytes.byte0004 &&
                   byte0005 == bytes.byte0005 &&
                   byte0006 == bytes.byte0006 &&
                   byte0007 == bytes.byte0007 &&
                   byte0008 == bytes.byte0008 &&
                   byte0009 == bytes.byte0009 &&
                   byte0010 == bytes.byte0010 &&
                   byte0011 == bytes.byte0011 &&
                   byte0012 == bytes.byte0012 &&
                   byte0013 == bytes.byte0013 &&
                   byte0014 == bytes.byte0014 &&
                   byte0015 == bytes.byte0015;
        }

        public override int GetHashCode()
        {
            int hashCode = 1437637520;
            hashCode = hashCode * -1521134295 + byte0000.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0001.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0002.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0003.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0004.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0005.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0006.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0007.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0008.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0009.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0010.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0011.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0012.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0013.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0014.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0015.GetHashCode();
            return hashCode;
        }
    }

    [System.Serializable]
    public struct Bytes30
    {
        public byte byte0000;
        public byte byte0001;
        public byte byte0002;
        public byte byte0003;
        public byte byte0004;
        public byte byte0005;
        public byte byte0006;
        public byte byte0007;
        public byte byte0008;
        public byte byte0009;
        public byte byte0010;
        public byte byte0011;
        public byte byte0012;
        public byte byte0013;
        public Bytes16 byte0014;

        public override bool Equals(object obj)
        {
            return obj is Bytes30 bytes &&
                   byte0000 == bytes.byte0000 &&
                   byte0001 == bytes.byte0001 &&
                   byte0002 == bytes.byte0002 &&
                   byte0003 == bytes.byte0003 &&
                   byte0004 == bytes.byte0004 &&
                   byte0005 == bytes.byte0005 &&
                   byte0006 == bytes.byte0006 &&
                   byte0007 == bytes.byte0007 &&
                   byte0008 == bytes.byte0008 &&
                   byte0009 == bytes.byte0009 &&
                   byte0010 == bytes.byte0010 &&
                   byte0011 == bytes.byte0011 &&
                   byte0012 == bytes.byte0012 &&
                   byte0013 == bytes.byte0013 &&
                   EqualityComparer<Bytes16>.Default.Equals(byte0014, bytes.byte0014);
        }

        public override int GetHashCode()
        {
            int hashCode = -1009847981;
            hashCode = hashCode * -1521134295 + byte0000.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0001.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0002.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0003.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0004.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0005.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0006.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0007.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0008.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0009.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0010.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0011.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0012.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0013.GetHashCode();
            hashCode = hashCode * -1521134295 + byte0014.GetHashCode();
            return hashCode;
        }
    }
}