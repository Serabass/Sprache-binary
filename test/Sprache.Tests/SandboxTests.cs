using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Sprache.Binary.Tests.ZIP;
using Xunit;

namespace Sprache.Binary.Tests
{
    public class SandboxTests
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
            Assert.Equal(2, 2);
            Debugger.Break();
        }
    }
}
