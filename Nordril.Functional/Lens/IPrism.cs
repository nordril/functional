using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A getter that "may work" for some types of input (e.g. on the left-case of <see cref="Either{TLeft, TRight}"/> only), and which can always be turned around (e.g. one can always turn a left-value into an see cref="Either{TLeft, TRight}"/>).
    /// N.B. Failure means "it returns Maybe.Nothing", not "throws an exception".
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="T">The type of the output data structure.</typeparam>
    /// <typeparam name="A">The type of the input-object in <typeparamref name="S"/>.</typeparam>
    /// <typeparam name="B">The type of the result-object in <typeparamref name="T"/>.</typeparam>
    public interface IPrism<S, T, A, B> : ISetter<S, T, A, B>
    {
        /// <summary>
        /// Returns the prism-function, which must work with any <see cref="IApplicative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="FB">The concrete functor for <typeparamref name="B"/>.</typeparam>
        /// <typeparam name="FT">The concrete functor for <typeparamref name="T"/>.</typeparam>
        /// <typeparam name="PAFB">The concrete profunctor from <typeparamref name="A"/> to <typeparamref name="FB"/>.</typeparam>
        /// <typeparam name="PSFT">The concrete profunctor from <typeparamref name="S"/> to <typeparamref name="FT"/>.</typeparam>
        Func<PAFB, PSFT> PrismFunc<FB, FT, PAFB, PSFT>()
            where FB : IApplicative<B>
            where FT : IApplicative<T>
            where PAFB : IChoice<A, FB>
            where PSFT : IChoice<S, FT>;

        /// <summary>
        /// Returns the prism-function, which must work with any <see cref="IApplicative{TSource}"/> for <typeparamref name="B"/> and <typeparamref name="T"/>.
        /// </summary>
        Func<IChoice<A, IApplicative<B>>, IChoice<S, IApplicative<T>>> PrismFunc(Type t);
    }
}
