namespace Flawless.Abstraction;

/// <summary>
/// An author setup to indicate who create a depot or identify a depot author when uploading it.
/// </summary>
public readonly struct Author : IEquatable<Author>
{
    public readonly string Name;
    
    public readonly string Email;

    public Author(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public bool Equals(Author other)
    {
        return Name == other.Name && Email == other.Email;
    }

    public override bool Equals(object? obj)
    {
        return obj is Author other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Email);
    }
}