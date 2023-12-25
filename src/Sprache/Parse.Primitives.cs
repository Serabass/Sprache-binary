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
                                                     select (short)(b1 | (b2 << 8));

        public static readonly Parser<ushort> UInt16 = from b1 in AnyByte
                                                       from b2 in AnyByte
                                                       select (ushort)(b1 | (b2 << 8));

        public static readonly Parser<int> Int32 = from b1 in AnyByte
                                                   from b2 in AnyByte
                                                   from b3 in AnyByte
                                                   from b4 in AnyByte
                                                   select b1 | (b2 << 8) | (b3 << 16) | b4 << 24;

        public static readonly Parser<uint> UInt32 = from b1 in AnyByte
                                                     from b2 in AnyByte
                                                     from b3 in AnyByte
                                                     from b4 in AnyByte
                                                     select (uint)(b1 | (b2 << 8) | (b3 << 16) | b4 << 24);

        public static readonly Parser<long> Int64 = from b1 in AnyByte
                                                    from b2 in AnyByte
                                                    from b3 in AnyByte
                                                    from b4 in AnyByte
                                                    from b5 in AnyByte
                                                    from b6 in AnyByte
                                                    from b7 in AnyByte
                                                    from b8 in AnyByte
                                                    select (long)((b1 << 56) | (b2 << 48) | (b3 << 40) | (b4 << 32) | (b5 << 24) | (b6 << 16) | (b7 << 8) | b8);

        public static readonly Parser<ulong> UInt64 = from b1 in AnyByte
                                                      from b2 in AnyByte
                                                      from b3 in AnyByte
                                                      from b4 in AnyByte
                                                      from b5 in AnyByte
                                                      from b6 in AnyByte
                                                      from b7 in AnyByte
                                                      from b8 in AnyByte
                                                      select (ulong)((b1 << 56) | (b2 << 48) | (b3 << 40) | (b4 << 32) | (b5 << 24) | (b6 << 16) | (b7 << 8) | b8);

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


        public static readonly Parser<byte[]> ByteArray = from length in AnyByte
                                                          from bytes in AnyByte.Repeat(length)
                                                          select bytes.ToArray();
    }
}
