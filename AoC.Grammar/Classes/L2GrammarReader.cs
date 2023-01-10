using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Grammar;

public class L2GrammarReader : IGrammarReader
{
    private const string EOF = "EOF";

    private readonly Dictionary<string, List<string>> rules = new();

    private readonly Stack<string> directorParseTokenStack = new();
    private readonly Stack<string> followerParseTokenStack = new();

	private GrammarData grammar;

	#region IGrammarReader Events

	public event EventHandler<GrammarLogEventArgs> OnLogMessageEmitted;

	#endregion IGrammarReader Events
	#region IGrammarReader Methods

	public GrammarData ReadGrammarDefinition(string grammarDefinition)
    {
        rules.Clear();
        directorParseTokenStack.Clear();
        followerParseTokenStack.Clear();
        grammar = new GrammarData();

		AddGrammarDefinition(EOF, EOF);

		bool doDefinitions = false;
        bool doGrammar = false;
        bool isFinished = false;

		byte[] buffer = Encoding.UTF8.GetBytes(grammarDefinition);
        var grammarStream = new MemoryStream(buffer) { Position = 0 };

		string line = ReadGrammarLine(grammarStream);
        while (!isFinished)
        {
            if (line.Equals("#DEFINITIONS", StringComparison.OrdinalIgnoreCase))
            {
                doDefinitions = true;
            }
            else if (line.Equals("#GRAMMAR", StringComparison.OrdinalIgnoreCase))
            {
                doGrammar = true;
            }
            else if (line.Equals("#END", StringComparison.OrdinalIgnoreCase))
            {
                isFinished = true;
            }
            else if (doGrammar)
            {
                AddGrammarRule(line);
            }
            else if (doDefinitions)
            {
                AddGrammarDefinition(line);
            }
            if (grammarStream.Position >= grammarStream.Length)
            {
                isFinished = true;
            }

            line = ReadGrammarLine(grammarStream);
        }

        InitializeGrammar();

        return grammar;
    }

	#endregion IGrammarReader Methods
	#region Private Methods

	private void AddGrammarDefinition(string definition)
    {
        string[] parts = definition.Split(new string[] { " = ", " | " }, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 2)
		{
			AddGrammarDefinition(parts[0], parts[1]);
		}
		else if (parts.Length == 3)
		{
			AddGrammarDefinition(parts[0], parts[2]);
		}
	}

	private void AddGrammarDefinition(string definitionName, string pattern)
    {
        if (!grammar.Intrinsics.ContainsKey(definitionName))
        {
            grammar.Intrinsics.Add(definitionName, pattern);
            if (string.IsNullOrEmpty(grammar.EndToken))
                grammar.EndToken = definitionName;
        }
    }

	private void AddGrammarRule(string rule)
    {
        string[] parts = rule.Split(new string[] { " = " }, StringSplitOptions.None);
        if (parts.Length == 2)
        {
            AddGrammarRule(parts[0], parts[1]);
        }
    }

	private void AddGrammarRule(string ruleName, string expression)
    {
        if (string.IsNullOrEmpty(grammar.StartToken))
            grammar.StartToken = ruleName;

        if (!this.rules.ContainsKey(ruleName))
        {
            this.rules.Add(ruleName, new List<string>());
        }

        if (expression.StartsWith("|"))
        {
            expression = " " + expression;
        }

        string[] rules = expression.Split(new string[] { " | " }, StringSplitOptions.None);

        for (int i = 0; i < rules.Length; i++)
        {
            string rule = rules[i].Trim();

            if (!this.rules[ruleName].Contains(rule))
            {
                this.rules[ruleName].Add(rule);
            }
        }
    }

	private void InitializeGrammar()
    {
        foreach (string parseToken in rules.Keys)
            grammar.ParseTokens.Add(parseToken, new HashSet<Production>());

        foreach (string parseToken in rules.Keys)
        {
            foreach (string rule in rules[parseToken])
            {
                var production = new Production();
                grammar.ParseTokens[parseToken].Add(production);

                string[] tokens = rule.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in tokens)
                {
                    bool fTerminal = token.StartsWith("'") ||
                                     token.StartsWith("\"");

                    if (fTerminal)
                    {
                        //  Strip quotation marks from keywords and symbols and production tokens list.
                        var stripped = token;
                        if ((token[0] == '"' && token[token.Length - 1] == '"') ||
                            (token[0] == '\'' && token[token.Length - 1] == '\''))
                        {
                            stripped = token.Substring(1, token.Length - 2);
                        }

                        //if (char.IsLetter(stripped[0]) || stripped[0] == '_' || !char.IsNumber(stripped[0]))
                        grammar.Terminals.Add(stripped);
                        production.Tokens.Add(stripped);
                    }
                    else if (!grammar.Intrinsics.ContainsKey(token) &&
                             !grammar.ParseTokens.ContainsKey(token))
                    {
                        grammar.CodeTokens.Add(token);
                        production.Tokens.Add(token);
                    }
                    else // parse token or intrinsic
                    {
                        production.Tokens.Add(token);
                    }
                }
            }
        }

