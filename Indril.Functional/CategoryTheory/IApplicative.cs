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
    }
}