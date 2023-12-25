using System.IO;
using System.Security.Cryptography;
using Xunit;

namespace SpracheBinary.Tests.GXT
{
  public class GXTTests
  {
    // http://gtamodding.ru/wiki/GXT_(VC)

    private GXTDocument ParseGXT()
    {
      var stream = File.OpenRead(@"../../../../../american.gxt");
      var hash = MD5.Create().ComputeHash(stream);
      var hashString = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
      Assert.Equal("af2b64060598d72b13f5e7fe443ad41a", hashString);

      return GTXReader.Document.Parse(stream);
    }

    [Fact]
    public void ReadGXTTest()
    {
      GXTDocument doc = ParseGXT();

      Assert.Equal(948, doc.TABL.Size);
      Assert.Equal(79, doc.TABL.Entries.Length); // 948 / 12

      Assert.Equal(29388, doc.TKEY.Size);
      Assert.Equal(2449, doc.TKEY.Entries.Length); // 29388 / 12

      Assert.Equal(154886, doc.TDAT.Size);
      Assert.Equal(2449, doc.TDAT.Strings.Length); // 29388 / 12

      Assert.Equal(78, doc.keys.Length);
      Assert.Equal(144, doc.keys[0].Size);
      Assert.Equal(12, doc.keys[0].strings.Length); // 144 / 12
      Assert.Equal("~g~Drive the patients to Hospital CAREFULLY. Each bump reduces their chances of survival.", doc.keys[0].strings[0]);
    }
  }
}
