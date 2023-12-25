using System.Collections.Generic;
using System.Linq;
using Xunit;
using Sprache;

namespace Sprache.Tests
{
    public class ParseTests
    {
        struct Color { public byte R, G, B; }

        [Fact]
        public void RGB()
        {
            var red = new byte[] { 255, 0, 0 };
            var parser = from r in Parse.ByteExcept(0)
                         from g in Parse.ByteExcept(255)
                         from b in Parse.ByteExcept(255)
                         select new Color { R = r, G = g, B = b };

            Assert.Equal(255, parser.Parse(red).R);
            Assert.Equal(0, parser.Parse(red).G);
            Assert.Equal(0, parser.Parse(red).B);
        }
    }
}
