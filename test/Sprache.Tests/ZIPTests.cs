using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Sprache.Binary.Tests.ZIP;
using Xunit;

namespace Sprache.Binary.Tests
{
    public class ZIPTests
    {
        // https://ide.kaitai.io/#
        [Fact]
        public void Test()
        {
            using var stream = File.OpenRead(@"../../../../../sample1.zip");
            var hash = MD5.Create().ComputeHash(stream);
            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            Assert.Equal("16c3bb924fae0e9de45107994716593c", hashString);

            var result = ZIPParser.zip.Parse(stream);
            Assert.Equal(7, result.Count());
            Assert.Equal(ZIPSectionType.LOCAL_FILE_HEADER, result.ToArray()[0].type);
            Assert.Equal("folder/", ((ZIPLocalFile)result.ToArray()[0].body).header.fileName);

            Assert.Equal(ZIPSectionType.LOCAL_FILE_HEADER, result.ToArray()[1].type);
            Assert.Equal("folder/fileInFolder.txt", ((ZIPLocalFile)result.ToArray()[1].body).header.fileName);

            Assert.Equal(ZIPSectionType.LOCAL_FILE_HEADER, result.ToArray()[2].type);
            Assert.Equal("file.txt", ((ZIPLocalFile)result.ToArray()[2].body).header.fileName);

            Assert.Equal(ZIPSectionType.CENTRAL_DIR_ENTRY, result.ToArray()[3].type);
            Assert.Equal("folder/", ((ZIPCentralDirEntry)result.ToArray()[3].body).fileName);

            Assert.Equal(ZIPSectionType.CENTRAL_DIR_ENTRY, result.ToArray()[4].type);
            Assert.Equal("folder/fileInFolder.txt", ((ZIPCentralDirEntry)result.ToArray()[4].body).fileName);

            Assert.Equal(ZIPSectionType.CENTRAL_DIR_ENTRY, result.ToArray()[5].type);
            Assert.Equal("file.txt", ((ZIPCentralDirEntry)result.ToArray()[5].body).fileName);

            Assert.Equal(ZIPSectionType.END_OF_CENTRAL_DIR, result.ToArray()[6].type);
            Debugger.Break();
        }
    }
}
