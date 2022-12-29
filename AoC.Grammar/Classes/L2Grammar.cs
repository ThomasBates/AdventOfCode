using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC.Grammar;

public class L2Grammar : IL2Grammar
{
    private const string EOF = "EOF";
    private const string EndOfFile = "End of file";

    private readonly Dictionary<string, string> definitions = new();
    private readonly Dictionary<string, string> patterns = new();
    private readonly Dictionary<string, List<string>> grammar = new();

    private readonly List<string> intrinsics = new();
    private readonly List<string> keywords = new();
    private readonly List<string> symbols = new();

    private readonly List<string> parseTokens = new();
    private readonly List<string> codeTokens = new();
    private readonly List<string> allTokens = new();

    private readonly List<int> firstProdList = new();
    private readonly List<int> firstTokenList = new();
    private List<string>[] _directors;

    private readonly Stack<int> directorParseTokenStack = new();
    private readonly Stack<int> followerParseTokenStack = new();

    #region Constructors

    public L2Grammar()
    {
        AddGrammarDefinition(EOF, EndOfFile, EOF);
    }

    #endregion
    #region IGrammar Events

    public event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

	#endregion IGrammar Events
	#region IL2Grammar Properties

	public Dictionary<string, string> Definitions => definitions;

	public Dictionary<string, string> Patterns => patterns;

	public List<string> Intrinsics => intrinsics;

	public List<string> Keywords => keywords;

	public List<string> Symbols => symbols;

	public List<string> ParseTokens => parseTokens;

	public List<string> CodeTokens => codeTokens;

	public List<string> AllTokens => allTokens;

	public List<int> FirstProdList => firstProdList;

	public List<int> FirstTokenList => firstTokenList;

	public List<string>[] Directors => _directors;

	#endregion IL2Grammar Properties
	#region IGrammar Methods

