using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A memoization-monad which allows one to run computations that make use of memoization of the results. This generally makes sense for recursive functions with overlapping subproblems where we only want to compute <c>f(x) = y</c> the first time and retrieve its stored result <c>y</c> whenever a subsequent call to <c>f(x)</c> is made.
    /// The main function to use with this structure is <see cref="Memo.Memoized{TState, TKey, TValue}(TKey, Func{TKey, Memo{TState, TKey, TValue}})"/>.
    /// <remarks>The memoization is backed by a finite <see cref="IDictionary{TKey, TValue}"/>. Commonly, there is no limit on the number of computed results to store, though one can limit this via a custom dictionary-implementation.</remarks>
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TKey">The type of the key by which results are memoized.</typeparam>
    /// <typeparam name="TValue">The type of the value which is computed.</typeparam>
    /// <example>
    /// We can calculate the fibonacci sequence, which has overlapping subproblems, as follows:
    /// <code>
    /// Memo&lt;Dictionary&lt;int, int&gt;, int, int&gt; fib(int x)
    /// {
    ///     if (x == 0)
    ///         return Memo.Pure&lt;Dictionary&lt;int, int&gt;, int, int&gt;(0);
    ///     else if (x == 1)
    ///         return Memo.Pure&lt;Dictionary&lt;int, int&gt;, int, int&gt;(1);
    ///     else
    ///         return from n1 in Memo.Memoized(x - 1, fib)
    ///                from n2 in Memo.Memoized(x - 2, fib)
    ///                select n1 + n2;
    /// }
    /// 
    /// //1346269
    /// var result = fib(30).RunForResult(new Dictionary&lt;int, int&gt;());
    /// </code>
    /// </example>
    public class Memo<TState, TKey, TValue> : IMonoMonad<Memo<TState, TKey, TValue>, TValue>
        where TState : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The state-function which takes an initial state and produces
        /// both a new state and a result.
        /// </summary>
        private readonly Func<TState, (TValue, TState)> runState;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="runState">The function to run.</param>
        public Memo(Func<TState, (TValue, TState)> runState)
        {
            this.runState = runState;
        }

        /// <summary>
        /// Runs the state function with an initial state and returns the result, plus the actual result.
        /// </summary>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        public (TValue result, TState finalState) Run(TState initialState) => runState(initialState);

        /// <inheritdoc />
        public Memo<TState, TKey, TValue> Bind(Func<TValue, Memo<TState, TKey, TValue>> f)
            => new Memo<TState, TKey, TValue>(s =>
            {
                var (v, s1) = runState(s);
                return f(v).Run(s1);
            });

        /// <inheritdoc />
        public Memo<TState, TKey, TValue> MonoMap(Func<TValue, TValue> f)
            => new Memo<TState, TKey, TValue>(s => {
                var (v, s1) = runState(s);
                return (f(v), s1);
            });

        /// <inheritdoc />
        public Memo<TState, TKey, TValue> Pure(TValue x)
            => new Memo<TState, TKey, TValue>(s => (x, s));
    }

    /// <summary>
    /// Extension methods for <see cref="Memo{TState, TKey, TValue}"/>.
    /// </summary>
    public static class Memo
    {
        /// <summary>
        /// Equivalent to <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>, but restricted to <see cref="Memo{TState, TKey, TValue}"/>. Useful for returning pure values, esp. in LINQ-queries.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TKey">The key of the memoization.</typeparam>
        /// <typeparam name="TValue">The type of the source's value.</typeparam>
        /// <param name="value">The value to return.</param>
        public static Memo<TState, TKey, TValue> Pure<TState, TKey, TValue>(TValue value)
            where TState : IDictionary<TKey, TValue>
            => new Memo<TState, TKey, TValue>(_ => (value, _));

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Memo{TState, TKey, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TKey">The key of the memoization.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Memo<TState, TKey, TSource> Select<TState, TKey, TSource>(this Memo<TState, TKey, TSource> source, Func<TSource, TSource> f)
            where TState : IDictionary<TKey, TSource>
            => (Memo<TState, TKey, TSource>)source.MonoMap(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Memo{TState, TKey, TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TKey">The key of the memoization.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Memo<TState, TKey, TSource> SelectMany<TState, TKey, TSource>
            (this Memo<TState, TKey, TSource> source,
             Func<TSource, Memo<TState, TKey, TSource>> f,
             Func<TSource, TSource, TSource> resultSelector)
            where TState : IDictionary<TKey, TSource>
            => new Memo<TState, TKey, TSource>(s =>
            {
                var (v1, s1) = source.Run(s);
                var (v2, s2) = f(v1).Run(s1);
                return (resultSelector(v1, v2), s2);
            });

        /// <summary>
        /// Memoizes a function, either returning the memoized value without running <paramref name="f"/> if the key <paramref name="key"/> is already memoized, or running it, memoizing the key, and then returning the result.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TKey">The key of the memoization.</typeparam>
        /// <typeparam name="TValue">The type of the source's value.</typeparam>
        /// <param name="key">The key of the value to search. If this is present in the state, <paramref name="f"/> is not run.</param>
        /// <param name="f">The function to run in memoized form.</param>
        public static Memo<TState, TKey, TValue> Memoized<TState, TKey, TValue>(TKey key, Func<TKey, Memo<TState, TKey, TValue>> f)
            where TState : IDictionary<TKey, TValue>
            => new Memo<TState, TKey, TValue>(s => {
                if (!s.TryGetValue(key, out var memoizedValue))
                {
                    var (result, finalState) = f(key).Run(s);
                    finalState.Add(key, result);
                    return (result, finalState);
                }
                else
                    return (memoizedValue, s);
            });
    }
}
