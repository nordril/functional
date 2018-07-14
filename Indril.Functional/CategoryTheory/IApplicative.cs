using Indril.Functional.Function;
using System;

namespace Indril.Functional.CategoryTheory
{
    /// <summary>
    /// An applicative functor which supports applying multi-argument functions to containers.
    /// Implementations must fulfill the following (assuming that Pure(a) == X.Pure(a) for readability)
    /// for all a, f, g, h.
    /// <code>
    ///     Pure(a).Ap(b => b) == Pure(a) (identity)
    ///     Pure(a).Ap(Pure(f)) == Pure(f(a)) (homomorphism)
    ///     Pure(a).Ap(f) == f.Ap(Pure(a => f(a))) (interchange)
    ///     h.Ap(g).Ap(f) == h.Ap(g.Ap(f.Ap(Pure(comp)))) (composition)
    ///     where
    ///        comp f2 f1 x = f2(f1(x)) 
    /// </code>
    /// </summary>
    /// <typeparam name="TSource">The data contained in the functor.</typeparam>
    public interface IApplicative<TSource> : IFunctor<TSource>
    {
        /// <summary>
        /// Wraps a value into an applicative. The this-value must be ignored by implementors.
        /// </summary>
        /// <typeparam name="TResult">The type of the value to wrap.</typeparam>
        /// <param name="x">The value to wrap.</param>
        IApplicative<TResult> Pure<TResult>(TResult x);

        /// <summary>
        /// Takes an applicative value and a function wrapped in an applicative value,
        /// and applies the function to this container.
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="f">The wrapped function to apply.</param>
        IApplicative<TResult> Ap<TResult>(IApplicative<Func<TSource, TResult>> f);
    }

    /// <summary>
    /// Extensions for <see cref="IApplicative{TSource}"/>.
    /// </summary>
    public static class ApplicativeExtensions
    {
        /// <summary>
        /// <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/> with the arguments reversed,
        /// meaning that the function is applied to the argument, which reads more natural.
        /// </summary>
        /// <typeparam name="TSource">The argument type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="f">The applicative containing the function mapping the source type to the result type.</param>
        /// <param name="x">The applicative containing the source type.</param>
        public static IApplicative<TResult> ApF<TSource, TResult>(this IApplicative<Func<TSource, TResult>> f, IApplicative<TSource> x) => x.Ap(f);

        /// <summary>
        /// Wraps an object in an applicative via <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the object to wrap.</typeparam>
        /// <typeparam name="TResult">The type of the applicative to return.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static TResult Pure<TSource, TResult>(this TSource x)
            where TResult : IApplicative<TSource>, new() => (TResult)(new TResult().Pure(x));

        /// <summary>
        /// Lifts a binary function to take two applicative arguments.
        /// </summary>
        /// <example>
        /// <code>
        ///     var x = Maybe.Just(5);
        ///     var y = Maybe.Nothing&lt;int&gt;();
        ///     Func&lt;int,int,int&gt; f = (x,y) => x + y;
        ///     
        ///     //f takes integer arguments, but we can apply it to two maybe-arguments,
        ///     //with automatic unpacking and packing of the results, via liftA*.
        ///     var result = f.LiftA2()(x, y);
        /// </code>
        /// </example>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<TResult>>
            LiftA<T1, T2, TResult>(this Func<T1, T2, TResult> f)
        => (x, y) => (x.Map(f.Curry()) as IApplicative<Func<T2, TResult>>).ApF(y);

        /// <summary>
        /// Lifts a ternary function to take three applicative arguments. See <see cref="LiftA{T1, T2, TResult}(Func{T1, T2, TResult})"/>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<T3>, IApplicative<TResult>>
            LiftA<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> f)
        => (x, y, z) => (x.Map(f.Curry3()) as IApplicative<Func<T2, Func<T3, TResult>>>).ApF(y).ApF(z);

        /// <summary>
        /// Lifts a quaternary function to take three applicative arguments. See <see cref="LiftA{T1, T2, TResult}(Func{T1, T2, TResult})"/>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the second argument.</typeparam>
        /// <typeparam name="T4">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<T3>, IApplicative<T4>, IApplicative<TResult>>
            LiftA<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, TResult> f)
        => (x, y, z, u) => (x.Map(f.Curry3()) as IApplicative<Func<T2, Func<T3, Func<T4, TResult>>>>).ApF(y).ApF(z).ApF(u);
    }
}