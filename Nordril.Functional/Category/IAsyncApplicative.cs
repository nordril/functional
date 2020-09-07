using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An <see cref="IApplicative{TSource}"/> which supports asynchronous operations.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the applicative.</typeparam>
    public interface IAsyncApplicative<TSource> : IApplicative<TSource>, IAsyncFunctor<TSource>
    {
        /// <summary>
        /// The asynchronous version of <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the value to wrap.</typeparam>
        /// <param name="x">The value to wrap.</param>
        Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x);

        /// <summary>
        /// The asynchronous version of <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the applicative.</param>
        Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<TSource, Task<TResult>>> f);
    }
}
