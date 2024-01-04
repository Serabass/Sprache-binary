namespace Sprache.Binary
{
    public static partial class Parse
    {
        public static Parser<byte> NUL = Byte(0x00);
        public static Parser<byte> SOH = Byte(0x01);
        public static Parser<byte> STX = Byte(0x02);
        public static Parser<byte> ETX = Byte(0x03);
        public static Parser<byte> EOT = Byte(0x04);
        public static Parser<byte> ENQ = Byte(0x05);
        public static Parser<byte> ACK = Byte(0x06);
        public static Parser<byte> BEL = Byte(0x07);
        public static Parser<byte> BS = Byte(0x08);
        public static Parser<byte> HT = Byte(0x09);
        public static Parser<byte> LF = Byte(0x0A);
        public static Parser<byte> VT = Byte(0x0B);
        public static Parser<byte> FF = Byte(0x0C);
        public static Parser<byte> CR = Byte(0x0D);
        public static Parser<byte> SO = Byte(0x0E);
        public static Parser<byte> SI = Byte(0x0F);
        public static Parser<byte> DLE = Byte(0x10);
        public static Parser<byte> DC1 = Byte(0x11);

        // Continue with Copilot
    }
}
