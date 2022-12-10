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
    public class L2ParserBase
    {
        private L2GrammarBase _grammar;


        #region Events


        /// <summary>
        /// Occurs when [intrinsic token emitted].
        /// </summary>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public event EventHandler<ParserEventArgs> ValueEmitted;

        /// <summary>
        /// Occurs when [code token emitted].
        /// </summary>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public event EventHandler<ParserEventArgs> TokenEmitted;


        #endregion
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="L2ParserBase"/> class.
        /// </summary>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2ParserBase()
        {
            _grammar = CreateGrammar();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="L2ParserBase"/> class.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <created>23/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2ParserBase(L2GrammarBase grammar)
        {
            _grammar = grammar;
        }


        #endregion
        #region Properties


        /// <summary>
        /// Gets the grammar.
        /// </summary>
        /// <value>The grammar.</value>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public L2GrammarBase Grammar
        {
            get { return _grammar; }
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
        protected virtual L2GrammarBase CreateGrammar()
        {
            return new L2GrammarBase();
        }


        #endregion
        #region Public Access Methods


        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public virtual void Parse(string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(buffer);
            Parse(stream);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <created>19/02/2009</created>
        /// <author>Thomas_Bates</author>
        public virtual void Parse(Stream input)
        {
        }


        #endregion
        #region Event Handlers


        /// <summary>
        /// Raises the <see cref="E:ValueEmitted"/> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="AoC.Parser.ParserEventArgs"/> instance containing the event data.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void OnValueEmitted(ParserEventArgs eventArgs)
        {
            EventHandler<ParserEventArgs> handler = ValueEmitted;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TokenEmitted"/> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="AoC.Parser.ParserEventArgs"/> instance containing the event data.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected virtual void OnTokenEmitted(ParserEventArgs eventArgs)
        {
            EventHandler<ParserEventArgs> handler = TokenEmitted;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }


        #endregion
    }
}
