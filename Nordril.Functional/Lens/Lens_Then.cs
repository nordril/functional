using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        /// <summary>
        /// Composes two getters. The second getter drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="A">The type of the outer result/inner container.</typeparam>
        /// <typeparam name="AInner">The type of the inner result.</typeparam>
        /// <param name="lens">The outer getter.</param>
        /// <param name="then">The inner getter.</param>
        public static IGetter<S, AInner> Then<S, A, AInner>(this IGetter<S, A> lens, IGetter<A, AInner> then)
        {
            return new Getter<S, AInner>(lens.GetFunc.After(then.GetFunc));
        }

        /// <summary>
        /// Composes two setters. The second setter drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="T">The type of the resulting outer container.</typeparam>
        /// <typeparam name="A">The type of the outer value.</typeparam>
        /// <typeparam name="AInner">The type of the inner outer value/inner container.</typeparam>
        /// <typeparam name="B">The type outer result value.</typeparam>
        /// <typeparam name="BInner">The type of the inner result value.</typeparam>
        /// <param name="lens">The outer setter.</param>
        /// <param name="then">The inner setter.</param>
        public static ISetter<S, T, AInner, BInner> Then<S, T, A, B, AInner, BInner>(this ISetter<S, T, A, B> lens, ISetter<A, B, AInner, BInner> then)
        {
            return new Setter<S, T, AInner, BInner>(lens.SetFunc.After(then.SetFunc));
        }

        /// <summary>
        /// Composes two lenses. The second lens drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="A1">The type of the outer result/inner container.</typeparam>
        /// <typeparam name="A2">The type of the inner result.</typeparam>
        /// <param name="lens">The outer getter.</param>
        /// <param name="then">The inner getter.</param>
        public static IMonoLens<S, A2> Then<S, A1, A2>(this IMonoLens<S, A1> lens, IMonoLens<A1, A2> then)
        {
            return new MonoLens<S, A2>(lens.LensFunc().After(then.LensFunc()));
        }

        /// <summary>
        /// Composes two prisms. The second prism drills further into the result of the first, if it exists.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="T">The type of the resulting outer container.</typeparam>
        /// <typeparam name="A">The type of the outer value.</typeparam>
        /// <typeparam name="AInner">The type of the inner outer value/inner container.</typeparam>
        /// <typeparam name="B">The type outer result value.</typeparam>
        /// <typeparam name="BInner">The type of the inner result value.</typeparam>
        /// <param name="lens">The outer setter.</param>
        /// <param name="then">The inner setter.</param>
        public static IPrism<S, T, AInner, BInner> Then<S, T, A, B, AInner, BInner>(this IPrism<S, T, A, B> lens, IPrism<A, B, AInner, BInner> then)
        {
            return new Prism<S, T, AInner, BInner>(t => lens.PrismFunc(t).After(then.PrismFunc(t)));
        }
    }
}
