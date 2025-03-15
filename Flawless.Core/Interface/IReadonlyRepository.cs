namespace Flawless.Core.Interface;

/// <summary>
/// Standardized interface to describe a place to store depots and how they connected with each other.
/// </summary>
public interface IReadonlyRepository
{
    public bool IsReadonly { get; }

    public IEnumerable<IRepositoryCommit> GetCommits();
    
    public IRepositoryCommit? GetCommitById(uint commitId);
    
    public IAsyncEnumerable<IRepositoryCommit> GetCommitsAsync(CancellationToken cancellationToken = default);
    
    public Task<IRepositoryCommit?> GetCommitByIdAsync(uint commitId, CancellationToken cancellationToken = default);

}