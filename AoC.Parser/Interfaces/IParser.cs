using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC.Parser
{
    /// <summary>
    /// L2ParserBase 
    /// </summary>
    /// <created>19/02/2009</created>
    /// <author>Thomas_Bates</author>
    public interface IParser
    {
        #region IParser Events

        /// <summary>
        /// Occurs when [intrinsic token emitted].
        /// </summary>
        event EventHandler<ParserEventArgs> OnValueEmitted;

        /// <summary>
        /// Occurs when [code token emitted].
        /// </summary>
        event EventHandler<ParserEventArgs> OnTokenEmitted;

        /// <summary>
        /// Occurs when log message emitted.
        /// </summary>
        event EventHandler<ParserLogEventArgs> OnLogMessageEmitted;

        #endregion
        #region IParser Methods

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        void Parse(string input);

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        void Parse(Stream input);

        #endregion
    }
}
