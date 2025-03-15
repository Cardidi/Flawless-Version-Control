namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface to describe a place to store depots and how they connected with each other with write support.
/// </summary>
public interface IRepository : IReadonlyRepository
{
    public uint GetActiveCommitId();
    
    public Task GetActiveCommitIdAsync(CancellationToken cancellationToken = default);
}