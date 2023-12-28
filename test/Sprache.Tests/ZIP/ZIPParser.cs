using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sprache.Binary.Tests.ZIP
{
  public enum CompressionMethod : ushort
  {
    STORED = 0,
    SHRUNK = 1,
    REDUCED_FACTOR_1 = 2,
    REDUCED_FACTOR_2 = 3,
    REDUCED_FACTOR_3 = 4,
    REDUCED_FACTOR_4 = 5,
    IMPLODED = 6,
    RESERVED_1 = 7,
    DEFLATED = 8,
    ENHANCED_DEFLATED = 9,
    PKWARE_DCL_IMPLODED = 10,
    RESERVED_2 = 11,
    BZIP2 = 12,
    RESERVED_3 = 13,
    LZMA = 14,
    RESERVED_4 = 15,
    RESERVED_5 = 16,
    IBM_TERSE = 18,
    IBM_LZ77_Z = 19,
    PPMD = 98,
    AEX_ENCRYPTION_MARKER = 99,
  }
  public enum ZIPSectionType
  {
    CENTAL_DIR_ENTRY = 0x0201,
    LOCAL_FILE_HEADER = 0x0403,
    DATA_DESCRIPTOR = 0x0807,
    END_OF_CENTRAL_DIR = 0x0605,
  }

  public struct ZIPFileHeader
  {
    public ushort version;
    public ushort flags;
    public CompressionMethod compressionMethod;
    public uint fileModTime;
    public uint crc32;
    public int compressedSize;
    public int uncompressedSize;
    public ushort fileNameLength;
    public ushort extraFieldLength;
    public IEnumerable<byte> fileName;
    public IEnumerable<byte> extraField;
    public string comment;
  }

  public struct ZIPSection
  {
    public ZIPSectionType type;
    public ZIPSectionBody body;
  }
  public struct ZIPSectionBody
  {
    public ZIPSectionType type;
    public ZIPFileHeader header;
    public IEnumerable<byte> body;

    public IEnumerable<byte> GetBody()
    {
      switch (header.compressionMethod)
      {
        case CompressionMethod.STORED:
          return body;
        case CompressionMethod.DEFLATED:
          return body;
        default:
          return body;
      }
    }
  }

  public class ZIPParser
  {
    public static Parser<string> zipMagic =
      from header1 in Parse.Byte(0x50) // P
      from header2 in Parse.Byte(0x4B) // K
      select "PK";

    public static Parser<ZIPSection> zipSection =
      from Magic in zipMagic
      from sectionType in Parse.UInt16
      from body in zipSectionBody.Optional()
      select new ZIPSection
      {
        type = (ZIPSectionType)sectionType,
        body = body.GetOrElse(new ZIPSectionBody
        {
          type = (ZIPSectionType)sectionType,
          header = new ZIPFileHeader(),
          body = System.Array.Empty<byte>(),
        })
      };

    private static Parser<CompressionMethod> compressionMethod =
      from compressionMethod in Parse.UInt16
      select (CompressionMethod)compressionMethod;

    public static Parser<ZIPFileHeader> zipFileHeader =
      from version in Parse.UInt16
      from flags in Parse.UInt16
      from compressionMethod in compressionMethod
      from fileModTime in Parse.UInt32
      from crc32 in Parse.UInt32
      from compressedSize in Parse.Int32
      from uncompressedSize in Parse.Int32
      from fileNameLength in Parse.UInt16
      from extraFieldLength in Parse.UInt16
      from fileName in Parse.AnyByte.Repeat(fileNameLength)
      from extraField in Parse.AnyByte.Repeat(extraFieldLength)
      from commentLength in Parse.UInt16
      from comment in Parse.AnyByte.Repeat(commentLength)
      select new ZIPFileHeader
      {
        version = version,
        flags = flags,
        compressionMethod = compressionMethod,
        fileModTime = fileModTime,
        crc32 = crc32,
        compressedSize = compressedSize,
        uncompressedSize = uncompressedSize,
        fileNameLength = fileNameLength,
        extraFieldLength = extraFieldLength,
        fileName = fileName,
        extraField = extraField,
        comment = System.Text.Encoding.UTF8.GetString(comment.ToArray()),
      };

    public static Parser<ZIPSectionBody> zipSectionBody =
      from header in zipFileHeader
      from body in Parse.AnyByte.Repeat(header.compressedSize)
      select new ZIPSectionBody
      {
        type = ZIPSectionType.LOCAL_FILE_HEADER,
        header = header,
        body = body,
      };

    public static Parser<IEnumerable<ZIPSection>> zip =
      from sections in zipSection.Many()
      select sections;

    public static IEnumerable<ZIPSection> ParseStream(Stream stream)
    {
      return zip.Parse(stream);
    }
  }
}
