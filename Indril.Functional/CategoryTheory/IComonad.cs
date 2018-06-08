using Indril.Functional.Function;
using System;

namespace Indril.Functional.CategoryTheory
{
    /// <summary>
    /// A comonad, which is the categorical dual of a <see cref="IMonad{TSource}"/>.
    /// Whereas monads support the wrapping of values, comonads support their unwrapping.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IComonad<TSource> : IFunctor<TSource>
    {
        /// <summary>
        /// Extracts a new value from the comonad.
        /// </summary>
        TSource Extract();
        /// <summary>
        /// Takes a function that extracts values from a comonad and chains it to this comonad by wrapping its result into a comonad.
        /// Implementros must fulfill the following for all X and f, g:
        /// <code>
        ///     X.Extend(x => x.Extract()) = id (comonadic left-identity)
        ///     X.Extend(f).Extract() = f (comonadic right-identity)
        ///     X.Extend(g).Extend(f) = X.Extend(x => f(x.Extend(g))) (comonadic associativity)
        /// </code>
        /// Alternately, one can state these laws via Cokleisli composition =>=:
        /// <code>
        ///     f =>= (x => x.Extract() = f (comonadic left-identity*)
        ///     (x => x.Extract()) =>= f = f (comonadic right-identity*)
        ///     (f =>= g) =>= h = f =>= (g =>= h) (comonadic associativity*)
        /// </code>
        /// For more detail, see https://hackage.haskell.org/package/comonad.
        /// </summary>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="f">The function to chain to the comonad.</param>
        IComonad<TResult> Extend<TResult>(Func<IComonad<TSource>, TResult> f);
    }

    /// <summary>
    /// Extension methods for <see cref="IComonad{TSource}"/>.
    /// </summary>
    public static class ComonadExtensions
    {
        /// <summary>
        /// The dual of <see cref="MonadExtensions.Join{T}(IMonad{IMonad{T}})"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of values produced by the comonad.</typeparam>
        /// <param name="m">The comonad to duplicate.</param>
        public static IComonad<IComonad<TSource>> Duplicate<TSource>(this IComonad<TSource> m) => m.Extend(Functions.Id<IComonad<TSource>>());

        //TODO: cokleisli composition
    }
}
