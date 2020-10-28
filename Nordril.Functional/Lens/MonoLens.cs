using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A lens, which is a combined <see cref="IGetter{S, A}"/> and <see cref="ISetter{S, T, A, B}"/>.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the object to get/modify in <typeparamref name="S"/>.</typeparam>
    internal class MonoLens<S, A> : IMonoLens<S, A>
    {
        /// <inheritdoc />
        public Func<Func<A, Const<object, A>>, Func<S, Const<object, S>>> GetFunc { get; }

        /// <inheritdoc />
        public Func<Func<A, Identity<A>>, Func<S, Identity<S>>> SetFunc { get; }

        /// <inheritdoc />
        private Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>> Func { get; }

        /// <summary>
        /// Creates a new lens.
        /// </summary>
        /// <param name="f">The lens-function. This uses the functor returned by its first input-function, though the compiler can't check this.</param>
        public MonoLens(Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>> f)
        {
            Func = f;
            GetFunc = LensFunc<Const<object, A>, Const<object, S>>();
            SetFunc = LensFunc<Identity<A>, Identity<S>>();
        }

        /// <inheritdoc />
        public Func<Func<A, FA>, Func<S, FS>> LensFunc<FA, FS>()
            where FA : IFunctor<A>
            where FS : IFunctor<S>
        {
            return g => s => (FS)Func(x => g(x))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>> LensFunc()
        {
            return g => s => Func(x => g(x))(s);
        }
    }
}
