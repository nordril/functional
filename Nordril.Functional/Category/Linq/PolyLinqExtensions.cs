using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category.Linq
{
    /// <summary>
    /// LINQ methods for generic <see cref="IFunctor{TSource}"/>s/<see cref="IMonad{TSource}"/>s, enabling the writing of LINQ-queries.
    /// </summary>
    public static class PolyLinqExtensions
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>. The functor-type is preserved, but checking for a concrete type is the responsibility of the caller.
        /// Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static IFunctor<TResult> Select<TSource, TResult>(this IFunctor<TSource> source, Func<TSource, TResult> f)
            => source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>. The monad-type is preserved, but checking that every computation in the query is in the same monad, and checking for a concrete type are the responsibility of the caller.
        /// Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static IMonad<TResult> SelectMany<TSource, TMiddle, TResult>
            (this IMonad<TSource> source,
             Func<TSource, IMonad<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => source.Bind(x => (IMonad<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IAsyncFunctor{TSource}.MapAsync{TResult}(Func{TSource, Task{TResult}})"/>. The functor-type is preserved, but checking for a concrete type is the responsibility of the caller.
        /// Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<IAsyncFunctor<TResult>> Select<TSource, TResult>(this Task<IAsyncFunctor<TSource>> source, Func<TSource, TResult> f)
            => (IAsyncFunctor<TResult>)PolyLinqExtensions.Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IAsyncFunctor{TSource}.MapAsync{TResult}(Func{TSource, Task{TResult}})"/>. The functor-type is preserved, but checking for a concrete type is the responsibility of the caller.
        /// Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<IAsyncFunctor<TResult>> Select<TSource, TResult>(this Task<IAsyncApplicative<TSource>> source, Func<TSource, TResult> f)
            => (IAsyncFunctor<TResult>)PolyLinqExtensions.Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IAsyncFunctor{TSource}.MapAsync{TResult}(Func{TSource, Task{TResult}})"/>. The functor-type is preserved, but checking for a concrete type is the responsibility of the caller.
        /// Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<IAsyncFunctor<TResult>> Select<TSource, TResult>(this Task<IAsyncMonad<TSource>> source, Func<TSource, TResult> f)
            => (IAsyncFunctor<TResult>)PolyLinqExtensions.Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IAsyncMonad{TSource}.BindAsync{TResult}(Func{TSource, Task{IAsyncMonad{TResult}}})"/>. The monad-type is preserved, but checking that every computation in the query is in the same monad, and checking for a concrete type are the responsibility of the caller.
        /// Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<IAsyncMonad<TResult>> SelectMany<TSource, TMiddle, TResult>
            (this Task<IAsyncMonad<TSource>> source,
             Func<TSource, Task<IAsyncMonad<TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

    }
}
