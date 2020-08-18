using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// Indicates that a string-key was already present when an attempt was made to
    /// insert it into a collection.
    /// </summary>
    public class KeyAlreadyPresentException : Exception
    {
        /// <summary>
        /// The key or its string-form.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Creates a new exception.
        /// </summary>
        /// <param name="key">The value of the duplicate key.</param>
        public KeyAlreadyPresentException(string key) : base()
        {
            Key = key;
        }

        /// <summary>
        /// Creates a new exception.
        /// </summary>
        public KeyAlreadyPresentException()
        {
        }

        /// <summary>
        /// Creates a new exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public KeyAlreadyPresentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
