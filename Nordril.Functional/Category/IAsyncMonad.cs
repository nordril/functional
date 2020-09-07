using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An <see cref="IMonad{TSource}"/> which supports asynchronous operations.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the monad.</typeparam>
    public interface IAsyncMonad<TSource> : IMonad<TSource>, IAsyncApplicative<TSource>
    {
        /// <summary>
        /// The asynchronous version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the monad.</param>
        Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<TSource, Task<IAsyncMonad<TResult>>> f);
    }
}
