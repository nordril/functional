using Nordril.Functional.Category;
using Nordril.HedgingEngine.Logic.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The identity functor which does nothing but wrap a value.
    /// Available as a data-source in LINQ-queries.
    /// </summary>
    /// <typeparam name="T">The type of the value being wrapped.</typeparam>
    public struct Identity<T> : IMonad<T>, IEquatable<Identity<T>>, IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets the wrapped value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Creates a new identity.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Identity(T value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is Identity<Func<T, TResult>>))
                throw new InvalidCastException();

            var fIdentity = (Identity<Func<T, TResult>>)f;

            return new Identity<TResult>(fIdentity.Value(Value));
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f) => f(Value);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new Identity<TResult>(f(Value));

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new Identity<TResult>(x);

        /// <summary>
        /// Determines equality based on the underlying <see cref="Identity{T}.Value"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Identity<T>))
                return false;

            var thatIdentity = (Identity<T>)obj;

            return Value.Equals(thatIdentity.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(Value);

        /// <inheritdoc />
        public static bool operator ==(Identity<T> left, Identity<T> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Identity<T> left, Identity<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Identity<T> other) => Equals((object)other);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            yield return Value;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Identity{T}"/>.
    /// </summary>
    public static class Identity
    {
        /// <summary>
        /// An isomorphism between <see cref="Identity{T}"/> and any type.
        /// </summary>
        /// <typeparam name="T">The underlying type to wrap.</typeparam>
        private class IdentityIso<T> : IIsomorphism<T, Identity<T>>
        {
            public T ConvertBack(Identity<T> from) => from.Value;

            public Identity<T> Convert(T from) => new Identity<T>(from);
        }

        /// <summary>
        /// An isomorphism between <see cref="Io{T}"/> and <see cref="Func{T, TResult}"/>.
        /// </summary>
        /// <typeparam name="T">The underlying type to wrap.</typeparam>
        private class IoIso<T> : IIsomorphism<Func<T>, Io<T>>
        {
            public Func<T> ConvertBack(Io<T> from) => () => from.Run();

            public Io<T> Convert(Func<T> from) => new Io<T>(from);
        }

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to an <see cref="Identity{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Identity<T> ToIdentity<T>(IFunctor<T> x) => (Identity<T>)x;

        /// <summary>
        /// Returns an isomorphism between <see cref="Func{TResult}"/> and <see cref="Io{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result of computation.</typeparam>
        public static IIsomorphism<T, Identity<T>> Iso<T>() => new IdentityIso<T>();
    }
}
