﻿namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface to describe a place to store depots and how they connected with each other.
/// </summary>
public interface IReadonlyRepository
{
    public bool IsReadonly { get; }

    public IEnumerable<RepositoryCommit> GetCommits();
    
    public RepositoryCommit? GetCommitById(uint commitId);
    
    public IAsyncEnumerable<RepositoryCommit> GetCommitsAsync(CancellationToken cancellationToken = default);
    
    public Task<RepositoryCommit?> GetCommitByIdAsync(uint commitId, CancellationToken cancellationToken = default);

}