﻿using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The "random"-monad, which provdes access to a pseudo-random number generator in a repeatable and pure way. Unlike using <see cref="Random"/>, two executions of <see cref="Random{TRng, TValue}"/>-computations will produce the same result (provided the compuations are pure) when run with the same RNG.
    /// </summary>
    /// <typeparam name="TRng">The type of the RNG. This will be wrapped in an <see cref="Rng{T}"/>.</typeparam>
    /// <typeparam name="TValue">The type of the produced value.</typeparam>
    public class Random<TRng, TValue> : IMonad<TValue>
    {
        /// <summary>
        /// The state-function which takes an initial state and produces
        /// both a new state and a result.
        /// </summary>
        private readonly Func<Rng<TRng>, (TValue, Rng<TRng>)> runState;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="runState">The state-function to put into the random monad.</param>
        public Random(Func<Rng<TRng>, (TValue, Rng<TRng>)> runState)
        {
            this.runState = runState;
        }

        /// <summary>
        /// Runs the state function with an initial state and returns the result, plus the actual result.
        /// </summary>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        public (TValue result, Rng<TRng> finalState) Run(Rng<TRng> initialState) => runState(initialState);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is Random<TRng, Func<TValue, TResult>>))
                throw new InvalidCastException();

            var fState = f as Random<TRng, Func<TValue, TResult>>;

            return new Random<TRng, TResult>(s => {
                var (func, s1) = fState.Run(s);
                var (arg, s2) = runState(s1);
                return (func(arg), s2);
            });
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
            => new Random<TRng, TResult>(s =>
            {
                var (v, s1) = runState(s);
                return (f(v) as Random<TRng, TResult>).Run(s1);
            });

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new Random<TRng, TResult>(s => {
                var (v, s1) = runState(s);
                return (f(v), s1);
            });

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Random<TRng, TResult>(s => (x, s));
    }

    /// <summary>
    /// Extension methods for <see cref="Random{TRng, TValue}"/>.
    /// </summary>
    public static class Rnd
    {
        /// <summary>
        /// Equivalent to <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>, but restricted to <see cref="Random{TRng, TValue}"/>. Useful for returning pure values, esp. in LINQ-queries.
        /// </summary>
        /// <typeparam name="TRng">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the source's value.</typeparam>
        /// <param name="value">The value to return.</param>
        public static Random<TRng, TValue> Pure<TRng, TValue>(TValue value)
            => new Random<TRng, TValue>(_ => (value, _));

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Random{TRng, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Random<TRng, TResult> Select<TRng, TSource, TResult>(this Random<TRng, TSource> source, Func<TSource, TResult> f)
            => (Random<TRng, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Random{TRng, TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Random<TRng, TResult> SelectMany<TRng, TSource, TMiddle, TResult>
            (this Random<TRng, TSource> source,
             Func<TSource, Random<TRng, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector) =>
            new Random<TRng, TResult>(s =>
            {
                var (v1, s1) = source.Run(s);
                var (v2, s2) = f(v1).Run(s1);
                return (resultSelector(v1, v2), s2);
            });

        /// <summary>
        /// Runs the state function with an initial state and returns the result, discarding the final state.
        /// </summary>
        /// <param name="s">The state to run.</param>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        /// <typeparam name="TRng">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static TResult RunForResult<TRng, TResult>(this Random<TRng, TResult> s, Rng<TRng> initialState)
            => s.Run(initialState).result;

        /// <summary>
        /// Runs the state function with an initial state and returns the final state, discarding the result.
        /// </summary>
        /// <param name="s">The state to run.</param>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        /// <typeparam name="TRng">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static Rng<TRng> RunForState<TRng, TResult>(this Random<TRng, TResult> s, Rng<TRng> initialState)
            => s.Run(initialState).finalState;

        /// <summary>
        /// Gets a new random integer.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        public static Random<TRng, int> RandomInt<TRng>()
            => new Random<TRng, int>(r => r.NextInt());

        /// <summary>
        /// Gets a new random integer in the specified range.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        public static Random<TRng, int> RandomInt<TRng>(int min, int max)
            => new Random<TRng, int>(r => r.NextInt(min, max));

        /// <summary>
        /// Gets a list of random integers, all in the specified range.
        /// When a large list of integers is required, this function is preferable to <see cref="ApplicativeExtensions.SelectAp{T, TResult, TResultList}(IEnumerable{T}, Func{T, IApplicative{TResult}})"/>, which might overflow.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive).</param>
        /// <param name="num">The number of integers to return.</param>
        public static Random<TRng, IList<int>> RandomInts<TRng>(int min, int max, int num)
            => new Random<TRng, IList<int>>(r => {
                var ret = new List<int>(num);
                var iRes = 0;

                for (int i = 0; i < num; i++)
                {
                    (iRes, r) = r.NextInt(min, max);

                    ret.Add(iRes);
                }

                return (ret, r);
            });

        /// <summary>
        /// Gets a new random double between 0.0 and 1.0.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        public static Random<TRng, double> RandomDouble<TRng>()
            => new Random<TRng, double>(r => r.NextDouble());

        /// <summary>
        /// Gets a new random double in the specified range.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="min">The minium value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        public static Random<TRng, double> RandomDouble<TRng>(double min, double max)
            => new Random<TRng, double>(r => r.NextDouble(min, max));

        /// <summary>
        /// Gets a list of random doubles, all in the specified range.
        /// When a large list of doubles is required, this function is preferable to <see cref="ApplicativeExtensions.SelectAp{T, TResult, TResultList}(IEnumerable{T}, Func{T, IApplicative{TResult}})"/>, which might overflow.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive).</param>
        /// <param name="num">The number of integers to return.</param>
        public static Random<TRng, IList<double>> RandomDoubles<TRng>(double min, double max, int num)
            => new Random<TRng, IList<double>>(r => {
                var ret = new List<double>(num);
                var iRes = 0D;

                for (int i = 0; i < num; i++)
                {
                    (iRes, r) = r.NextDouble(min, max);

                    ret.Add(iRes);
                }

                return (ret, r);
            });

        /// <summary>
        /// Fills a buffer <paramref name="buffer"/> with random bytes (mutating it) and returns it.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="buffer">The buffer to fill and return.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is null.</exception>
        public static Random<TRng, byte[]> RandomBytes<TRng>(byte[] buffer)
            => new Random<TRng, byte[]>(r => r.NextBytes(buffer));

        /// <summary>
        /// Replaces the current state with a new one.
        /// </summary>
        /// <param name="seed">The new seed of the new PRNG.</param>
        public static Random<Random, Unit> Put(int seed) => new Random<Random, Unit>(_ => (new Unit(), Rng<Random>.FromRandom(seed)));

        /// <summary>
        /// Runs the state function with an initial seed and returns the result, discarding the final state.
        /// </summary>
        /// <param name="s">The computation to run.</param>
        /// <param name="seed">The initial state (the starting point of the computation).</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static TResult RunRandom<TResult>(this Random<Random, TResult> s, int seed)
            => s.Run(Rng<Random>.FromRandom(seed)).result;

        /// <summary>
        /// Runs the state function with with an intial seed equalling <see cref="Environment.TickCount"/> (the same as when calling the parameterless constructor of <see cref="Random"/>), discarding the final state. As this is in an impure function where subsequent calls will return different results, the return-value is an <see cref="Io{T}"/>.
        /// </summary>
        /// <param name="s">The computation to run.</param>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static Io<TResult> RunRandom<TResult>(this Random<Random, TResult> s)
            => new Io<TResult>(() => s.Run(Rng<Random>.FromRandom(Environment.TickCount)).result);
    }

    /// <summary>
    /// A virtual interface for a random number generator, acting as a wrapper which can be used even if the underlying RNG does not implement any specific interface.
    /// </summary>
    /// <typeparam name="T">The type of the underlying RNG.</typeparam>
    public struct Rng<T>
    {
        private readonly T obj;
        private readonly Func<T, int, int, (int, T)> nextInt;
        private readonly Func<T, (double, T)> nextDouble;
        private readonly Func<T, double, double, (double, T)> nextBoundedDouble;
        private readonly Func<T, byte[], (byte[], T)> nextBytes;

        /// <summary>
        /// Gets a new random integer.
        /// </summary>
        public (int, Rng<T>) NextInt() => MkRng(nextInt(obj, int.MinValue, int.MaxValue));

        /// <summary>
        /// Gets a new random integer in the specified range.
        /// </summary>
        /// <param name="min">The minium value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        public (int, Rng<T>) NextInt(int min, int max) => MkRng(nextInt(obj, min, max));

        /// <summary>
        /// Gets a new random double between 0.0 and 1.0.
        /// </summary>
        public (double, Rng<T>) NextDouble() => MkRng(nextDouble(obj));

        /// <summary>
        /// Gets a new random double in the specified range.
        /// </summary>
        /// <param name="min">The minium value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        public (double, Rng<T>) NextDouble(double min, double max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            return MkRng(nextBoundedDouble(obj, min, max));
        }

        /// <summary>
        /// Fills a buffer <paramref name="buffer"/> with random bytes (mutating it) and returns it.
        /// </summary>
        /// <param name="buffer">The buffer to fill and return.</param>
        public (byte[], Rng<T>) NextBytes(byte[] buffer) => MkRng(nextBytes(obj, buffer));

        /// <summary>
        /// Creates a new RNG-wrapper.
        /// </summary>
        /// <param name="obj">The underlying RNG.</param>
        /// <param name="nextInt">The function to return a random integer.</param>
        /// <param name="nextDouble">The function to return a random double between 0.0 and 1.0</param>
        /// <param name="nextBoundedDouble">The function to return a random double in a custom interval.</param>
        /// <param name="nextBytes">The function to fill a byte-array with random bytes.</param>
        public Rng(
            T obj,
            Func<T, int, int, (int, T)> nextInt,
            Func<T, (double, T)> nextDouble,
            Func<T, double, double, (double, T)> nextBoundedDouble,
            Func<T, byte[], (byte[], T)> nextBytes)
        {
            this.obj = obj;
            this.nextInt = nextInt;
            this.nextDouble = nextDouble;
            this.nextBoundedDouble = nextBoundedDouble;
            this.nextBytes = nextBytes;
        }

        private (TResult, Rng<T>) MkRng<TResult>((TResult result, T obj) res)
            => (res.result, new Rng<T>(res.obj, nextInt, nextDouble, nextBoundedDouble, nextBytes));

        /// <summary>
        /// Creates an <see cref="Rng{T}"/>-wrapper out of an instance of <see cref="Random"/>.
        /// </summary>
        /// <param name="seed">The see of the <see cref="Random"/>-object.</param>
        public static Rng<Random> FromRandom(int seed)
            => new Rng<Random>(
                new Random(seed),
                (r, min, max) => (r.Next(min, max), r),
                r => (r.NextDouble(), r),
                (r, min, max) => (min + r.NextDouble() * (max - min), r),
                (r, buffer) => { r.NextBytes(buffer); return (buffer, r); });
    }
}