	public void ReadGrammarDefinition(string grammarDefinition)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(grammarDefinition);
        var stream = new MemoryStream(buffer);
        ReadGrammarStream(stream);
    }

	#endregion IGrammar Methods
	#region Private Methods

	private void ReadGrammarStream(Stream grammarStream)
    {
        bool fDefinitions = false;
        bool fGrammar = false;
        bool fFinished = false;

        grammarStream.Position = 0;
        string line = ReadGrammarLine(grammarStream);
        while (!fFinished)
        {
            if (line.Equals("#DEFINITIONS", StringComparison.OrdinalIgnoreCase))
            {
                fDefinitions = true;
            }
            else if (line.Equals("#GRAMMAR", StringComparison.OrdinalIgnoreCase))
            {
                fGrammar = true;
            }
            else if (line.Equals("#END", StringComparison.OrdinalIgnoreCase))
            {
                fFinished = true;
            }
            else if (fGrammar)
            {
                AddGrammarRule(line);
            }
            else if (fDefinitions)
            {
                AddGrammarDefinition(line);
            }
            if (grammarStream.Position >= grammarStream.Length)
            {
                fFinished = true;
            }

            line = ReadGrammarLine(grammarStream);
        }

        InitializeGrammar();
    }

    private void AddGrammarDefinition(string definition)
    {
        string[] parts = definition.Split(new string[] { " = " }, StringSplitOptions.None);
        if (parts.Length == 2)
        {
            AddGrammarDefinition(parts[0], parts[1]);
        }
    }

	private void AddGrammarDefinition(string definitionName, string expression)
    {
        string[] parts = expression.Split(new string[] { " | " }, StringSplitOptions.None);
        if (parts.Length == 2)
        {
            AddGrammarDefinition(definitionName, parts[0], parts[1]);
        }
    }

	private void AddGrammarDefinition(string definitionName, string description, string pattern)
    {
        if (!definitions.ContainsKey(definitionName))
        {
            definitions.Add(definitionName, description);
            patterns.Add(definitionName, pattern);
            intrinsics.Add(definitionName);
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
        if (!grammar.ContainsKey(ruleName))
        {
            grammar.Add(ruleName, new List<string>());
        }

        if (expression.StartsWith("|"))
        {
            expression = " " + expression;
        }

        string[] rules = expression.Split(new string[] { " | " }, StringSplitOptions.None);

        for (int i = 0; i < rules.Length; i++)
        {
            string rule = rules[i].Trim();

            if (!grammar[ruleName].Contains(rule))
            {
                grammar[ruleName].Add(rule);
            }
        }
    }

	private void InitializeGrammar()
    {
        foreach (string parseToken in grammar.Keys)
        {
            if (!parseTokens.Contains(parseToken))
            {
                parseTokens.Add(parseToken);
            }
        }

        foreach (string parseToken in grammar.Keys)
        {
            firstProdList.Add(firstTokenList.Count);

            foreach (string rule in grammar[parseToken])
            {
                bool newProduction = true;

                string[] tokens = rule.Split(' ');
                foreach (string token in tokens)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        bool fTerminal = token.StartsWith("'") ||
                                         token.StartsWith("\"");

                        if (fTerminal)
                        {
                            if (char.IsLetter(token[1]) || token[1] == '_')
                            {
                                if (!keywords.Contains(token))
                                {
                                    keywords.Add(token);
                                }
                            }
                            else if (!char.IsNumber(token[1]))
                            {
                                if (!symbols.Contains(token))
                                {
                                    symbols.Add(token);
                                }
                            }
                        }
                        else if (!intrinsics.Contains(token) && !parseTokens.Contains(token))
                        {
                            if (!codeTokens.Contains(token))
                            {
                                codeTokens.Add(token);
                            }
                        }
                    }
                    if (newProduction)
                    {
                        firstTokenList.Add(allTokens.Count);
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        allTokens.Add(token);
                    }
                    newProduction = false;
                }
            }
        }
        firstProdList.Add(firstTokenList.Count);
        firstTokenList.Add(allTokens.Count);

        //  Strip quotation marks from keywords and symbols and all tokens list.
        for (int i = 0; i < keywords.Count; i++)
        {
            string keyword = keywords[i];
            if ((keyword[0] == '"' && keyword[keyword.Length - 1] == '"') ||
                    (keyword[0] == '\'' && keyword[keyword.Length - 1] == '\''))
            {
                keyword = keyword.Substring(1, keyword.Length - 2);
                keywords[i] = keyword;
            }
        }

        for (int i = 0; i < symbols.Count; i++)
        {
            string symbol = symbols[i];
            if ((symbol[0] == '"' && symbol[symbol.Length - 1] == '"') ||
                    (symbol[0] == '\'' && symbol[symbol.Length - 1] == '\''))
            {
                symbol = symbol.Substring(1, symbol.Length - 2);
                symbols[i] = symbol;
            }
        }

        for (int i = 0; i < allTokens.Count; i++)
        {
            string token = allTokens[i];
            if ((token[0] == '"' && token[token.Length - 1] == '"') ||
                (token[0] == '\'' && token[token.Length - 1] == '\''))
            {
                token = token.Substring(1, token.Length - 2);
                allTokens[i] = token;
            }
        }

        //  Sort keywords, symbols and code tokens.
        keywords.Sort();
        symbols.Sort();
        codeTokens.Sort();

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

        _directors = new List<string>[firstTokenList.Count];
        for (int i = 0; i < firstTokenList.Count; i++)
        {
            _directors[i] = new List<string>();
        }

        for (int i = 0; i < parseTokens.Count; i++)
        {
            CalcParseDirectors(i);
        }

        DebugUnindent();
        DebugPrint("Grammar1", "... CalculateDirectors");
    }

	private void CalcParseDirectors(int parseTokenIndex)
    {
        DebugPrint("Grammar1", $"CalcParseDirectors({parseTokens[parseTokenIndex]}) ...");
        DebugIndent();

        DebugShowStack("if (_directorParseTokenStack.Contains(parseTokenIndex))", "_directorParseTokenStack", directorParseTokenStack);

        if (directorParseTokenStack.Contains(parseTokenIndex))
        {
            throw new GrammarException("Grammar is not L2");
        }

        directorParseTokenStack.Push(parseTokenIndex);
        DebugShowStack("_directorParseTokenStack.Push(parseTokenIndex);", "_directorParseTokenStack", directorParseTokenStack);

        for (int i = firstProdList[parseTokenIndex]; i < firstProdList[parseTokenIndex + 1]; i++)
        {
            if (_directors[i].Count == 0)
            {
                for (int j = firstTokenList[i]; j < firstTokenList[i + 1]; j++)
                {
                    string token = allTokens[j];
                    bool fTerminal = (intrinsics.Contains(token) || keywords.Contains(token) || symbols.Contains(token));
                    if (fTerminal)
                    {
                        _directors[i].Add(token);
                        break;
                    }
                    else if (!string.IsNullOrEmpty(token) && token[1] != '_')
                    {
                        int index = parseTokens.IndexOf(token);
                        AddDirectors(i, index);
                        break;
                    }
                }

                if (_directors[i].Count == 0)
                {
                    AddFollowers(i, parseTokenIndex);
                }
            }
        }

        DebugShowStack("if (_directorParseTokenStack.Pop() != parseTokenIndex)", "_directorParseTokenStack", directorParseTokenStack);
        if (directorParseTokenStack.Pop() != parseTokenIndex)
        {
            throw new GrammarException("Internal Stack Error.");
        }

        DebugUnindent();
        DebugPrint("Grammar1", "... CalcParseDirectors({_parseTokens[parseTokenIndex]})");
    }

	private void AddDirectors(int productionIndex, int parseTokenIndex)
    {
        DebugPrint("Grammar1", $"AddFollowers({productionIndex}, {parseTokens[parseTokenIndex]}) ...");
        DebugIndent();

        //  Make sure the specified parse token's director set has been calculated.
        CalcParseDirectors(parseTokenIndex);

        //  For each production of the specified parse token ...
        for (int i = firstProdList[parseTokenIndex]; i < firstProdList[parseTokenIndex + 1]; i++)
        {
            //  For each existing director for the current production ...
            foreach (string token in _directors[i])
            {
                //  If the specified production's directors do not include
                //  the director from the specified parse token, then add it.
                if (!_directors[productionIndex].Contains(token))
                {
                    _directors[productionIndex].Add(token);
                }
            }
        }

        DebugUnindent();
        DebugPrint("Grammar1", $"... AddDirectors({productionIndex}, {parseTokens[parseTokenIndex]})");
    }

	private void AddFollowers(int productionIndex, int parseTokenIndex)
    {
        DebugPrint("Grammar1", $"AddFollowers({productionIndex}, {parseTokens[parseTokenIndex]}) ...");
        DebugIndent();
        try
        {
            DebugShowStack("if (_followerParseTokenStack.Contains(parseTokenIndex))", "_followerParseTokenStack", followerParseTokenStack);
            if (followerParseTokenStack.Contains(parseTokenIndex))
            {
                return;
            }

            string parseToken = parseTokens[parseTokenIndex];

            if (parseTokenIndex == 0)
            {
                string token = EOF;
                if (!_directors[productionIndex].Contains(token))
                {
                    _directors[productionIndex].Add(token);
                }
                return;
            }

            followerParseTokenStack.Push(parseTokenIndex);
            DebugShowStack("_followerParseTokenStack.Push(parseTokenIndex);", "_followerParseTokenStack", followerParseTokenStack);

            for (int i = 0; i < parseTokens.Count; i++)
            {
                for (int j = firstProdList[i]; j < firstProdList[i + 1]; j++)
                {
                    bool fInProd = false;
                    bool fFollowed = false;

                    for (int k = firstTokenList[j]; k < firstTokenList[j + 1]; k++)
                    {
                        string token = allTokens[k];

                        if (fInProd)
                        {
                            bool fTerminal = (intrinsics.Contains(token) || keywords.Contains(token) || symbols.Contains(token));
                            if (fTerminal)
                            {
                                if (!_directors[productionIndex].Contains(token))
                                {
                                    _directors[productionIndex].Add(token);
                                }
                                fFollowed = true;
                                break;
                            }
                            else if (!string.IsNullOrEmpty(token) && !codeTokens.Contains(token))// token[1] != '_')
                            {
                                int index = parseTokens.IndexOf(token);
                                AddDirectors(productionIndex, index);
                                fFollowed = true;
                                break;
                            }
                        }
                        if (allTokens[k].Equals(parseToken))
                        {
                            fInProd = true;
                        }
                    }
                    if (fInProd && !fFollowed)
                    {
                        AddFollowers(productionIndex, i);
                    }
                }
            }

            DebugShowStack("if (_followerParseTokenStack.Pop() != parseTokenIndex)", "_followerParseTokenStack", followerParseTokenStack);
            if (followerParseTokenStack.Pop() != parseTokenIndex)
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
        OnLogMessageEmitted?.Invoke(this, new ParserLogEventArgs("Debug", category, indent + message));
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

    private void DebugShowStack(string caption, string stackName, Stack<int> stack)
    {
        if (OnLogMessageEmitted != null)
        {
            string tokens = string.Empty;
            int[] indeces = stack.ToArray();
            foreach (int index in indeces)
            {
                string token = parseTokens[index];
                if (!string.IsNullOrEmpty(tokens)) tokens = ", " + tokens;
                tokens = token + tokens;
            }
            DebugPrint("Grammar1", caption);
            DebugPrint("Grammar1", "  " + stackName + ":  " + tokens);
        }
    }

    private void DebugDumpLists()
    {
		if (OnLogMessageEmitted != null)
		{
			DebugDumpList("First the standard definitions", intrinsics);
            DebugDumpList("Then the reserved (key)words in alphabetical order", keywords);
            DebugDumpList("Then the others in random order", symbols);
            DebugDumpList("Grammar Parse Tokens", parseTokens);
            DebugDumpList("Grammar Code Tokens", codeTokens);
            DebugDumpList("For each parse token, the first production number is as follows:", firstProdList);

            DebugPrint("Grammar2", "Director Sets");
            for (int i = 0; i < _directors.Length; i++)
            {
                DebugDumpList(i, _directors[i]);
            }
            DebugPrint("Grammar2", "");

            DebugDumpList("For each production, the first token is in the following position:", firstTokenList);
        }
    }

    private void DebugDumpList(string caption, List<string> list)
    {
        DebugPrint("Grammar2", caption);
        foreach (string token in list)
        {
            DebugPrint("Grammar2", "  " + token);
        }
        DebugPrint("Grammar2", "");
    }

    private void DebugDumpList(string caption, List<int> list)
    {
        DebugPrint("Grammar2", caption);
        string line = string.Empty;
        foreach (int index in list)
        {
            if (!string.IsNullOrEmpty(line)) line += ", ";
            line += index.ToString();
        }
        DebugPrint("Grammar2", "  " + line);
        DebugPrint("Grammar2", "");
    }

    private void DebugDumpList(int index, List<string> list)
    {
        string line = string.Empty;
        foreach (string token in list)
        {
            if (!string.IsNullOrEmpty(line)) line += ", ";
            line += "\"" + token + "\"";
        }
        DebugPrint("Grammar2", "  " + index + ":  " + line);
    }

    #endregion Private Debug Methods
}
