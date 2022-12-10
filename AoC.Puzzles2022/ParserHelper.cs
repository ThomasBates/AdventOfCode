using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.IO;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	public class ParserHelper
	{
		public static void RunParser(
			string input,
			StringBuilder output,
			string grammar,
			Action<string, Stack<string>> scopeControllerAction,
			Action<string, Stack<string>> typeCheckerAction,
			Action<string, Stack<string>> codeGeneratorAction)
		{
			var valueStack = new Stack<string>();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				try
				{
					_grammar.ReadGrammar(grammar);
				}
				catch (GrammarException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				try
				{
					_parser.Parse(input);
				}
				catch (ParserException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
			}

			void Parser_ValueEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"value emitted: {e.Value}");
				valueStack.Push(e.Value);
			}
			void Parser_TokenEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"token emitted: {e.Token}");

				if (string.IsNullOrEmpty(e.Token)) return;

				switch (e.Token[0])
				{
					case 's': scopeControllerAction?.Invoke(e.Token, valueStack); break;
					case 't': typeCheckerAction?.Invoke(e.Token, valueStack); break;
					case 'c': codeGeneratorAction(e.Token, valueStack); break;
				}
			}
		}
	}
}
