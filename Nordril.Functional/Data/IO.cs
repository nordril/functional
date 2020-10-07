using Nordril.Functional.Category;
using Nordril.Functional.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A value which has an arbitrary computation (mainly Io) attached to it. This corresponds to <see cref="Func{TResult}"/>, since in C#, any function can perform Io without declaring that fact in its type; however, this type enables one to build lazy pipelines via the <see cref="IMonad{TSource}"/>-functionality, like mapping over the result while executing the attached action, run a multi-argument function over the results of computations, and composing functions. The boilerplate which this type takes care of is essentially that of function composition.
    /// </summary>
    /// <example>
    /// Suppose we have two actions: one which reads from the command-line and one which writes to it. We then assemble a computation which first reads two numbers from the command-line, adds them, and prints out the results:
    /// <code>
    ///     var read = Io.ToIo(() => Console.ReadLine()).Map(int.Parse).ToIo();
    ///     var write = ((int x) => Console.WriteLine(x)).AsIoAction();
    ///     
    ///     var pipeline = read.Bind(x => read.Bind(y => write(x,y))).ToIo();
    ///     
    ///     //So far, nothing has been read from or written to console, pipeline is a delayed computation.
    ///     
    ///     //Now we run it
    ///     pipeline.Run();
    ///     //Results:
    ///     //&lt;read line&gt; 5
    ///     //&lt;read line&gt; 3
    ///     //&lt;write line&gt; 8
    /// </code>
    /// </example>
    /// <typeparam name="T">The type of the result.</typeparam>
    public struct Io<T> : IMonad<T>, IAsyncMonad<T>
    {
        private readonly Func<Task<T>> value;

        /// <summary>
        /// Creates a new <see cref="Io{T}"/> out of a <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="value">The synchronous function which constitutes this computation.</param>
        public Io(Func<T> value)
        {
            this.value = () => Task.FromResult(value());
        }

        /// <summary>
        /// Creates a new <see cref="Io{T}"/> out of a <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="value">The asynchronous function which constitutes this computation.</param>
        public Io(Func<Task<T>> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the result of the computation, while also performing the side-effect.
        /// </summary>
        public Task<T> Run() => value();

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (!(f is Io<Func<T, TResult>> fIo))
                throw new InvalidCastException();

            var valueThis = value;

            return new Io<TResult>(async () => (await fIo.Run())(await valueThis()));
        }

        /// <summary>
        /// Chains another <see cref="Io{T}"/>-computation to this one.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the second computation.</typeparam>
        /// <param name="f">A function which takes the result of this computation and returns a new one. The return-type of the function has to be <see cref="Io{T}"/> with type <typeparamref name="TResult"/>, otherwise, an <see cref="InvalidCastException"/> is thrown.</param>
        /// <exception cref="InvalidCastException">If the result of <paramref name="f"/> isn't <see cref="Io{T}"/> with type <typeparamref name="TResult"/>.</exception>
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
        {
            var valueThis = value;

            return new Io<TResult>(async () => await ((Io<TResult>)f(await valueThis())).Run());
        }

        /// <summary>
        /// Chains a function to this computation, with the same semantics as <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>, i.e. lazily.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the second computation.</typeparam>
        /// <param name="f">The function which takes the argument of this computation and returns a result, possibly performing side-effects.</param>
        public Io<TResult> BindFunc<TResult>(Func<T, TResult> f)
        {
            var valueThis = value;
            return new Io<TResult>(async () => f(await valueThis()));
        }

        /// <summary>
        /// Chains an <see cref="Action{T}"/> to this computation, with the same semantics as <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>, i.e. lazily, and returns <see cref="Unit"/> as the overall result.
        /// </summary>
        /// <param name="f">The function which takes the argument of this computation and returns no result, possibly performing side-effects.</param>
        public Io<Unit> BindAction(Action<T> f)
        {
            var valueThis = value;
            return new Io<Unit>(async () => { f(await valueThis()); return new Unit(); });
        }
        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
        {
            var valueThis = value;
            return new Io<TResult>(async () => f(await valueThis()));
        }

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new Io<TResult>(() => x);

        /// <inheritdoc />
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
        {
            var valueThis = value;

            return new Io<TResult>(
                async () => await ((Io<TResult>)await f(await valueThis())).Run());
        }

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => Pure(await x());

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            if (!(f is Io<Func<T, Task<TResult>>> fIo))
                throw new InvalidCastException();

            var valueThis = value;

            return new Io<TResult>(async () => await (await fIo.Run())(await valueThis()));
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
        {
            var valueThis = value;
            return new Io<TResult>(async () => await f(await valueThis()));
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }

    /// <summary>
    /// Extension methods for <see cref="Io{T}"/>.
    /// </summary>
    public static class Io
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Io{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Io<TResult> Select<TSource, TResult>(this Io<TSource> source, Func<TSource, TResult> f)
            => (Io<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Io{TValue}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Io<TResult> SelectMany<TSource, TMiddle, TResult>
            (this Io<TSource> source,
             Func<TSource, Io<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
        {
            return new Io<TResult>(async () =>
            {
                var sourceRes = await source.Run();
                var middleRes = await f(sourceRes).Run();
                return resultSelector(sourceRes, middleRes);
            });
        }

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Io{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Io<TResult>> Select<TSource, TResult>(
            this Task<Io<TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Io{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Io<TResult>> SelectMany<TSource, TMiddle, TResult>
            (this Task<Io<TSource>> source,
             Func<TSource, Task<Io<TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Io<TResult>)(await (await source).BindAsync(async x => (IAsyncMonad<TResult>)(await f(x)).Map(y => resultSelector(x, y))));

        /// <summary>
        /// An isomorphism between <see cref="Io{T}"/> and <see cref="Func{T, TResult}"/>.
        /// </summary>
        /// <typeparam name="T">The underlying type to wrap.</typeparam>
        private class IoIso<T> : IIsomorphism<Func<Task<T>>, Io<T>>
        {
            public Func<Task<T>> ConvertBackWith(Unit _, Io<T> from) => () => from.Run();

            public Io<T> ConvertWith(Unit _, Func<Task<T>> from) => new Io<T>(from);
        }

        /// <summary>
        /// Returns a value as <see cref="Io{T}"/>, without performing any side-effects. Shorthand for <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to wrap in <see cref="Io{T}"/>.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static Io<T> Pure<T>(T x) => new Io<T>(() => x);

        /// <summary>
        /// Wraps a function into an <see cref="Io{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="f">The function to wrap.</param>
        public static Io<T> AsIo<T>(this Func<T> f) => new Io<T>(f);

        /// <summary>
        /// Wraps an asynchronous function into an <see cref="Io{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="f">The function to wrap.</param>
        public static Io<T> AsIo<T>(this Func<Task<T>> f) => new Io<T>(f);

        /// <summary>
        /// Lifts a <see cref="Func{T, TResult}"/> into a function which takes a value of type <typeparamref name="T"/> and returns a value of type <typeparamref name="TResult"/> into a function which takes a value of type <typeparamref name="T"/> and returns an <see cref="Io{T}"/>. Useful for <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/> on <see cref="Io{T}"/>-computations.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift into <see cref="Io{T}"/>.</param>
        public static Func<T, Io<TResult>> LiftIoFunc<T, TResult>(this Func<T, TResult> f) => x => new Io<TResult>(() => f(x));

        /// <summary>
        /// Lifts an <see cref="Action{T, TResult}"/> into a function which takes a value of type <typeparamref name="T"/> into a function which takes a value of type <typeparamref name="T"/> and returns an <see cref="Io{T}"/> with no result. Useful for <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/> on <see cref="Io{T}"/>-computations.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift into <see cref="Io{T}"/>.</param>
        public static Func<T, Io<Unit>> LiftIoAction<T, TResult>(this Action<T> f) => x => new Io<Unit>(() => { f(x); return new Unit(); });

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Io{T}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to a maybe.</param>
        public static Io<T> ToIo<T>(this IFunctor<T> f) => (Io<T>)f;

        /// <summary>
        /// Returns an isomorphism between <see cref="Func{TResult}"/> and <see cref="Io{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result of computation.</typeparam>
        public static IIsomorphism<Func<Task<T>>, Io<T>> Iso<T>() => new IoIso<T>();
        
        /// <summary>
        /// Unwraps an <see cref="Io{T}"/> into a <see cref="Func{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the result of the computation.</typeparam>
        /// <param name="io">The computation to unwrap.</param>
        public static Func<Task<T>> Unwrap<T>(this Io<T> io) => () => io.Run();
    }
}
