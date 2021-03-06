﻿using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The identity functor which does nothing but wrap a value.
    /// This is equivalent to a one-value <see cref="Either{TLeft, TRight}"/>.
    /// Available as a data-source in LINQ-queries.
    /// </summary>
    /// <typeparam name="T">The type of the value being wrapped.</typeparam>
    public struct Identity<T>
        : IMonad<T>
        , IAsyncMonad<T>
        , ITraversable<T>
        , IEquatable<Identity<T>>
        , ICoproductFirst<T>
        , IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets the wrapped value.
        /// </summary>
        public T Value { get; set; }

        /// <inheritdoc />
        public bool IsFirst => true;

        /// <inheritdoc />
        public Maybe<T> First => Maybe.Just(Value);

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
            if (f == null || !(f is Identity<Func<T, TResult>> fIdentity))
                throw new InvalidCastException();

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

        /// <summary>
        /// Extends this 1-elementcoproduct with an additional component, returning the a copy of the original coproduct if <paramref name="value"/> is <see cref="Maybe.Nothing{T}"/> and a coproduct containing <paramref name="value"/> if it is <see cref="Maybe.Just{T}(T)"/>.
        /// </summary>
        /// <typeparam name="TNew">The type by which to extend this coproduct.</typeparam>
        /// <param name="value">The optional value by which to extend this coproduct.</param>
        public Either<T, TNew> Extend<TNew>(Maybe<TNew> value)
        {
            var y = Value;

            return value.ValueOrLazy(
                  x => new Either<T, TNew>(new Either2<TNew>(x)),
                  () => new Either<T, TNew>(new Either1<T>(y)));
        }

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

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
            => await f(Value);

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => Pure(await x());

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            if (f == null || !(f is Identity<Func<T, Task<TResult>>> fIdentity))
                throw new InvalidCastException();

            return new Identity<TResult>(await fIdentity.Value(Value));
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
            => new Identity<TResult>(await f(Value));

        /// <inheritdoc />
        public IApplicative<ITraversable<TResult>> Traverse<TApplicative, TResult>(Func<T, TApplicative> f) where TApplicative : IApplicative<TResult>
        => (IApplicative<ITraversable<TResult>>)f(Value).Map(x => (ITraversable<TResult>)new Identity<TResult>(x));

        /// <inheritdoc />
        public IApplicative<ITraversable<TResult>> Traverse<TResult>(Type applicative, Func<T, IApplicative<TResult>> f)
            => (IApplicative<ITraversable<TResult>>)f(Value).Map(x => (ITraversable<TResult>)new Identity<TResult>(x));

        /// <inheritdoc />
        public T1 FoldMap<T1>(IMonoid<T1> monoid, Func<T, T1> f)
            => f(Value);

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
            => f(Value, accumulator);
    }

    /// <summary>
    /// Extension methods for <see cref="Identity{T}"/>.
    /// </summary>
    public static class Identity
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Identity{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Identity<TResult> Select<TSource, TResult>(this Identity<TSource> source, Func<TSource, TResult> f)
            => (Identity<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Io{TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Identity<TResult> SelectMany<TSource, TMiddle, TResult>
            (this Identity<TSource> source,
             Func<TSource, Identity<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
        {
            var sourceRes = source.Value;
            return new Identity<TResult>(resultSelector(sourceRes, f(sourceRes).Value));
        }

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Identity{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Identity<TResult>> Select<TSource, TResult>(
            this Task<Identity<TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Creates a new <see cref="Identity{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Identity<TResult> Make<TResult>(TResult x) => new Identity<TResult>(x);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Identity{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Identity<TResult>> SelectMany<TSource, TMiddle, TResult>
            (this Task<Identity<TSource>> source,
             Func<TSource, Task<Identity<TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Identity<TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// An isomorphism between <see cref="Identity{T}"/> and any type.
        /// </summary>
        /// <typeparam name="T">The underlying type to wrap.</typeparam>
        private class IdentityIso<T> : IIsomorphism<T, Identity<T>>
        {
            public T ConvertBackWith(Unit _, Identity<T> from) => from.Value;

            public Identity<T> ConvertWith(Unit _, T from) => new Identity<T>(from);
        }

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to an <see cref="Identity{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Identity<T> ToIdentity<T>(this IFunctor<T> x) => (Identity<T>)x;

        /// <summary>
        /// Returns an isomorphism between <see cref="Func{TResult}"/> and <see cref="Io{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result of computation.</typeparam>
        public static IIsomorphism<T, Identity<T>> Iso<T>() => new IdentityIso<T>();
    }
}
