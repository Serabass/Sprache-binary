using System.IO;
using System.Linq;
using Xunit;

namespace SpracheBinary.Tests
{
  public class GXTTests
  {
    public struct GXTDocument
    {
      public TABLBlock TABL;
      public TKEYBlock TKEY;
      public TDATBlock TDAT;
    }

    public struct TKEYBlock
    {
      public int Size;
      public TKEYEntry[] Entries;
    }

    public struct TDATBlock
    {
      public int Size;
      public string[] Strings;
    }

    public struct TABLBlock
    {
      public int Size;
      public TABLEntry[] Entries;
    }

    public struct TABLEntry
    {
      public string Name { get; set; }
      public int Offset { get; set; }
    }

    struct GTXTable
    {
      public string Name;
      public int Address;
    }

    public struct TKEYEntry
    {
      public int Offset { get; set; }
      public string Name { get; set; }
    }

    class GTXReader
    {
      readonly Stream stream;

      public GTXReader(Stream stream)
      {
        this.stream = stream;
      }

      private Parser<string> String8 => from str in Parse.AnyByte.Repeat(8)
                                        select System.Text.Encoding.ASCII.GetString(str.ToArray());

      private Parser<int> Header => from tabl in Parse.ConstString("TABL")
                                    from size in Parse.Int32
                                    select size;

      private Parser<GTXTable> Table => from name in String8
                                        from addr in Parse.Int32
                                        select new GTXTable
                                        {
                                          Name = name.TrimEnd('\0'),
                                          Address = addr
                                        };

      private Parser<int> Tkeys => from tkey in Parse.ConstString("TKEY")
                                   from size in Parse.Int32
                                   select size;

      public Parser<TKEYEntry> TkeyEntry =>
        from Offset in Parse.Int32
        from Name in String8
        select new TKEYEntry
        {
          Offset = Offset,
          Name = Name
        };

      private Parser<TABLEntry> TablEntry =>
        from name in String8
        from entryOffset in Parse.Int32
        select new TABLEntry
        {
          Name = name,
          Offset = entryOffset
        };
      public Parser<TABLBlock> Tabl =>
        from TABL in Parse.ConstString("TABL")
        from size in Parse.Int32
        from entries in TablEntry.Repeat(size / 12)
        select new TABLBlock
        {
          Size = size,
          Entries = entries.ToArray()
        };

      public Parser<TKEYBlock> Tkey =>
        from TKEY in Parse.ConstString("TKEY")
        from size in Parse.Int32
        from entries in TkeyEntry.Repeat(size / 12)
        select new TKEYBlock
        {
          Size = size,
          Entries = entries.ToArray()
        };

    public static Parser<char> WChar =>
      from b1 in Parse.AnyByte.Except(Parse.Byte(0x00))
      from b2 in Parse.Byte(0x00)
      select (char)b1;

    public Parser<string> GXTString =>
      from c1 in WChar.Many().Text()
      from c2 in Parse.Byte(0x00)
      from c3 in Parse.Byte(0x00)
      select c1.TrimEnd('\0');

    public Parser<TDATBlock> Tdat =>
      from TDAT in Parse.ConstString("TDAT")
      from size in Parse.Int32
      from strings in GXTString.Many()
      select new TDATBlock
      {
        Size = size,
        Strings = strings.ToArray()
      };

      public Parser<GXTDocument> Document =>
        from TABL in Tabl
        from TKEY in Tkey
        from TDAT in Tdat
          // from EOF in Parse.String("END\0")
        select new GXTDocument
        {
          TABL = TABL,
          TKEY = TKEY,
          TDAT = TDAT
        };

      public GTXReader Read()
      {
        var s = Document.Parse(stream);

        return this;
      }
    }

    [Fact]
    public void ReadGXTTest()
    {
      var md5 = System.Security.Cryptography.MD5.Create();
      var stream = File.OpenRead(@"M:\dev\csharp\Sprache-binary\american.gxt");
      var hash = md5.ComputeHash(stream);
      var hashString = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
      Assert.Equal("af2b64060598d72b13f5e7fe443ad41a", hashString);
      stream.Position = 0;
      var reader = new GTXReader(stream).Read();

      stream.Close();
    }
  }
}
