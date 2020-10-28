using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        /// <summary>
        /// Runs a setter on an input and returns the modified input.
        /// Also known as <em>set</em>.
        /// </summary>
        public static T Set<S, T, A, B>(this ISetter<S, T, A, B> lens, S input, B value)
            => lens.SetFunc(_ => new Identity<B>(value))(input).Value;

        /// <summary>
        /// Runs a setter with an update-function on an input, returns the modified input.
        /// Also known as <em>over</em>.
        /// </summary>
        public static T Update<S, T, A, B>(this ISetter<S, T, A, B> lens, S input, Func<A, B> f)
            => lens.SetFunc(a => new Identity<B>(f(a)))(input).Value;

        /// <summary>
        /// Runs a getter on an input and returns the result.
        /// Also known as <em>view</em>.
        /// </summary>
        public static A Get<S, A>(this IGetter<S, A> lens, S input)
            => (A)lens.GetFunc(a => new Const<object, A>(a))(input).RealValue;

        /// <summary>
        /// Tries to get the result of a prism from an input.
        /// Also known an <em>preview</em>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <param name="lens">The lens to run.</param>
        /// <param name="input">The input.</param>
        public static Maybe<A> TryGet<S, A>(this IPrism<S, S, A, A> lens, S input)
        {
            var f = lens.PrismFunc<
                    ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, A>,
                    ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, S>,
                    ProfunctorFunc<A, ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, A>>,
                    ProfunctorFunc<S, ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, S>>>();

            Func<A, ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, A>> innerF = a => new ConstMonoid<Monoid.FirstMonoid<object>, Maybe<object>, A>(Maybe.Just<object>(a));

            var st = f(innerF.FuncToProfunctor()).ToFunc();

            return st(input).RealValue.Map(o => (A)o).ToMaybe();
        }

        /// <summary>
        /// The opposite of <see cref="TryGet{S, A}(IPrism{S, S, A, A}, S)"/> (if it succeeds); this function composes a part <typeparamref name="B"/> into a whole <typeparamref name="T"/>. With this function, one can use a <see cref="IPrism{S, T, A, B}"/> as a constructor for, say, <see cref="Either{TLeft, TRight}"/>. In fact:
        /// <code>
        /// if (Lens.TryGet(L, T).HasValue) then Lens.Review(L, Lens.TryGet(L, T).Value()) == T
        /// </code>
        /// </summary>
        /// <typeparam name="B">The type of the value to pipe backwards.</typeparam>
        /// <typeparam name="T">The type of the resulting object, from which the prism normally tries to extract <typeparamref name="B"/> via <see cref="TryGet{S, A}(IPrism{S, S, A, A}, S)"/>.</typeparam>
        /// <param name="lens">The prism.</param>
        /// <param name="input">The object which should be assembled into <typeparamref name="T"/>.</param>
        public static T Review<B, T>(this IPrism<T, T, B, B> lens, B input)
        {
            return lens.PrismFunc<Identity<B>, Identity<T>, Tagged<B, Identity<B>>, Tagged<T, Identity<T>>>()
                (new Tagged<B, Identity<B>>(new Identity<B>(input))).RealValue.Value;
        }
    }
}
