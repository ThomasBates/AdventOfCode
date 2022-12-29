using System.Collections.Generic;

namespace AoC.Grammar;

public interface IL2Grammar : IGrammar
{
	public Dictionary<string, string> Definitions { get; }

	public Dictionary<string, string> Patterns { get; }

	public List<string> Intrinsics { get; }

	public List<string> Keywords { get; }

	public List<string> Symbols { get; }

	public List<string> ParseTokens { get; }

	public List<string> CodeTokens { get; }

	public List<string> AllTokens { get; }

	public List<int> FirstProdList { get; }

	public List<int> FirstTokenList { get; }

	public List<string>[] Directors { get; }
}
