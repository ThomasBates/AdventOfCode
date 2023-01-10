using System;
using System.IO;

namespace AoC.Grammar;

public interface IGrammarParser
{
	#region IGrammarParser Events

	event EventHandler<GrammarEmitEventArgs> OnValueEmitted;

    event EventHandler<GrammarEmitEventArgs> OnTokenEmitted;

    event EventHandler<GrammarLogEventArgs> OnLogMessageEmitted;

	#endregion
	#region IGrammarParser Methods

	void ParseInput(string input, string startToken = "");

    void ParseInput(Stream input, string startToken = "");

    #endregion
}
