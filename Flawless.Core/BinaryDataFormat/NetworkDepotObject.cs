using System.Runtime.InteropServices;

namespace Flawless.Core.BinaryDataFormat;

/* Depot Transmission Format Design - Version 1
 *
 * We have shrink some layout design and remap fields in order to optimize for networking transmission.
 *
 * Notice that we have assumed that all data are represent as LITTLE ENDIAN.
 *
 * Padding with 8 byte width, so:
 *
 * ------------------------------------------
 * 0       : Version Code
 * 1       : Network Transmission Feature
 * 2       : Compressing Algorithm Type (CompressType)
 * 3       : Checksum Confuser
 * 4       : Depot Generate Time
 * 5       : ~
 * 6       : ~
 * 7       : ~
 * ------------------------------------------
 * 8       : ~
 * 9       : ~
 * 10      : ~
 * 11      : ~
 * 12      : File Map String Size
 * 13      : ~
 * 14      : ~
 * 15      : ~
 * ------------------------------------------
 * 16      : ~
 * 17      : ~
 * 18      : ~
 * 19      : ~
 * 20      : ~
 * 21      : ~
 * 22      : ~
 * 23      : ~
 * ------------------------------------------
 * 24      : ~
 * 25      : ~
 * 26      : ~
 * 27      : ~
 * 28      : Payload Size
 * 29      : ~
 * 30      : ~
 * 31      : ~
 * ------------------------------------------
 * 32      : ~
 * 33      : ~
 * 34      : ~
 * 35      : ~
 * 36      : MD5 Checksum (Standard MD5Checksum)
 * 37      : ~
 * 38      : ~
 * 39      : ~
 * ------------------------------------------
 * 40      : ~
 * 41      : ~
 * 42      : ~
 * 43      : ~
 * 44      : ~
 * 45      : ~
 * 46      : ~
 * 47      : ~
 * ------------------------------------------
 * 48      : ~
 * 49      : ~
 * 50      : ~
 * 51      : ~
 * 52      : ~
 * ------------------------------------------
 * FILE NAME MAP (STRING IN UTF8)
 * ------------------------------------------
 * PAYLOAD
 */

[Flags]
public enum NetworkTransmissionFeatureFlag: byte
{
    FileMapIsJson = 1 << 0,
    FileMapUseCompressionArgument = 1 << 7,
}

[Serializable, StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 8, Size = 52)]
public struct NetworkDepotHeaderV1
{
    [FieldOffset(0)] public byte Version;
    
    [FieldOffset(1)] public NetworkTransmissionFeatureFlag NetworkTransmissionFeature;
    
    [FieldOffset(2)] public byte CompressType;

    [FieldOffset(3)] public byte Md5Confuser;

    [FieldOffset(4)] public ulong GenerateTime;

    [FieldOffset(12)] public ulong FileMapStringSizeLower;
    
    [FieldOffset(20)] public ulong FileMapStringSizeUpper;
    
    [FieldOffset(28)] public ulong PayloadSize;
    
    [FieldOffset(36)] public ulong Md5ChecksumLower;
    
    [FieldOffset(44)] public ulong Md5ChecksumUpper;
}
