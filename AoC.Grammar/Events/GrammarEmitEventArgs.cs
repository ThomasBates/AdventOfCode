using System;

namespace AoC.Grammar;

public class GrammarEmitEventArgs : EventArgs
{
	public GrammarEmitEventArgs(string token, string value)
	{
		Token = token;
		Value = value;
	}

	public string Token { get; }

	public string Value { get; }
}