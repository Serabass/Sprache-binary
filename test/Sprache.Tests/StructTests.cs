using System.Collections.Generic;
using System.Linq;
using Xunit;
using Sprache;

namespace SpracheBinary.Tests
{
    public class StructTests
    {
        struct RGB { public byte R, G, B; }

        [Fact]
        public void Parser_OfStruct_AcceptsThatStruct()
        {
            var parser = from r in Parse.AnyByte
                         from g in Parse.AnyByte
                         from b in Parse.AnyByte
                         select new RGB { R = r, G = g, B = b };

            var rgb = parser.Parse(new byte[] { 255, 0, 0 });

            Assert.Equal(255, rgb.R);
            Assert.Equal(0, rgb.G);
            Assert.Equal(0, rgb.B);
        }
    }
}
