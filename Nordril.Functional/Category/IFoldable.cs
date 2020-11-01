using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// Data structures that suppoort folding, i.e. aggregation over their members.
    /// <see cref="IFoldable{TSource}"/> is basically just <see cref="IEnumerable{T}"/>, with extra functions.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the data structure.</typeparam>
    public interface IFoldable<out TSource> : IFunctor<TSource>, IEnumerable<TSource>
    {
        /// <summary>
        /// Maps each element of the structure to a monoid and then combines the elements
        /// using the monoid's <see cref="IMagma{T}.Op(T, T)"/> operation.
        /// </summary>
        /// <param name="monoid">The monoid dictionary that should be used.</param>
        /// <param name="f">The function that maps an element to a monoid.</param>
        T FoldMap<T>(IMonoid<T> monoid, Func<TSource, T> f);

        /// <summary>
        /// Right-associative fold of the data structure.
        /// </summary>
        /// <typeparam name="TResult">The result type of the fold.</typeparam>
        /// <param name="f">The aggregation function, which takes an element of the structure and the current accumulator value,
        /// and returns the new accumulator value.</param>
        /// <param name="accumulator">The initial accumulator value.</param>
        TResult Foldr<TResult>(Func<TSource, TResult, TResult> f, TResult accumulator);
    }

    /// <summary>
    /// Extension methods for <see cref="IFoldable{TSource}"/>.
    /// </summary>
    public static class Foldable
    {
        /// <summary>
        /// Maps each element of the structure to a monoid and then combines the elements
        /// using the monoid's <see cref="IMagma{T}.Op(T, T)"/> operation.
        /// </summary>
        /// <typeparam name="TSource">The type of elements contained in <paramref name="foldable"/>.</typeparam>
        /// <typeparam name="TMonoid">The monoid to which to mape the elements.</typeparam>
        /// <param name="foldable">The object to fold.</param>
        /// <param name="empty">The neutral element of the monoid.</param>
        /// <param name="f">The function that maps an element to a monoid.</param>
        public static TMonoid FoldMap<TSource, TMonoid>(this IFoldable<TSource> foldable, Func<TMonoid> empty, Func<TSource, TMonoid> f) where TMonoid : IMonoid<TMonoid>
        => foldable.FoldMap(new Monoid<TMonoid>(empty(), (x, y) => x.Op(y)), f);

        /// <summary>
        /// A limited form of traversal which traverses a <see cref="IFoldable{TSource}"/> structure and discards the results.
        /// </summary>
        /// <typeparam name="TApplicative">The type of the applicative.</typeparam>
        /// <typeparam name="TSource">The type of the element in the input structure.</typeparam>
        /// <typeparam name="TResult">The type of the result of the action.</typeparam>
        /// <param name="foldable">The object to fold.</param>
        /// <param name="f">The function to apply to each contained element.</param>
        public static TApplicative TraverseDiscard<TApplicative, TSource, TResult>(IFoldable<TSource> foldable, Func<TSource, IApplicative<TResult>> f)
            where TApplicative : IApplicative<Unit>
            => (TApplicative)TraverseDiscard(foldable, typeof(TApplicative), f);

        /// <summary>
        /// A limited form of traversal which traverses a <see cref="IFoldable{TSource}"/> structure and discards the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the element in the input structure.</typeparam>
        /// <typeparam name="TResult">The type of the result of the action.</typeparam>
        /// <param name="applicative">The type of the applicative.</param>
        /// <param name="foldable">The object to fold.</param>
        /// <param name="f">The function to apply to each contained element.</param>
        public static IApplicative<Unit> TraverseDiscard<TSource, TResult>(
            this IFoldable<TSource> foldable,
            Type applicative,
            Func<TSource, IApplicative<TResult>> f)
        {
            return foldable.Foldr(
                (x, acc) => f(x).ApRight(acc),
                Applicative.PureUnsafe(new Unit(), applicative));
        }
    }
}
