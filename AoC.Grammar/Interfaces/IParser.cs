using System;
using System.IO;

namespace AoC.Grammar;

public interface IParser
{
    #region IParser Events

    event EventHandler<ParserEventArgs> OnValueEmitted;

    event EventHandler<ParserEventArgs> OnTokenEmitted;

    event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

    #endregion
    #region IParser Methods

    void Parse(string input);

    void Parse(Stream input);

    #endregion
}
