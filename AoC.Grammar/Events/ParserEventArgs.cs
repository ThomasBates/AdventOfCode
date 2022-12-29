using System;

namespace AoC.Grammar;

public class ParserEventArgs : EventArgs
{
	public ParserEventArgs(string token, string value)
	{
		Token = token;
		Value = value;
	}

	public string Token { get; }

	public string Value { get; }
}