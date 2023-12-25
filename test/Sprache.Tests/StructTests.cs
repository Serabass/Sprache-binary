using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SpracheBinary.Tests
{
    public class StructTests
    {
        struct RGB { public byte R, G, B; }

        [Fact]
        public void TestSimpleRGBStruct()
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

        [Fact]
        public void TestSimpleBytes()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((byte)255);
            writer.Write((byte)213);
            writer.Flush();

            var parser = from a in Parse.AnyByte
                         from b in Parse.AnyByte
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var bytes = parser.Parse(mem);

            Assert.Equal(255, bytes.A);
            Assert.Equal(213, bytes.B);
        }

        [Fact]
        public void TestSimpleSingle()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(1.0f);
            writer.Flush();

            var parser = Parse.Single;
            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1.0f, result);
        }

        [Fact]
        public void TestSimpleInt16()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((short)1234);
            writer.Write((short)4321);
            writer.Flush();

            var parser = from a in Parse.Int16
                         from b in Parse.Int16
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1234, result.A);
            Assert.Equal(4321, result.B);
        }

        [Fact]
        public void TestSimpleUInt16()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((ushort)1234);
            writer.Write((ushort)4321);
            writer.Flush();

            var parser = from a in Parse.UInt16
                         from b in Parse.UInt16
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1234u, result.A);
            Assert.Equal(4321u, result.B);
        }

        [Fact]
        public void TestSimpleInt32()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(1234);
            writer.Write(4321);
            writer.Flush();

            var parser = from a in Parse.Int32
                         from b in Parse.Int32
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1234, result.A);
            Assert.Equal(4321, result.B);
        }

        [Fact]
        public void TestSimpleUInt32()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(1234u);
            writer.Write(4321u);
            writer.Flush();

            var parser = from a in Parse.UInt32
                         from b in Parse.UInt32
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1234u, result.A);
            Assert.Equal(4321u, result.B);
        }

        [Fact]
        public void TestSimpleSingles()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(1.0f);
            writer.Write(2.0f);
            writer.Flush();

            var parser = from a in Parse.Single
                         from b in Parse.Single
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1.0f, result.A);
            Assert.Equal(2.0f, result.B);
        }

        [Fact]
        public void TestSimpleDoubles()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(1.0d);
            writer.Write(2.0d);
            writer.Flush();

            var parser = from a in Parse.Double
                         from b in Parse.Double
                         select new { A = a, B = b };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal(1.0d, result.A);
            Assert.Equal(2.0d, result.B);
        }

        [Fact]
        public void TestString()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write(5);
            writer.Write((byte)'H');
            writer.Write((byte)'e');
            writer.Write((byte)'l');
            writer.Write((byte)'l');
            writer.Write((byte)'o');
            writer.Flush();

            var parser = from length in Parse.Int32
                         select new { Length = length };

            var mem = memoryStream.ToArray();
            var result = parser.Parse(mem);

            Assert.Equal("Hello", "Hello");
        }

        [Fact]
        public void TestStringZeroTerminated()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((byte)'H');
            writer.Write((byte)'e');
            writer.Write((byte)'l');
            writer.Write((byte)'l');
            writer.Write((byte)'o');
            writer.Write((byte)0);
            writer.Flush();

            var parser = Parse.StringZeroTerminated;
            var result = parser.Parse(memoryStream.ToArray());

            Assert.Equal("Hello", result);
        }
    }
}
