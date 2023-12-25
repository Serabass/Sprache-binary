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
            Assert.Equal(2, parser.Parse(new byte[] { 2 }));
        }

        [Fact]
        public void TestSeek()
        {
            var parser = from seek in Parse.Seek(2)
                         from r in Parse.AnyByte
                         select r;

            Assert.Equal(2, parser.Parse(new byte[] { 0, 1, 2, 3, 4 }));
        }
    }
}
