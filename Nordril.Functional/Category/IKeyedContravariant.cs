using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A keyed contravariant functor. A contravariant functor is a functor that <em>needs</em> keys of type <typeparamref name="TKey"/> and values of type <typeparamref name="TSource"/> instead of containing them.
    /// </summary>
    /// <typeparam name="TKey">The type of keys needed by the functor.</typeparam>
    /// <typeparam name="TSource">The type of values needed by the functor.</typeparam>
    public interface IKeyedContravariant<TKey, in TSource> : IContravariant<TSource>
    {
        /// <summary>
        /// Applies a function to the functor and returns a new functor without changing the original functor.
        /// The first argument of the function is the key associated with the value.
        /// Implementors must fulfill the following for all X and functions f and g:
        /// <code>
        ///     X.ContraMap((k, a) => a) == X (identity)<br />
        ///     X.ContraMap((k, a) => g(k, f(k, a))) == X.Map(g).Map(f) (contravariant homomorphism)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the functor.</param>
        IKeyedContravariant<TKey, TResult> ContraMapWithKey<TResult>(Func<TKey, TResult, TSource> f);
    }
}
