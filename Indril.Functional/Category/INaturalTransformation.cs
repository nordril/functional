using Indril.Functional.Data;
using System;

namespace Indril.Functional.Category
{
    /// <summary>
    /// A natural transformation which transforms one functor into another.
    /// </summary>
    public interface INaturalTransformation
    {
        /// <summary>
        /// Transforms one functor into another. If the implementor can't transform between
        /// the requested source and target functors, it should return Nothing.
        /// </summary>
        /// <typeparam name="TSource">The type of the source elements.</typeparam>
        /// <typeparam name="TFSource">The type of the source functor.</typeparam>
        /// <typeparam name="TFTarget">The type of the target functor.</typeparam>
        /// <param name="s">The source functor.</param>
        /// <returns></returns>
        Maybe<TFTarget> Transform<TSource, TFSource, TFTarget>(TFSource s)
            where TFSource : IFunctor<TSource>
            where TFTarget : IFunctor<TSource>;
    }
}
