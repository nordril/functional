using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The reader-monad, which reads a state <typeparamref name="TEnvironment"/> and produces a result
    /// <typeparamref name="TValue"/>. Composing multiple reader-actions is like composing
    /// simple functions, except that an implicit variable of type <typeparamref name="TEnvironment"/>
    /// is being passed around.
    /// As such, state is a more well-behaved replacement to reader some global variable.
    /// </summary>
    /// <typeparam name="TEnvironment">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value produced.</typeparam>
    public class Reader<TEnvironment, TValue> : IMonad<TValue>
    {
        /// <summary>
        /// The state-function which takes an initial state and produces
        /// both a new state and a result.
        /// </summary>
        private readonly Func<TEnvironment, TValue> runReader;

        /// <summary>
        /// Creates a new reader from a function that takes an initial state
        /// and produces a new state, plus a result.
        /// </summary>
        /// <param name="runReader">The state-function to put into the state monad.</param>
        public Reader(Func<TEnvironment, TValue> runReader)
        {
            this.runReader = runReader;
        }

        /// <summary>
        /// Runs the reader function with an initial state and returns the result.
        /// </summary>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        public TValue  Run(TEnvironment initialState) => runReader(initialState);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is Reader<TEnvironment, Func<TValue, TResult>>))
                throw new InvalidCastException();

            var fState = f as Reader<TEnvironment, Func<TValue, TResult>>;

            return new Reader<TEnvironment, TResult>(s => {
                var func = fState.Run(s);
                var arg = runReader(s);
                return func(arg);
            });
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
            => new Reader<TEnvironment, TResult>(s =>
            {
                var v = runReader(s);
                return (f(v) as Reader<TEnvironment, TResult>).Run(s);
            });

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new Reader<TEnvironment, TResult>(s => {
                var v = runReader(s);
                return f(v);
            });

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Reader<TEnvironment, TResult>(s => x);
    }

    /// <summary>
    /// Extension methods for <see cref="Reader{TState, TValue}"/>.
    /// </summary>
    public static class Reader
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Reader{TState, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Reader<TEnvironment, TResult> Select<TEnvironment, TSource, TResult>(this Reader<TEnvironment, TSource> source, Func<TSource, TResult> f)
            => (Reader<TEnvironment, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Reader{TState, TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Reader<TEnvironment, TResult> SelectMany<TEnvironment, TSource, TMiddle, TResult>
            (this Reader<TEnvironment, TSource> source,
             Func<TSource, Reader<TEnvironment, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector) =>
            new Reader<TEnvironment, TResult>(s =>
            {
                var v1 = source.Run(s);
                var v2 = f(v1).Run(s);
                return resultSelector(v1, v2);
            });

        /// <summary>
        /// Returns the current state. Also known as <em>ask</em>.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        public static Reader<TEnvironment, TEnvironment> Get<TEnvironment>() => new Reader<TEnvironment, TEnvironment>(s => s);

        /// <summary>
        /// Returns the result to of a function which takes the state as an input. Also known as <em>reader</em>.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="f">The function to run.</param>
        public static Reader<TEnvironment, TResult> With<TEnvironment, TResult>(Func<TEnvironment, TResult> f) => new Reader<TEnvironment, TResult>(s => f(s));

        /// <summary>
        /// Returns a <see cref="Reader{TEnvironment, TValue}"/> which runs <paramref name="r"/>, but first applies <paramref name="f"/> to the environment, effectively running a <see cref="Reader"/> in a modified environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="f">The function which modifies the environment.</param>
        /// <param name="r">The <see cref="Reader"/> to run in the modified environment.</param>
        public static Reader<TEnvironment, TResult> Local<TEnvironment, TResult>(Func<TEnvironment, TEnvironment> f, Reader<TEnvironment, TResult> r)
            => new Reader<TEnvironment, TResult>(s => r.Run(f(s)));

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Reader{TEnvironment, TValue}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to a writer.</param>
        public static Reader<TEnvironment, TValue> ToReader<TEnvironment, TValue>(this IFunctor<TValue> f) => (Reader<TEnvironment, TValue>)f;
    }
}
