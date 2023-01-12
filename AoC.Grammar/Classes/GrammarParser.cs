using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Grammar;

public class GrammarParser : IGrammarParser
{
    private readonly GrammarData grammar;

    private int line = 0;
    private int column = 0;
    private char nextChar;
    private string nextValue = string.Empty;
    private string scanToken = string.Empty;

    private readonly Stack<string> parseStack = new();

    #region Constructors

    public GrammarParser(GrammarData grammar)
    {
        this.grammar = grammar;
    }

    #endregion Constructors
    #region IParser Events

    public event EventHandler<GrammarEmitEventArgs> OnValueEmitted;

    public event EventHandler<GrammarEmitEventArgs> OnTokenEmitted;

    public event EventHandler<GrammarLogEventArgs> OnLogMessageEmitted;

    #endregion IParser Events
    #region IParser Methods

    public void ParseInput(string input, string startToken = "")
    {
        byte[] buffer = Encoding.UTF8.GetBytes(input);
        var stream = new MemoryStream(buffer);
        ParseInput(stream, startToken);
    }

    public void ParseInput(Stream input, string startToken = "")
    {
        if (string.IsNullOrEmpty(startToken))
            startToken = grammar.StartToken;

        parseStack.Push(grammar.EndToken);
        parseStack.Push(startToken);

        nextChar = '\n';
        line = 0;
        column = 0;

        Scan(input);
        do
        {
            var token = parseStack.Pop();

            if (grammar.ParseTokens.TryGetValue(token, out var productions))
            {
                var found = false;
                foreach (var production in productions)
                {
                    if (production.Directors.Contains(scanToken))
                    {
                        for (int t = production.Tokens.Count - 1; t >= 0; t--)
                            parseStack.Push(production.Tokens[t]);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (grammar.Intrinsics.ContainsKey(scanToken))
                        throw new ParserException($"Parser Error:  line {line}, column {column}:  \"{nextValue}\" cannot appear there.");
                    else
                        throw new ParserException($"Parser Error:  line {line}, column {column}:  \"{scanToken}\" cannot appear there.");
                }
            }
            else if (grammar.CodeTokens.Contains(token))
            {
                DebugPrint("Parser", $"token emitted: {token}");
                OnTokenEmitted?.Invoke(this, new GrammarEmitEventArgs(token, nextValue));
            }
            else
            {
                if (!token.Equals(scanToken))
                {
                    if (grammar.Intrinsics.ContainsKey(scanToken))
                        throw new ParserException($"Parser Error:  line {line}, column {column}:  token \"{token}\" expected, but \"{nextValue}\" found.");
                    else
                        throw new ParserException($"Parser Error:  line {line}, column {column}:  token \"{token}\" expected, but \"{scanToken}\" found.");
                }
                if (grammar.Intrinsics.ContainsKey(token))
                {
                    if (!grammar.EndToken.Equals(token))
                    {
                        DebugPrint("Parser", $"value emitted: {nextValue}");
                        OnValueEmitted?.Invoke(this, new GrammarEmitEventArgs(token, nextValue));
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
            else if (nextChar == 26)  //  EOF
            {
                scanToken = grammar.EndToken;
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
                        if (grammar.Terminals.Contains(testValue))
                        {
                            lastToken = testValue;
                            lastValue = testValue;
                            count++;
                        }
                        else
                        {
                            foreach (string token in grammar.Intrinsics.Keys)
                            {
                                string pattern = grammar.Intrinsics[token];
                                if (Regex.IsMatch(testValue, "^" + pattern + "$"))
                                {
                                    lastToken = token;
                                    lastValue = testValue;
                                    count++;
                                }
                            }
                        }
					}
					if (!foundStart && count > 0)
					{
						foundStart = true;
					}
					if (foundStart && count == 0)  //  Current test failed
                    {
						scanToken = lastToken;
						nextValue = lastValue;
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
                    if (grammar.Terminals.Contains(nextValue))
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
        OnLogMessageEmitted?.Invoke(this, new GrammarLogEventArgs("Verbose", category, message));
    }

    #endregion
}
