namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface for any platform to describe data block (Not the actual data) in repository. 
/// </summary>
public interface IDepotLabel
{
    public abstract HashId Id { get; }
    
    public IEnumerable<HashId> Dependencies { get; }
}