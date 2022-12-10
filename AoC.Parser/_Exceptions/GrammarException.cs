using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace AoC.Parser
{
    /// <summary>
    /// GrammarException is thrown when an error occurs in the Grammar.
    /// </summary>
    /// <created>20/02/2009</created>
    /// <author>Thomas_Bates</author>
    public class GrammarException : Exception
    {
        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarException"/> class.
        /// </summary>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public GrammarException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarException"/> class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public GrammarException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized
        ///     object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual
        ///     information about the source or destination.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        protected GrammarException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarException"/> class
        /// with a specified error message and a reference to the inner exception 
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <created>20/02/2009</created>
        /// <author>Thomas_Bates</author>
        public GrammarException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        #endregion
    }
}
