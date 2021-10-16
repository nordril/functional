using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A fusing of <see cref="Reader{TState, TValue}"/>, <see cref="Writer{TState, TValue, TMonoid}"/>, and <see cref="State{TState, TValue}"/>, where computations can read from an environment <typeparamref name="TEnvironment"/>, write to an output <typeparamref name="TOutput"/>, and modify a state <typeparamref name="TState"/>.
    /// <br />
    /// See also <see cref="RwsCxt{TEnvironment, TOutput, TMonoid, TState}"/> on how to make usage more convenient by being able to omit type parameter.
    /// </summary>
    /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the result.</typeparam>
    public class Rws<TEnvironment, TOutput, TMonoid, TState, TValue> : IMonad<TValue>
        where TMonoid : IMonoid<TOutput>
    {
        /// <summary>
        /// The RWS-function.
        /// </summary>
        private readonly Func<TEnvironment, TState, (TValue, TState, TOutput)> runRws;

        /// <summary>
        /// The output monoid for combining outputs.
        /// </summary>
        public IMonoid<TOutput> Monoid { get; private set; }

        /// <summary>
        /// Creates a new RWS from a result.
        /// </summary>
        /// <param name="result">The result to produce.</param>
        public Rws(TValue result)
        {
            var neutral = Algebra.Monoid.NeutralUnsafe<TOutput, TMonoid>();
            Monoid = new Monoid<TOutput>(neutral, Algebra.Monoid.OpUnsafe<TOutput, TMonoid>());
            runRws = (e, s) => (result, s, neutral);
        }

        /// <summary>
        /// Creates a new RWS from an RWS-function.
        /// </summary>
        /// <param name="runRws">The RWS-function.</param>
        public Rws(Func<TEnvironment, TState, (TValue, TState, TOutput)> runRws)
        {
            var neutral = Algebra.Monoid.NeutralUnsafe<TOutput, TMonoid>();
            Monoid = new Monoid<TOutput>(neutral, Algebra.Monoid.OpUnsafe<TOutput, TMonoid>());
            this.runRws = runRws;
        }

        /// <summary>
        /// Creates a new RWS from an RWS-function.
        /// </summary>
        /// <param name="runRws">The RWS-function.</param>
        /// <param name="monoid">The monooid on <typeparamref name="TMonoid"/>.</param>
        public Rws(Func<TEnvironment, TState, (TValue, TState, TOutput)> runRws, IMonoid<TOutput> monoid)
        {
            this.runRws = runRws;
            Monoid = monoid;
        }

        /// <summary>
        /// Runs the RWS-computation and returns the final result, state, and output.
        /// </summary>
        /// <param name="environment">The read-only environment.</param>
        /// <param name="state">The mutable state.</param>
        public (TValue value, TState state, TOutput output) Run(TEnvironment environment, TState state) => runRws(environment, state);

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TValue, IMonad<TResult>> f)
            => new Rws<TEnvironment, TOutput, TMonoid, TState, TResult>((e, s) =>
            {
                var (a1, s1, w1) = runRws(e, s);
                var (a2, s2, w2) = (f(a1) as Rws<TEnvironment, TOutput, TMonoid, TState, TResult>).runRws(e, s1);
                return (a2, s2, Monoid.Op(w1, w2));
            });

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Rws<TEnvironment, TOutput, TMonoid, TState, TResult>(x);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TValue, TResult>> f)
            => new Rws<TEnvironment, TOutput, TMonoid, TState, TResult>((e, s) =>
            {
                if (f == null || f is not Rws<TEnvironment, TOutput, TMonoid, TState, Func<TValue, TResult>> fThat)
                    throw new InvalidCastException();

                var (a1, s1, w1) = runRws(e, s);
                var (a2, s2, w2) = fThat.runRws(e, s1);
                return (a2(a1), s2, Monoid.Op(w1, w2));
            });

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
        => new Rws<TEnvironment, TOutput, TMonoid, TState, TResult>((e, s) =>
        {
            var (a1, s1, w1) = runRws(e, s);
            return (f(a1), s1, w1);
        });
    }

    /// <summary>
    /// Extension methods for <see cref="Rws{TEnvironment, TOutput, TMonoid, TState, TValue}"/>.
    /// </summary>
    public static class Rws
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Rws{TEnvironment, TOutput, TMonoid, TState, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TResult> Select<TEnvironment, TOutput, TMonoid, TState, TSource, TResult>(this Rws<TEnvironment, TOutput, TMonoid, TState, TSource> source, Func<TSource, TResult> f)
            where TMonoid : IMonoid<TOutput>
            => (Rws<TEnvironment, TOutput, TMonoid, TState, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Rws{TEnvironment, TOutput, TMonoid, TState, TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TResult> SelectMany<TEnvironment, TOutput, TMonoid, TState, TSource, TMiddle, TResult>
            (this Rws<TEnvironment, TOutput, TMonoid, TState, TSource> source,
             Func<TSource, Rws<TEnvironment, TOutput, TMonoid, TState, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            where TMonoid : IMonoid<TOutput>
            =>
            new ((e,s) =>
            {
                var (a1, s1, w1) = source.Run(e, s);
                var (a2, s2, w2) = f(a1).Run(e, s1);
                return (resultSelector(a1, a2), s2, source.Monoid.Op(w1, w2));
            });

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TState> Get<TEnvironment, TOutput, TMonoid, TState>()
            where TMonoid : IMonoid<TOutput>
            => new ((e,s) => (s,s,Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Returns the current state.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TState> Get<TEnvironment, TOutput, TMonoid, TState>(
            this RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (s, s, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Replaces the current state with a new one.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="value">The new state.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Put<TEnvironment, TOutput, TMonoid, TState>(TState value)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (new Unit(), value, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Replaces the current state with a new one.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="value">The new state.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Put<TEnvironment, TOutput, TMonoid, TState>(
            this RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt, TState value)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (new Unit(), value, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Modifies the current state by running a function on it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="f">The function to apply to the state.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Modify<TEnvironment, TOutput, TMonoid, TState>(Func<TState, TState> f)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (new Unit(), f(s), Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Modifies the current state by running a function on it.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        /// <param name="f">The function to apply to the state.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Modify<TEnvironment, TOutput, TMonoid, TState>(
            this RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt,
            Func<TState, TState> f)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (new Unit(), f(s), Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Returns the current environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TEnvironment> GetEnvironment<TEnvironment, TOutput, TMonoid, TState>()
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (e, s, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Returns the current environment.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TEnvironment> GetEnvironment<TEnvironment, TOutput, TMonoid, TState>(
            this RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (e, s, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Returns the result to of a function which takes the state as an input. Also known as <em>reader</em>.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="f">The function to run.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TResult> With<TEnvironment, TOutput, TMonoid, TState, TResult>(Func<TEnvironment, TResult> f)
            where TMonoid : IMonoid<TOutput>
            => new ((e,s) => (f(e), s, Monoid.NeutralUnsafe<TOutput, TMonoid>()));

        /// <summary>
        /// Returns a <see cref="Reader{TEnvironment, TValue}"/> which runs <paramref name="r"/>, but first applies <paramref name="f"/> to the environment, effectively running a <see cref="Reader"/> in a modified environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the function's result.</typeparam>
        /// <param name="f">The function which modifies the environment.</param>
        /// <param name="r">The <see cref="Reader"/> to run in the modified environment.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, TResult> Local<TEnvironment, TOutput, TMonoid, TState, TResult>(Func<TEnvironment, TEnvironment> f, Rws<TEnvironment, TOutput, TMonoid, TState, TResult> r)
            where TMonoid : IMonoid<TOutput>
            => new ((e,s) => r.Run(f(e), s));

        /// <summary>
        /// Stores a new output.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name = "output" > The output to store and return.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Tell<TEnvironment, TOutput, TMonoid, TState>(TOutput output)
            where TMonoid : IMonoid<TOutput>
            => new ((e,s) => (new Unit(), s, output));


        /// <summary>
        /// Stores a new output.
        /// This is a convenience-method which does not require explicitly specifying the type arguments.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid used to combine outputs.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="output"> The output to store and return.</param>
        /// <param name="_cxt">The context to fix the type variables.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "type tag")]
        public static Rws<TEnvironment, TOutput, TMonoid, TState, Unit> Tell<TEnvironment, TOutput, TMonoid, TState>(
            this RwsCxt<TEnvironment, TOutput, TMonoid, TState> _cxt,
            TOutput output)
            where TMonoid : IMonoid<TOutput>
            => new ((e, s) => (new Unit(), s, output));

        /// <summary>
        /// Returns the result of the computation as well as the write-output.
        /// </summary>
        /// <param name="rws">The RWS to which to tell the output.</param>
        public static Rws<TEnvironment, TOutput, TMonoid, TState, (TValue, TOutput)> Listen<TEnvironment, TOutput, TMonoid, TState, TValue>(this Rws<TEnvironment, TOutput, TMonoid, TState, TValue> rws)
            where TMonoid : IMonoid<TOutput>
        {
            return new Rws<TEnvironment, TOutput, TMonoid, TState, (TValue, TOutput)>((e, s) => {
                var (a1, s1, w1) = rws.Run(e, s);
                return ((a1,w1), s1, w1);
            });
        }
    }
}
