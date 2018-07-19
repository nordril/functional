using System;

namespace Indril.Functional.Category
{
    /// <summary>
    /// A contravariant functor. A contravariant functor is a functor that <em>needs</em> values of type <typeparamref name="TSource"/>
    /// instead of containing them.
    /// </summary>
    /// <typeparam name="TSource">The of the required values.</typeparam>
    public interface IContravariant<in TSource>
    {
        /// <summary>
        /// Applies a function to the functor and returns a new functor without changing the original functor.
        /// Implementors must fulfill the following for all X and functions f and g:
        /// <code>
        ///     X.ContraMap(a => a) == X (identity)
        ///     X.ContraMap(a => g(f(a))) == X.Map(g).Map(f) (contravariant homomorphism)
        /// </code>
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the functor.</param>
        IContravariant<TResult> ContraMap<TResult>(Func<TResult, TSource> f);
    }
}
