using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A keyed functor, meaning a container which not only has values of type <typeparamref name="TSource"/>, but also immutable keys of type <typeparamref name="TKey"/> which the mapping-function may use as input.
    /// </summary>
    /// <typeparam name="TKey">The type of keys contained in the functor.</typeparam>
    /// <typeparam name="TSource">The type of values contained in the functor.</typeparam>
    public interface IKeyedFunctor<out TKey, out TSource> : IFunctor<TSource>
    {
        /// <summary>
        /// Applies a function to the functor and returns a new functor without changing the original functor.
        /// The first argument of the function is the key associated with the value.
        /// Implementors must fulfill the following for all X and functions f and g:
        /// <code>
        ///     X.Map((k,a) => a) == X (identity)<br />
        ///     X.Map((k,a) => g(k, f(k, a))) == X.Map(f).Map(g) (homomorphism)<br />
        /// </code>
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the functor.</param>
        IKeyedFunctor<TKey, TResult> MapWithKey<TResult>(Func<TKey, TSource, TResult> f);
    }
}
