namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface for depot to represent a depot inner data handles.
/// </summary>
public interface IDepotConnection : IDisposable

{
    public IReadonlyRepository Repository { get; }
    
    public IDepotLabel Label { get; }
    
    public IDepotStorage Storage { get; }
}