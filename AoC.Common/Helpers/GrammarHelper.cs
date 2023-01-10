using System;
using System.Collections.Generic;

using AoC.Common.Logger;
using AoC.Grammar;

namespace AoC.Common.Helpers;

public class GrammarHelper
{
	public static GrammarData CreateGrammar(ILogger logger, string grammarFile)
	{
		IGrammarReader grammarReader = new L2GrammarReader();

		if (logger != null)
			grammarReader.OnLogMessageEmitted += (sender, e) => HandleLogMessageEmitted(logger, e);

		try
		{
			var grammar = grammarReader.ReadGrammarDefinition(grammarFile);
			return grammar;
		}
		catch (GrammarException ex)
		{
			logger?.SendError("Grammar", $"{ex.GetType().Name}: {ex.Message}");
			return null;
		}
	}

	public static IGrammarParser CreateParser(
		ILogger logger,
		string grammarFile,
		Action<string, Stack<string>> scopeControllerAction,
		Action<string, Stack<string>> typeCheckerAction,
		Action<string, Stack<string>> codeGeneratorAction)
	{
		var grammar = CreateGrammar(logger, grammarFile);

		if (grammar == null)
			return null;

		return CreateParser(logger, grammar, scopeControllerAction, typeCheckerAction, codeGeneratorAction);
	}

	public static IGrammarParser CreateParser(
		ILogger logger,
		GrammarData grammar,
		Action<string, Stack<string>> scopeControllerAction,
		Action<string, Stack<string>> typeCheckerAction,
		Action<string, Stack<string>> codeGeneratorAction)
	{
		var valueStack = new Stack<string>();

		IGrammarParser parser = new GrammarParser(grammar);

		if (logger != null)
			parser.OnLogMessageEmitted += (sender, e) => HandleLogMessageEmitted(logger, e);

		parser.OnValueEmitted += (sender, e) => valueStack.Push(e.Value);

		parser.OnTokenEmitted += (sender, e) =>
		{
			if (string.IsNullOrEmpty(e.Token))
				return;

			switch (e.Token[0])
			{
				case 's': scopeControllerAction?.Invoke(e.Token, valueStack); break;
				case 't': typeCheckerAction?.Invoke(e.Token, valueStack); break;
				case 'c': codeGeneratorAction?.Invoke(e.Token, valueStack); break;
				default:
					logger?.SendError("Parser", $"Unknown token: {e.Token}");
					break;
			}
		};

		return parser;
	}

	public static bool ParseInput(
		ILogger logger,
		string input,
		string grammarFile,
		Action<string, Stack<string>> scopeControllerAction,
		Action<string, Stack<string>> typeCheckerAction,
		Action<string, Stack<string>> codeGeneratorAction)
	{
		IGrammarParser parser = CreateParser(logger, grammarFile, scopeControllerAction, typeCheckerAction, codeGeneratorAction);
		return ParseInput(logger, parser, input);
	}

	public static bool ParseInput(
		ILogger logger,
		IGrammarParser parser,
		string input)
	{
		if (parser == null)
			return false;

		try
		{
			parser.ParseInput(input);
		}
		catch (ParserException ex)
		{
			logger?.SendError("Parser", $"{ex.GetType().Name}: {ex.Message}");
			return false;
		}

		return true;
	}

	public static bool ParseInput(
		ILogger logger,
		string input,
		GrammarData grammar,
		Action<string, Stack<string>> scopeControllerAction,
		Action<string, Stack<string>> typeCheckerAction,
		Action<string, Stack<string>> codeGeneratorAction)
	{
		IGrammarParser parser = CreateParser(logger, grammar, scopeControllerAction, typeCheckerAction, codeGeneratorAction);
		return ParseInput(logger, parser, input);
	}

	private static void HandleLogMessageEmitted(ILogger logger, GrammarLogEventArgs e)
	{
		if (logger == null)
			return;

		switch (e.Severity.ToLower())
		{
			case "error":
				logger.SendError(e.Category, e.Message);
				break;
			case "warning":
				logger.SendWarning(e.Category, e.Message);
				break;
			case "info":
				logger.SendInfo(e.Category, e.Message);
				break;
			case "debug":
				logger.SendDebug(e.Category, e.Message);
				break;
			case "verbose":
				logger.SendVerbose(e.Category, e.Message);
				break;
		}
	}
}
