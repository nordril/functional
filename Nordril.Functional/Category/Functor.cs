using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A value-level functor which can map the contents of a data structure. See <see cref="IFunctor{TSource}"/>.
    /// </summary>
    /// <typeparam name="TFSource">The type of the source data structure.</typeparam>
    /// <typeparam name="TFResult">The type of the result data structure.</typeparam>
    /// <typeparam name="TSource">The type of the contained elements in <typeparamref name="TFSource"/>.</typeparam>
    /// <typeparam name="TResult">The type of the contained elements in <typeparamref name="TFResult"/>.</typeparam>
    public interface IFunctor<TFSource, TFResult, TSource, TResult>
    {
        /// <summary>
        /// Applies a function to each element of a data structure. Implementors must obey the same laws as in <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>.
        /// </summary>
        /// <param name="f">The function to apply.</param>
        TFResult Map(Func<TSource, TResult> f);
    }
}
