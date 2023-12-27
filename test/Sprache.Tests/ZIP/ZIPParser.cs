using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Xunit;

namespace Sprache.Binary.Tests.ZIP
{
  public enum ZIPSectionType
  {
    CENTAL_DIR_ENTRY = 0x0201,
    LOCAL_FILE_HEADER = 0x0403,
    DATA_DESCRIPTOR = 0x0807,
    END_OF_CENTRAL_DIR = 0x0605,
  }

  public class ZIPParser
  {
    public static Parser<string> zipMagic =
      from header1 in Parse.Byte(0x50) // PK
      from header2 in Parse.Byte(0x4B) // PK
      select "PK";

    public static Parser<ZIPSectionType> zipHeader =
      from Magic in zipMagic
      from sectionType in Parse.UInt16
      select (ZIPSectionType)sectionType;

    public static Parser<IEnumerable<byte>> zipSectionHeader =
      from version in Parse.UInt16
      from flags in Parse.UInt16
      from compressionMethod in Parse.UInt16
      from fileModTime in Parse.UInt32
      from crc32 in Parse.UInt32
      from compressedSize in Parse.UInt32
      from uncompressedSize in Parse.UInt32
      from fileNameLength in Parse.UInt16
      from extraFieldLength in Parse.UInt16
      from fileName in Parse.AnyByte.Repeat(fileNameLength)
      from header in Parse.AnyByte.Repeat(2)
      from extraField in Parse.AnyByte.Repeat(extraFieldLength)
      select header;

    public static Parser<IEnumerable<byte>> zipSection =
      from h in zipHeader
      from header in zipSectionHeader
      select header;

    public static IEnumerable<byte> ParseStream(Stream stream)
    {
      return zipSection.Parse(stream);
    }
  }
}
