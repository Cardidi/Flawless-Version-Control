using System.Security.Cryptography;

namespace Flawless.Abstraction;

/// <summary>
/// An MD5 based hash code storage.
/// </summary>
[Serializable]
public record struct HashId
{
    public static HashId Empty => new(0);
    
    private readonly UInt128 _hash;

    public HashId(UInt128 hash)
    {
        _hash = hash;
    }

    public HashId(ulong upper, ulong lower)
    {
        _hash = new UInt128(upper, lower);
    }

    public bool Equals(HashId other)
    {
        return _hash == other._hash;
    }

    public override int GetHashCode()
    {
        return _hash.GetHashCode();
    }
}

public static class HashIdExtensions
{
    public static HashId ToHashId(Stream input)
    {
        UInt128 tmp = 0;
        var d = MD5.HashData(input);
        for (var i = 0; i < d.Length && i < MD5.HashSizeInBytes; i++)
        {
            UInt128 adder = d[i];
            tmp += adder << (i * 8);
        }
        
        return new HashId(tmp);
    }
}