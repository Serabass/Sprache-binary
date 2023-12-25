using System.IO;
using System.Linq;
using SpracheBinary;
using Xunit;

namespace SpracheBinary.Tests.TDR2000
{
  public struct TBlock
  {
    public string str;
    public int probOffset;
    public int probSize;
  }

  public class DirParser
  {
    static readonly Parser<char> TChar = from ch in Parse.AnyByte
                                         from unk in Parse.Bytes(0x40, 0xC0)
                                         select (char)ch;

    public static readonly Parser<char> TLastChar = from ch in Parse.AnyByte
                                                    from end in Parse.Byte(0x08)
                                                    select (char)ch;

    public static readonly Parser<string> TString = from chars in TChar.Many()
                                                    from end in TLastChar
                                                    select new string(chars.ToArray().Concat(new[] { end }).ToArray());

    public static readonly Parser<TBlock> TBlock = from str in TString
                                                   from probOffset in Parse.Int32
                                                   from probSize in Parse.Int32
                                                   select new TBlock
                                                   {
                                                     str = str,
                                                     probOffset = probOffset,
                                                     probSize = probSize
                                                   };

    public static readonly Parser<TBlock[]> TDocument = from blocks in TBlock.Many()
                                                         select blocks.ToArray();

  }

  public class TDR2000Tests
  {
    [Fact]
    public void ReadDirTest()
    {
      using var dir = File.OpenRead(@"C:\Program Files (x86)\Новый Диск\Кармагеддон. Колеса смерти\Assets\MovableObjects\MOVABLEOBJECTS.dir");
      dir.Position = 0;
      var doc = DirParser.TDocument.Parse(dir);
    }
  }
}