        CalculateDirectors();

        DebugDumpLists();
    }

	private string ReadGrammarLine(Stream grammar)
    {
        const int TAB = 9;
        const int SPACE = ' ';
        const int CR = 13;
        const int LF = 10;
        const int EQUALS = '=';
        const int PIPE = '|';
        const int QUOTE1 = '\'';
        const int QUOTE2 = '"';

        string line = string.Empty;
        bool inQuotes1 = false;
        bool inQuotes2 = false;
        bool newLine = false;

        int code = grammar.ReadByte();
        while (code != -1)
        {
            char nextChar = (char)code;
            switch (code)
            {
                case TAB:
                case SPACE:
                    if (inQuotes1 || inQuotes2 || !line.EndsWith(" "))
                    {
                        line += " ";
                    }
                    break;

                case CR:    //  ignore
                    break;

                case LF:
                    newLine = true;
                    break;

                case EQUALS:
                case PIPE:
                    //if (!inQuotes1 && !inQuotes2 && !line.EndsWith(" "))
                    //{
                    //    line += " ";
                    //}
                    line += nextChar;
                    //if (!inQuotes1 && !inQuotes2)
                    //{
                    //    line += " ";
                    //}
                    newLine = false;
                    break;

                case QUOTE1:
                    if (newLine)
                    {
                        grammar.Position--;
                        return line;
                    }
                    if (!inQuotes2)
                    {
                        inQuotes1 = !inQuotes1;
                    }
                    line += nextChar;
                    break;

                case QUOTE2:
                    if (newLine)
                    {
                        grammar.Position--;
                        return line;
                    }
                    if (!inQuotes1)
                    {
                        inQuotes2 = !inQuotes2;
                    }
                    line += nextChar;
                    break;

                default:
                    if (newLine)
                    {
                        grammar.Position--;
                        return line;
                    }
                    line += nextChar;
                    break;
            }

            code = grammar.ReadByte();
        }
        return line;
    }

	#endregion Private Methods
	#region Director Calculation Methods

	private void CalculateDirectors()
    {
        DebugPrint("Grammar1", "CalculateDirectors ...");
        DebugIndent();

        foreach(var parseToken in grammar.ParseTokens.Keys)
        {
            CalcParseDirectors(parseToken);
        }

        DebugUnindent();
        DebugPrint("Grammar1", "... CalculateDirectors");
    }

	private void CalcParseDirectors(string parseToken)
    {
        DebugPrint("Grammar1", $"CalcParseDirectors({parseToken}) ...");
        DebugIndent();

        DebugShowStack($"if (directorParseTokenStack.Contains({parseToken}))", "directorParseTokenStack", directorParseTokenStack);
        if (directorParseTokenStack.Contains(parseToken))
        {
            throw new GrammarException("Grammar is not L2");
        }

        directorParseTokenStack.Push(parseToken);
        DebugShowStack($"directorParseTokenStack.Push({parseToken});", "directorParseTokenStack", directorParseTokenStack);

        var productions = grammar.ParseTokens[parseToken];

        foreach (var production in productions)
        {
            if (production.Directors.Count > 0)
                continue;

            foreach (var token in production.Tokens)
            {
                bool isTerminal = grammar.Intrinsics.ContainsKey(token) ||
                                  grammar.Terminals.Contains(token);
                if (isTerminal)
                {
                    production.Directors.Add(token);
                    break;
                }
                else if (grammar.ParseTokens.ContainsKey(token))
                {
                    AddDirectors(production.Directors, token);
					break;
				}
			}

            if (production.Directors.Count == 0)
                AddFollowers(production.Directors, parseToken);
        }

        DebugShowStack($"if (directorParseTokenStack.Pop() != {parseToken})", "directorParseTokenStack", directorParseTokenStack);
        if (directorParseTokenStack.Pop() != parseToken)
        {
            throw new GrammarException("Internal Stack Error.");
        }

        DebugUnindent();
        DebugPrint("Grammar1", $"... CalcParseDirectors({parseToken})");
    }

	private void AddDirectors(HashSet<string> directors, string parseToken)
    {
        DebugPrint("Grammar1", $"AddFollowers(directors, {parseToken}) ...");
        DebugIndent();

        //  Make sure the specified parse token's director set has been calculated.
        CalcParseDirectors(parseToken);

		//  For each production of the specified parse token ...
		foreach (var production in grammar.ParseTokens[parseToken])
            directors.UnionWith(production.Directors);

        DebugUnindent();
        DebugPrint("Grammar1", $"... AddDirectors(directors, {parseToken})");
    }

	private void AddFollowers(HashSet<string> directors, string parseToken)
    {
        DebugPrint("Grammar1", $"AddFollowers(directors, {parseToken}) ...");
        DebugIndent();
        try
        {
            DebugShowStack($"if (followerParseTokenStack.Contains({parseToken}))", "followerParseTokenStack", followerParseTokenStack);
            if (followerParseTokenStack.Contains(parseToken))
            {
                return;
            }

            if (parseToken == grammar.StartToken)
            {
                directors.Add(grammar.EndToken);
                return;
            }

            followerParseTokenStack.Push(parseToken);
            DebugShowStack($"followerParseTokenStack.Push({parseToken});", "followerParseTokenStack", followerParseTokenStack);

            foreach(var parseTokenEntry in grammar.ParseTokens)
            {
                var productions = parseTokenEntry.Value;
                foreach(var production in productions)
                {
                    bool fInProd = false;
                    bool fFollowed = false;

                    foreach (var token in production.Tokens)
                    {
                        if (fInProd)
                        {
							bool isTerminal = grammar.Intrinsics.ContainsKey(token) || 
                                              grammar.Terminals.Contains(token);
                            if (isTerminal)
                            {
                                directors.Add(token);
                                fFollowed = true;
                                break;
                            }
                            else if (!grammar.CodeTokens.Contains(token))
                            {
                                AddDirectors(directors, token);
                                fFollowed = true;
                                break;
                            }
                        }
                        if (token.Equals(parseToken))
                        {
                            fInProd = true;
                        }
                    }
                    if (fInProd && !fFollowed)
                    {
                        AddFollowers(directors, parseTokenEntry.Key);
                    }
                }
            }

            DebugShowStack($"if (followerParseTokenStack.Pop() != {parseToken})", "followerParseTokenStack", followerParseTokenStack);
            if (followerParseTokenStack.Pop() != parseToken)
            {
                throw new GrammarException("Internal Stack Error.");
            }
        }
        finally
        {
            DebugUnindent();
            DebugPrint("Grammar1", "... AddFollowers({productionIndex}, {_parseTokens[parseTokenIndex]})");
        }
    }


	#endregion Director Calculation Methods
	#region Private Debug Methods

	private string indent = "";

    private void DebugPrint(string category, string message)
    {
        OnLogMessageEmitted?.Invoke(this, new GrammarLogEventArgs("Verbose", category, indent + message));
    }

    private void DebugIndent()
    {
        indent = "  " + indent;
    }

    private void DebugUnindent()
    {
        if (indent.Length > 2)
            indent = indent.Substring(2);
        else
            indent = string.Empty;
    }

    private void DebugShowStack(string caption, string stackName, Stack<string> stack)
    {
		DebugPrint("Grammar1", caption);
		DebugPrint("Grammar1", $"  {stackName}:  {string.Join(", ", stack)}");
	}

	private void DebugDumpLists()
    {
		if (OnLogMessageEmitted != null)
		{
			DebugPrint("Grammar2", "");
			DebugDumpSet("Intrinsics", grammar.Intrinsics.Keys);
            DebugDumpSet("Terminals", grammar.Terminals);
            DebugDumpSet("Code Tokens", grammar.CodeTokens);

			DebugPrint("Grammar2", "Parse Tokens");
			foreach (var parseTokenEntry in grammar.ParseTokens)
            {
				DebugPrint("Grammar2", $"  {parseTokenEntry.Key}");
                foreach (var production in parseTokenEntry.Value)
                {
					DebugPrint("Grammar2", $"    ==> {string.Join(" ", production.Tokens)}");
					DebugPrint("Grammar2", $"        ({string.Join(", ", production.Directors)})");
                }
            }
            DebugPrint("Grammar2", "");
        }
    }

    private void DebugDumpSet(string caption, IEnumerable<string> list)
    {
        DebugPrint("Grammar2", caption);
        foreach (string token in list)
            DebugPrint("Grammar2", "  " + token);
        DebugPrint("Grammar2", "");
    }

    #endregion Private Debug Methods
}
