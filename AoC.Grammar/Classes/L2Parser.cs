using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Grammar;

public class L2Parser : IParser
{
    private readonly IL2Grammar grammar;

    private int line = 0;
    private int column = 0;
    private char nextChar;
    private string nextValue = string.Empty;
    private string scanToken = string.Empty;

    private readonly Stack<string> parseStack = new();

    #region Constructors

    public L2Parser(IL2Grammar grammar)
    {
        this.grammar = grammar;
    }

    #endregion Constructors
    #region IParser Events

    public event EventHandler<ParserEventArgs> OnValueEmitted;

    public event EventHandler<ParserEventArgs> OnTokenEmitted;

    public event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

    #endregion IParser Events
    #region IParser Methods

    public virtual void Parse(string input)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(input);
        var stream = new MemoryStream(buffer);
        Parse(stream);
    }

    public void Parse(Stream input)
    {
        string token;

        parseStack.Push(grammar.Intrinsics[0]);
        parseStack.Push(grammar.ParseTokens[0]);

        nextChar = '\n';
        line = 0;
        column = 0;

        Scan(input);
        do
        {
            token = parseStack.Pop();

            if (grammar.ParseTokens.Contains(token))
            {
                int tokenIndex = grammar.ParseTokens.IndexOf(token);
                int prod = grammar.FirstProdList[tokenIndex];
                int lastProd = grammar.FirstProdList[tokenIndex + 1] - 1;

                while (true)
                {
                    if (grammar.Directors[prod].Contains(scanToken))
                    {
                        for (int t = grammar.FirstTokenList[prod + 1] - 1; t >= grammar.FirstTokenList[prod]; t--)
                        {
                            parseStack.Push(grammar.AllTokens[t]);
                        }
                        break;
                    }
                    else if (prod == lastProd)
                    {
                        if (grammar.Intrinsics.Contains(scanToken))
                        {
                            throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  \"{2}\" cannot appear there.", line, column, nextValue));
                        }
                        else
                        {
                            throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  \"{2}\" cannot appear there.", line, column, scanToken));
                        }
                    }
                    else
                    {
                        prod++;
                    }
                }
            }
            else if (grammar.CodeTokens.Contains(token))
            {
                DebugPrint("Parser", $"token emitted: {token}");
                OnTokenEmitted?.Invoke(this, new ParserEventArgs(token, nextValue));
            }
            else
            {
                if (!token.Equals(scanToken))
                {
                    if (grammar.Intrinsics.Contains(scanToken))
                    {
                        throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  token \"{2}\" expected, but \"{3}\" found.", line, column, token, nextValue));
                    }
                    else
                    {
                        throw new ParserException(string.Format("Parser Error:  line {0}, column {1}:  token \"{2}\" expected, but \"{3}\" found.", line, column, token, scanToken));
                    }
                }
                if (grammar.Intrinsics.Contains(token))
                {
                    if (grammar.Intrinsics.IndexOf(token) != 0)
                    {
                        DebugPrint("Parser", $"value emitted: {nextValue}");
                        OnValueEmitted?.Invoke(this, new ParserEventArgs(token, nextValue));
                        Scan(input);
                    }
                }
                else
                {
                    Scan(input);
                }
            }


        } while (parseStack.Count > 0);
    }

    #endregion IParser Methods
    #region Private Methods

    private void Scan(Stream input)
    {
        //  Precondition:  c = next character (unprocessed)

        while (true)
        {
            if (nextChar == ' ' || nextChar == '\t' || nextChar == '\r')
            {
                nextChar = GetChar(input);
            }
            else if (nextChar == '\n')
            {
                line++;
                column = 0;
                nextChar = GetChar(input);
            }
            else if (grammar.Symbols.Contains(nextChar.ToString()))
            {
                scanToken = nextChar.ToString();
                nextChar = GetChar(input);
                return;
            }
            else if (nextChar == 26)  //  EOF
            {
                scanToken = grammar.Intrinsics[0];
                return;
            }
            else
            {
                string testValue = string.Empty;
                string lastToken = string.Empty;
                string lastValue = string.Empty;
                bool foundStart = false;

                while (nextChar != 26)
                {
                    testValue += nextChar;

                    int count = 0;
                    if (" \t\r\n".IndexOf(nextChar) < 0)
                    {
                        foreach (string token in grammar.Patterns.Keys)
                        {
                            string pattern = grammar.Patterns[token];
                            if (Regex.IsMatch(testValue, "^" + pattern + "$"))
                            {
                                lastToken = token;
                                lastValue = testValue;
                                count++;
                            }
                        }
                    }
                    if (!foundStart && count > 0)
                    {
                        foundStart = true;
                    }
                    if (foundStart && count == 0)  //  Current test failed
                    {
                        if (!string.IsNullOrEmpty(lastValue))  //  Previous test succeeded:  use last value.
                        {
                            scanToken = lastToken;
                            nextValue = lastValue;

                            //  check for keywords.
                            if (grammar.Keywords.Contains(nextValue))
                            {
                                scanToken = nextValue;
                            }
                            return;
                        }

                        //  Illegal character.
                        scanToken = testValue;
                        nextValue = testValue;
                        return;
                    }
                    nextChar = GetChar(input);
                }

                //  EOF:  use last successful value.
                if (!string.IsNullOrEmpty(lastValue))  //  Previous test succeeded:  use last value.
                {
                    scanToken = lastToken;
                    nextValue = lastValue;

                    //  check for keywords.
                    if (grammar.Keywords.Contains(nextValue))
                    {
                        scanToken = nextValue;
                    }
                    return;
                }
            }
        }
    }

    protected virtual char GetChar(Stream input)
    {
        int b = input.ReadByte();
        column++;
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
