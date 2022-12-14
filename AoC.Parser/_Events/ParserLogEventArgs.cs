using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Parser
{
    /// <summary>
    /// ParserEventArgs 
    /// </summary>
    /// <created>20/02/2009</created>
    /// <author>Thomas_Bates</author>
    public class ParserLogEventArgs : EventArgs
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParserLogEventArgs"/> class.
		/// </summary>
		/// <param name="severity">The token.</param>
		/// <param name="category">The value.</param>
		/// <param name="message"></param>
		/// <created>20/02/2009</created>
		/// <author>Thomas_Bates</author>
		public ParserLogEventArgs(string severity, string category, string message)
        {
            Severity = severity;
            Category = category;
            Message = message;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets the logging severity.
        /// </summary>
        public string Severity { get; }

        /// <summary>
        /// Gets the loggin category.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the logging message.
        /// </summary>
        public string Message { get; }

        #endregion
    }
}
