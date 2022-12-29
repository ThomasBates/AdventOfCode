using System;

namespace AoC.Grammar;

public interface IGrammar
{
	public event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

	void ReadGrammarDefinition(string grammarDefinition);
}
