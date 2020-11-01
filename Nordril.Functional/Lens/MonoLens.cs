using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Algebra;
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
        public Func<Func<A, Identity<A>>, Func<S, Identity<S>>> SetFunc { get; }

        /// <inheritdoc />
        private Func<Type, Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>>> Func { get; }

        /// <summary>
        /// Creates a new lens.
        /// </summary>
        /// <param name="f">The lens-function. This uses the functor returned by its first input-function, though the compiler can't check this.</param>
        public MonoLens(Func<Type, Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>>> f)
        {
            Func = f;
            //GetFunc = LensFunc<Const<object, A>, Const<object, S>>();
            SetFunc = LensFunc<Identity<A>, Identity<S>>();
        }

        /// <inheritdoc />
        public Func<Func<A, FA>, Func<S, FS>> LensFunc<FA, FS>()
            where FA : IFunctor<A>
            where FS : IFunctor<S>
        {
            return g => s => (FS)Func(typeof(FA))(x => g(x))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IFunctor<A>>, Func<S, IFunctor<S>>> LensFunc(Type t)
        {
            return g => s => Func(t)(x => g(x))(s);
        }

        public Func<Func<A, IPhantomFunctor<A>>, Func<S, IPhantomFunctor<S>>> GetFunc(Type t)
        {
            return g => s => (IPhantomFunctor<S>)Func(t)(x => g(x))(s);
        }

        public Func<Func<A, FA>, Func<S, FS>> GetFunc<FA, FS>()
            where FA : IPhantomFunctor<A>
            where FS : IPhantomFunctor<S>
            => LensFunc<FA, FS>();

        public Func<Func<A, FA>, Func<S, FS>> FoldFunc<FA, FS>()
            where FA : IContravariantApplicative<A>
            where FS : IContravariantApplicative<S>
            => LensFunc<FA, FS>();

        public Func<Func<A, Const<TMonoid, R, A>>, Func<S, Const<TMonoid, R, S>>> FoldFuncConst<TMonoid, R>() where TMonoid : IMonoid<R>
            => LensFunc<Const<TMonoid, R, A>, Const<TMonoid, R, S>>();

        public Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>> FoldFunc(Type t)
        {
            var f = Func;
            return g => s => (IContravariantApplicative<S>)f(t)(g.Then(a => a))(s);
        }
    }
}
