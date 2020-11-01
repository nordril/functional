using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    internal class MonoTraversal<S, A> : Traversal<S, S, A, A>, IMonoTraversal<S, A>
    {
        public MonoTraversal(Func<Type, Func<Func<A, IApplicative<A>>, Func<S, IApplicative<S>>>> f) : base(f)
        {
        }

        public Func<Func<A, FA>, Func<S, FS>> FoldFunc<FA, FS>()
            where FA : IContravariantApplicative<A>
            where FS : IContravariantApplicative<S>
        {
            var f = Func;
            return g => s => (FS)f(typeof(FA))(a => g(a))(s);
        }

        public Func<Func<A, IContravariantApplicative<A>>, Func<S, IContravariantApplicative<S>>> FoldFunc(Type t)
        {
            var f = Func;
            return g => s => (IContravariantApplicative<S>)f(t)(g.Then(a => a))(s);
        }

        public Func<Func<A, Const<TMonoid, R, A>>, Func<S, Const<TMonoid, R, S>>> FoldFuncConst<TMonoid, R>()
            where TMonoid : IMonoid<R>
        {
            var f = Func;
            return g => s => (Const<TMonoid, R, S>)f(typeof(Const<TMonoid, R, A>))(a => g(a))(s);
        }
    }
}
