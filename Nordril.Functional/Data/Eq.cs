using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// Lifting to structural equality.
    /// </summary>
    public static class Eq
    {
        /// <summary>
        /// Creates an <see cref="IEquatable{T}"/> out of a value and an equality comparison function.
        /// </summary>
        /// <typeparam name="T">The type of the value to wrap.</typeparam>
        /// <param name="value">The value to wrap.</param>
        /// <param name="equals">The equality comparison function.</param>
        public static Eq<T> Make<T>(T value, Func<T, T, bool> equals)
            => new Eq<T>(value, equals);
    }

    /// <summary>
    /// An <see cref="IEquatable{T}"/>-wrapper around a type.
    /// </summary>
    /// <typeparam name="T">The type of the wrapped element.</typeparam>
    public struct Eq<T> : IEquatable<Eq<T>>
    {
        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public T Value { get; }

        private readonly Func<T, T, bool> equals;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">The underlying value.</param>
        /// <param name="equals">The equality comparison function.</param>
        public Eq(T value, Func<T, T, bool> equals)
        {
            Value = value;
            this.equals = equals;
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Eq<T> other)
        {
            if (Value is null && other.Value is null)
                return true;
            else if (Value is null != other.Value is null)
                return false;
            else return equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Eq<T> that))
                return false;

            return Equals(that);
        }

        /// <inheritdoc />
        public static bool operator ==(Eq<T> t1, Eq<T> t2)
        {
            if (t1.Value is null && t2.Value is null)
                return true;
            else if (t1.Value is null != t2.Value is null)
                return false;
            else return t1.Value.Equals(t2.Value);
        }

        /// <inheritdoc />
        public static bool operator !=(Eq<T> t1, Eq<T> t2) => !(t1 == t2);

        /// <inheritdoc />
        public override int GetHashCode()
            => Value is null ? 0 : Value.GetHashCode();
    }
}
