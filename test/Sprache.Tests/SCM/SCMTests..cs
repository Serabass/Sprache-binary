using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace SpracheBinary.Tests.SCM
{
  public struct NameThread
  {
    public string name;
  }

  public enum SCMOpcode : ushort
  {
    NAME_THREAD = 0x03A4,
  }

  public class SCMTests
  {
    [Fact]
    public void ReadSCMTest()
    {
      using var stream = File.OpenRead(@"../../../../../main.scm");
      var hash = MD5.Create().ComputeHash(stream);
      var hashString = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
      Assert.Equal("97398200d4423c3cf93285b178136e75", hashString);

      var String8 = from str in Parse.AnyByte.Repeat(8)
                    select System.Text.Encoding.ASCII.GetString(str.ToArray());

      var opcode = from _ in Parse.AnyByte.Repeat(0x9AE4)
                   from op in Parse.UInt16
                   select (SCMOpcode)op;

      var p = opcode.Parse(stream);
    }
  }
}
