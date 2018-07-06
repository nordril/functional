using System;
using System.Collections.Generic;
using System.Text;
using Indril.Functional.Data;
using Indril.TypeToolkit;

namespace Indril.Functional.CategoryTheory.NaturalTransformations
{
    /// <summary>
    /// A natural transformation from <see cref="Maybe{T}"/> to <see cref="FuncList{T}"/> which creates a 0-element list for <see cref="Maybe.Nothing{T}"/> and a 1-element list containing <see cref="Maybe{T}.Value"/> for <see cref="Maybe{T}.Just(T)"/>.
    /// </summary>
    public class MaybeToList : INaturalTransformation
    {
        /// <inheritdoc />
        public Maybe<TFTarget> Transform<TSource, TFSource, TFTarget>(TFSource s)
            where TFSource : IFunctor<TSource>
            where TFTarget : IFunctor<TSource>
        {
            if (typeof(TFSource) != typeof(Maybe<TSource>) || typeof(TFTarget) != typeof(List<TSource>))
                return Maybe.Nothing<TFTarget>();

            var sCast = (Maybe<TSource>)(object)s;

            return Maybe.Just((TFTarget)(object)sCast.ValueOrLazy(x => new FuncList<TSource> { x }, () => new FuncList<TSource>()));
        }
    }
}
