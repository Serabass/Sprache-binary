using System.Linq;
using System.Text;

namespace SpracheBinary.Tests.GXT
{
  class GTXReader
  {
    private static Parser<string> String8 =
      from str in Parse.AnyByte.Repeat(8)
      select Encoding.ASCII.GetString(str.ToArray()).TrimEnd('\0');

    private static Parser<int> Header =
      from tabl in Parse.ConstString("TABL")
      from size in Parse.Int32
      select size;

    private static Parser<GTXTable> Table =
      from name in String8
      from addr in Parse.Int32
      select new GTXTable
      {
        Name = name.TrimEnd('\0'),
        Address = addr
      };

    private static Parser<int> Tkeys =
      from tkey in Parse.ConstString("TKEY")
      from size in Parse.Int32
      select size;

    public static Parser<TKEYEntry> TkeyEntry =
      from Offset in Parse.Int32
      from Name in String8
      select new TKEYEntry
      {
        Offset = Offset,
        Name = Name
      };

    private static Parser<TABLEntry> TablEntry =
      from name in String8
      from entryOffset in Parse.Int32
      select new TABLEntry
      {
        Name = name,
        Offset = entryOffset
      };

    public static Parser<TABLBlock> Tabl =
      from TABL in Parse.ConstString("TABL")
      from size in Parse.Int32
      from entries in TablEntry.Repeat(size / 12)
      select new TABLBlock
      {
        Size = size,
        Entries = entries.ToArray()
      };

    public static Parser<TKEYBlock> Tkey =
      from TKEY in Parse.ConstString("TKEY")
      from size in Parse.Int32
      from entries in TkeyEntry.Repeat(size / 12)
      select new TKEYBlock
      {
        Size = size,
        Entries = entries.ToArray()
      };

    public static Parser<char> WChar =
      from b1 in Parse.AnyByte.Except(Parse.NUL)
      from b2 in Parse.NUL
      select (char)b1;

    public static Parser<string> GXTString =
      from c1 in WChar.Many().Text()
      from c2 in Parse.NUL.Many()
      select c1.TrimEnd('\0');

    public static Parser<TDATBlock> Tdat =
      from TDAT in Parse.ConstString("TDAT")
      from size in Parse.Int32
      from strings in GXTString.Many()
      select new TDATBlock
      {
        Size = size,
        Strings = strings.ToArray()
      };

    public static Parser<TKEYTableKey> TKeyTableKey =
      from Offset in Parse.Int32
      from Name in String8
      select new TKEYTableKey
      {
        Offset = Offset,
        Name = Name,
      };

    public static Parser<TKEYTable> TKEyTable =
      from Name in String8
      from tkeyConst in Parse.ConstString("TKEY")
      from tkeySize in Parse.Int32
      from keys in TKeyTableKey.Repeat(tkeySize / 12)
      from tdatConst in Parse.ConstString("TDAT")
      from tdatSize in Parse.Int32
      from strings in GXTString.Many()
      select new TKEYTable
      {
        Name = Name,
        Size = tkeySize,
        Keys = keys.ToArray(),
        strings = strings.ToArray()
      };

    public static Parser<GXTDocument> Document =
      from TABL in Tabl
      from TKEY in Tkey
      from TDAT in Tdat
      from keys in TKEyTable.Many()

      select new GXTDocument
      {
        TABL = TABL,
        TKEY = TKEY,
        TDAT = TDAT,
        keys = keys.ToArray()
      };
  }
}
