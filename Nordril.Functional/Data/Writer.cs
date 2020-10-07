using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The writer-monad, which provides write-only access to a state, in addition to producing a result.
    /// The output can then be later extracted, but not by the computations within the writer-monad themselves.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TValue">The type of the result.</typeparam>
    /// <typeparam name="TMonoid">The monoid on <typeparamref name="TOutput"/> used to combine the outputs. This MUST be a type whose <see cref="IMagma{T}.Op(T, T)"/>- and <see cref="INeutralElement{T}.Neutral"/>-operations DO NOT use the this-pointer, i.e. NOT a generic <see cref="Monoid{T}"/>-instance, but some user-made <see cref="IMonoid{T}"/>-instance.</typeparam>
    /// <remarks>
    /// The two main functions one can use with a writer are tell (which stores a new output in the state of the writer) and listen (which stores a new output in the state of the writer and also returns it as the writer's result).
    /// Due to limits of type-inference, using <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/> and <see cref="IFunctor{T}.Map{TResult}(Func{T, TResult})"/> are quite cumbersome; for this reason, specialized versions are provided in the form of <see cref="Writer.BindTell{TState, TValue, TMonoid}(Writer{TState, TValue, TMonoid}, Func{TValue, TState})"/> (as a specialized replacement for <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>; and <see cref="Writer{TState, TValue, TMonoid}.MapWriter{TResult}(Func{TValue, TResult})"/> (as a specialized replacement for <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>).
    /// </remarks>
    public class Writer<TOutput, TValue, TMonoid> : IMonad<TValue>
        where TMonoid : IMonoid<TOutput>
    {
        /// <summary>
        /// The write-only output of the computation.
        /// </summary>
        public TOutput State { get; private set; }
        /// <summary>
        /// The result of the computation.
        /// </summary>
        public TValue Result { get; private set; }

        /// <summary>
        /// The output monoid for combining outputs.
        /// </summary>
        public IMonoid<TOutput> Monoid { get; private set; }

        /// <summary>
        /// Creates a new writer from a result.
        /// </summary>
        /// <param name="result">The result to produce.</param>
        public Writer(TValue result)
        {
            var neutral = Algebra.Monoid.NeutralUnsafe<TOutput, TMonoid>();
            State = neutral;
            Result = result;
            Monoid = new Monoid<TOutput>(neutral, Algebra.Monoid.OpUnsafe<TOutput, TMonoid>());
        }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="state">The initial state of the writer.</param>
        /// <param name="result">The result to produce.</param>
        public Writer(TValue result, TOutput state)
        {
            State = state;
            Result = result;
            Monoid = new Monoid<TOutput>(Algebra.Monoid.NeutralUnsafe<TOutput, TMonoid>(), Algebra.Monoid.OpUnsafe<TOutput, TMonoid>());
        }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="state">The initial state of the writer.</param>
        /// <param name="result">The result to produce.</param>
        /// <param name="monoid">The monoid on <typeparamref name="TOutput"/>.</param>
        public Writer(TValue result, TOutput state, IMonoid<TOutput> monoid)
        {
            State = state;
            Result = result;
            Monoid = monoid;
        }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="args">The initial state of the writer, the result to produce, and the monoid on <typeparamref name="TOutput"/>.</param>
        public Writer((TValue result, TOutput state, IMonoid<TOutput> monoid) args)
        {
            State = args.state;
            Result = args.result;
            Monoid = args.monoid;
        }

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is Writer<TOutput, Func<TValue, TResult>, TMonoid>))
                throw new InvalidCastException();

            var fWriter = f as Writer<TOutput, Func<TValue, TResult>, TMonoid>;

            return new Writer<TOutput, TResult, TMonoid>(fWriter.Result(Result), Monoid.Op(fWriter.State, State), Monoid);
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
        {
            var ret = f(Result) as Writer<TOutput, TResult, TMonoid>;

            return new Writer<TOutput, TResult, TMonoid>(ret.Result, Monoid.Op(State, ret.State));
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new Writer<TOutput, TResult, TMonoid>(f(Result), State);

        /// <summary>
        /// A specialized version of <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/> which preserves the type-information that the result is a <see cref="Writer{TState, TValue, TMonoid}"/>. Useful in oder to avoid having to specify types.
        /// </summary>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="f">The </param>
        /// <returns></returns>
        public Writer<TOutput, TResult, TMonoid> MapWriter<TResult>(Func<TValue, TResult> f)
            => new Writer<TOutput, TResult, TMonoid>(f(Result), State);

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Writer<TOutput, TResult, TMonoid>(x);
    }

    /// <summary>
    /// Extension methods for <see cref="Writer{TState, TValue, TMonoid}"/>.
    /// </summary>
    public static class Writer
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Writer{TState, TValue, TMonoid}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TOutput">The type of the state.</typeparam>
        /// <typeparam name="TMonoid">The type of the output monoid.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Writer<TOutput, TResult, TMonoid> Select<TOutput, TMonoid, TSource, TResult>(this Writer<TOutput, TSource, TMonoid> source, Func<TSource, TResult> f)
            where TMonoid : IMonoid<TOutput>
            => (Writer<TOutput, TResult, TMonoid>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Writer{TState, TValue, TMonoid}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TOutput">The type of the state.</typeparam>
        /// <typeparam name="TMonoid">The type of the output monoid.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Writer<TOutput, TResult, TMonoid> SelectMany<TOutput, TMonoid, TSource, TMiddle, TResult>
            (this Writer<TOutput, TSource, TMonoid> source,
             Func<TSource, Writer<TOutput, TMiddle, TMonoid>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            where TMonoid : IMonoid<TOutput>
        {
            var sourceRes = source.Result;
            var midRes = f(sourceRes);
            var midState = source.Monoid.Op(source.State, midRes.State);

            return new Writer<TOutput, TResult, TMonoid>(resultSelector(sourceRes, midRes.Result), midState);
        }

        /// <summary>
        /// Stores a new output in the writer and returns no result.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TOutput"/>.</typeparam>
        /// <param name="state">The output to store.</param>
        public static Writer<TOutput, Unit, TMonoid> Tell<TOutput, TMonoid>(TOutput state)
            where TMonoid : IMonoid<TOutput>
            => new Writer<TOutput, Unit, TMonoid>(new Unit(), state);

        /// <summary>
        /// Stores a new output in the writer and returns no result.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TOutput"/>.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="state">The output to store.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Writer<TOutput, Unit, TMonoid> Tell<TOutput, TMonoid>(this WriterCxt<TOutput, TMonoid> _cxt, TOutput state)
            where TMonoid : IMonoid<TOutput>
            => new Writer<TOutput, Unit, TMonoid>(new Unit(), state);

        /// <summary>
        /// Stores a new output in the writer.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The result of the writer.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TOutput"/>.</typeparam>
        /// <param name="w">The writer.</param>
        /// <param name="f">The function which takes the writer's result and returns the output to store.</param>
        public static Writer<TOutput, TValue, TMonoid> BindTell<TOutput, TValue, TMonoid>(this Writer<TOutput, TValue, TMonoid> w, Func<TValue, TOutput> f)
            where TMonoid : IMonoid<TOutput>
        {
            return (Writer<TOutput, TValue, TMonoid>)w.Bind(x => (Writer<TOutput, TValue, TMonoid>)Tell<TOutput, TMonoid>(f(x)).Map(_ => x));
        }

        /// <summary>
        /// Returns the result of the computation as well as the write-output.
        /// </summary>
        /// <typeparam name="TValue">The result of the original writer.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The <see cref="IMonoid{T}"/> on <typeparamref name="TOutput"/>.</typeparam>
        /// <param name="w">The writer.</param>
        public static Writer<TOutput, (TValue, TOutput), TMonoid> Listen<TValue, TOutput, TMonoid>(this Writer<TOutput, TValue, TMonoid> w)
            where TMonoid : IMonoid<TOutput>
        {
            return new Writer<TOutput, (TValue, TOutput), TMonoid>((w.Result, w.State), w.State, w.Monoid);
        }

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Writer{TState, TValue, TMonoid}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="TValue">The type of the value contained in the functor.</typeparam>
        /// <typeparam name="TOutput">The type of the writer's state.</typeparam>
        /// <typeparam name="TMonoid">The type of the writer's monoid.</typeparam>
        /// <param name="f">The functor to cast to a writer.</param>
        public static Writer<TOutput, TValue, TMonoid> ToWriter<TOutput, TValue, TMonoid>(this IFunctor<TValue> f) where TMonoid : IMonoid<TOutput> => (Writer<TOutput, TValue, TMonoid>)f;
    }
}
