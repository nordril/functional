using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A lens, which is a combined <see cref="IGetter{S, A}"/> and <see cref="ISetter{S, T, A, B}"/>.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the object to get/modify in <typeparamref name="S"/>.</typeparam>
    public interface IMonoLens<S, A> : IGetter<S, A>, ISetter<S, S, A, A> /*, ITraversal<S, S, A, A>*/
    {
        /// <summary>
        /// Returns the lens-function, which must work with any <see cref="IFunctor{TSource}"/> for <typeparamref name="A"/> and <typeparamref name="S"/>.
        /// </summary>
        /// <typeparam name="FA">The concrete functor for <typeparamref name="A"/>.</typeparam>
        /// <typeparam name="FS">The concrete functor for <typeparamref name="S"/>.</typeparam>
        Func<Func<A, FA>, Func<S, FS>> LensFunc<FA, FS>()
            where FA : IFunctor<A>
            where FS : IFunctor<S>;

        /// <summary>
        /// Returns the lens-function, which must work with any <see cref="IFunctor{TSource}"/> for <typeparamref name="A"/> and <typeparamref name="S"/>.
        /// </summary>
        Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>> LensFunc(Type t);
    }
}
