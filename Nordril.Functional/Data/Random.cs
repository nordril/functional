using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 The random-functionality includes the Ziggurat algorithm as written by Colin Green (https://github.com/colgreen/Redzen), licensed under the MIT License:

SPDX identifier
MIT

License text
 MIT License

Copyright (c) _____

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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
        /// Gets a new random integer.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, int> RandomInt<TRng>(this RandomCxt<TRng> _cxt) => RandomInt<TRng>();

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
        /// Gets a new random integer in the specified range.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, int> RandomInt<TRng>(this RandomCxt<TRng> _cxt, int min, int max) => RandomInt<TRng>(min, max);

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
        /// Gets a list of random integers, all in the specified range.
        /// When a large list of integers is required, this function is preferable to <see cref="ApplicativeExtensions.SelectAp{T, TResult, TResultList}(IEnumerable{T}, Func{T, IApplicative{TResult}})"/>, which might overflow.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive).</param>
        /// <param name="num">The number of integers to return.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, IList<int>> RandomInts<TRng>(this RandomCxt<TRng> _cxt, int min, int max, int num) => RandomInts<TRng>(min, max, num);

        /// <summary>
        /// Gets a new random double between 0.0 and 1.0.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        public static Random<TRng, double> RandomDouble<TRng>()
            => new Random<TRng, double>(r => r.NextDouble());

        /// <summary>
        /// Gets a new random double between 0.0 and 1.0.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, double> RandomDouble<TRng>(this RandomCxt<TRng> _cxt) => RandomDouble<TRng>();

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
        /// Gets a new random double in the specified range.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="min">The minium value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive). If <paramref name="max"/> is equal to <paramref name="min"/>, <paramref name="min"/> is always returned by this method.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, double> RandomDouble<TRng>(this RandomCxt<TRng> _cxt, double min, double max) => RandomDouble<TRng>(min, max);

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
        /// Gets a list of random doubles, all in the specified range.
        /// When a large list of doubles is required, this function is preferable to <see cref="ApplicativeExtensions.SelectAp{T, TResult, TResultList}(IEnumerable{T}, Func{T, IApplicative{TResult}})"/>, which might overflow.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive).</param>
        /// <param name="num">The number of integers to return.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, IList<double>> RandomDoubles<TRng>(this RandomCxt<TRng> _cxt, double min, double max, int num) => RandomDoubles<TRng>(min, max, num);

        /// <summary>
        /// Fills a buffer <paramref name="buffer"/> with random bytes (mutating it) and returns it.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="buffer">The buffer to fill and return.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is null.</exception>
        public static Random<TRng, byte[]> RandomBytes<TRng>(byte[] buffer)
            => new Random<TRng, byte[]>(r => r.NextBytes(buffer));

        /// <summary>
        /// Fills a buffer <paramref name="buffer"/> with random bytes (mutating it) and returns it.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="buffer">The buffer to fill and return.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="buffer"/> is null.</exception>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, byte[]> RandomBytes<TRng>(this RandomCxt<TRng> _cxt, byte[] buffer) => RandomBytes<TRng>(buffer);

        /// <summary>
        /// Gets 32 uniformly random bits.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        public static Random<TRng, uint> Random32Bits<TRng>()
            => new Random<TRng, uint>(r =>
            {
                var bytes = new byte[4];
                (bytes, r) = r.NextBytes(bytes);
                return (BitConverter.ToUInt32(bytes), r);
            });

        /// <summary>
        /// Gets 32 uniformly random bits.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, uint> Random32Bits<TRng>(this RandomCxt<TRng> _cxt) => Random32Bits<TRng>();

        /// <summary>
        /// Gets 64 uniformly random bits.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        public static Random<TRng, ulong> Random64Bits<TRng>()
            => new Random<TRng, ulong>(r => Random64Bits(r));

        /// <summary>
        /// Gets 64 uniformly random bits.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, ulong> Random64Bits<TRng>(this RandomCxt<TRng> _cxt) => Random64Bits<TRng>();

        /// <summary>
        /// Gets 64 uniformly random bits.
        /// </summary>
        /// <typeparam name="TRng"></typeparam>
        /// <param name="rng">The RNG.</param>
        private static (ulong, Rng<TRng>) Random64Bits<TRng>(this Rng<TRng> rng)
        {
            var bytes = new byte[8];
            (bytes, rng) = rng.NextBytes(bytes);
            return (BitConverter.ToUInt64(bytes), rng);
        }

        /// <summary>
        /// Returns the elements of a sequence in uniformly random order.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="xs">The sequence to randomly order.</param>
        public static Random<TRng, IList<T>> RandomOrder<TRng, T>(IEnumerable<T> xs)
            => new Random<TRng, IList<T>>(r =>
            {
                var list = xs.ToList();
                var ret = new FuncList<T>(list.Count);
                var unusedIndexes = Enumerable.Range(0, list.Count).ToHashSet();
                int index = 0;

                while (list.Count > 0)
                {
                    (index, r) = r.NextInt(0, unusedIndexes.Count);
                    ret.Add(list[index]);
                    unusedIndexes.Remove(index);
                }

                return (ret, r);
            });

        /// <summary>
        /// Returns the elements of a sequence in uniformly random order.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="xs">The sequence to randomly order.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, IList<T>> RandomOrder<TRng, T>(this RandomCxt<TRng> _cxt, IEnumerable<T> xs) => RandomOrder<TRng, T>(xs);

        /// <summary>
        /// Gets a double-value from a Gaussian normal distribution with mean <paramref name="mean"/> and standard deviation <paramref name="standardDeviation"/>.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        public static Random<TRng, double> RandomGaussianDouble<TRng>(double mean, double standardDeviation)
            => new Random<TRng, double>(r =>
            {
                double ret = 0D;
                (ret, r) = ZigguratGaussian.Sample(r);
                ret = mean + (standardDeviation * ret);
                return (ret, r);
            });

        /// <summary>
        /// Gets a double-value from a Gaussian normal distribution with mean <paramref name="mean"/> and standard deviation <paramref name="standardDeviation"/>.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, double> RandomGaussianDouble<TRng>(this RandomCxt<TRng> _cxt, double mean, double standardDeviation)
            => RandomGaussianDouble<TRng>(mean, standardDeviation);

        /// <summary>
        /// Gets a list of double-values from a Gaussian normal distribution with mean <paramref name="mean"/> and standard deviation <paramref name="standardDeviation"/>.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <param name="num">The number of doubles to generate.</param>
        public static Random<TRng, IList<double>> RandomGaussianDoubles<TRng>(double mean, double standardDeviation, int num)
            => new Random<TRng, IList<double>>(r => {
                var ret = new List<double>(num);
                var iRes = 0D;

                for (int i = 0; i < num; i++)
                {
                    (iRes, r) = Rnd.RandomGaussianDouble<TRng>(mean, standardDeviation).Run(r);

                    ret.Add(iRes);
                }

                return (ret, r);
            });

        /// <summary>
        /// Gets a alist double-values from a Gaussian normal distribution with mean <paramref name="mean"/> and standard deviation <paramref name="standardDeviation"/>.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TRng">The type of the RNG.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <param name="num">The number of doubles to generate.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Random<TRng, IList<double>> RandomGaussianDoubles<TRng>(this RandomCxt<TRng> _cxt, double mean, double standardDeviation, int num)
            => RandomGaussianDoubles<TRng>(mean, standardDeviation, num);

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

        //Source (with minimal adaptations): https://github.com/colgreen/Redzen/blob/master/Redzen/Numerics/Distributions/Double/ZigguratGaussian.cs
        private static class ZigguratGaussian
        {
            /// <summary>
            /// Number of blocks.
            /// </summary>
            const int __blockCount = 128;

            /// <summary>
            /// Right hand x coord of the base rectangle, thus also the left hand x coord of the tail 
            /// (pre-determined/computed for 128 blocks).
            /// </summary>
            const double __R = 3.442619855899;

            /// <summary>
            /// Area of each rectangle (pre-determined/computed for 128 blocks).
            /// </summary>
            const double __A = 9.91256303526217e-3;

            /// <summary>
            /// Denominator for __INCR constant. This is the number of distinct values this class is capable 
            /// of generating in the interval [0,1], i.e. (2^53)-1 distinct values.
            /// </summary>
            const ulong __MAXINT = (1UL << 53) - 1;

            /// <summary>
            /// Scale factor for converting a ULong with interval [0, 0x1f_ffff_ffff_ffff] to a double with interval [0,1].
            /// </summary>
            const double __INCR = 1.0 / __MAXINT;

            /// <summary>
            /// Binary representation of +1.0 in IEEE 754 double-precision floating-point format.
            /// </summary>
            const ulong __oneBits = 0x3ff0_0000_0000_0000UL;

            // __x[i] and __y[i] describe the top-right position of rectangle i.
            static readonly double[] __x;
            static readonly double[] __y;

            // The proportion of each segment that is entirely within the distribution, expressed as ulong where 
            // a value of 0 indicates 0% and 2^53-1 (i.e. 53 binary 1s) 100%. Expressing this as an integer value 
            // allows some floating point operations to be replaced with integer operations.
            static readonly ulong[] __xComp;

            // Useful precomputed values.
            // Area A divided by the height of B0. Note. This is *not* the same as __x[i] because the area 
            // of B0 is __A minus the area of the distribution tail.
            static readonly double __A_Div_Y0;

            static ZigguratGaussian()
            {
                // Initialise rectangle position data. 
                // __x[i] and __y[i] describe the top-right position of Box i.

                // Allocate storage. We add one to the length of _x so that we have an entry at __x[__blockCount], this avoids having 
                // to do a special case test when sampling from the top box.
                __x = new double[__blockCount + 1];
                __y = new double[__blockCount];

                // Determine top right position of the base rectangle/box (the rectangle with the Gaussian tale attached). 
                // We call this Box 0 or B0 for short.
                // Note. x[0] also describes the right-hand edge of B1. (See diagram).
                __x[0] = __R;
                __y[0] = GaussianPdfDenorm(__R);

                // The next box (B1) has a right hand X edge the same as B0. 
                // Note. B1's height is the box area divided by its width, hence B1 has a smaller height than B0 because
                // B0's total area includes the attached distribution tail.
                __x[1] = __R;
                __y[1] = __y[0] + (__A / __x[1]);

                // Calc positions of all remaining rectangles.
                for (int i = 2; i < __blockCount; i++)
                {
                    __x[i] = GaussianPdfDenormInv(__y[i - 1]);
                    __y[i] = __y[i - 1] + (__A / __x[i]);
                }

                // For completeness we define the right-hand edge of a notional box 6 as being zero (a box with no area).
                __x[__blockCount] = 0.0;

                // Useful precomputed values.
                __A_Div_Y0 = __A / __y[0];
                __xComp = new ulong[__blockCount];

                // Special case for base box. __xComp[0] stores the area of B0 as a proportion of __R 
                // (recalling that all segments have area __A, but that the base segment is the combination of B0 and the distribution tail).
                // Thus __xComp[0] is the probability that a sample point is within the box part of the segment.
                __xComp[0] = (ulong)(((__R * __y[0]) / __A) * (double)__MAXINT);

                for (int i = 1; i < __blockCount - 1; i++)
                {
                    __xComp[i] = (ulong)((__x[i + 1] / __x[i]) * (double)__MAXINT);
                }
                __xComp[__blockCount - 1] = 0;  // Shown for completeness.

                // Sanity check. Test that the top edge of the topmost rectangle is at y=1.0.
                // Note. We expect there to be a tiny drift away from 1.0 due to the inexactness of floating
                // point arithmetic.
                Debug.Assert(Math.Abs(1.0 - __y[__blockCount - 1]) < 1e-10);
            }

            #region Public Static Methods

            /// <summary>
            /// Take a sample from the standard Gaussian distribution, i.e. with mean of 0 and standard deviation of 1.
            /// </summary>
            /// <returns>A random sample.</returns>
            public static (double, Rng<TRng>) Sample<TRng>(Rng<TRng> rng)
            {
                for (; ; )
                {
                    // Generate 64 random bits.
                    ulong u;
                    (u, rng) = rng.Random64Bits();

                    // Note. 61 random bits are required and therefore the lowest three bits are discarded
                    // (a typical characteristic of PRNGs is that the least significant bits exhibit lower
                    // quality randomness than the higher bits).
                    // Select a segment (7 bits, bits 3 to 9).
                    int s = (int)((u >> 3) & 0x7f);

                    // Select the sign bit (bit 10), and convert to a double-precision float value of -1.0 or +1.0 accordingly.
                    // Notes.
                    // Here we convert the single chosen bit directly into IEEE754 double-precision floating-point format.
                    // Previously this conversion used a branch, which is considerably slower because modern superscalar
                    // CPUs rely heavily on branch prediction, but the outcome of this branch is pure random noise and thus
                    // entirely unpredictable, i.e. the absolute worse case scenario!
                    double sign = BitConverter.Int64BitsToDouble(unchecked((long)(((u & 0x400UL) << 53) | __oneBits)));

                    // Get a uniform random value with interval [0, 2^53-1], or in hexadecimal [0, 0x1f_ffff_ffff_ffff] 
                    // (i.e. a random 53 bit number) (bits 11 to 63).
                    ulong u2 = u >> 11;

                    // Special case for the base segment.
                    if (0 == s)
                    {
                        if (u2 < __xComp[0])
                        {
                            // Generated x is within R0.
                            return (u2 * __INCR * __A_Div_Y0 * sign, rng);
                        }
                        // Generated x is in the tail of the distribution.
                        double sampledTail;
                        (sampledTail, rng) = SampleTail(rng);
                        return (sampledTail * sign, rng);
                    }

                    // All other segments.
                    if (u2 < __xComp[s])
                    {
                        // Generated x is within the rectangle.
                        return (u2 * __INCR * __x[s] * sign, rng);
                    }

                    // Generated x is outside of the rectangle.
                    // Generate a random y coordinate and test if our (x,y) is within the distribution curve.
                    // This execution path is relatively slow/expensive (makes a call to Math.Exp()) but is relatively rarely executed,
                    // although more often than the 'tail' path (above).
                    double yy;

                    (yy, rng) = rng.NextDouble();

                    double x = u2 * __INCR * __x[s];
                    if (__y[s - 1] + ((__y[s] - __y[s - 1]) * yy) < GaussianPdfDenorm(x))
                    {
                        return (x * sign, rng);
                    }
                }
            }

            #endregion

            #region Private Static Methods

            /// <summary>
            /// Sample from the distribution tail (defined as having x >= __R).
            /// </summary>
            /// <returns></returns>
            private static (double, Rng<TRng>) SampleTail<TRng>(Rng<TRng> rng)
            {
                double x, y;
                do
                {
                    double xr, yr;

                    (xr, rng) = rng.NextDouble();
                    (yr, rng) = rng.NextDouble();
                    // Note. we use NextDoubleNonZero() because Log(0) returns -Infinity and will also tend to be a very slow execution path (when it occurs, which is rarely).
                    x = -Math.Log(xr) / __R;
                    y = -Math.Log(yr);
                }
                while (y + y < x * x);
                return (__R + x, rng);
            }

            /// <summary>
            /// Gaussian probability density function, denormalised, that is, y = e^-(x^2/2).
            /// </summary>
            private static double GaussianPdfDenorm(double x)
            {
                return Math.Exp(-(x * x / 2.0));
            }

            /// <summary>
            /// Inverse function of GaussianPdfDenorm(x)
            /// </summary>
            private static double GaussianPdfDenormInv(double y)
            {
                // Operates over the y interval (0,1], which happens to be the y interval of the pdf, 
                // with the exception that it does not include y=0, but we would never call with 
                // y=0 so it doesn't matter. Note that a Gaussian effectively has a tail going
                // into infinity on the x-axis, hence asking what is x when y=0 is an invalid question
                // in the context of this class.
                return Math.Sqrt(-2.0 * Math.Log(y));
            }

            #endregion
        }
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
