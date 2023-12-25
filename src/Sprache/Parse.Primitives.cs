using System;
using System.Linq;
using System.Text;

namespace SpracheBinary
{
    partial class Parse
    {
        /// <summary>
        /// Parse any character.
        /// </summary>
        public static readonly Parser<short> Int16 = from bytes in AnyByte.Repeat(2)
                                                     select BitConverter.ToInt16(bytes.ToArray(), 0);

        public static readonly Parser<ushort> UInt16 = from bytes in AnyByte.Repeat(2)
                                                       select BitConverter.ToUInt16(bytes.ToArray(), 0);

        public static readonly Parser<int> Int32 = from bytes in AnyByte.Repeat(4)
                                                   select BitConverter.ToInt32(bytes.ToArray(), 0);

        public static readonly Parser<uint> UInt32 = from bytes in AnyByte.Repeat(4)
                                                     select BitConverter.ToUInt32(bytes.ToArray(), 0);

        public static readonly Parser<long> Int64 = from bytes in AnyByte.Repeat(8)
                                                    select BitConverter.ToInt64(bytes.ToArray(), 0);

        public static readonly Parser<ulong> UInt64 = from bytes in AnyByte.Repeat(8)
                                                      select BitConverter.ToUInt64(bytes.ToArray(), 0);

        public static readonly Parser<float> Single = from bytes in AnyByte.Repeat(4)
                                                      select BitConverter.ToSingle(bytes.ToArray(), 0);

        public static readonly Parser<double> Double = from bytes in AnyByte.Repeat(8)
                                                       select BitConverter.ToDouble(bytes.ToArray(), 0);

        public static readonly Parser<string> String = from length in Int32
                                                       from bytes in AnyByte.Repeat(length)
                                                       select Encoding.UTF8.GetString(bytes.ToArray());

        public static readonly Parser<string> StringZeroTerminated = from bytes in AnyByte.Until(Byte(0))
                                                                     select Encoding.UTF8.GetString(bytes.ToArray());


        //        [Obsolete]
        //        public static readonly Parser<byte[]> ByteArray = from length in AnyByte
        //                                                          from bytes in AnyByte.Repeat(length)
        //                                                          select bytes.ToArray();
    }
}
