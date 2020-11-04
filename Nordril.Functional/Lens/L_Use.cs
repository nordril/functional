using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            => (A)lens.GetFunc<Const<object, A>, Const<object, S>>()(a => new Const<object, A>(a))(input).RealValue;

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
                    Const<Monoid.FirstMonoid<object>, Maybe<object>, A>,
                    Const<Monoid.FirstMonoid<object>, Maybe<object>, S>,
                    Fun<A, Const<Monoid.FirstMonoid<object>, Maybe<object>, A>>,
                    Fun<S, Const<Monoid.FirstMonoid<object>, Maybe<object>, S>>>();

            Func<A, Const<Monoid.FirstMonoid<object>, Maybe<object>, A>> innerF = a => new Const<Monoid.FirstMonoid<object>, Maybe<object>, A>(Maybe.Just<object>(a));

            var st = f(innerF.MakeFun()).ToFunc();

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

        /// <summary>
        /// Runs an <see cref="ITraversal{S, T, A, B}"/> on <paramref name="input"/>.
        /// </summary>
        /// <typeparam name="TApplicative">The type of the applicative result of the inner function.</typeparam>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="T">The type of the resultant structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <typeparam name="B">The type of the result of the inner function.</typeparam>
        /// <param name="lens">The traversal to run.</param>
        /// <param name="f">The function to apply to each element.</param>
        /// <param name="input">The input.</param>
        public static IApplicative<T> Traverse<TApplicative, S, T, A, B>(this ITraversal<S, T, A, B> lens, Func<A, TApplicative> f,
            S input)
            where TApplicative : IApplicative<B>
            => lens.TraversalFunc(typeof(TApplicative))(x => f(x))(input);

        /// <summary>
        /// Runs an <see cref="ITraversal{S, T, A, B}"/> on <paramref name="input"/>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="T">The type of the resultant structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <typeparam name="B">The type of the result of the inner function.</typeparam>
        /// <param name="applicative">The type of the applicative result of the inner function.</param>
        /// <param name="lens">The traversal to run.</param>
        /// <param name="f">The function to apply to each element.</param>
        /// <param name="input">The input.</param>
        public static IApplicative<T> Traverse<S, T, A, B>(
            this ITraversal<S, T, A, B> lens,
            Type applicative,
            Func<A, IApplicative<B>> f,
            S input)
            => lens.TraversalFunc(applicative)(f)(input);

        /// <summary>
        /// Runs an <see cref="IWithering{S, T, A, B}"/> on <paramref name="input"/>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="T">The type of the resultant structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <typeparam name="B">The type of the result of the inner function.</typeparam>
        /// <typeparam name="TAlternative">The type of the alternative result of the inner function.</typeparam>
        /// <param name="lens">The traversal to run.</param>
        /// <param name="f">The function to apply to each element.</param>
        /// <param name="input">The input.</param>
        public static IApplicative<T> TraverseMaybe<TAlternative, S, T, A, B>(
            this IWithering<S, T, A, B> lens,
            Func<A, IAlternative<B>> f,
            S input)
            => lens.WitherFunc(typeof(TAlternative))(x => f(x))(input);

        /// <summary>
        /// Runs an <see cref="IWithering{S, T, A, B}"/> on <paramref name="input"/>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="T">The type of the resultant structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <typeparam name="B">The type of the result of the inner function.</typeparam>
        /// <param name="alternative">The type of the alternative result of the inner function.</param>
        /// <param name="lens">The traversal to run.</param>
        /// <param name="f">The function to apply to each element.</param>
        /// <param name="input">The input.</param>
        public static IApplicative<T> TraverseMaybe<S, T, A, B>(
            this IWithering<S, T, A, B> lens,
            Type alternative,
            Func<A, IAlternative<B>> f,
            S input)
            => lens.WitherFunc(alternative)(x => f(x))(input);

        /// <summary>
        /// Turns the element(s) retrieved by an <see cref="IGetter{S, A}"/> into a list.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="A">The type of the part to get.</typeparam>
        /// <param name="lens">The getter.</param>
        /// <param name="input">The input structure.</param>
        public static IFuncList<A> ToList<S, A>(this IFold<S, A> lens, S input)
            => Fold(lens, (x, xs) => { xs.Push(x); return xs; }, new Stack<A>(), input).MakeFuncList();

        /// <summary>
        /// Folds the elements of a container using <paramref name="f"/> as the combining-function and <paramref name="acc"/> as the initial accumulator.
        /// Also known as <em>foldrOf</em>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="A">The type of the elements.</typeparam>
        /// <typeparam name="TResult">The type of the monoidal result.</typeparam>
        /// <param name="lens">The fold to run.</param>
        /// <param name="f">The function to combine two elements.</param>
        /// <param name="acc">The initial accumulator value.</param>
        /// <param name="input">The input structure.</param>
        public static TResult Fold<S, A, TResult>(IFold<S, A> lens, Func<A, TResult, TResult> f, TResult acc, S input)
            => lens.FoldFunc<Const<Monoid.EndoMonoid<TResult>, Func<TResult, TResult>, A>, Const<Monoid.EndoMonoid<TResult>, Func<TResult, TResult>, S>>()(a => new Const<Monoid.EndoMonoid<TResult>, Func<TResult, TResult>, A>(x => f(a, x)))(input).RealValue(acc);

        /// <summary>
        /// Maps the elements in a container <typeparamref name="S"/> to an <see cref="IHasMonoid{T}"/> and uses the monoid-logic to combine the results.
        /// Also known as <em>foldMapOf</em>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="A">The type of the elements.</typeparam>
        /// <typeparam name="TResult">The type of the monoidal result.</typeparam>
        /// <param name="lens">The fold to run.</param>
        /// <param name="f">The function to map each element to a monoid.</param>
        /// <param name="input">The input structure.</param>
        public static TResult Fold<S, A, TResult>(IFold<S, A> lens, Func<A, TResult> f, S input)
            where TResult : IHasMonoid<TResult>
            => lens.FoldFunc<Const<TResult, TResult, A>, Const<TResult, TResult, S>>()(f.Then(x => new Const<TResult, TResult, A>(x)))(input).RealValue;

        /// <summary>
        /// Maps the elements in a container <typeparamref name="S"/> to an value which has an associated monoid <typeparamref name="TMonoid"/> and uses the monoid-logic to combine the results.
        /// Also known as <em>foldMapOf</em>.
        /// </summary>
        /// <typeparam name="S">The type of the input structure.</typeparam>
        /// <typeparam name="A">The type of the elements.</typeparam>
        /// <typeparam name="TResult">The type of the monoidal result.</typeparam>
        /// <typeparam name="TMonoid">The type of the monoid.</typeparam>
        /// <param name="lens">The fold to run.</param>
        /// <param name="f">The function to map each element to a monoid.</param>
        /// <param name="input">The input structure.</param>
        public static TResult Fold<S, A, TResult, TMonoid>(IFold<S, A> lens, Func<A, TResult> f, S input)
            where TMonoid : IMonoid<TResult>
            => lens.FoldFunc<Const<TMonoid, TResult, A>, Const<TMonoid, TResult, S>>()(f.Then(x => new Const<TMonoid, TResult, A>(x)))(input).RealValue;
    }
}
