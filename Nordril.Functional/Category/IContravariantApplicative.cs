using Nordril.Functional.Algebra;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An <see cref="IPhantomFunctor{T}"/> that is also an <see cref="IApplicative{TSource}"/>.
    /// The stereotypical example is <see cref="Const{TMonoid, TReal, TPhantom}"/>, which can use the <see cref="IMonoid{T}"/>-instance of the real value to put in a neutral element for <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
    /// </summary>
    /// <typeparam name="TSource">The phantom type.</typeparam>
    public interface IContravariantApplicative<TSource>
        : IApplicative<TSource>
        , IPhantomFunctor<TSource>
    {
    }
}
