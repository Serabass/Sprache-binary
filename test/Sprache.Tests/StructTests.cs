using System;
using System.IO;
using Xunit;

namespace Sprache.Binary.Tests
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

            var memoryStream = new MemoryStream([255, 0, 0]);
            var rgb = parser.Parse(memoryStream);

            Assert.Equal(255, rgb.R);
            Assert.Equal(0, rgb.G);
            Assert.Equal(0, rgb.B);
        }

        [Fact]
        public void TestSimpleBytes()
        {
            var parser = from a in Parse.AnyByte
                         from b in Parse.AnyByte
                         select new { A = a, B = b };

            var bytes = parser.Parse((writer) =>
            {
                writer.Write((byte)255);
                writer.Write((byte)213);
            });

            Assert.Equal(255, bytes.A);
            Assert.Equal(213, bytes.B);
        }

        [Fact]
        public void TestSimpleSingle()
        {
            var parser = Parse.Single;
            var result = parser.Parse((writer) =>
            {
                writer.Write(1.0f);
            });

            Assert.Equal(1.0f, result);
        }

        [Fact]
        public void TestSimpleDouble()
        {
            var parser = Parse.Double;
            var result = parser.Parse((writer) =>
            {
                writer.Write(1.0d);
            });

            Assert.Equal(1.0d, result);
        }

        [Fact]
        public void TestSimpleInt16()
        {
            var parser = from a in Parse.Int16
                         from b in Parse.Int16
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write((short)1234);
                writer.Write((short)4321);
            });
            Assert.Equal(1234, result.A);
            Assert.Equal(4321, result.B);
        }

        [Fact]
        public void TestSimpleUInt16()
        {
            var parser = from a in Parse.UInt16
                         from b in Parse.UInt16
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write((ushort)1234);
                writer.Write((ushort)4321);
            });

            Assert.Equal(1234u, result.A);
            Assert.Equal(4321u, result.B);
        }

        [Fact]
        public void TestSimpleInt32()
        {
            var parser = from a in Parse.Int32
                         from b in Parse.Int32
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1234);
                writer.Write(4321);
            });

            Assert.Equal(1234, result.A);
            Assert.Equal(4321, result.B);
        }

        [Fact]
        public void TestSimpleUInt32()
        {
            var parser = from a in Parse.UInt32
                         from b in Parse.UInt32
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1234u);
                writer.Write(4321u);
            });

            Assert.Equal(1234u, result.A);
            Assert.Equal(4321u, result.B);
        }

        [Fact]
        public void TestSimpleInt64()
        {
            var parser = from a in Parse.Int64
                         from b in Parse.Int64
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1234L);
                writer.Write(4321L);
            });

            Assert.Equal(1234L, result.A);
            Assert.Equal(4321L, result.B);
        }

        [Fact]
        public void TestSimpleUInt64()
        {
            var parser = from a in Parse.UInt64
                         from b in Parse.UInt64
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1234Lu);
                writer.Write(4321Lu);
            });

            Assert.Equal(1234Lu, result.A);
            Assert.Equal(4321Lu, result.B);
        }

        [Fact]
        public void TestSimpleSingles()
        {
            var parser = from a in Parse.Single
                         from b in Parse.Single
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1.0f);
                writer.Write(2.0f);
            });

            Assert.Equal(1.0f, result.A);
            Assert.Equal(2.0f, result.B);
        }

        [Fact]
        public void TestSimpleDoubles()
        {
            var parser = from a in Parse.Double
                         from b in Parse.Double
                         select new { A = a, B = b };

            var result = parser.Parse((writer) =>
            {
                writer.Write(1.0d);
                writer.Write(2.0d);
            });

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

            var parser = from value in Parse.String
                         select value;

            memoryStream.Position = 0;

            var result = parser.Parse(memoryStream);

            Assert.Equal(5, result.Length);
            Assert.Equal("Hello", result);
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

            memoryStream.Position = 0;

            var parser = Parse.StringZeroTerminated;
            var result = parser.Parse(memoryStream);

            Assert.Equal("Hello", result);
        }

        [Fact]
        public void TestComplexStruct()
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            writer.Write((byte)255);

            writer.Write((short)12345);
            writer.Write(55111111);

            writer.Write((byte)'H');
            writer.Write((byte)'e');
            writer.Write((byte)'l');
            writer.Write((byte)'l');
            writer.Write((byte)'o');
            writer.Write((byte)0);
            writer.Flush();

            memoryStream.Position = 0;

            var parser = from b in Parse.AnyByte
                         from i16 in Parse.Int16
                         from i32 in Parse.Int32
                         from s in Parse.StringZeroTerminated
                         select new { B = b, I16 = i16, I32 = i32, S = s };

            var result = parser.Parse(memoryStream);

            Assert.Equal(255, result.B);
            Assert.Equal(12345, result.I16);
            Assert.Equal(55111111, result.I32);
            Assert.Equal("Hello", result.S);
        }
    }
}
