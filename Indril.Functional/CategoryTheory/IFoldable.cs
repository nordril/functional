using Indril.Functional.Algebra;
using System;
using System.Collections.Generic;

namespace Indril.Functional.CategoryTheory
{
    /// <summary>
    /// Data structures that suppoort folding, i.e. aggregation over their members.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the data structure.</typeparam>
    public interface IFoldable<TSource> : IFunctor<TSource>, IEnumerable<TSource>
    {
        /// <summary>
        /// Maps each element of the structure to a monoid and then combines the elements
        /// using the monoid's <see cref="IMagma{T}.Op(T)"/> operation.
        /// </summary>
        /// <typeparam name="TMonoid">The monoid to which to mape the elements.</typeparam>
        /// <param name="f">The function that maps an element to a monoid.</param>
        TMonoid FoldMap<TMonoid>(Func<TSource, TMonoid> f) where TMonoid : IMonoid<TMonoid>;

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
    public static class FoldableExtensions
    {
        //TODO : FoldableExtensions (foldl, sequence, length, etc.)
    }
}
