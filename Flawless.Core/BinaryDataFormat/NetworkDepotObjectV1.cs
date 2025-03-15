using System.Runtime.InteropServices;

namespace Flawless.Core.BinaryDataFormat;

/* Depot Transmission Format Design - Version 1
 *
 * We have shrink some layout design and remap fields in order to optimize for networking transmission. You may noticed
 * that we don't have a compressing info, this is due to compressing is mainly about how did depot stored in local disk,
 * when using network transmission, we may compress it from outside. So we let compressing to go.
 *
 * Notice that we assume that all data are represent as LITTLE ENDIAN.
 *
 * Padding with 4 byte width, so:
 *
 * ------------------------------------------------------------
 * 0       : Version Code
 * 1       : Network Transmission Feature
 * 2       : (Preserve)
 * 3       : (Preserve)
 * ------------------------------------------------------------
 * 4       : (Preserve)
 * 5       : (Preserve)
 * 6       : (Preserve)
 * 7       : (Preserve)
 * ------------------------------------------------------------
 * 8       : File Map String Size
 * 9       : ~
 * 10      : ~
 * 11      : ~
 * ------------------------------------------------------------
 * 12      : ~
 * 13      : ~
 * 14      : ~
 * 15      : ~
 * ------------------------------------------------------------
 * 16      : MD5 Checksum (Standard DepotMD5Checksum)
 * 17      : ~
 * 18      : ~
 * 19      : ~
 * ------------------------------------------------------------
 * 20      : ~
 * 21      : ~
 * 22      : ~
 * 23      : ~
 * ------------------------------------------------------------
 * 24      : 
 * 25      : ~
 * 26      : ~
 * 27      : ~
 * ------------------------------------------------------------
 * 28      : ~
 * 29      : ~
 * 30      : ~
 * 31      : ~
 * ------------------------------------------------------------
 * 32      : Depot Generate Time
 * 33      : ~
 * 34      : ~
 * 35      : ~
 * ------------------------------------------------------------
 * 36      : ~
 * 37      : ~
 * 38      : ~
 * 39      : ~
 * ------------------------------------------------------------
 * 40      : Payload Size
 * 41      : ~
 * 42      : ~
 * 43      : ~
 * ------------------------------------------------------------
 * 44      : ~
 * 45      : ~
 * 46      : ~
 * 47      : ~
 * ------------------------------------------------------------
 * PAYLOAD (OPTIONAL)
 * ------------------------------------------------------------
 * FILE NAME MAP (OPTIONAL)
 * ------------------------------------------------------------
 */

[Flags]
public enum NetworkTransmissionFeatureFlag: byte
{
    FileMapIsJson = 1 << 0,
    WithFileMap = 1 << 1,
    WithPayload = 1 << 2,
    CompressFileMap = 1 << 7,
}

[Serializable, StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 4, Size = 48)]
public struct NetworkDepotHeaderV1
{
    [FieldOffset(0)] public byte Version;
    
    [FieldOffset(1)] public NetworkTransmissionFeatureFlag NetworkTransmissionFeature;
    
    [FieldOffset(8)] public ulong FileMapStringSize;

    [FieldOffset(16)] public ulong Md5ChecksumLower;
    
    [FieldOffset(24)] public ulong Md5ChecksumUpper;
    
    [FieldOffset(32)] public ulong GenerateTime;
    
    [FieldOffset(40)] public ulong PayloadSize;

}
