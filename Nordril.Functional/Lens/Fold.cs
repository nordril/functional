using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A fold.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    internal class Fold<S, A> : IFold<S, A>
    {
        protected Func<Type, Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>>> Func { get; }

        /// <summary>
        /// Creates a new fold.
        /// </summary>
        /// <param name="f">The fold-function.</param>
        public Fold(Func<Type, Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>>> f)
        {
            Func = f;
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
        public Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>> FoldFunc(Type t)
            => Func(t);

        /// <inheritdoc />
        public Func<Func<A, Const<TMonoid, R, A>>, Func<S, Const<TMonoid, R, S>>> FoldFuncConst<TMonoid, R>() where TMonoid : Algebra.IMonoid<R>
        {
            var f = Func;
            return g => s => (Const<TMonoid, R, S>)f(typeof(Const<TMonoid, R, A>))(a => g(a))(s);
        }
    }
}
