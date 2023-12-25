using System;
using System.Linq;

namespace SpracheBinary
{
    partial class Parse
    {
        /// <summary>
        /// Parse any character.
        /// </summary>
        public static readonly Parser<short> Int16 = from b1 in AnyByte
                                                     from b2 in AnyByte
                                                     select BitConverter.ToInt16(new[] { b1, b2 }, 0);

        public static readonly Parser<ushort> UInt16 = from b1 in AnyByte
                                                       from b2 in AnyByte
                                                       select BitConverter.ToUInt16(new[] { b1, b2 }, 0);

        public static readonly Parser<int> Int32 = from b1 in AnyByte
                                                   from b2 in AnyByte
                                                   from b3 in AnyByte
                                                   from b4 in AnyByte
                                                   select BitConverter.ToInt32(new[] { b1, b2, b3, b4 }, 0);

        public static readonly Parser<uint> UInt32 = from b1 in AnyByte
                                                     from b2 in AnyByte
                                                     from b3 in AnyByte
                                                     from b4 in AnyByte
                                                     select BitConverter.ToUInt32(new[] { b1, b2, b3, b4 }, 0);

        public static readonly Parser<long> Int64 = from b1 in AnyByte
                                                    from b2 in AnyByte
                                                    from b3 in AnyByte
                                                    from b4 in AnyByte
                                                    from b5 in AnyByte
                                                    from b6 in AnyByte
                                                    from b7 in AnyByte
                                                    from b8 in AnyByte
                                                    select BitConverter.ToInt64(new[] { b1, b2, b3, b4, b5, b6, b7, b8 }, 0);

        public static readonly Parser<ulong> UInt64 = from b1 in AnyByte
                                                      from b2 in AnyByte
                                                      from b3 in AnyByte
                                                      from b4 in AnyByte
                                                      from b5 in AnyByte
                                                      from b6 in AnyByte
                                                      from b7 in AnyByte
                                                      from b8 in AnyByte
                                                      select BitConverter.ToUInt64(new[] { b1, b2, b3, b4, b5, b6, b7, b8 }, 0);

        public static readonly Parser<float> Single = from b1 in AnyByte
                                                      from b2 in AnyByte
                                                      from b3 in AnyByte
                                                      from b4 in AnyByte
                                                      select BitConverter.ToSingle(new[] { b1, b2, b3, b4 }, 0);

        public static readonly Parser<double> Double = from b1 in AnyByte
                                                       from b2 in AnyByte
                                                       from b3 in AnyByte
                                                       from b4 in AnyByte
                                                       from b5 in AnyByte
                                                       from b6 in AnyByte
                                                       from b7 in AnyByte
                                                       from b8 in AnyByte
                                                       select BitConverter.ToDouble(new[] { b1, b2, b3, b4, b5, b6, b7, b8 }, 0);

        public static readonly Parser<string> String = from length in Int32
                                                       from bytes in AnyByte.Repeat(length)
                                                       select System.Text.Encoding.UTF8.GetString(bytes.ToArray());

        public static readonly Parser<string> StringZeroTerminated = from bytes in AnyByte.Until(Byte(0))
                                                                     select System.Text.Encoding.UTF8.GetString(bytes.ToArray());


//        [Obsolete]
//        public static readonly Parser<byte[]> ByteArray = from length in AnyByte
//                                                          from bytes in AnyByte.Repeat(length)
//                                                          select bytes.ToArray();
    }
}
