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
    internal class Prism<S, T, A, B> : IPrism<S, T, A, B>
    {
        public Func<Func<A, Identity<B>>, Func<S, Identity<T>>> SetFunc { get; }

        private Func<Type, Func<IChoice<A, IApplicative<B>>, IChoice<S, IApplicative<T>>>> Func { get; }

        /// <summary>
        /// Creates a new prism.
        /// </summary>
        /// <param name="f">The prism-function. This uses the applicative returned by its first input-function, though the compiler can't check this.</param>
        public Prism(Func<Type, Func<IChoice<A, IApplicative<B>>, IChoice<S, IApplicative<T>>>> f)
        {
            Func = f;
            var sf = PrismFunc<Identity<B>, Identity<T>, ProfunctorFunc<A, Identity<B>>, ProfunctorFunc<S, Identity<T>>>();
            SetFunc = g => s => sf(g.FuncToProfunctor()).ToFunc()(s);
        }

        /// <inheritdoc />
        public Func<PAFB, PSFT> PrismFunc<FB, FT, PAFB, PSFT>()
            where FB : IApplicative<B>
            where FT : IApplicative<T>
            where PAFB : IChoice<A, FB>
            where PSFT : IChoice<S, FT>
        {
            //Worse than Lens's functor, which isa use-only universally quantified rank-2 type (since you get your functor-instance from g and map over it),
            //this is a can-construct universal type, because you can call Pure from nothing. Therefore, we pass in the type of FT and Make.Prism does its dark magic.
            //It also uses universally quantified profunctors.
            return g => (PSFT)Func(typeof(FT))((IChoice<A, IApplicative<B>>)g.Map(r => (IApplicative<B>)r)).Map(r => (FT)r);
        }

        /// <inheritdoc />
        public Func<IChoice<A, IApplicative<B>>, IChoice<S, IApplicative<T>>> PrismFunc(Type t)
        {
            return g => Func(t)(g);
        }

        /// <inheritdoc />
        public Func<Func<A, FB>, Func<S, FT>> TraversalFunc<FB, FT>()
            where FB : IApplicative<B>
            where FT : IApplicative<T>
        {
            return g => s => PrismFunc<FB, FT, ProfunctorFunc<A, FB>, ProfunctorFunc<S, FT>>()(g.FuncToProfunctor()).ToFunc()(s);
        }

        /// <inheritdoc />
        public Func<Func<A, IApplicative<B>>, Func<S, IApplicative<T>>> TraversalFunc(Type t)
        {
            return g => s => Func(t)(g.FuncToProfunctor()).ToFunc()(s);
        }
    }
}
