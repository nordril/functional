using Nordril.Functional.Category;
using System;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A const-functor that does not actually contain any values.
    /// </summary>
    /// <typeparam name="T">The type of value by which to tag the functor. No values are actually present.</typeparam>
    public struct Const<T> : IPhantomFunctor<T>, IEquatable<Const<T>>, IMonad<T>
    {
        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, T> f) => new Const<TResult>();

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new Const<TResult>();

        /// <inheritdoc />
        public override bool Equals(object obj) => (!(obj is null) && (obj is Const<T>));

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash();

        /// <inheritdoc />
        public static bool operator ==(Const<T> left, Const<T> right) => true;

        /// <inheritdoc />
        public static bool operator !=(Const<T> left, Const<T> right) => false;

        /// <inheritdoc />
        public bool Equals(Const<T> other) => Equals((object)other);

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
            => new Const<TResult>();

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
           => new Const<TResult>();

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
            => new Const<TResult>();
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

    /// <summary>
    /// Extension methods for <see cref="Const{T}"/>.
    /// </summary>
    public static class Const
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Const{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Const<TResult> Select<TSource, TResult>(this Const<TSource> source, Func<TSource, TResult> f)
            => (Const<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Const{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Const<TResult>> Select<TSource, TResult>(
            this Task<Const<TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Const{TReal, TPhantom}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TReal">The real value in the source.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Const<TReal, TResult> Select<TReal, TSource, TResult>(this Const<TReal, TSource> source, Func<TSource, TResult> f)
            => (Const<TReal, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Const{TReal, TPhantom}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TReal">The real value in the source.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Const<TReal, TResult> SelectMany<TReal, TSource, TMiddle, TResult>
            (this Const<TReal, TSource> source,
             Func<TSource, Const<TReal, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector) =>
            new Const<TReal, TResult>();

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to a <see cref="Const{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Const<T> ToConst<T>(this IFunctor<T> x) => (Const<T>)x;

        /// <summary>
        /// Unsafely casts an <see cref="IContravariant{TSource}"/> to a <see cref="Const{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Const<T> ToConst<T>(this IContravariant<T> x) => (Const<T>)x;
    }
}
