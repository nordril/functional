using Nordril.Functional.Category;
using System;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A const-functor that does not actually contain any values.
    /// </summary>
    /// <typeparam name="T">The type of value by which to tag the functor. No values are actually present.</typeparam>
    public struct Const<T> : IPhantomFunctor<T>, IEquatable<Const<T>>
    {
        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, T> f) => new Const<TResult>();

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new Const<TResult>();

        /// <inheritdoc />
        public override bool Equals(object obj) => (obj != null && (obj is Const<T>));

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash();

        /// <inheritdoc />
        public static bool operator ==(Const<T> left, Const<T> right) => true;

        /// <inheritdoc />
        public static bool operator !=(Const<T> left, Const<T> right) => false;

        /// <inheritdoc />
        public bool Equals(Const<T> other) => Equals((object)other);
    }

    /// <summary>
    /// A const-functor that contains a "real" value, but which is a functor according to a
    /// second value which is does not actually contain.
    /// This is analogous to <see cref="Const{T}"/>, with the difference that we have a "hidden" value
    /// that does not influence the functor-instance.
    /// </summary>
    /// <typeparam name="TReal">The actual value contained in the functor.</typeparam>
    /// <typeparam name="TPhantom">The value which is only present as a phantom.</typeparam>
    public struct Const<TReal, TPhantom> : IPhantomFunctor<TPhantom>, IEquatable<Const<TReal, TPhantom>>
    {
        /// <summary>
        /// The value contained in the functor.
        /// </summary>
        public TReal RealValue { get; private set; }

        /// <summary>
        /// Creates a new const-instance with a value.
        /// </summary>
        /// <param name="realValue">The value to store in the functor.</param>
        public Const(TReal realValue)
        {
            RealValue = realValue;
        }

        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, TPhantom> f) => new Const<TReal, TResult>(RealValue);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TPhantom, TResult> f) => new Const<TReal, TResult>(RealValue);

        /// <summary>
        /// Determines equality based on <see cref="RealValue"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Const<TReal, TPhantom>))
                return false;

            var thatConst = (Const<TReal, TPhantom>)obj;

            return RealValue.Equals(thatConst.RealValue);
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(RealValue);

        /// <inheritdoc />
        public static bool operator ==(Const<TReal, TPhantom> left, Const<TReal, TPhantom> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Const<TReal, TPhantom> left, Const<TReal, TPhantom> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Const<TReal, TPhantom> other) => Equals((object)other);
    }
}
