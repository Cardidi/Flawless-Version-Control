using System.Runtime.InteropServices;
using Flawless.Core.Modal;

namespace Flawless.Core.BinaryDataFormat;

/* Depot File System Format Design - Version 1
 *
 * For best accessing performance, consider use checksum as filename. Binary info did not contains file name mapping, so
 * use another file to get the file map of this depot. The structure to describe a map has already defined below and you
 * can choose binary or text to store them. Advice to use JSON as text-based solution.
 *
 * Consider of compability when depot format was updated, we have configure a lots of area as empty.
 *
 * Notice that we assume that all data are represent as LITTLE ENDIAN.
 *
 * Padding with 8 byte width, so:
 *
 * ------------------------------------------------------------
 * 0       : Magic Number
 * 1       : ~
 * 2       : ~
 * 3       : ~
 * 4       : Header CRC Checksum (From self range 8 to 63)
 * 5       : ~
 * 6       : ~
 * 7       : ~
 * ------------------------------------------------------------
 * 8       : Version Code
 * 9       : Compressing Algorithm Type
 * 10      : (Preserve)
 * 11      : (Preserve)
 * 12      : (Preserve)
 * 13      : (Preserve)
 * 14      : (Preserve)
 * 15      : (Preserve)
 * ------------------------------------------------------------
 * 16      : File Map MD5 Checksum (From extern map data)
 * 17      : ~
 * 18      : ~
 * 19      : ~
 * 20      : ~
 * 21      : ~
 * 22      : ~
 * 23      : ~
 * ------------------------------------------------------------
 * 24      : 
 * 25      : ~
 * 26      : ~
 * 27      : ~
 * 28      : ~
 * 29      : ~
 * 30      : ~
 * 31      : ~
 * ------------------------------------------------------------
 * 32      : Depot MD5 Checksum (From 48 to end, uncompressed)
 * 33      : ~
 * 34      : ~
 * 35      : ~
 * 36      : ~
 * 37      : ~
 * 38      : ~
 * 39      : ~
 * ------------------------------------------------------------
 * 40      : ~
 * 41      : ~
 * 42      : ~
 * 43      : ~
 * 44      : ~
 * 45      : ~
 * 46      : ~
 * 47      : ~
 * ------------------------------------------------------------
 * 48      : Depot Generate Time
 * 49      : ~
 * 50      : ~
 * 51      : ~
 * 52      : ~
 * 53      : ~
 * 54      : ~
 * 55      : ~
 * ------------------------------------------------------------
 * 56      : Payload Size
 * 57      : ~
 * 58      : ~
 * 59      : ~
 * 60      : ~
 * 61      : ~
 * 62      : ~
 * 63      : ~
 * ------------------------------------------------------------
 * PAYLOAD
 * ------------------------------------------------------------
 */

[Serializable, StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 8, Size = 64)]
public struct StandardDepotHeaderV1
{
    // 1A: Not a text-based file
    // F7A373 = Flawless
    public const uint FormatMagicNumber = 0x1AF7A373u;
    
    [FieldOffset(0)] public uint MagicNumber;

    [FieldOffset(4)] public uint HeaderCRCChecksum;
    
    [FieldOffset(8)] public byte Version;

    [FieldOffset(9)] public byte CompressType;
    
    [FieldOffset(16)] public ulong FileMapMd5ChecksumLower;
    
    [FieldOffset(24)] public ulong FileMapMd5ChecksumUpper;
    
    [FieldOffset(32)] public ulong DepotMd5ChecksumLower;
    
    [FieldOffset(40)] public ulong DepotMd5ChecksumUpper;

    [FieldOffset(48)] public ulong GenerateTime;
    
    [FieldOffset(56)] public ulong PayloadSize;
}

[Serializable]
public struct StandardDepotMapV1
{
    public uint FileCount;

    public DepotFileInfo[] Files;
}