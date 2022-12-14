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
    public interface IGrammar
    {
		/// <summary>
		/// Occurs when log message emitted.
		/// </summary>
		public event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;


		/// <summary>
		/// Reads the grammar string.
		/// </summary>
		/// <param name="grammarDefinition">The grammar.</param>
		/// <created>19/02/2009</created>
		/// <author>Thomas_Bates</author>
		void ReadGrammarDefinition(string grammarDefinition);
	}
}
