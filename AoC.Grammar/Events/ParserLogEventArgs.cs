using System;

namespace AoC.Grammar;

public class ParserLogEventArgs : EventArgs
{
    public ParserLogEventArgs(string severity, string category, string message)
    {
        Severity = severity;
        Category = category;
        Message = message;
    }

    public string Severity { get; }

    public string Category { get; }

    public string Message { get; }
}