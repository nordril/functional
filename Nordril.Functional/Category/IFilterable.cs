using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// Filterable data structures. A filterable structure supports a <see cref="Filter(Func{TSource, bool})"/>-method which returns a filtered copy of the structure, with exactly the elements which fulfill a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TSource">The type of element in the structure.</typeparam>
    public interface IFilterable<T, TSource> : ISemifilterable<T, TSource>
        where T : IFoldable<TSource>, IFilterable<T, TSource>
    {
        /// <summary>
        /// Filters the container's elements, leaving the original unchanged and returning a version of the container which have the same structure as the original, but only with those elements which fulfill <paramref name="f"/>. What "leaving out elements" means exactly, depends on the implementor.
        /// </summary>
        /// <param name="f">The predicate.</param>
        T Filter(Func<TSource, bool> f);
    }

    /// <summary>
    /// Filterable data structures which do not support the removal of all elements. This is a weaker form of <see cref="IFilterable{TSource}"/> which does not support returning empty data structures if all elements have been excluded, but returns <see cref="Maybe.Nothing{T}"/> instead.
    /// An example is <see cref="Tree{T}"/>, which cannot represent empty trees, but which still supports a kind of filtering of its nodes.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TSource">The type of element in the structure.</typeparam>
    public interface ISemifilterable<T, TSource> : IFoldable<TSource>
        where T : IFoldable<TSource>, ISemifilterable<T, TSource>
    {
        /// <summary>
        /// Filters the container's elements, leaving the original unchanged and returning a version of the container which have the same structure as the original, but only with those elements which fulfill <paramref name="f"/>. What "leaving out elements" means exactly, depends on the implementor.
        /// If all elements have been excluded by <paramref name="f"/>, <see cref="Maybe.Nothing{T}"/> is returned.
        /// </summary>
        /// <param name="f">The predicate.</param>
        Maybe<T> Semifilter(Func<TSource, bool> f);
    }
}
