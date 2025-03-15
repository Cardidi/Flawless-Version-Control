using Flawless.Abstraction;

namespace Flawless.Core.Modal;

[Serializable]
public record struct DepotLabel(HashId Id, HashId[] Baselines);