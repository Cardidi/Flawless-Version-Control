using Flawless.Core.Modal;

namespace Flawless.Core.Interface;

public interface IRepositoryCommit
{
    public IReadonlyRepository Repository { get; }
    
    public UInt64 CommitId { get; }
    
    public Author Author { get; }
    
    public DateTime CommitTime { get; }

    public string Message { get; }

    public IRepositoryCommit? GetParentCommit();
    
    public IRepositoryCommit? GetChildCommit();
    
    public ValueTask<CommitManifest> GetManifest(CancellationToken cancellationToken = default);
}