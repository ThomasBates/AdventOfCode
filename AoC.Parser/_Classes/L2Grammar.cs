using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC.Parser
{
    /// <summary>
    /// L2Grammar 
    /// </summary>
    /// <created>18/02/2009</created>
    /// <author>Thomas_Bates</author>
    public class L2Grammar : L2GrammarBase, IDisposable
    {
        private const string EOF = "EOF";
        private const string EndOfFile = "End of file";

        private Dictionary<string, string> _definitions = new Dictionary<string, string>();
        private Dictionary<string, string> _patterns = new Dictionary<string, string>();
        private Dictionary<string, List<string>> _grammar = new Dictionary<string, List<string>>();

        private List<string> _intrinsics = new List<string>();
        private List<string> _keywords = new List<string>();
        private List<string> _symbols = new List<string>();

        private List<string> _parseTokens = new List<string>();
        private List<string> _codeTokens = new List<string>();
        private List<string> _allTokens = new List<string>();

        private List<int> _firstProdList = new List<int>();
        private List<int> _firstTokenList = new List<int>();
        private List<string>[] _directors;

        private Stack<int> _directorParseTokenStack = new Stack<int>();
        private Stack<int> _followerParseTokenStack = new Stack<int>();


        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="L2Grammar"/> class.
        /// </summary>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2Grammar()
        {
            AddGrammarDefinition(EOF, EndOfFile, EOF);
        }


        #endregion
        #region Properties


        /// <summary>
        /// Gets the definitions.
        /// </summary>
        /// <value>The definitions.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public Dictionary<string, string> Definitions
        {
            get { return _definitions; }
        }

        /// <summary>
        /// Gets the regular-expression patterns for the intrinsic tokens.
        /// </summary>
        /// <value>The patterns.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public Dictionary<string, string> Patterns
        {
            get { return _patterns; }
        }

        /// <summary>
        /// Gets the intrinsics.
        /// </summary>
        /// <value>The intrinsics.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> Intrinsics
        {
            get { return _intrinsics; }
        }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> Keywords
        {
            get { return _keywords; }
        }

        /// <summary>
        /// Gets the symbols.
        /// </summary>
        /// <value>The symbols.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> Symbols
        {
            get { return _symbols; }
        }

        /// <summary>
        /// Gets the parse tokens.
        /// </summary>
        /// <value>The parse tokens.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> ParseTokens
        {
            get { return _parseTokens; }
        }

        /// <summary>
        /// Gets the code tokens.
        /// </summary>
        /// <value>The code tokens.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> CodeTokens
        {
            get { return _codeTokens; }
        }

        /// <summary>
        /// Gets all tokens.
        /// </summary>
        /// <value>All tokens.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string> AllTokens
        {
            get { return _allTokens; }
        }

        /// <summary>
        /// Gets the first prod list.
        /// </summary>
        /// <value>The first prod list.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<int> FirstProdList
        {
            get { return _firstProdList; }
        }

        /// <summary>
        /// Gets the first token list.
        /// </summary>
        /// <value>The first token list.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<int> FirstTokenList
        {
            get { return _firstTokenList; }
        }

        /// <summary>
        /// Gets the directors.
        /// </summary>
        /// <value>The directors.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public List<string>[] Directors
        {
            get { return _directors; }
        }


        #endregion
        #region Initialization Methods


        /// <summary>
        /// Reads the grammar string.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void ReadGrammar(string grammar)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(grammar);
            MemoryStream stream = new MemoryStream(buffer);
            ReadGrammar(stream);
        }

        /// <summary>
        /// Reads the grammar stream.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void ReadGrammar(Stream grammar)
        {
            bool fDefinitions = false;
            bool fGrammar = false;
            bool fFinished = false;

            grammar.Position = 0;
            string line = ReadGrammarLine(grammar);
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
                if (grammar.Position >= grammar.Length)
                {
                    fFinished = true;
                }

                line = ReadGrammarLine(grammar);
            }

            InitializeGrammar();
        }

        /// <summary>
        /// Adds the grammar definition.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void AddGrammarDefinition(string definition)
        {
            string[] parts = definition.Split(new string[] { " = " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                AddGrammarDefinition(parts[0], parts[1]);
            }
        }

        /// <summary>
        /// Adds the grammar definition.
        /// </summary>
        /// <param name="definitionName">Name of the definition.</param>
        /// <param name="expression">The expression.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void AddGrammarDefinition(string definitionName, string expression)
        {
            string[] parts = expression.Split(new string[] { " | " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                AddGrammarDefinition(definitionName, parts[0], parts[1]);
            }
        }

        /// <summary>
        /// Adds the grammar definition.
        /// </summary>
        /// <param name="definitionName">Name of the definition.</param>
        /// <param name="description">The description.</param>
        /// <param name="pattern">The pattern.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void AddGrammarDefinition(string definitionName, string description, string pattern)
        {
            if (!_definitions.ContainsKey(definitionName))
            {
                _definitions.Add(definitionName, description);
                _patterns.Add(definitionName, pattern);
                _intrinsics.Add(definitionName);
            }
        }

        /// <summary>
        /// Adds the grammar rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void AddGrammarRule(string rule)
        {
            string[] parts = rule.Split(new string[] { " = " }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                AddGrammarRule(parts[0], parts[1]);
            }
        }

        /// <summary>
        /// Adds the grammar rule.
        /// </summary>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="expression">The expression.</param>
        /// <created>18/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void AddGrammarRule(string ruleName, string expression)
        {
            if (!_grammar.ContainsKey(ruleName))
            {
                _grammar.Add(ruleName, new List<string>());
            }

            if (expression.StartsWith("|"))
            {
                expression = " " + expression;
            }

            string[] rules = expression.Split(new string[] { " | " }, StringSplitOptions.None);

            for (int i = 0; i < rules.Length; i++)
            {
                string rule = rules[i].Trim();

                if (!_grammar[ruleName].Contains(rule))
                {
                    _grammar[ruleName].Add(rule);
                }
            }
        }

        /// <summary>
        /// Initializes the grammar.
        /// </summary>
        /// <created>18/02/2009</created>
        /// <author>Thomas_Bates</author>
        public void InitializeGrammar()
        {
            foreach (string parseToken in _grammar.Keys)
            {
                if (!_parseTokens.Contains(parseToken))
                {
                    _parseTokens.Add(parseToken);
                }
            }
            
            foreach (string parseToken in _grammar.Keys)
            {
                _firstProdList.Add(_firstTokenList.Count);

                foreach (string rule in _grammar[parseToken])
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
                                    if (!_keywords.Contains(token))
                                    {
                                        _keywords.Add(token);
                                    }
                                }
                                else if (!char.IsNumber(token[2]))
                                {
                                    if (!_symbols.Contains(token))
                                    {
                                        _symbols.Add(token);
                                    }
                                }
                            }
                            else if (!_intrinsics.Contains(token) && !_parseTokens.Contains(token))
                            {
                                if (!_codeTokens.Contains(token))
                                {
                                    _codeTokens.Add(token);
                                }
                            }
                        }
                        if (newProduction)
                        {
                            _firstTokenList.Add(_allTokens.Count);
                        }
                        if (!string.IsNullOrEmpty(token))
                        {
                            _allTokens.Add(token);
                        }
                        newProduction = false;
                    }
                }
            }
            _firstProdList.Add(_firstTokenList.Count);
            _firstTokenList.Add(_allTokens.Count);

            //  Strip quotation marks from keywords and symbols and all tokens list.
            for (int i = 0; i < _keywords.Count; i++)
            {
                string keyword = _keywords[i];
                if (keyword[0] == '"' && keyword[keyword.Length - 1] == '"')
                {
                    keyword = keyword.Substring(1, keyword.Length - 2);
                    _keywords[i] = keyword;
                }
            }

            for (int i = 0; i < _symbols.Count; i++)
            {
                string symbol = _symbols[i];
                if (symbol[0] == '"' && symbol[symbol.Length - 1] == '"')
                {
                    symbol = symbol.Substring(1, symbol.Length - 2);
                    _symbols[i] = symbol;
                }
            }

            for (int i = 0; i < _allTokens.Count; i++)
            {
                string token = _allTokens[i];
                if (token[0] == '"' && token[token.Length - 1] == '"')
                {
                    token = token.Substring(1, token.Length - 2);
                    _allTokens[i] = token;
                }
            }

            //  Sort keywords, symbols and code tokens.
            _keywords.Sort();
            _symbols.Sort();
            _codeTokens.Sort();

            CalculateDirectors();

            DebugDumpLists();
        }


        #endregion
        #region Local Support Methods


        /// <summary>
        /// Reads the grammar line.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <returns></returns>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual string ReadGrammarLine(Stream grammar)
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


        #endregion
        #region Director Calculation Methods


        /// <summary>
        /// Calculates the directors.
        /// </summary>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void CalculateDirectors()
        {
            //Debug.Print("Grammar1", "CalculateDirectors ...");
            //Debug.Indent("Grammar1");

            _directors = new List<string>[_firstTokenList.Count];
            for (int i = 0; i < _firstTokenList.Count; i++)
            {
                _directors[i] = new List<string>();
            }

            for (int i = 0; i < _parseTokens.Count; i++)
            {
                CalcParseDirectors(i);
            }

            //Debug.Unindent("Grammar1");
            //Debug.Print("Grammar1", "... CalculateDirectors");
        }

        /// <summary>
        /// Calcs the parse directors.
        /// </summary>
        /// <param name="parseTokenIndex">Index of the parse token.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void CalcParseDirectors(int parseTokenIndex)
        {
            //Debug.Print("Grammar1", "CalcParseDirectors({0}) ...", _parseTokens[parseTokenIndex]);
            //Debug.Indent("Grammar1");

            DebugShowStack("if (_directorParseTokenStack.Contains(parseTokenIndex))", "_directorParseTokenStack", _directorParseTokenStack);

            if (_directorParseTokenStack.Contains(parseTokenIndex))
            {
                throw new GrammarException("Grammar is not L2");
            }

            _directorParseTokenStack.Push(parseTokenIndex);
            DebugShowStack("_directorParseTokenStack.Push(parseTokenIndex);", "_directorParseTokenStack", _directorParseTokenStack);

            for (int i = _firstProdList[parseTokenIndex]; i < _firstProdList[parseTokenIndex + 1]; i++)
            {
                if (_directors[i].Count == 0)
                {
                    for (int j = _firstTokenList[i]; j < _firstTokenList[i + 1]; j++)
                    {
                        string token = _allTokens[j];
                        bool fTerminal = (_intrinsics.Contains(token) || _keywords.Contains(token) || _symbols.Contains(token));
                        if (fTerminal)
                        {
                            _directors[i].Add(token);
                            break;
                        }
                        else if (!string.IsNullOrEmpty(token) && token[1] != '_')
                        {
                            int index = _parseTokens.IndexOf(token);
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

            DebugShowStack("if (_directorParseTokenStack.Pop() != parseTokenIndex)", "_directorParseTokenStack", _directorParseTokenStack);
            if (_directorParseTokenStack.Pop() != parseTokenIndex)
            {
                throw new GrammarException("Internal Stack Error.");
            }

            //Debug.Unindent("Grammar1");
            //Debug.Print("Grammar1", "... CalcParseDirectors({0})", _parseTokens[parseTokenIndex]);
        }

        /// <summary>
        /// Adds the directors.
        /// </summary>
        /// <param name="productionIndex">Index of the production.</param>
        /// <param name="parseTokenIndex">Index of the parse token.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void AddDirectors(int productionIndex, int parseTokenIndex)
        {
            //Debug.Print("Grammar1", "AddDirectors({0}, {1}) ...", productionIndex, _parseTokens[parseTokenIndex]);
            //Debug.Indent("Grammar1");

            //  Make sure the specified parse token's director set has been calculated.
            CalcParseDirectors(parseTokenIndex);

            //  For each production of the specified parse token ...
            for (int i = _firstProdList[parseTokenIndex]; i < _firstProdList[parseTokenIndex + 1]; i++)
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

            //Debug.Unindent("Grammar1");
            //Debug.Print("Grammar1", "... AddDirectors({0}, {1})", productionIndex, _parseTokens[parseTokenIndex]);
        }

        /// <summary>
        /// Adds the followers.
        /// </summary>
        /// <param name="productionIndex">Index of the production.</param>
        /// <param name="parseTokenIndex">Index of the parse token.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void AddFollowers(int productionIndex, int parseTokenIndex)
        {
            //Debug.Print("Grammar1", "AddFollowers({0}, {1}) ...", productionIndex, _parseTokens[parseTokenIndex]);
            //Debug.Indent("Grammar1");
            try
            {
                DebugShowStack("if (_followerParseTokenStack.Contains(parseTokenIndex))", "_followerParseTokenStack", _followerParseTokenStack);
                if (_followerParseTokenStack.Contains(parseTokenIndex))
                {
                    return;
                }

                string parseToken = _parseTokens[parseTokenIndex];

                if (parseTokenIndex == 0)
                {
                    string token = EOF;
                    if (!_directors[productionIndex].Contains(token))
                    {
                        _directors[productionIndex].Add(token);
                    }
                    return;
                }

                _followerParseTokenStack.Push(parseTokenIndex);
                DebugShowStack("_followerParseTokenStack.Push(parseTokenIndex);", "_followerParseTokenStack", _followerParseTokenStack);

                for (int i = 0; i < _parseTokens.Count; i++)
                {
                    for (int j = _firstProdList[i]; j < _firstProdList[i + 1]; j++)
                    {
                        bool fInProd = false;
                        bool fFollowed = false;

                        for (int k = _firstTokenList[j]; k < _firstTokenList[j + 1]; k++)
                        {
                            string token = _allTokens[k];

                            if (fInProd)
                            {
                                bool fTerminal = (_intrinsics.Contains(token) || _keywords.Contains(token) || _symbols.Contains(token));
                                if (fTerminal)
                                {
                                    if (!_directors[productionIndex].Contains(token))
                                    {
                                        _directors[productionIndex].Add(token);
                                    }
                                    fFollowed = true;
                                    break;
                                }
                                else if (!string.IsNullOrEmpty(token) && !_codeTokens.Contains(token))// token[1] != '_')
                                {
                                    int index = _parseTokens.IndexOf(token);
                                    AddDirectors(productionIndex, index);
                                    fFollowed = true;
                                    break;
                                }
                            }
                            if (_allTokens[k].Equals(parseToken))
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

                DebugShowStack("if (_followerParseTokenStack.Pop() != parseTokenIndex)", "_followerParseTokenStack", _followerParseTokenStack);
                if (_followerParseTokenStack.Pop() != parseTokenIndex)
                {
                    throw new GrammarException("Internal Stack Error.");
                }
            }
            finally
            {
                //Debug.Unindent("Grammar1");
                //Debug.Print("Grammar1", "... AddFollowers({0}, {1})", productionIndex, _parseTokens[parseTokenIndex]);
            }
        }


        #endregion
        #region Private Debug Methods


        /// <summary>
        /// Writes the specified stack to the debug file.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="stackName">Name of the stack.</param>
        /// <param name="stack">The stack.</param>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void DebugShowStack(string caption, string stackName, Stack<int> stack)
        {
            //if (Debug.IsEnabled("Grammar1"))
            {
                string tokens = string.Empty;
                int[] indeces = stack.ToArray();
                foreach (int index in indeces)
                {
                    string token = _parseTokens[index];
                    if (!string.IsNullOrEmpty(tokens)) tokens = ", " + tokens;
                    tokens = token + tokens;
                }
                //Debug.Print("Grammar1", caption);
                //Debug.Print("Grammar1", "  " + stackName + ":  " + tokens);
            }
        }

        /// <summary>
        /// Dumps lists to debug file.
        /// </summary>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void DebugDumpLists()
        {
            //if (Debug.IsEnabled("Grammar2"))
            {
                DebugDumpList("First the standard definitions", _intrinsics);
                DebugDumpList("Then the reserved (key)words in alphabetical order", _keywords);
                DebugDumpList("Then the others in random order", _symbols);
                DebugDumpList("Grammar Parse Tokens", _parseTokens);
                DebugDumpList("Grammar Code Tokens", _codeTokens);
                DebugDumpList("For each parse token, the first production number is as follows:", _firstProdList);

                //Debug.Print("Grammar2", "Director Sets");
                for (int i = 0; i < _directors.Length; i++)
                {
                    DebugDumpList(i, _directors[i]);
                }
                //Debug.Print("Grammar2", "");

                DebugDumpList("For each production, the first token is in the following position:", _firstTokenList);
            }
        }

        /// <summary>
        /// Dumps a list to the debug file.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="list">The list.</param>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void DebugDumpList(string caption, List<string> list)
        {
            //Debug.Print("Grammar2", caption);
            foreach (string token in list)
            {
                //Debug.Print("Grammar2", "  " + token);
            }
            //Debug.Print("Grammar2", "");
        }

        /// <summary>
        /// Dumps a list to the debug file.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="list">The list.</param>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void DebugDumpList(string caption, List<int> list)
        {
            //Debug.Print("Grammar2", caption);
            string line = string.Empty;
            foreach (int index in list)
            {
                if (!string.IsNullOrEmpty(line)) line += ", ";
                line += index.ToString();
            }
            //Debug.Print("Grammar2", "  " + line);
            //Debug.Print("Grammar2", "");
        }

        /// <summary>
        /// Dumps a director list to the debug file.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="list">The list.</param>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void DebugDumpList(int index, List<string> list)
        {
            string line = string.Empty;
            foreach (string token in list)
            {
                if (!string.IsNullOrEmpty(line)) line += ", ";
                line += "\"" + token + "\"";
            }
            //Debug.Print("Grammar2", "  " + index + ":  " + line);
        }


        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _definitions.Clear();
            _patterns.Clear();
            _grammar.Clear();
            _intrinsics.Clear();
            _keywords.Clear();
            _symbols.Clear();
            _parseTokens.Clear();
            _codeTokens.Clear();
            _allTokens.Clear();
            _firstProdList.Clear();
            _firstTokenList.Clear();
            _directorParseTokenStack.Clear();
            _followerParseTokenStack.Clear();
        }
    }
}
