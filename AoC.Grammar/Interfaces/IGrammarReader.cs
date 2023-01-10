using System;

namespace AoC.Grammar;

public interface IGrammarReader
{
	public event EventHandler<GrammarLogEventArgs> OnLogMessageEmitted;

	GrammarData ReadGrammarDefinition(string grammarDefinition);
}
