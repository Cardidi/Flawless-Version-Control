namespace Flawless.Abstraction;

public abstract class RepositoryCommit
{
    public abstract IReadonlyRepository Repository { get; }
    
    public abstract UInt64 Id { get; }
    
    public abstract Author Author { get; }
    
    public abstract DateTime CommitTime { get; }

    public abstract string Message { get; }
    
    public abstract IDepotLabel Depot { get; }

    public abstract RepositoryCommit? GetParentCommit();
    
    public abstract RepositoryCommit? GetChildCommit();
    
}