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
    public class L2Parser : IParser
    {
        private readonly IL2Grammar Grammar;

        private int _line = 0;
        private int _column = 0;
        private char _nextChar;
        private string _nextValue = string.Empty;
        private string _scanToken = string.Empty;

        private readonly Stack<string> _parseStack = new();

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="L2Parser"/> class.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2Parser(IL2Grammar grammar)
        {
            this.Grammar = grammar;
        }

		#endregion Constructors
		#region IParser Events

		/// <summary>
		/// Occurs when [intrinsic token emitted].
		/// </summary>
		public event EventHandler<ParserEventArgs> OnValueEmitted;

		/// <summary>
		/// Occurs when [code token emitted].
		/// </summary>
		public event EventHandler<ParserEventArgs> OnTokenEmitted;

		/// <summary>
		/// Occurs when log message emitted.
		/// </summary>
		public event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

		#endregion IParser Events
		#region IParser Methods

		/// <summary>
		/// Parses the specified input.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <created>19/02/2009</created>
		/// <author>Thomas_Bates</author>
		public virtual void Parse(string input)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(input);
			var stream = new MemoryStream(buffer);
			Parse(stream);
		}

		/// <summary>
		/// Parses the specified input stream.
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public void Parse(Stream input)
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
                    DebugPrint("Parser", $"token emitted: {token}");
                    OnTokenEmitted?.Invoke(this, new ParserEventArgs(token, _nextValue));
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
                            DebugPrint("Parser", $"value emitted: {_nextValue}");
                            OnValueEmitted?.Invoke(this, new ParserEventArgs(token, _nextValue));
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


        #endregion IParser Methods
        #region Private Methods


        /// <summary>
        /// Scans the specified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        private void Scan(Stream input)
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
                        testValue += _nextChar;

                        int count = 0;
                        if (" \t\r\n".IndexOf(_nextChar) < 0)
                        {
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

		private void DebugPrint(string category, string message)
		{
			OnLogMessageEmitted?.Invoke(this, new ParserLogEventArgs("Debug", category, message));
		}

		#endregion
	}
}
