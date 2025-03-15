namespace Flawless.Core.Modal;

public record class CommitManifest(ulong ManifestId, DepotLabel Depot, string[] FilePaths);