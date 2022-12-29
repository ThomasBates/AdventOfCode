using System;
using System.Runtime.Serialization;

namespace AoC.Grammar;

public class ParserException : Exception
{
    public ParserException() : base() { }

    public ParserException(string message) : base(message) { }

    protected ParserException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public ParserException(string message, Exception innerException) : base(message, innerException) { }
}
