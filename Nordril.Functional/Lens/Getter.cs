using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A concrete getter.
    /// </summary>
    /// <typeparam name="S">The type of the input data structure.</typeparam>
    /// <typeparam name="A">The type of the object to get in <typeparamref name="S"/>.</typeparam>
    internal struct Getter<S, A> : IGetter<S, A>
    {
        private Func<Type, Func<Func<A, IPhantomFunctor<A>>, Func<S, IPhantomFunctor<S>>>> Func { get; }

        public Getter(Func<Type, Func<Func<A, IPhantomFunctor<A>>, Func<S, IPhantomFunctor<S>>>> f)
        {
            Func = f;
        }

        /// <inheritdoc />
        public Func<Func<A, Const<TMonoid, R, A>>, Func<S, Const<TMonoid, R, S>>> FoldFuncConst<TMonoid, R>() where TMonoid : IMonoid<R>
        {
            var f = Func;
            return g => s => (Const<TMonoid, R, S>)f(typeof(Const<TMonoid, R, A>))(a => g(a))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, FA>, Func<S, FS>> FoldFunc<FA, FS>()
            where FA : IContravariantApplicative<A>
            where FS : IContravariantApplicative<S>
        {
            var f = Func;
            return g => s => (FS)f(typeof(FA))(a => g(a))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, FA>, Func<S, FS>> GetFunc<FA, FS>()
            where FA : IPhantomFunctor<A>
            where FS : IPhantomFunctor<S>
        {
            var f = Func;
            return g => s => (FS)f(typeof(FA))(a => g(a))(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IPhantomFunctor<A>>, Func<S, IPhantomFunctor<S>>> GetFunc(Type t)
            => Func(t);

        /// <inheritdoc />
        public Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>> FoldFunc(Type t)
        {
            var f = Func;
            return g => s => (IContravariantApplicative<S>)f(t)(g.Then(a => a))(s);
        }
    }
}
