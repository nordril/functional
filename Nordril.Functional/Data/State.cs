using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The state-monad, which maintains a state <typeparamref name="TState"/> and produces a result
    /// <typeparamref name="TValue"/>. Composing multiple state-actions is like composing
    /// simple functions, except that an implicit variable of type <typeparamref name="TState"/>
    /// is being passed around.
    /// As such, state is a more well-behaved replacement to modifying some global variable.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value produced.</typeparam>
    public class State<TState, TValue> : IMonad<TValue>
    {
        /// <summary>
        /// The state-function which takes an initial state and produces
        /// both a new state and a result.
        /// </summary>
        private readonly Func<TState, (TValue, TState)> runState;

        /// <summary>
        /// Creates a new state from a function that takes an initial state
        /// and produces a new state, plus a result.
        /// </summary>
        /// <param name="runState">The state-function to put into the state monad.</param>
        public State(Func<TState, (TValue, TState)> runState)
        {
            this.runState = runState;
        }

        /// <summary>
        /// Runs the state function with an initial state and returns the final state, plus the result.
        /// </summary>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        public (TValue result, TState finalState) Run(TState initialState) => runState(initialState);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is State<TState, Func<TValue, TResult>>))
                throw new InvalidCastException();

            var fState = f as State<TState, Func<TValue, TResult>>;

            return new State<TState, TResult>(s => {
                var (func, s1) = fState.Run(s);
                var (arg, s2) = runState(s1);
                return (func(arg), s2);
               });
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
            => new State<TState, TResult>(s =>
            {
                var (v, s1) = runState(s);
                return (f(v) as State<TState, TResult>).Run(s1);
            });

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new State<TState, TResult>(s => {
                var (v, s1) = runState(s);
                return (f(v), s1);
            });

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new State<TState, TResult>(s => (x, s));
    }

    /// <summary>
    /// Extension methods for <see cref="State{TState, TValue}"/>.
    /// </summary>
    public static class State
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="State{TState, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static State<TState, TResult> Select<TState, TSource, TResult>(this State<TState, TSource> source, Func<TSource, TResult> f)
            => (State<TState, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="State{TState, TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static State<TState, TResult> SelectMany<TState, TSource, TMiddle, TResult>
            (this State<TState, TSource> source,
             Func<TSource, State<TState, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector) =>
            new State<TState, TResult>(s =>
            {
                var (v1, s1) = source.Run(s);
                var (v2, s2) = f(v1).Run(s1);
                return (resultSelector(v1, v2), s2);
            });

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        public static State<TState, TState> Get<TState>() => new State<TState, TState>(s => (s, s));

        /// <summary>
        /// Replaces the current state with a new one.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="value">The new state.</param>
        public static State<TState, Unit> Put<TState>(TState value) => new State<TState, Unit>(_ => (new Unit(), value));

        /// <summary>
        /// Modifies the current state by running a function on it.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="f">The function to apply to the state.</param>
        public static State<TState, Unit> Modify<TState>(Func<TState, TState> f) => new State<TState, Unit>(s => (new Unit(), f(s)));

        /// <summary>
        /// Runs the state function with an initial state and returns the result, discarding the final state.
        /// </summary>
        /// <param name="s">The state to run.</param>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static TResult RunForResult<TState, TResult>(this State<TState, TResult> s, TState initialState)
            => s.Run(initialState).result;

        /// <summary>
        /// Runs the state function with an initial state and returns the final state, discarding the result.
        /// </summary>
        /// <param name="s">The state to run.</param>
        /// <param name="initialState">The initial state (the starting point of the computation).</param>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public static TState RunForState<TState, TResult>(this State<TState, TResult> s, TState initialState)
            => s.Run(initialState).finalState;

        /// <summary>
        /// Runs the state function on a newly created <see cref="StringBuilder"/> and returns the final string. This is a convenience method if one uses <see cref="State{TState, TValue}"/> to efficiently build up strings.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="s">The state to run.</param>
        public static string RunStringBuilder<TResult>(this State<StringBuilder, TResult> s)
            => s.Run(new StringBuilder()).finalState.ToString();
    }
}
