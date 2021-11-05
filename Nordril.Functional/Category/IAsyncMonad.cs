using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nordril.Functional.Category.Linq;
using Nordril.Functional.Data;

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

        /// <summary>
        /// The asynchronous version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// The default implementation ignores the cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the monad.</param>
        /// <param name="token">The cancellation token.</param>
        Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<TSource, Task<IAsyncMonad<TResult>>> f, CancellationToken token)
            => BindAsync(f);
    }

    /// <summary>
    /// Extension methods for <see cref="IAsyncMonad{TSource}"/>.
    /// </summary>
    public static class AsyncMonad
    {
        /// <summary>
        /// The asynchronous version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>.
        /// </summary>
        /// <typeparam name="TMonad">The monad on which to bind.</typeparam>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="task">The task on whose result to run <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/></param>
        /// <param name="f">The function to apply to the monad.</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task<IAsyncMonad<TResult>> MapAsync<TMonad, TSource, TResult>(this Task<TMonad> task, Func<TSource, Task<TResult>> f, CancellationToken token = default)
            where TMonad : IAsyncMonad<TSource>
        {
            var m = await task;
            return (await m.MapAsync(f, token)).ToAsyncMonad();
        }

        /// <summary>
        /// The asynchronous version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TMonad">The monad on which to bind.</typeparam>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="task">The task on whose result to run <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/></param>
        /// <param name="f">The function to apply to the monad.</param>
        public static async Task<IAsyncMonad<TResult>> BindAsync<TMonad, TSource, TResult>(this Task<TMonad> task, Func<TSource, Task<IAsyncMonad<TResult>>> f)
            where TMonad : IAsyncMonad<TSource>
        {
            var m = await task;
            return await m.BindAsync(f);
        }

        /// <summary>
        /// The asynchronous version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TMonad">The monad on which to bind.</typeparam>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="task">The task on whose result to run <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/></param>
        /// <param name="f">The function to apply to the monad.</param>
        internal static async Task<IAsyncMonad<TResult>> BindAsync<TMonad, TSource, TResult>(this Task<TMonad> task, Func<Type, TSource, Task<IAsyncMonad<TResult>>> f, CancellationToken token)
            where TMonad : IAsyncMonad<TSource>
        {
            var m = await task;
            return await m.BindAsync(x => f(m.GetType(), x), token);
        }

        /// <summary>
        /// The asynchronous version of <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TMonad">The monad on which to bind.</typeparam>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="task">The task on whose result to run <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/></param>
        /// <param name="f">The function to apply to the monad.</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task<IAsyncMonad<TResult>> BindAsync<TMonad, TSource, TResult>(this Task<TMonad> task, Func<TSource, Task<IAsyncMonad<TResult>>> f, CancellationToken token)
            where TMonad : IAsyncMonad<TSource>
        {
            var m = await task;
            return await m.BindAsync(f, token);
        }

        /// <summary>
        /// Runs <paramref name="condition"/> repeatedly as long as it returns <c>true</c>, and runs <paramref name="body"/> each time, collecting the results.<br />
        /// Equivalent to:
        /// <code>
        /// while (await condition())
        /// {
        ///     yield return await body();
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TSource">The type of the seed/generated elements.</typeparam>
        /// <param name="condition">The asynchronous monadic function which computes the condition.</param>
        /// <param name="body">The monadic body.</param>
        /// <param name="token">The cancellation token.</param>
        public static Task<IAsyncMonad<IEnumerable<TSource>>> WhileMAsync<TSource>(
            this Task<IAsyncMonad<bool>> condition,
            Func<Task<IAsyncMonad<TSource>>> body,
            CancellationToken token = default)
        {
            Type conditionType = null;

            Task<IAsyncMonad<IEnumerable<TSource>>> go()
            {
                if (token.IsCancellationRequested && conditionType is not null)
                    return Task.FromResult(
                        Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), conditionType)
                        .ToAsyncMonad());

                var q = condition.BindAsync((Type t, bool conditionResult) =>
                {
                    conditionType ??= t;
                    return (conditionResult
                        ? (body().BindAsync((TSource bodyResult) =>
                            go().MapAsync((IEnumerable<TSource> rest) =>
                                Task.FromResult(rest.Prepend(bodyResult)), token), token))
                        : Task.FromResult(Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), t).ToAsyncMonad()));
                }, token);

                /*var q =
                    from conditionResult in condition
                    from results in conditionResult
                        ? from bodyResult in body()
                          from rest in go()
                          select rest.Prepend(bodyResult)
                        : Task.FromResult((IAsyncMonad<IEnumerable<TSource>>)Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), condition.GetType()))
                    select results;*/

                return q;
            }

            return go();
        }

        /// <summary>
        /// Runs <paramref name="computeCondition"/> repeatedly as long as <c>enterBody(computeCondition())</c>returns <c>true</c>, and runs <paramref name="body"/> each time, collecting the results.<br />
        /// Equivalent to:
        /// <code>
        /// do
        /// {
        ///    var cond = await condition();
        ///    if (enterBody(cond))
        ///       yield return await body(cond);
        ///    else
        ///       break;
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TCondition">The type of the condition</typeparam>
        /// <typeparam name="TSource">The type of the seed/generated elements.</typeparam>
        /// <param name="computeCondition">The asynchronous monadic function which computes the condition.</param>
        /// <param name="enterBody"></param>
        /// <param name="body">The monadic body.</param>
        /// <param name="token">The cancellation token.</param>
        public static Task<IAsyncMonad<IEnumerable<TSource>>> WhileMAsync<TCondition, TSource>(
            this Task<IAsyncMonad<TCondition>> computeCondition,
            Func<TCondition, bool> enterBody,
            Func<TCondition, Task<IAsyncMonad<TSource>>> body,
            CancellationToken token = default)
        {
            Type conditionType = null;

            Task<IAsyncMonad<IEnumerable<TSource>>> go()
            {
                if (token.IsCancellationRequested && conditionType is not null)
                    return Task.FromResult(Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), conditionType).ToAsyncMonad());

                var q = computeCondition.BindAsync((Type t, TCondition conditionResult) =>
                {
                    conditionType ??= t;
                    return (enterBody(conditionResult)
                        ? (body(conditionResult).BindAsync((TSource bodyResult) =>
                            go().MapAsync((IEnumerable<TSource> rest) =>
                                Task.FromResult(rest.Prepend(bodyResult)), token), token))
                        : Task.FromResult(Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), conditionType).ToAsyncMonad()));
                }, token);

                /*var q =
                    from conditionResult in computeCondition
                    from results in enterBody(conditionResult)
                        ? from bodyResult in body(conditionResult)
                          from rest in go()
                          select rest.Prepend(bodyResult)
                        : Task.FromResult((IAsyncMonad<IEnumerable<TSource>>)Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), computeCondition.GetType()))
                    select results;*/

                return q;
            }

            return go();
        }

        /// <summary>
        /// A asynchronous, monadic version of <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> which aggregates a sequence <paramref name="xs"/> with a monadic function <paramref name="f"/> and an accumulator <paramref name="acc"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TAcc">The accumulator type.</typeparam>
        /// <typeparam name="TMonadAcc"><typeparamref name="TAcc"/>, wrapped in an <see cref="IAsyncMonad{TSource}"/>.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="acc">The initial accumulator.</param>
        /// <param name="f">The folding function taking the aggregator and a sequence-element, and returning a monadic accumulator-value.</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task<TMonadAcc> AggregateMAsync<TSource, TAcc, TMonadAcc>(this IEnumerable<TSource> xs, TAcc acc, Func<TSource, TAcc, Task<TMonadAcc>> f,
            CancellationToken token = default)
            where TMonadAcc : IAsyncMonad<TAcc>
        {
            var pureAcc = acc.PureUnsafe<TAcc, TMonadAcc>();
            var res = pureAcc;

            foreach (var x in xs)
            {
                if (token.IsCancellationRequested)
                    break;
                res = (TMonadAcc)await res.BindAsync(async acc2 => (IAsyncMonad<TAcc>)await f(x, acc2), token);
            }

            return res;
        }

        /// <summary>
        /// An asynchronous monadic <see cref="CollectionExtensions.Unfold{TSeed, TResult}(TSeed, Func{TSeed, Maybe{ValueTuple{TSeed, TResult}}})"/> which generates a sequence each time the <paramref name="seed"/>-value evaluates to <see cref="Maybe.Just{T}(T)"/>.<br />
        /// Equivalent to:
        /// <code>
        /// while ((await seed()).TryGetValue(_, y)) { yield return y; }
        /// </code>
        /// </summary>
        /// <typeparam name="TSource">The type of the seed/generated elements.</typeparam>
        /// <param name="seed">The monadic seed-value.</param>
        /// <param name="token">The cancellation token.</param>
        public static Task<IAsyncMonad<IEnumerable<TSource>>> UnfoldMAsync<TSource>(
            this Task<IAsyncMonad<Maybe<TSource>>> seed,
            CancellationToken token = default)
        {
            Task<IAsyncMonad<IEnumerable<TSource>>> go()
            {
                if (token.IsCancellationRequested)
                    return Task.FromResult(Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), seed.GetType()).ToAsyncMonad());

                var q = seed.BindAsync((Maybe<TSource> x) =>
                    x.TryGetValue(default, out var y)
                    ? go().MapAsync((IEnumerable<TSource> zs) => Task.FromResult(zs.Prepend(y)))
                    : Task.FromResult(Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), seed.GetType()).ToAsyncMonad()));

                return q;
            };

            return go();
        }

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="IAsyncMonad{TSource}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to an <see cref="IAsyncMonad{TSource}"/>.</param>
        public static IAsyncMonad<T> ToAsyncMonad<T>(this IFunctor<T> f) => (IAsyncMonad<T>)f;
    }
}
