namespace Flawless.Core.Modal;

[Serializable]
public record struct DepotFileInfo(ulong Size, ulong Offset, string Path);