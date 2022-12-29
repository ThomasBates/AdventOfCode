using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dynamic.Utilities
{
    /// <summary>
    /// OriginalL2Grammar 
    /// </summary>
    /// <created>19/02/2009</created>
    /// <author>Thomas_Bates</author>
    public class OriginalL2Grammar : DynamicL2GrammarBase
    {
        private Dictionary<string, string> _intrinsics = new Dictionary<string, string>();
        private Dictionary<string, List<string>> _grammar = new Dictionary<string, List<string>>();

        private List<string> _keywords = new List<string>();
        private List<string> _symbols = new List<string>();

        private Dictionary<string, int> _parseTokens0 = new Dictionary<string, int>();
        private List<string> _parseTokens = new List<string>();
        private List<string> _scopeTokens = new List<string>();
        private List<string> _typeTokens = new List<string>();
        private List<string> _codeTokens = new List<string>();
        private List<string> _allTokens = new List<string>();

        private List<int> _firstProdList = new List<int>();
        private List<int> _firstTokenList = new List<int>();
        private List<string>[] _directors;

        private Stack<int> _directorParseTokenStack = new Stack<int>();
        private Stack<int> _followerParseTokenStack = new Stack<int>();


        //private int mintIntrinsics;
        //private int mintKeywords;
        //private int mintSymbols;
        //private int mintParseToken;
        //private int mintParseTokens;
        //private int mintScopeTokens;
        //private int mintTypeTokens;
        //private int mintCodeTokens;
        //private int mintAllTokens;
        //private int mintProductions;

        //private string[][] mastrIntrinsics = new string[10,2];
        //private string[] mastrKeywords = new string[57];
        //private string[] mastrSymbols = new string[32]; //  !"#$%&'()*+,-./:;<=>?@[\]^_`{|}~

        private char[] mcachSymbolSet = { '!','"','#','$','%','&','\'','(',
                                          ')','*','+',',','-','.','/',':',
                                          ';','<','=','>','?','@','[','\\',
                                          ']','^','_','`','{','|','}','~' };
        private string[] mcastrSymbolNames = {
            "__EXCLAMATION__",
            "__QUOTE__",
            "__POUND__",
            "__DOLLAR__",
            "__PERCENT__",
            "__AMPERSAND__",
            "__APOSTROPHE__",
            "__OPARENTH__",
            "__CPARENTH__",
            "__ASTERISK__",
            "__PLUS__",
            "__COMMA__",
            "__HYPHEN__",
            "__PERIOD__",
            "__SLASH__",
            "__COLON__",
            "__SEMICOLON__",
            "__LT__",
            "__EQ__",
            "__GT__",
            "__QUESTION__",
            "__AT__",
            "__OBRACKET__",
            "__BACKSLASH__",
            "__CBRACKET__",
            "__CARET__",
            "__UNDERSCORE__",
            "__YOT__",
            "__OBRACE__",
            "__PIPE__",
            "__CBRACE__",
            "__TILDE__"
        };

        private const int LANG_C_OLD  = 0;
        private const int LANG_C_NEW  = 1;
        private const int LANG_PASCAL = 2;

        //private ParseToken[] mastParseTokens = new ParseToken[100];
        //private string[] mastrScopeTokens = new string[100];
        //private string[] mastrTypeTokens = new string[100];
        //private string[] mastrCodeTokens = new string[120];
        //private string[] mapstrAllTokens = new string[1000];

        //private int[] maintFirstProd = new int[100];
        //private int[] maintFirstToken = new int[130];
        //private string[][] mapstrDir = new string[130,100];

        //private int[][] maintParseTokenStack = new int[2,100];
        //private int[] maintParseTokenSP = new int[2];

        private const int TAB = 9;
        private const int LF = 10;
        private const int CR = 13;
        private const int EOF = 26;
        private const int SPACE = ' ';
        private const int QUOTE = '"';
        private const int EQUALS = '=';
        private const int PIPE = '|';


        #region Events


        #endregion
        #region Constructors


        #endregion
        #region Properties


        #endregion
        #region Property Accessor Methods


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
            byte[] buffer = Encoding.Unicode.GetBytes(grammar);
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
            bool fNewLine = false;
            bool newProduction = true;
            bool newParseToken = true;            
            int parseTokens = 0;
            string definitionName = string.Empty;
            string token = string.Empty;

            int code = GetGrammarToken(grammar, ref token);
            while (code != EOF && !fFinished)
            {
                if (token.Equals("#DEFINITIONS", StringComparison.OrdinalIgnoreCase))
                {
                    fDefinitions = true;
                    fGrammar = false;
                    fFinished = false;
                }
                else if (token.Equals("#GRAMMAR", StringComparison.OrdinalIgnoreCase))
                {
                    fDefinitions = false;
                    fGrammar = true;
                    fFinished = false;
                }
                else if (token.Equals("#END", StringComparison.OrdinalIgnoreCase))
                {
                    fDefinitions = false;
                    fGrammar = false;
                    fFinished = true;

                    _firstProdList.Add(_firstTokenList.Count);
                    _firstTokenList.Add(_allTokens.Count);
                }
                else if (fDefinitions)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (fNewLine)
                        {
                            definitionName = token;
                        }
                        else
                        {
                            _intrinsics.Add(definitionName, token);
                        }
                    }
                    fNewLine = code == LF;
                }
                else if (fGrammar)
                {
                    if (token.StartsWith("s_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!_scopeTokens.Contains(token))
                        {
                            _scopeTokens.Add(token);
                        }
                        if (newProduction)
                        {
                            _firstTokenList.Add(_allTokens.Count);
                        }
                        _allTokens.Add(token);
                        newProduction = false;
                    }
                    else if (token.StartsWith("t_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!_typeTokens.Contains(token))
                        {
                            _typeTokens.Add(token);
                        }
                        if (newProduction)
                        {
                            _firstTokenList.Add(_allTokens.Count);
                        }
                        _allTokens.Add(token);
                        newProduction = false;
                    }
                    else if (token.StartsWith("c_", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!_codeTokens.Contains(token))
                        {
                            _codeTokens.Add(token);
                        }
                        if (newProduction)
                        {
                            _firstTokenList.Add(_allTokens.Count);
                        }
                        _allTokens.Add(token);
                        newProduction = false;
                    }
                    else if (!string.IsNullOrEmpty(token))
                    {
                        bool fTerminal = token.StartsWith("\"");

                        if (fTerminal)
                        {
                            if (char.IsLetter(token[2]))
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
                        else if (_intrinsics.ContainsKey(token))
                        {
                            fTerminal = true;
                        }

                        if (fTerminal)
                        {
                            if (newProduction)
                            {
                                _firstTokenList.Add(_allTokens.Count);
                            }
                            _allTokens.Add(token);
                            newProduction = false;
                        }
                        else
                        {
                            if (!_parseTokens0.ContainsKey(token))
                            {
                                _parseTokens0.Add(token, -1);
                            }
                            if (newParseToken)
                            {
                                _firstProdList.Add(_firstTokenList.Count);
                                _parseTokens0[token] = parseTokens++;

                                if (code == LF)
                                {
                                    _firstTokenList.Add(_allTokens.Count);
                                }
                            }
                            else if (newProduction)
                            {
                                _firstTokenList.Add(_allTokens.Count);
                                _allTokens.Add(token);
                                newProduction = false;
                            }
                            else
                            {
                                _allTokens.Add(token);
                                newProduction = false;
                            }
                        }
                    }

                    if (newParseToken || code == LF)
                    {
                        newProduction = true;
                    }
                    newParseToken = code == LF && string.IsNullOrEmpty(token);
                }
                code = GetGrammarToken(grammar, ref token);
            }
            CalculateDirectors();
        }


        #endregion
        #region Event Handler Methods


        #endregion
        #region Public Access Methods


        #endregion
        #region Local Support Methods


        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual int GetGrammarToken(Stream grammar, ref string token)
        {
            token = string.Empty;
            bool inQuotes = false;
            int code = grammar.ReadByte();
            while (code != -1)
            {
                char nextChar = (char)code;
                switch (code)
                {
                    case TAB:
                    case SPACE:
                    case EQUALS:
                        if (inQuotes)
                        {
                            token += nextChar;
                        }
                        else if (token.Length > 0)
                        {
                            return SPACE;
                        }
                        break;

                    case LF:
                        return LF;

                    case PIPE:
                        if (inQuotes)
                        {
                            token += nextChar;
                        }
                        else
                        {
                            return LF;
                        }
                        break;

                    case CR:    //  ignore
                        break;

                    case QUOTE:
                        inQuotes = !inQuotes;
                        token += nextChar;
                        break;
                }

                code = grammar.ReadByte();
            }
            return EOF;
        }

        /// <summary>
        /// Calculates the directors.
        /// </summary>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void CalculateDirectors()
        {
            _directors = new List<string>[_firstProdList.Count];
            for (int i = 0; i < _firstProdList.Count; i++)
            {
                _directors[i] = new List<string>();
            }

            for (int i = 0; i < _parseTokens.Count; i++)
            {
                CalcParseDirectors(i);
            }
        }

        /// <summary>
        /// Calcs the parse directors.
        /// </summary>
        /// <param name="parseTokenIndex">Index of the parse token.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void CalcParseDirectors(int parseTokenIndex)
        {
            if (_directorParseTokenStack.Contains(parseTokenIndex))
            {
                throw new Exception("Grammar is not L2");
            }

            _directorParseTokenStack.Push(parseTokenIndex);

            for (int i = _firstProdList[parseTokenIndex]; i < _firstProdList[parseTokenIndex + 1]; i++)
            {
                if (_directors[i].Count == 0)
                {
                    for (int j = _firstTokenList[i]; j < _firstTokenList[i + 1]; j++)
                    {
                        string token = _allTokens[j];
                        bool fTerminal = token.StartsWith("\"");
                        if (_intrinsics.ContainsKey(token))
                        {
                            fTerminal = true;
                        }
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

            if (_directorParseTokenStack.Pop() != parseTokenIndex)
            {
                throw new Exception("Internal Stack Error.");
            }
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
            if (_followerParseTokenStack.Contains(parseTokenIndex))
            {
                return;
            }

            string parseToken = _parseTokens[parseTokenIndex];

            if (parseTokenIndex == 0)
            {
                string token = "EOF";
                if (!_directors[productionIndex].Contains(token))
                {
                    _directors[productionIndex].Add(token);
                }
                return;
            }

            _followerParseTokenStack.Push(productionIndex);

            for (int i = 0; i < _parseTokens.Count; i++)
            {
                for (int j = _firstProdList[i]; j < _firstProdList[i + 1]; j++)
                {
                    bool fInProd = false;
                    bool fFollowed = false;

                    for (int k = _firstProdList[j]; k < _firstProdList[j + 1]; k++)
                    {
                        string token = _allTokens[k];

                        if (fInProd)
                        {
                            bool fTerminal = token.StartsWith("\"");
                            if (_intrinsics.ContainsKey(token))
                            {
                                fTerminal = true;
                            }
                            if (fTerminal)
                            {
                                if (!_directors[productionIndex].Contains(token))
                                {
                                    _directors[productionIndex].Add(token);
                                }
                                fFollowed = true;
                                break;
                            }
                            else if (!string.IsNullOrEmpty(token) && token[1] != '_')
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

            if (_followerParseTokenStack.Pop() != parseTokenIndex)
            {
                throw new Exception("Internal Stack Error.");
            }
        }


        #endregion
        #region Event Handlers


        #endregion
    }
}
