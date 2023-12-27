using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using SpracheBinary;
using Xunit;

namespace SpracheBinary.Tests.TDR2000
{
  public struct TBlock
  {
    public string fileName;
    public int offset;
    public int size;
  }

  public struct PakEntry
  {
    public string name;
    public IEnumerable<byte> data;
    public string content;
    public int offset;
    public int size;
  }

  public class DirParser : IDisposable
  {
    private FileStream dirStream;
    private FileStream pakStream;
    private string path;

    private PakEntry[] pakEntries;

    static readonly Parser<char> TChar = from ch in Parse.AnyByte
                                         from unk in Parse.Bytes(0x40, 0xC0)
                                         select (char)ch;

    public static readonly Parser<char> TLastChar = from ch in Parse.AnyByte
                                                    from end in Parse.Byte(0x08)
                                                    select (char)ch;

    public static readonly Parser<string> TString = from chars in TChar.Many()
                                                    from end in TLastChar
                                                    select new string(chars.ToArray().Concat(new[] { end }).ToArray());

    public static readonly Parser<TBlock> TBlock = from fileName in TString
                                                   from offset in Parse.Int32
                                                   from size in Parse.Int32
                                                   select new TBlock
                                                   {
                                                     fileName = fileName,
                                                     offset = offset,
                                                     size = size
                                                   };

    public static readonly Parser<TBlock[]> TDocument = from blocks in TBlock.Many()
                                                        select blocks.ToArray();

    public DirParser(string path)
    {
      this.path = path;
    }

    private PakEntry ReadPakEntry(int offset, int size, string name)
    {
      pakStream.Flush();
      pakStream.Seek(offset, SeekOrigin.Begin);
      var reader = new BinaryReader(pakStream);
      var data = reader.ReadBytes(size);
      var content = Encoding.UTF8.GetString(data); //.Reverse().Aggregate("", (acc, ch) => acc + ch).Trim();

      return new PakEntry
      {
        name = name,
        data = data,
        content = content,
        offset = offset,
        size = size
      };
    }

    public DirParser Init()
    {
      dirStream = File.OpenRead($"{path}.dir");
      pakStream = File.OpenRead($"{path}.pak");

      var blocks = TDocument.Parse(dirStream);
      pakEntries = blocks.Select(b => ReadPakEntry(b.offset, b.size, b.fileName)).ToArray();

      return this;
    }

    public void Dispose()
    {
      dirStream.Dispose();
      pakStream.Dispose();
    }
  }

  public class TDR2000Tests
  {
    [Fact]
    public void ReadDirTest()
    {
      using DirParser p = new DirParser(@"C:\Program Files (x86)\Новый Диск\Кармагеддон. Колеса смерти\Assets\Sound\COMMON")
        .Init();

    }
  }
}
