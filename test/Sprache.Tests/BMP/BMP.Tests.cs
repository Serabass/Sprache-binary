
// https://ide.kaitai.io/
using Xunit;

namespace Sprache.Binary.Tests.BMP
{
  public class BMPFileHeader
  {
    public byte magic;
    public int fileSize;
    public short reserved1;
    public short reserved2;
    public int ofsBitmap;
  }

  public class BMPTests
  {
    [Fact]
    public void TestParseBMP()
    {
      var bmpFileHeader =
        from magic in Parse.Byte(0x42).Then(_ => Parse.Byte(0x4d))
        from fileSize in Parse.Int32
        from reserved1 in Parse.Int16
        from reserved2 in Parse.Int16
        from ofsBitmap in Parse.Int32
        select new BMPFileHeader
        {
          magic = magic,
          fileSize = fileSize,
          reserved1 = reserved1,
          reserved2 = reserved2,
          ofsBitmap = ofsBitmap,
        };
    }
  }
}
