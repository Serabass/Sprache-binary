using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Xunit;

namespace Sprache.Binary.Tests.SCM
{
  public class SCMTests
  {
    [Fact]
    public void ReadSCMTest()
    {
      using var stream = File.OpenRead(@"../../../../../main.scm");
      var hash = MD5.Create().ComputeHash(stream);
      var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
      Assert.Equal("97398200d4423c3cf93285b178136e75", hashString);

      var String8 = from str in Parse.AnyByte.Repeat(8)
                    select Encoding.ASCII.GetString(str.ToArray());

      var nameThread = from name in String8
                       select name;

      var opcode = from op in Parse.UInt16
                   select (SCMOpcode)op;

      var scmDoc = from _ in Parse.AnyByte.Repeat(0x9AE4)
                   from op in opcode
                   from args in op switch
                   {
                     SCMOpcode.NAME_THREAD => nameThread,
                     _ => throw new NotImplementedException(),
                   }
                   select op;

      var p = from opcode1 in scmDoc
              select opcode1;
    }
  }
}
