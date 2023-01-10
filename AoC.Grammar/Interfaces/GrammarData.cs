using System.Collections.Generic;

namespace AoC.Grammar;

public class Production
{
	public List<string> Tokens { get; set; } = new();
	public HashSet<string> Directors { get; set; } = new();
}

public class GrammarData
{
	public string StartToken { get; set; }

	public string EndToken { get; set; }

	public Dictionary<string, string> Intrinsics { get; } = new();

	public HashSet<string> Terminals { get; } = new();

	public HashSet<string> CodeTokens { get; } = new();

	public Dictionary<string, HashSet<Production>> ParseTokens { get; } = new();
}
