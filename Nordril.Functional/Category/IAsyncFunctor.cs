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
}
