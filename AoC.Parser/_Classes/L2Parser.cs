using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

//using AoC.Debug;

namespace AoC.Parser
{
    /// <summary>
    /// L2Parser 
    /// </summary>
    /// <created>19/02/2009</created>
    /// <author>Thomas_Bates</author>
    public class L2Parser : L2ParserBase, IDisposable
    {
        private int _line = 0;
        private int _column = 0;
        private char _nextChar;
        private string _nextValue = string.Empty;
        private string _scanToken = string.Empty;

        private Stack<string> _parseStack = new Stack<string>();


        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="L2Parser"/> class.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2Parser(L2GrammarBase grammar)
            : base(grammar)
        {
        }


        #endregion
        #region Properties


        /// <summary>
        /// Gets the grammar.
        /// </summary>
        /// <value>The grammar.</value>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected new L2Grammar Grammar
        {
            get { return base.Grammar as L2Grammar; }
        }


        #endregion
        #region Property Accessor Methods


        #endregion
        #region Initialization Methods


        /// <summary>
        /// Creates the grammar.
        /// </summary>
        /// <returns>The created grammar.</returns>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected override L2GrammarBase CreateGrammar()
        {
            return new L2Grammar();
        }


        #endregion
        #region Event Handler Methods


        #endregion
        #region Public Access Methods


        /// <summary>
        /// Parses the specified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public override void Parse(Stream input)
        {
            string token;

            _parseStack.Push(Grammar.Intrinsics[0]);
            _parseStack.Push(Grammar.ParseTokens[0]);

            _nextChar = '\n';
            _line = 0;
            _column = 0;

            Scan(input);
            do
            {
                token = _parseStack.Pop();

                if (Grammar.ParseTokens.Contains(token))
                {
                    int tokenIndex = Grammar.ParseTokens.IndexOf(token);
                    int prod = Grammar.FirstProdList[tokenIndex];
                    int lastProd = Grammar.FirstProdList[tokenIndex + 1] - 1;

                    while (true)
                    {
                        if (Grammar.Directors[prod].Contains(_scanToken))
                        {
                            for (int t = Grammar.FirstTokenList[prod + 1] - 1; t >= Grammar.FirstTokenList[prod]; t--)
                            {
                                _parseStack.Push(Grammar.AllTokens[t]);
                            }
                            break;
                        }
                        else if (prod == lastProd)
                        {
                            if (Grammar.Intrinsics.Contains(_scanToken))
                            {
                                throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  \"{2}\" cannot appear there.", _line, _column, _nextValue));
                            }
                            else
                            {
                                throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  \"{2}\" cannot appear there.", _line, _column, _scanToken));
                            }
                        }
                        else
                        {
                            prod++;
                        }
                    }
                }
                else if (Grammar.CodeTokens.Contains(token))
                {
                    //Debug.Print("Parser", token);
                    OnTokenEmitted(new ParserEventArgs(token, _nextValue));
                }
                else
                {
                    if (!token.Equals(_scanToken))
                    {
                        if (Grammar.Intrinsics.Contains(_scanToken))
                        {
                            throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  token \"{2}\" expected, but \"{3}\" found.", _line, _column, token, _nextValue));
                        }
                        else
                        {
                            throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  token \"{2}\" expected, but \"{3}\" found.", _line, _column, token, _scanToken));
                        }
                    }
                    if (Grammar.Intrinsics.Contains(token))
                    {
                        if (Grammar.Intrinsics.IndexOf(token) != 0)
                        {
                            //Debug.Print("Parser", "{0}", _nextValue);
                            OnValueEmitted(new ParserEventArgs(token, _nextValue));
                            Scan(input);
                        }
                    }
                    else
                    {
                        Scan(input);
                    }
                }


            } while (_parseStack.Count > 0);
        }


        #endregion
        #region Local Support Methods


        /// <summary>
        /// Scans the specified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void Scan(Stream input)
        {
            //  Precondition:  c = next character (unprocessed)

            while (true)
            {
                if (_nextChar == ' ' || _nextChar == '\t' || _nextChar == '\r')
                {
                    _nextChar = GetChar(input);
                }
                else if (_nextChar == '\n')
                {
                    _line++;
                    _column = 0;
                    _nextChar = GetChar(input);
                }
                else if (Grammar.Symbols.Contains(_nextChar.ToString()))
                {
                    _scanToken = _nextChar.ToString();
                    _nextChar = GetChar(input);
                    return;
                }
                else if (_nextChar == 26)  //  EOF
                {
                    _scanToken = Grammar.Intrinsics[0];
                    return;
                }
                else
                {
                    string testValue = string.Empty;
                    string lastToken = string.Empty;
                    string lastValue = string.Empty;

                    while (_nextChar != 26)
                    {
                        //c = GetChar(input);
                        testValue += _nextChar;

                        int count = 0;
                        foreach (string token in Grammar.Patterns.Keys)
                        {
                            string pattern = Grammar.Patterns[token];
                            if (Regex.IsMatch(testValue, "^" + pattern + "$"))
                            {
                                lastToken = token;
                                lastValue = testValue;
                                count++;
                            }
                        }
                        if (count == 0)  //  Current test failed
                        {
                            if (!string.IsNullOrEmpty(lastValue))  //  Previous test succeeded:  use last value.
                            {
                                _scanToken = lastToken;
                                _nextValue = lastValue;

                                //  check for keywords.
                                if (Grammar.Keywords.Contains(_nextValue))
                                {
                                    _scanToken = _nextValue;
                                }
                                return;
                            }

                            //  Illegal character.
                            _scanToken = testValue;
                            _nextValue = testValue;
                            return;
                        }
                        _nextChar = GetChar(input);
                    }

                    //  EOF:  use last successful value.
                    if (!string.IsNullOrEmpty(lastValue))  //  Previous test succeeded:  use last value.
                    {
                        _scanToken = lastToken;
                        _nextValue = lastValue;

                        //  check for keywords.
                        if (Grammar.Keywords.Contains(_nextValue))
                        {
                            _scanToken = _nextValue;
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the char.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual char GetChar(Stream input)
        {
            int b = input.ReadByte();
            _column++;
            if (b == -1)
            {
                return (char)26;  //  ^Z (EOF)
            }
            return (char)b;
        }


        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _parseStack.Clear();
        }
    }
}
