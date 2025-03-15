namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface to describe a place to store depots and how they connected with each other with write support.
/// </summary>
public interface IRepository : IReadonlyRepository
{
    public IWorkspace Workspace { get; }
    
    public IOccupationChart OccupationChart { get; }
    
    
    public uint GetActiveCommitId();

    public RepositoryCommit SubmitWorkspace();
    
    public void SyncOccupationChart();
    
    
    public Task GetActiveCommitIdAsync(CancellationToken cancellationToken = default);
    
    public Task<RepositoryCommit> SubmitWorkspaceAsync(CancellationToken cancellationToken = default);
    
    public Task SyncOccupationChartAsync(CancellationToken cancellationToken = default);
}