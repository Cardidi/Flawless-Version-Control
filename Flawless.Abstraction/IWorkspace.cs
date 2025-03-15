namespace Flawless.Abstraction;

/// <summary>
/// Standardized interface for repository working area (Which means those changes are not committed yet.
/// </summary>
public interface IWorkspace
{
    public string Message { get; set; }
}