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
            return new Getter<S, AInner>(t => lens.GetFunc(t).After(then.GetFunc(t)));
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
        /// <typeparam name="AOuter">The type of the outer result/inner container.</typeparam>
        /// <typeparam name="AInner">The type of the inner result.</typeparam>
        /// <param name="lens">The outer getter.</param>
        /// <param name="then">The inner getter.</param>
        public static IMonoLens<S, AInner> Then<S, AOuter, AInner>(this IMonoLens<S, AOuter> lens, IMonoLens<AOuter, AInner> then)
        {
            return new MonoLens<S, AInner>(t => lens.LensFunc(t).After(then.LensFunc(t)));
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

        /// <summary>
        /// Composes two folds. The second folds drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="AOuter">The type of the outer value.</typeparam>
        /// <typeparam name="AInner">The type of the inner outer value/inner container.</typeparam>
        /// <param name="lens">The outer setter.</param>
        /// <param name="then">The inner setter.</param>
        public static IFold<S, AInner> Then<S, AOuter, AInner>(this IFold<S, AOuter> lens, IFold<AOuter, AInner> then)
        {
            return new Fold<S, AInner>(t => lens.FoldFunc(t).After(then.FoldFunc(t)));
        }

        /// <summary>
        /// Composes two traversals. The second traversal drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="T">The type of the resulting outer container.</typeparam>
        /// <typeparam name="A">The type of the outer value.</typeparam>
        /// <typeparam name="AInner">The type of the inner outer value/inner container.</typeparam>
        /// <typeparam name="B">The type outer result value.</typeparam>
        /// <typeparam name="BInner">The type of the inner result value.</typeparam>
        /// <param name="lens">The outer setter.</param>
        /// <param name="then">The inner setter.</param>
        public static ITraversal<S, T, AInner, BInner> Then<S, T, A, B, AInner, BInner>(this ITraversal<S, T, A, B> lens, ITraversal<A, B, AInner, BInner> then)
        {
            return new Traversal<S, T, AInner, BInner>(t => lens.TraversalFunc(t).After(then.TraversalFunc(t)));
        }

        /// <summary>
        /// Composes two witherings. The second withering drills further into the result of the first.
        /// </summary>
        /// <typeparam name="S">The type of the outer container.</typeparam>
        /// <typeparam name="T">The type of the resulting outer container.</typeparam>
        /// <typeparam name="A">The type of the outer value.</typeparam>
        /// <typeparam name="AInner">The type of the inner outer value/inner container.</typeparam>
        /// <typeparam name="B">The type outer result value.</typeparam>
        /// <typeparam name="BInner">The type of the inner result value.</typeparam>
        /// <param name="lens">The outer setter.</param>
        /// <param name="then">The inner setter.</param>
        public static IWithering<S, T, AInner, BInner> Then<S, T, A, B, AInner, BInner>(this IWithering<S, T, A, B> lens, IWithering<A, B, AInner, BInner> then)
        {
            return new Withering<S, T, AInner, BInner>(t => lens.WitherFunc(t).After(then.WitherFunc(t)));
        }
    }
}
