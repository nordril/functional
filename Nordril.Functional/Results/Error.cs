using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// Represents an error in the application, with a message and a target, plus an optional system-exception.
    /// </summary>
    public struct Error : IEquatable<Error>
    {
        /// <summary>
        /// The unique code of the error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The underlying system exception, if applicable.
        /// </summary>
        public Maybe<Exception> InnerException { get; set; }

        /// <summary>
        /// The human-readable error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The target of the error; i.e. the thing that was faulty (like a property or the name of an API method).
        /// </summary>
        public Maybe<string> Target { get; set; }

        /// <summary>
        /// Creates a new error out of a message and an optional code, target and exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The unique error code; null if not present.</param>
        /// <param name="target">The target; null if not present.</param>
        /// <param name="exception">The exception; null if not present.</param>
        public Error(string message, Enum code, string target = null, Exception exception = null)
        {
            Message = message;
            Target = Maybe.JustIf(target != null, () => target);
            InnerException = Maybe.JustIf(exception != null, () => exception);
            Code = code?.ToString() ?? "";
        }

        /// <summary>
        /// Creates a new error out of a message and an optional code, target and exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The unique error code.</param>
        /// <param name="target">The target; null if not present.</param>
        /// <param name="exception">The exception; null if not present.</param>
        public Error(string message, int code, string target = null, Exception exception = null)
        {
            Message = message;
            Target = Maybe.JustIf(target != null, () => target);
            InnerException = Maybe.JustIf(exception != null, () => exception);
            Code = code.ToString();
        }

        /// <summary>
        /// Creates a new error out of a message and an optional code, target and exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The unique error code; null if not present.</param>
        /// <param name="target">The target; null if not present.</param>
        /// <param name="exception">The exception; null if not present.</param>
        public Error(string message, string code, string target = null, Exception exception = null)
        {
            Message = message;
            Target = Maybe.JustIf(target != null, () => target);
            InnerException = Maybe.JustIf(exception != null, () => exception);
            Code = code;
        }

        /// <summary>
        /// Creates a new error out of a message and an optional code, target and exception.
        /// </summary>
        /// <param name="msg">The tuple of error message and code.</param>
        /// <param name="target">The target; null if not present.</param>
        /// <param name="exception">The exception; null if not present.</param>
        public Error((string message, Enum code) msg, string target = null, Exception exception = null)
            : this(msg.message, msg.code, target, exception)
        {

        }

        /// <summary>
        /// Determines equality based on <see cref="Code"/>, <see cref="Message"/>, <see cref="InnerException"/>, and <see cref="Target"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Error))
                return false;

            var thatError = (Error)obj;

            return (Code.Equals(thatError.Code)
                && Message.Equals(thatError.Message, StringComparison.InvariantCulture)
                && Target.Equals(thatError.Target)
                && InnerException.Equals(thatError.InnerException));
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(Code, Message, InnerException, Target);

        /// <inheritdoc />
        public static bool operator ==(Error left, Error right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Error left, Error right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Error other) => Equals((object)other);

        /// <inheritdoc />
        public override string ToString()
        {
            var target = Target.HasValue ? $"(target: {Target.Value()})" : "";
            return $"{Code} {target}: {Message}.";
        }
    }
}
