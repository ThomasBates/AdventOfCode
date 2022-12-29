using System;
using System.Runtime.Serialization;

namespace AoC.Grammar;

public class GrammarException : Exception
{
    public GrammarException() : base() { }

    public GrammarException(string message) : base(message) { }

    protected GrammarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public GrammarException(string message, Exception innerException) : base(message, innerException) { }
}
