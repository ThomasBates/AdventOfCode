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
    public class ParserEventArgs : EventArgs
    {
        private string _token;
        private string _value;


        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="ParserEventArgs"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="value">The value.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public ParserEventArgs(string token, string value)
        {
            _token = token;
            _value = value;
        }


        #endregion
        #region Properties


        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public string Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public string Value
        {
            get { return _value; }
        }


        #endregion
    }
}
