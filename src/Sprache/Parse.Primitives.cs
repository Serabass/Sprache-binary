using System;
using System.Linq;
using System.Text;

namespace Sprache.Binary
{
    partial class Parse
    {
        /// <summary>
        /// Parses a Byte from the input.
        /// </summary>
        public static readonly Parser<byte> Int8 = from @byte in AnyByte
                                                   select @byte;

        /// <summary>
        /// Parses a Short (2 bytes) from the input.
        /// </summary>
        public static readonly Parser<short> Int16 = from bytes in AnyByte.Repeat(2)
                                                     select BitConverter.ToInt16(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an UShort (2 bytes) from the input.
        /// </summary>
        public static readonly Parser<ushort> UInt16 = from bytes in AnyByte.Repeat(2)
                                                       select BitConverter.ToUInt16(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an Int (4 bytes) from the input.
        /// </summary>
        public static readonly Parser<int> Int32 = from bytes in AnyByte.Repeat(4)
                                                   select BitConverter.ToInt32(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an Int (4 bytes) from the input.
        /// </summary>
        public static readonly Parser<uint> UInt32 = from bytes in AnyByte.Repeat(4)
                                                     select BitConverter.ToUInt32(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an Long (8 bytes) from the input.
        /// </summary>
        public static readonly Parser<long> Int64 = from bytes in AnyByte.Repeat(8)
                                                    select BitConverter.ToInt64(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an ULong (8 bytes) from the input.
        /// </summary>
        public static readonly Parser<ulong> UInt64 = from bytes in AnyByte.Repeat(8)
                                                      select BitConverter.ToUInt64(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an Float (4 bytes) from the input.
        /// </summary>
        public static readonly Parser<float> Single = from bytes in AnyByte.Repeat(4)
                                                      select BitConverter.ToSingle(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an Double (8 bytes) from the input.
        /// </summary>
        public static readonly Parser<double> Double = from bytes in AnyByte.Repeat(8)
                                                       select BitConverter.ToDouble(bytes.ToArray(), 0);

        /// <summary>
        /// Parses an String from the input.
        /// </summary>
        public static readonly Parser<string> String = from length in Int32
                                                       from bytes in AnyByte.Repeat(length)
                                                       select Encoding.UTF8.GetString(bytes.ToArray());

        /// <summary>
        /// Parses an String from the input.
        /// </summary>
        public static Parser<string> FixedString(int length) => from bytes in AnyByte.Repeat(length)
                                                                select Encoding.UTF8.GetString(bytes.ToArray());

        /// <summary>
        /// Parses an ZString from the input.
        /// </summary>
        public static readonly Parser<string> StringZeroTerminated = from bytes in AnyByte.Until(Byte(0))
                                                                     select Encoding.UTF8.GetString(bytes.ToArray());


        [Obsolete]
        public static readonly Parser<byte[]> ByteArray = from length in AnyByte
                                                          from bytes in AnyByte.Repeat(length)
                                                          select bytes.ToArray();

        public static Parser<string> ASCIIConstString(string value)
        {
            return from bytes in AnyByte.Repeat(value.Length)
                   where Encoding.ASCII.GetString(bytes.ToArray()) == value
                   select value;
        }
        public static Parser<string> UTF8ConstString(string value)
        {
            return from bytes in AnyByte.Repeat(value.Length)
                   where Encoding.UTF8.GetString(bytes.ToArray()) == value
                   select value;
        }
    }
}
