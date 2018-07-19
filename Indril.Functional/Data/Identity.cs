using Indril.Functional.Category;
using Indril.TypeToolkit;
using System;

namespace Indril.Functional.Data
{
    /// <summary>
    /// The identity functor which does nothing but wrap a value.
    /// </summary>
    /// <typeparam name="T">The type of the value being wrapped.</typeparam>
    public struct Identity<T> : IMonad<T>, IEquatable<Identity<T>>
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
    }
}
