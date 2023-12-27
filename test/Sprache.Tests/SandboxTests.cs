using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Xunit;

namespace Sprache.Binary.Tests
{
    public class SandboxTests
    {
        public enum ZIPSectionType
        {
            CENTAL_DIR_ENTRY = 0x0201,
            LOCAL_FILE_HEADER = 0x0403,
            DATA_DESCRIPTOR = 0x0807,
            END_OF_CENTRAL_DIR = 0x0605,
        }

        // https://ide.kaitai.io/#
        [Fact]
        public void Test()
        {
            using var stream = File.OpenRead(@"../../../../../sample1.zip");
            var hash = MD5.Create().ComputeHash(stream);
            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            Assert.Equal("16c3bb924fae0e9de45107994716593c", hashString);

            var zipMagic = from header1 in Parse.Byte(0x50) // PK
                           from header2 in Parse.Byte(0x4B) // PK
                           select "PK";

            var zipHeader = from Magic in zipMagic
                            from sectionType in Parse.UInt16
                            select (ZIPSectionType)sectionType;

            var zipSectionHeader = from version in Parse.UInt16
                                   from flags in Parse.UInt16
                                   from compressionMethod in Parse.UInt16
                                   from fileModTime in Parse.UInt32
                                   from crc32 in Parse.UInt32
                                   from compressedSize in Parse.UInt32
                                   from uncompressedSize in Parse.UInt32
                                   from fileNameLength in Parse.UInt16
                                   from extraFieldLength in Parse.UInt16
                                   from fileName in Parse.AnyByte.Repeat(fileNameLength)
                                   from header in Parse.AnyByte.Repeat(2)
                                   from extraField in Parse.AnyByte.Repeat(extraFieldLength)
                                   select header;

            var zipSection = from h in zipHeader
                             from header in zipSectionHeader
                             select header;

            var result = zipSection.Parse(stream);
            Assert.Equal(2, 2);
        }
    }
}
