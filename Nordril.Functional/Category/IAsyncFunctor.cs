using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An <see cref="IFunctor{TSource}"/> which supports asynchronous operations.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the functor.</typeparam>
    public interface IAsyncFunctor<TSource> : IFunctor<TSource>
    {
        /// <summary>
        /// The asynchronous version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="f">The function to apply to the function.</param>
        Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<TSource, Task<TResult>> f);
    }

    /// <summary>
    /// Extension methods for <see cref="IAsyncFunctor{TSource}"/>
    /// </summary>
    public static class AsyncFunctor
    {
        /// <summary>
        /// The asynchronous version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="task">The source on whose result to run <see cref="IAsyncFunctor{TSource}.MapAsync{TResult}(Func{TSource, Task{TResult}})"/>.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<IAsyncFunctor<TResult>> MapAsync<TSource, TResult>(this Task<IAsyncFunctor<TSource>> task, Func<TSource, Task<TResult>> f)
        {
            var m = await task;
            return await m.MapAsync(f);
        }
    }
}
