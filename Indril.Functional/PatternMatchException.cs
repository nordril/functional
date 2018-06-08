using Indril.Functional.Data;
using System;

namespace Indril.Functional
{
    /// <summary>
    /// The object didn't correspond to the asked-for pattern. An example would be <see cref="Maybe{T}"/> whose value
    /// was requested but which contained nothing.
    /// </summary>
    public class PatternMatchException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="PatternMatchException"/> with a standard message.
        /// </summary>
        /// <param name="requested">The requested member.</param>
        /// <param name="className">The name of the class.</param>
        /// <param name="actualPattern">The pattern that was actually present. This should be the name of a member whose call would've succeeded.</param>
        public PatternMatchException(string requested, string className, string actualPattern) : base($"{requested} called on {className}.{actualPattern}!") { }

        /// <summary>
        /// Creates a new <see cref="PatternMatchException"/>.
        /// </summary>
        public PatternMatchException() : base("Pattern match exception.")
        {
        }

        /// <summary>
        /// Creates a new <see cref="PatternMatchException"/> with a custom message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PatternMatchException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PatternMatchException"/> with a custom message an an inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PatternMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

