using System.Diagnostics;
using System.IO;
using Xunit;

namespace Sprache.Binary.Tests.ProjectIGI
{
  public class IGITests
  {
    [Fact]
    public void ReadQVMTest()
    {
      var file = @"M:\Program Files (x86)\IGI Collection\Project IGI\pc\weapons\AK47\weapon.qvm";
      using Stream s = File.OpenRead(file);
      var parser = from header in Parse.ASCIIConstString("LOOP")
                   from i in Parse.Int32.Repeat(15)
                   from clip in Parse.Int32
                   from ii in Parse.Int32.Repeat(6)
                   from st in Parse.StringZeroTerminated.Repeat(7)
                   from i2 in Parse.Int32.Repeat(10)
                   from st2 in Parse.StringZeroTerminated.Repeat(10)
                   from unk in Parse.AnyByte.Repeat(3)
                   from i3 in Parse.Int32.Repeat(51)
                   from i4 in Parse.AnyByte.Many()
                   select new { clip, header, i, ii, st, i2, st2, i3, i4, unk };

      var result = parser.Parse(s);
      Debugger.Break();
    }
  }
}
