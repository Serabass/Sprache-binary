using System.Collections.Generic;

namespace SpracheBinary.Tests.GXT
{
  public struct GXTDocument
  {
    public TABLBlock TABL;
    public TKEYBlock TKEY;
    public TDATBlock TDAT;
    public TKEYTable[] keys;
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

  public struct TKEYTable
  {
    public string Name { get; set; }
    public int Size { get; set; }
    public TKEYTableKey[] Keys { get; set; }
    public string[] strings { get; set; }

    public readonly Dictionary<string, string> ToDictionary()
    {
      var dict = new Dictionary<string, string>();
      for (int i = 0; i < Keys.Length; i++)
      {
        dict.Add(Keys[i].Name, strings[i]);
      }
      return dict;
    }
  }

  public struct TKEYTableKey
  {
    public int Offset { get; set; }
    public string Name { get; set; }
  }
}
