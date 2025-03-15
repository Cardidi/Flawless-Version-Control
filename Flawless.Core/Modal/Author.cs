namespace Flawless.Core.Modal;

/// <summary>
/// An author setup to indicate who create a depot or identify a depot author when uploading it.
/// </summary>
[Serializable]
public record struct Author(string Name, string Email);