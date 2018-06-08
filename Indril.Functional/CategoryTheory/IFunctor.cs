using Indril.Functional.Data;
using System;

namespace Indril.Functional.CategoryTheory
{
    /// <summary>
    /// A functor. A functor is a container that contains 0 or more instances of <typeparamref name="TSource"/>.
    /// One can apply functions to functors to change their contained type.
    /// </summary>
    /// <typeparam name="TSource">The data contained in the functor.</typeparam>
    public interface IFunctor<out TSource>
    {
        /// <summary>
        /// Applies a function to the functor and returns a new functor without changing the original functor.
        /// Implementors must fulfill the following for all X and functions f and g:
        /// <code>
        ///     X.Map(a => a) == X (identity)
        ///     X.Map(a => g(f(a))) == X.Map(f).Map(g) (homomorphism)
        /// </code>
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the functor.</param>
        IFunctor<TResult> Map<TResult>(Func<TSource, TResult> f);
    }

    /// <summary>
    /// Extensions for the <see cref="IFunctor{TSource}"/> class.
    /// </summary>
    public static class FunctorExtensions
    {
        /// <summary>
        /// Erases the contents of a functor.
        /// </summary>
        /// <typeparam name="TSource">The source type of the functor.</typeparam>
        /// <param name="f">The functor.</param>
        public static IFunctor<Unit> Void<TSource>(this IFunctor<TSource> f) => f.Map(x => new Unit());

        /// <summary>
        /// Applies an action to a functor and returns it.
        /// The action is presumable mutating.
        /// </summary>
        /// <typeparam name="TSource">The source type of the functor.</typeparam>
        /// <param name="f">The functor.</param>
        /// <param name="a">The action to apply to the values in the functor.</param>
        public static IFunctor<TSource> Map<TSource>(this IFunctor<TSource> f, Action<TSource> a)
            => f.Map(x => { a(x); return x; });
    }
}
