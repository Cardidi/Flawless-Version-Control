using System.Runtime.Serialization;

namespace Flawless.Abstraction.Exceptions;

public class FlawlessException : Exception
{
    public FlawlessException()
    {
    }

    protected FlawlessException(SerializationInfo info, StreamingContext context) : base(info, context) {}

    public FlawlessException(string? message) : base(message) {}

    public FlawlessException(string? message, Exception? innerException) : base(message, innerException) {}
}