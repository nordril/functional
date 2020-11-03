using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Category;
using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A prism, which is a setter that "may work" depending on its input.
    /// </summary>
    internal class MonoPrism<S, A> : IMonoPrism<S, A>
    {
        public Func<Func<A, Identity<A>>, Func<S, Identity<S>>> SetFunc { get; }

        private Func<Type, Func<IChoice<A, IApplicative<A>>, IChoice<S, IApplicative<S>>>> Func { get; }

        /// <summary>
        /// Creates a new prism.
        /// </summary>
        /// <param name="f">The prism-function. This uses the applicative returned by its first input-function, though the compiler can't check this.</param>
        public MonoPrism(Func<Type, Func<IChoice<A, IApplicative<A>>, IChoice<S, IApplicative<S>>>> f)
        {
            Func = f;
            var sf = PrismFunc<Identity<A>, Identity<S>, Fun<A, Identity<A>>, Fun<S, Identity<S>>>();
            SetFunc = g => s => sf(g.MakeFun()).ToFunc()(s);
        }

        /// <inheritdoc />
        public Func<PAFB, PSFT> PrismFunc<FB, FT, PAFB, PSFT>()
            where FB : IApplicative<A>
            where FT : IApplicative<S>
            where PAFB : IChoice<A, FB>
            where PSFT : IChoice<S, FT>
        {
            //Worse than Lens's functor, which isa use-only universally quantified rank-2 type (since you get your functor-instance from g and map over it),
            //this is a can-construct universal type, because you can call Pure from nothing. Therefore, we pass in the type of FT and Make.Prism does its dark magic.
            //It also uses universally quantified profunctors.
            return g => (PSFT)Func(typeof(FT))((IChoice<A, IApplicative<A>>)g.Map(r => (IApplicative<A>)r)).Map(r => (FT)r);

            //g => s => (FT)Func(typeof(FT))(x => g(x))(s);
        }

        /// <inheritdoc />
        public Func<IChoice<A, IApplicative<A>>, IChoice<S, IApplicative<S>>> PrismFunc(Type t)
        {
            return g => Func(t)(g);
            //return g => s => Func(t)(x => g(x))(s);
        }

        public Func<Func<A, FB>, Func<S, FT>> TraversalFunc<FB, FT>()
            where FB : IApplicative<A>
            where FT : IApplicative<S>
        {
            var tf = PrismFunc<FB, FT, Fun<A, FB>, Fun<S, FT>>();
            return g => s => tf(g.MakeFun()).ToFunc()(s);
        }

        public Func<Func<A, IApplicative<A>>, Func<S, IApplicative<S>>> TraversalFunc(Type t)
        {
            var tf = PrismFunc(t);
            return g => s => tf(g.MakeFun()).ToFunc()(s);
        }
    }
}
