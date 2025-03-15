using System.Runtime.InteropServices;
using Flawless.Core.Modal;

namespace Flawless.Core.BinaryDataFormat;

/* Depot File System Format Design - Version 1
 *
 * For best accessing performance, consider use checksum as filename. Binary info did not contains file name mapping, so
 * use another file to get the file map of this depot. The structure to describe a map has already defined below.
 *
 * Consider of compability when depot format was updated, we have configure a lots of area as empty.
 *
 * Notice that we assuming that all data are represent as LITTLE ENDIAN.
 *
 * Padding with 4 byte width, so:
 *
 * ------------------------------------------
 * 0       : Magic Number
 * 1       : ~
 * 2       : ~
 * 3       : ~
 * ------------------------------------------
 * 4       : MD5 Checksum (Header + Data)
 * 5       : ~
 * 6       : ~
 * 7       : ~
 * ------------------------------------------
 * 8       : ~
 * 9       : ~
 * 10      : ~
 * 11      : ~
 * ------------------------------------------
 * 12      : ~
 * 13      : ~
 * 14      : ~
 * 15      : ~
 * ------------------------------------------
 * 16      : ~
 * 17      : ~
 * 18      : ~
 * 19      : ~
 * ------------------------------------------
 * 20      : Version Code
 * 21      : Checksum Confuser
 * 22      : (Unused)
 * 23      : (Unused)
 * ------------------------------------------
 * 24      : Depot Generate Time
 * 25      : ~
 * 26      : ~
 * 27      : ~
 * ------------------------------------------
 * 28      : ~
 * 29      : ~
 * 30      : ~
 * 31      : ~
 * ------------------------------------------
 * 32      : Compressing Algorithm Type (CompressType)
 * 33      : (Preserve)
 * 34      : (Preserve)
 * 35      : (Preserve)
 * ------------------------------------------
 * 36      : (Preserve)
 * 37      : (Preserve)
 * 38      : (Preserve)
 * 39      : (Preserve)
 * ------------------------------------------
 * 40      : Payload Size
 * 41      : ~
 * 42      : ~
 * 43      : ~
 * ------------------------------------------
 * 44      : ~
 * 45      : ~
 * 46      : ~
 * 47      : ~
 * ------------------------------------------
 * 48      : (Unused)
 * 49      : (Unused)
 * 50      : (Unused)
 * 51      : (Unused)
 * ------------------------------------------
 * 52      : (Unused)
 * 53      : (Unused)
 * 54      : (Unused)
 * 55      : (Unused)
 * ------------------------------------------
 * 56      : (Unused)
 * 57      : (Unused)
 * 58      : (Unused)
 * 59      : (Unused)
 * ------------------------------------------
 * 60      : (Unused)
 * 61      : (Unused)
 * 62      : (Unused)
 * 63      : (Unused)
 * ------------------------------------------
 * PAYLOAD
 */

[Serializable, StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 4, Size = 64)]
public struct StandardDepotHeaderV1
{
    public const uint FormatMagicNumber = 0xAAAAAAAAu;
    
    [FieldOffset(0)] public uint MagicNumber;
    
    [FieldOffset(4)] public ulong Md5ChecksumLower;
    
    [FieldOffset(12)] public ulong Md5ChecksumUpper;
    
    [FieldOffset(20)] public byte Version;

    [FieldOffset(21)] public byte Md5Confuser;

    [FieldOffset(22)] public ushort PayloadUnit;

    [FieldOffset(24)] public ulong GenerateTime;
    
    [FieldOffset(32)] public byte CompressType;
    
    [FieldOffset(40)] public ulong PayloadSize;
}

[Serializable]
public struct StandardDepotMapV1
{
    public uint FileCount;

    public DepotFileInfo[] Files;
}