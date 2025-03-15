using Flawless.Abstraction;

namespace Flawless.Core.Modal;

public class Depot
{
    public string RawDataPath { get; }

    public HashId DepotHash { get; }
    
    public byte Version { get; }

    public byte ChecksumConfuser { get; }
    
    public DateTime GenerateTime { get; }

    public CompressType CompressType { get; }
    
    public ulong PayloadSize { get; }
    
    public uint FileCount { get; }

    public DepotFileInfo[] Files { get; }
}