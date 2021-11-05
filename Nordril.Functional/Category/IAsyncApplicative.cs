using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        /// <summary>
        /// The asynchronous version of <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>. The default implementation ignores the <see cref="CancellationToken"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the applicative.</param>
        /// <param name="token">The cancellation token.</param>
        Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<TSource, Task<TResult>>> f, CancellationToken token)
            => ApAsync(f);
    }

    /// <summary>
    /// Extension methods for <see cref="IAsyncApplicative{TSource}"/>.
    /// </summary>
    public static class AsyncApplicative
    {
        /// <summary>
        /// The asynchronous version of <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="x">The task which returns the source-applicative.</param>
        /// <param name="f">The function to apply to the applicative.</param>
        public static async Task<IAsyncApplicative<TResult>> ApAsync<TSource, TResult>(
            this Task<IAsyncApplicative<TSource>> x,
            IApplicative<Func<TSource, Task<TResult>>> f)
        {
            var m = await x;
            return await m.ApAsync(f);
        }

        /// <summary>
        /// The asynchronous version of <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="x">The task which returns the source-applicative.</param>
        /// <param name="f">The function to apply to the applicative.</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task<IAsyncApplicative<TResult>> ApAsync<TSource, TResult>(
            this Task<IAsyncApplicative<TSource>> x,
            IApplicative<Func<TSource, Task<TResult>>> f,
            CancellationToken token)
        {
            var m = await x;
            return await m.ApAsync(f, token);
        }

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="IAsyncApplicative{TSource}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to an <see cref="IAsyncApplicative{TSource}"/>.</param>
        public static IAsyncApplicative<T> ToAsyncApplicative<T>(this IFunctor<T> f) => (IAsyncApplicative<T>)f;
    }
}
