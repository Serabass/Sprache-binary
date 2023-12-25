using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SpracheBinary.Tests
{
    public class BytesTests
    {
        [Fact]
        public void TestBytes()
        {
            var parser = from r in Parse.Bytes(3, 2)
                         select r;

            Assert.Throws<ParseException>(() => parser.Parse(new byte[] { 4 }));
            Assert.Equal(3, parser.Parse(new byte[] { 3 }));
            Assert.Equal(2, parser.Parse(new byte[] { 3 }));
        }

        [Fact]
        public void TestByteArray()
        {
            var parser = from r in Parse.ByteArray
                         select r;

            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((byte)2);
            writer.Write((byte)255);
            writer.Write((byte)213);
            writer.Flush();

            var bytes = parser.Parse(memoryStream);

            Assert.Equal(2, bytes.Length);
            Assert.Equal(255, bytes[0]);
            Assert.Equal(213, bytes[1]);
        }
    }
}
