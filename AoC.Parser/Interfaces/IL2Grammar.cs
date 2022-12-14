using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC.Parser
{
    /// <summary>
    /// L2GrammarBase 
    /// </summary>
    /// <created>19/02/2009</created>
    /// <author>Thomas_Bates</author>
    public interface IL2Grammar : IGrammar
    {

		/// <summary>
		/// Gets the definitions.
		/// </summary>
		/// <value>The definitions.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public Dictionary<string, string> Definitions { get; }

		/// <summary>
		/// Gets the regular-expression patterns for the intrinsic tokens.
		/// </summary>
		/// <value>The patterns.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public Dictionary<string, string> Patterns { get; }

		/// <summary>
		/// Gets the intrinsics.
		/// </summary>
		/// <value>The intrinsics.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> Intrinsics { get; }

		/// <summary>
		/// Gets the keywords.
		/// </summary>
		/// <value>The keywords.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> Keywords { get; }

		/// <summary>
		/// Gets the symbols.
		/// </summary>
		/// <value>The symbols.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> Symbols { get; }

		/// <summary>
		/// Gets the parse tokens.
		/// </summary>
		/// <value>The parse tokens.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> ParseTokens { get; }

		/// <summary>
		/// Gets the code tokens.
		/// </summary>
		/// <value>The code tokens.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> CodeTokens { get; }

		/// <summary>
		/// Gets all tokens.
		/// </summary>
		/// <value>All tokens.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string> AllTokens { get; }

		/// <summary>
		/// Gets the first prod list.
		/// </summary>
		/// <value>The first prod list.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<int> FirstProdList { get; }

		/// <summary>
		/// Gets the first token list.
		/// </summary>
		/// <value>The first token list.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<int> FirstTokenList { get; }

		/// <summary>
		/// Gets the directors.
		/// </summary>
		/// <value>The directors.</value>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public List<string>[] Directors { get; }
	}
}
