using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Sprache.Binary.Tests
{
    public class SandboxTests
    {
        struct RGB { public byte R, G, B; }

        private readonly Parser<RGB> RGBParser =
            from r in Parse.AnyByte
            from g in Parse.AnyByte
            from b in Parse.AnyByte
            select new RGB { R = r, G = g, B = b };

        [Fact]
        public void DelimitedByByte()
        {
            var parser = from r in RGBParser.DelimitedBy(Parse.Byte(100))
                         select r;

            var memoryStream = new MemoryStream([
                255, 0, 0,
                100,
                0, 255, 0,
                100,
                0, 0, 255,
            ]);

            var rgbs = parser.Parse(memoryStream);

            Assert.Equal(3, rgbs.Count());
            var rgb0 = rgbs.ElementAt(0);
            Assert.Equal(255, rgb0.R);
            Assert.Equal(0, rgb0.G);
            Assert.Equal(0, rgb0.B);

            var rgb1 = rgbs.ElementAt(1);
            Assert.Equal(0, rgb1.R);
            Assert.Equal(255, rgb1.G);
            Assert.Equal(0, rgb1.B);

            var rgb2 = rgbs.ElementAt(2);
            Assert.Equal(0, rgb2.R);
            Assert.Equal(0, rgb2.G);
            Assert.Equal(255, rgb2.B);
        }
    }
}
