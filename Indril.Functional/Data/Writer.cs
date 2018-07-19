using Indril.Functional.Algebra;
using Indril.Functional.Category;
using System;

namespace Indril.Functional.Data
{
    /// <summary>
    /// The writer-monad, which provides write-only access to output, in addition to producing a result.
    /// The output can then be later extracted, but not by the computations within the writer-monad themselves.
    /// </summary>
    /// <typeparam name="TState">The type of the output.</typeparam>
    /// <typeparam name="TValue">The type of the result.</typeparam>
    public class Writer<TState, TValue> : IMonad<TValue>
    {
        /// <summary>
        /// The write-only output of the computation.
        /// </summary>
        public TState Output { get; private set; }
        /// <summary>
        /// The result of the computation.
        /// </summary>
        public TValue Result { get; private set; }
        /// <summary>
        /// The monoid to use when combining outputs.
        /// </summary>
        public Monoid<TState> OutputMonoid { get; private set; }

        /// <summary>
        /// Creates a new writer from a result, an initial output, and a monoid to combine successive outputs.
        /// </summary>
        /// <param name="output">The initial output.</param>
        /// <param name="result">The result to produce.</param>
        /// <param name="outputMonoid">The monoid to use to combine outputs.</param>
        public Writer(TState output, TValue result, Monoid<TState> outputMonoid)
        {
            Output = output;
            Result = result;
            OutputMonoid = outputMonoid;
        }

        /// <summary>
        /// Creates a new writer from a result and a monoid to combine successive outputs.
        /// The neutral element of the monoid will be used as the initial output.
        /// </summary>
        /// <param name="result">The result to produce.</param>
        /// <param name="outputMonoid">The monoid to use to combine outputs.</param>
        public Writer(TValue result, Monoid<TState> outputMonoid) : this(outputMonoid.Neutral, result, outputMonoid)
        {
        }

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
        {
            if (f == null || !(f is Writer<TState, Func<TValue, TResult>>))
                throw new InvalidCastException();

            var fWriter = f as Writer<TState, Func<TValue, TResult>>;

            return new Writer<TState, TResult>(fWriter.OutputMonoid.Op(fWriter.Output, Output), fWriter.Result(Result), fWriter.OutputMonoid);
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
        {
            var ret = f(Result) as Writer<TState, TResult>;

            return new Writer<TState, TResult>(OutputMonoid.Op(Output, ret.Output), ret.Result, OutputMonoid);
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
            => new Writer<TState, TResult>(Output, f(Result), OutputMonoid);

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Writer<TState, TResult>(OutputMonoid.Neutral, x, OutputMonoid);
    }

    /// <summary>
    /// Extension methods for <see cref="Writer{TState, TValue}"/>.
    /// </summary>
    public static class Writer
    {
        /// <summary>
        /// Stores a new output in the writer and returns no result.
        /// The <see cref="IMonad{TSource}"/> instance of the output is used
        /// to combine the new output with the existing ones.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <param name="output">The output to store.</param>
        public static Writer<TState, Unit> Tell<TState>(TState output)
            where TState : IMonoid<TState>
            => new Writer<TState, Unit>(output, new Unit(),Monoid.FromMonoidInstance(output));
        
        /// <summary>
        /// Stores a new output in the writer and returns no result.
        /// The caller can specify how to combine the new piece of output with
        /// the existing ones.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <param name="output">The output to store.</param>
        /// <param name="monoid">The monoid to use when combining outputs.</param>
        public static Writer<TState, Unit> Tell<TState>(TState output, Monoid<TState> monoid)
            => new Writer<TState, Unit>(output, new Unit(), monoid);

        /// <summary>
        /// Stores a new output and returns it.
        /// The <see cref="IMonad{TSource}"/> instance of the output is used
        /// to combine the new output with the existing ones.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <param name="output">The output to store and return.</param>
        public static Writer<TState, TState> Listen<TState>(TState output)
            where TState : IMonoid<TState>
            => new Writer<TState, TState>(output, output, Monoid.FromMonoidInstance(output));

        /// <summary>
        /// Stores a new output and returns it.
        /// The <see cref="IMonad{TSource}"/> instance of the output is used
        /// to combine the new output with the existing ones.
        /// </summary>
        /// <typeparam name="TState">The type of the output.</typeparam>
        /// <param name="output">The output to store and return.</param>
        /// <param name="monoid">The monoid to use when combining outputs.</param>
        public static Writer<TState, TState> Listen<TState>(TState output, Monoid<TState> monoid)
            => new Writer<TState, TState>(output, output, monoid);
    }
}
