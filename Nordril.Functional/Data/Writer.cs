using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The writer-monad, which provides write-only access to a state, in addition to producing a result.
    /// The output can then be later extracted, but not by the computations within the writer-monad themselves.
    /// </summary>
    /// <typeparam name="TState">The type of the output.</typeparam>
    /// <typeparam name="TValue">The type of the result.</typeparam>
    /// <typeparam name="TMonoid">The monoid on <typeparamref name="TState"/> used to combine the outputs. This MUST be a type whose <see cref="IMagma{T}.Op(T, T)"/>- and <see cref="INeutralElement{T}.Neutral"/>-operations DO NOT use the this-pointer, i.e. NOT a generic <see cref="Monoid{T}"/>-instance, but some user-made <see cref="IMonoid{T}"/>-instance.</typeparam>
    /// <remarks>
    /// The two main functions one can use with a writer are tell (which stores a new output in the state of the writer) and listen (which stores a new output in the state of the writer and also returns it as the writer's result).
    /// Due to limits of type-inference, using <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/> and <see cref="IFunctor{T}.Map{TResult}(Func{T, TResult})"/> are quite cumbersome; for this reason, specialized versions are provided in the form of <see cref="Writer.BindTell{TState, TValue, TMonoid}(Writer{TState, TValue, TMonoid}, Func{TValue, TState})"/> and <see cref="Writer.BindListen{TState, TValue, TMonoid}(Writer{TState, TValue, TMonoid}, Func{TValue, TState})"/> (as a specialized replacement for <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>; and <see cref="Writer{TState, TValue, TMonoid}.MapWriter{TResult}(Func{TValue, TResult})"/> (as a specialized replacement for <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>).
    /// </remarks>
    public class Writer<TState, TValue, TMonoid> : IMonad<TValue>
        where TMonoid : IMonoid<TState>
    {
        /// <summary>
        /// The write-only output of the computation.
        /// </summary>
        public TState State { get; private set; }
        /// <summary>
        /// The result of the computation.
        /// </summary>
        public TValue Result { get; private set; }

        private readonly IMonoid<TState> monoid;

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="result">The result to produce.</param>
        public Writer(TValue result)
        {
            var neutral = Monoid.NeutralUnsafe<TState, TMonoid>();
            State = neutral;
            Result = result;
            monoid = new Monoid<TState>(neutral, Monoid.OpUnsafe<TState, TMonoid>());
        }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="state">The initial state of the writer.</param>
        /// <param name="result">The result to produce.</param>
        public Writer(TValue result, TState state)
        {
            State = state;
            Result = result;
            monoid = new Monoid<TState>(Monoid.NeutralUnsafe<TState, TMonoid>(), Monoid.OpUnsafe<TState, TMonoid>());
        }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="state">The initial state of the writer.</param>
        /// <param name="result">The result to produce.</param>
        /// <param name="monoid">The monoid on <typeparamref name="TState"/>.</param>
        public Writer(TValue result, TState state, IMonoid<TState> monoid)
        {
            State = state;
            Result = result;
            this.monoid = monoid;
        }

        /// <summary>
        /// Returns a new writer which stores a new ouput <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The new output to store.</param>
        public Writer<TState, TValue, TMonoid> Tell(TState state)
        {
            return new Writer<TState, TValue, TMonoid>(Result, monoid.Op(State, state));
        }

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is Writer<TState, Func<TValue, TResult>, TMonoid>))
                throw new InvalidCastException();

            var fWriter = f as Writer<TState, Func<TValue, TResult>, TMonoid>;

            return new Writer<TState, TResult, TMonoid>(fWriter.Result(Result), monoid.Op(fWriter.State, State), monoid);
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
        {
            var ret = f(Result) as Writer<TState, TResult, TMonoid>;

            return new Writer<TState, TResult, TMonoid>(ret.Result, monoid.Op(State, ret.State));
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new Writer<TState, TResult, TMonoid>(f(Result), State);

        /// <summary>
        /// A specialized version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/> which preserves the type-information that the result is a <see cref="Writer{TState, TValue, TMonoid}"/>. Useful in oder to avoid having to specify types.
        /// </summary>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="f">The </param>
        /// <returns></returns>
        public Writer<TState, TResult, TMonoid> MapWriter<TResult>(Func<TValue, TResult> f)
            => new Writer<TState, TResult, TMonoid>(f(Result), State);

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Writer<TState, TResult, TMonoid>(x);
    }

    /// <summary>
    /// Extension methods for <see cref="Writer{TState, TValue, TMonoid}"/>.
    /// </summary>
    public static class Writer
    {
        /// <summary>
        /// Stores a new output in the writer and returns no result.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TState"/>.</typeparam>
        /// <param name="state">The output to store.</param>
        public static Writer<TState, Unit, TMonoid> TellUnit<TState, TMonoid>(TState state)
            where TMonoid : IMonoid<TState>
            => new Writer<TState, Unit, TMonoid>(new Unit(), state);

        /// <summary>
        /// Stores a new output in the writer.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <typeparam name="TValue">The result of the writer.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TState"/>.</typeparam>
        /// <param name="w">The writer.</param>
        /// <param name="f">The function which takes the writer's result and returns the output to store.</param>
        public static Writer<TState, TValue, TMonoid> BindTell<TState, TValue, TMonoid>(this Writer<TState, TValue, TMonoid> w, Func<TValue, TState> f)
            where TMonoid : IMonoid<TState>
        {
            return w.Tell(f(w.Result));
        }

        /// <summary>
        /// Stores a new output in the writer and returns it.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <typeparam name="TValue">The result of the writer.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TState"/>.</typeparam>
        /// <param name="w">The writer.</param>
        /// <param name="f">The function which takes the writer's result and returns the output to store.</param>
        public static Writer<TState, Unit, TMonoid> BindTellUnit<TState, TValue, TMonoid>(this Writer<TState, TValue, TMonoid> w, Func<TValue, TState> f)
            where TMonoid : IMonoid<TState>
        {
            return w.Tell(f(w.Result)).Map(_ => new Unit()).ToWriter<TState, Unit, TMonoid>();
        }

        /// <summary>
        /// Stores a new output and returns it.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TState"/>.</typeparam>
        /// <param name="state">The output to store and return.</param>
        public static Writer<TState, TState, TMonoid> Listen<TState, TMonoid>(TState state)
            where TMonoid : IMonoid<TState>
            => new Writer<TState, TState, TMonoid>(state, state);

        /// <summary>
        /// Stores a new output and returns it.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <typeparam name="TValue">The result of the writer.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TState"/>.</typeparam>
        /// <param name="w">The writer.</param>
        /// <param name="f">The output to store and return.</param>
        public static Writer<TState, TState, TMonoid> BindListen<TState, TValue, TMonoid>(this Writer<TState, TValue, TMonoid> w, Func<TValue, TState> f)
            where TMonoid : IMonoid<TState>
        {
            var outputValue = f(w.Result);
            return w.Tell(outputValue).MapWriter(_ => outputValue);
        }

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Writer{TState, TValue, TMonoid}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="TValue">The type of the value contained in the functor.</typeparam>
        /// <typeparam name="TState">The type of the writer's state.</typeparam>
        /// <typeparam name="TMonoid">The type of the writer's monoid.</typeparam>
        /// <param name="f">The functor to cast to a writer.</param>
        public static Writer<TState, TValue, TMonoid> ToWriter<TState, TValue, TMonoid>(this IFunctor<TValue> f) where TMonoid : IMonoid<TState> => (Writer<TState, TValue, TMonoid>)f;
    }
}
