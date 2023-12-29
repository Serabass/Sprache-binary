using System;
using System.Collections.Generic;
using System.IO;

// https://ide.kaitai.io/
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
    public string fileName;
    public IEnumerable<byte> extraField;
  }

  public class ZIPSection
  {
    public ZIPSectionType type;
    public ZIPSectionBody body;
  }

  public abstract class ZIPSectionBody
  {

  }

  public class ZIPLocalFile : ZIPSectionBody
  {
    public ZIPFileHeader header;
    public IEnumerable<byte> body;
  }

  public class ZIPCentralDirEntry : ZIPSectionBody
  {
    public ushort versionMadeBy;
    public ushort versionNeededToExtract;
    public ushort flags;
    public CompressionMethod compressionMethod;
    public uint fileModTime;
    public uint crc32;
    public int compressedSize;
    public int uncompressedSize;
    public ushort fileNameLength;
    public ushort extraFieldLength;
    public ushort fileCommentLength;
    public ushort diskNumberStart;
    public ushort internalFileAttributes;
    public uint externalFileAttributes;
    public uint relativeOffsetOfLocalHeader;
    public string fileName;
    public IEnumerable<byte> extraField;
    public string fileComment;
    // public ZIPSection localHeader;
  }

  public class ZIPEndOfCentralDir : ZIPSectionBody
  {
    public ushort diskOfEndOfCentralDir;
    public ushort diskOfCentralDir;
    public ushort numberOfCentralDirRecordsOnThisDisk;
    public ushort numberOfCentralDirRecordsTotal;
    public uint sizeOfCentralDir;
    public uint offsetOfStartOfCentralDir;
    public ushort commentLength;
    public string comment;
  }

  public class ZIPLocalFileHeader
  {
    public ZIPFileHeader header;
  }

  public class ZIPParser
  {
    public static Parser<string> zipMagic =
      from header1 in Parse.Byte(0x50) // P
      from header2 in Parse.Byte(0x4B) // K
      select "PK";

    private static Parser<ZIPSectionType> zipSectionType =
      from sectionType in Parse.UInt16
      select (ZIPSectionType)sectionType;

    public static Parser<ZIPSection> zipSection =
      from Magic in zipMagic
      from sectionType in zipSectionType
      from body in zipSectionBody(sectionType)
      select new ZIPSection
      {
        type = sectionType,
        body = body,
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
      from fileName in Parse.FixedString(fileNameLength)
      from extraField in Parse.AnyByte.Repeat(extraFieldLength)
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
      };

    public static Parser<ZIPFileHeader> zipLocalFileHeader =
      from header in zipFileHeader
      select header;

    public static Parser<ZIPLocalFile> zipLocalFile =
      from header in zipLocalFileHeader
      from body in Parse.AnyByte.Repeat(header.compressedSize)
      select new ZIPLocalFile
      {
        header = header,
        body = body,
      };

    public static Parser<ZIPCentralDirEntry> zipCentralDirEntry =
      from versionMadeBy in Parse.UInt16
      from versionNeededToExtract in Parse.UInt16
      from flags in Parse.UInt16
      from compressionMethod in compressionMethod
      from fileModTime in Parse.UInt32
      from crc32 in Parse.UInt32
      from compressedSize in Parse.Int32
      from uncompressedSize in Parse.Int32
      from fileNameLength in Parse.UInt16
      from extraFieldLength in Parse.UInt16
      from fileCommentLength in Parse.UInt16
      from diskNumberStart in Parse.UInt16
      from internalFileAttributes in Parse.UInt16
      from externalFileAttributes in Parse.UInt32
      from relativeOffsetOfLocalHeader in Parse.UInt32
      from fileName in Parse.FixedString(fileNameLength)
      from extraField in Parse.AnyByte.Repeat(extraFieldLength)
      from fileComment in Parse.FixedString(fileCommentLength)
        // from localHeader in zipSection
      select new ZIPCentralDirEntry
      {
        versionMadeBy = versionMadeBy,
        versionNeededToExtract = versionNeededToExtract,
        flags = flags,
        compressionMethod = compressionMethod,
        fileModTime = fileModTime,
        crc32 = crc32,
        compressedSize = compressedSize,
        uncompressedSize = uncompressedSize,
        fileNameLength = fileNameLength,
        extraFieldLength = extraFieldLength,
        fileCommentLength = fileCommentLength,
        diskNumberStart = diskNumberStart,
        internalFileAttributes = internalFileAttributes,
        externalFileAttributes = externalFileAttributes,
        relativeOffsetOfLocalHeader = relativeOffsetOfLocalHeader,
        fileName = fileName,
        extraField = extraField,
        fileComment = fileComment,
        // localHeader = localHeader,
      };

    public static Parser<ZIPEndOfCentralDir> zipEndOfCentralDir =
      from diskNumber in Parse.UInt16
      from diskNumberWithStartOfCentralDir in Parse.UInt16
      from numberOfCentralDirRecordsOnThisDisk in Parse.UInt16
      from numberOfCentralDirRecords in Parse.UInt16
      from sizeOfCentralDir in Parse.UInt32
      from offsetOfStartOfCentralDir in Parse.UInt32
      from commentLength in Parse.UInt16
      from comment in Parse.FixedString(commentLength)
      select new ZIPEndOfCentralDir
      {
        diskOfEndOfCentralDir = diskNumber,
        diskOfCentralDir = diskNumberWithStartOfCentralDir,
        numberOfCentralDirRecordsOnThisDisk = numberOfCentralDirRecordsOnThisDisk,
        numberOfCentralDirRecordsTotal = numberOfCentralDirRecords,
        sizeOfCentralDir = sizeOfCentralDir,
        offsetOfStartOfCentralDir = offsetOfStartOfCentralDir,
        commentLength = commentLength,
        comment = comment,
      };

    public static Parser<ZIPSectionBody> zipSectionBody(ZIPSectionType type)
      => type switch
      {
        ZIPSectionType.LOCAL_FILE_HEADER => zipLocalFile,
        ZIPSectionType.CENTAL_DIR_ENTRY => zipCentralDirEntry,
        ZIPSectionType.END_OF_CENTRAL_DIR => zipEndOfCentralDir,
        _ => throw new NotImplementedException(),
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
