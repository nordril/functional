﻿using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Nordril.Functional.Category.Linq;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A monad. Monads, in addition to wrapping values (via <see cref="IApplicative{TSource}"/>
    /// and mapping over their contained values (via <see cref="IFunctor{TSource}"/> support temporarily
    /// unpacking their contained values and re-packing them via the <see cref="Bind{TResult}(Func{TSource, IMonad{TResult}})"/>
    /// operator. Unlike <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>,
    /// this allows dynamically controlling the computational path at runtime, instead of just applying a fixed
    /// function over one of more wrapped arguments.
    /// </summary>
    /// <typeparam name="TSource">The type of the values contained in the monad.</typeparam>
    public interface IMonad<out TSource> : IApplicative<TSource>
    {
        /// <summary>
        /// Unpacks the value(s) contained in this monad and applies a function to them.
        /// Bind corresponds to chaining functions, with the addition of the monadic context.
        /// Implementors must fulfill the following laws:
        /// <code>
        ///     Pure(a).Bind(f) == f(a) (left identity of pure)<br />
        ///     X.Bind(a =&gt; Pure(a)) = X (right identity of pure)<br />
        ///     f.Bind(x => g(x).Bind(h)) == f.Bind(g).Bind(h) (associativity) <br />
        /// </code>
        /// These laws are identical to the laws of <see cref="Algebra.IHasMonoid{TSource}"/>, except for the
        /// type variable in <see cref="IMonad{TSource}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply.</param>
        IMonad<TResult> Bind<TResult>(Func<TSource, IMonad<TResult>> f);
    }

    /// <summary>
    /// Extension methods for <see cref="IMonad{TSource}"/>.
    /// </summary>
    public static class MonadExtensions
    {
        /// <summary>
        /// A more readable alias to <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="a">The value containing the source.</param>
        /// <param name="f">The function to apply.</param>
        public static IMonad<TResult> Then<TSource, TResult>(this IMonad<TSource> a, Func<TSource, IMonad<TResult>> f) => a.Bind(f);

        /// <summary>
        /// Chains two monad-producing functions by executing the first, then the second, gluing them together using
        /// <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>. The monadic equivalent of
        /// <see cref="F.Then{TA, TB, TC}(Func{TA, TB}, Func{TB, TC})"/>.
        /// </summary>
        /// <typeparam name="TSource">The input of the first function.</typeparam>
        /// <typeparam name="TResult1">The output of the first function (wrapped in a monad) and the input of the second.</typeparam>
        /// <typeparam name="TResult2">The output of the second function (wrapped in a monad).</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TSource, IMonad<TResult2>> ThenM<TSource, TResult1, TResult2>(this Func<TSource, IMonad<TResult1>> f, Func<TResult1, IMonad<TResult2>> g)
            => x => f(x).Bind(g);

        /// <summary>
        /// Chains two monad-producing functions by executing the second, then the first, gluing them together using
        /// <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>. The monadic equivalent of
        /// <see cref="F.After{TA, TB, TC}(Func{TB, TC}, Func{TA, TB})"/>.
        /// </summary>
        /// <typeparam name="TSource">The input of the second function.</typeparam>
        /// <typeparam name="TResult1">The output of the first function (wrapped in a monad) and the input of the second.</typeparam>
        /// <typeparam name="TResult2">The output of the second function (wrapped in a monad).</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TSource, IMonad<TResult2>> AfterM<TSource, TResult1, TResult2>(this Func<TResult1, IMonad<TResult2>> f, Func<TSource, IMonad<TResult1>> g)
            => x => g(x).Bind(f);

        /// <summary>
        /// Extracts the value(s) from the source and replaces it with another one, ignoring the values of the source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="a">The value containing the source.</param>
        /// <param name="b">The value containing the result.</param>
        public static IMonad<TResult> Then<TSource, TResult>(this IMonad<TSource> a, IMonad<TResult> b) => a.Bind(x => b);

        /// <summary>
        /// Concatenates two monad-functions.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TResult1">The intermediate result type.</typeparam>
        /// <typeparam name="TResult2">The funal result type.</typeparam>
        /// <param name="f">The first monad function.</param>
        /// <param name="g">The second monad function.</param>
        public static Func<TSource, IMonad<TResult2>> Comp<TSource, TResult1, TResult2>(this Func<TSource, IMonad<TResult1>> f, Func<TResult1, IMonad<TResult2>> g) => x => f(x).Bind(g);

        /// <summary>
        /// Flattens the structure of a monad. The following holds for all X and f, given a correctly implemented
        /// <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/> and
        /// <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>:
        /// <code>
        ///     X.Map(x => x.Join()).Join() = X.Join().Join()
        ///     X.Map(x => x.Pure()).Join() = X.Pure().Join() = id
        ///     X.Map(x => x.Map(f)).Join() = X.Join().Map(f)
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type in the monad.</typeparam>
        /// <param name="a">The monad to flatten.</param>
        /// <returns></returns>
        public static IMonad<T> Join<T>(this IMonad<IMonad<T>> a) => a.Bind(x => x);

        /// <summary>
        /// A monadic version of <see cref="Enumerable.Aggregate{TSource, TAccumulate, TResult}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/> which aggregates a sequence <paramref name="xs"/> with a monadic function <paramref name="f"/> and an accumulator <paramref name="acc"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TAcc">The accumulator type.</typeparam>
        /// <typeparam name="TMonadAcc"><typeparamref name="TAcc"/>, wrapped in an <see cref="IMonad{TSource}"/>.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="acc">The initial accumulator.</param>
        /// <param name="f">The folding function taking the aggregator and a sequence-element, and returning a monadic accumulator-value.</param>
        public static TMonadAcc AggregateM<TSource, TAcc, TMonadAcc>(this IEnumerable<TSource> xs, TAcc acc, Func<TSource, TAcc, TMonadAcc> f)
            where TMonadAcc : IMonad<TAcc>
        {
            var pureAcc = acc.PureUnsafe<TAcc, TMonadAcc>();
            return xs.Aggregate(pureAcc, (acc, x) => (TMonadAcc)acc.Bind(acc2 => f(x, acc2)));
        }

        /// <summary>
        /// A monadic <see cref="CollectionExtensions.Unfold{TSeed, TResult}(TSeed, Func{TSeed, Maybe{ValueTuple{TSeed, TResult}}})"/> which generates a sequence each time the <paramref name="seed"/>-value evaluates to <see cref="Maybe.Just{T}(T)"/>.<br />
        /// Equivalent to:
        /// <code>
        /// while (seed().TryGetValue(_, y)) { yield return y; }
        /// </code>
        /// </summary>
        /// <typeparam name="TSource">The type of the seed/generated elements.</typeparam>
        /// <param name="seed">The monadic seed-value.</param>
        public static IMonad<IEnumerable<TSource>> UnfoldM<TSource>(this IMonad<Maybe<TSource>> seed)
        {
            IMonad<IEnumerable<TSource>> go()
            {
                var q =
                    from x in seed
                    from result in x.TryGetValue(default, out var y)
                        ? (IMonad<IEnumerable<TSource>>)go().Map(zs => zs.Prepend(y))
                        : (IMonad<IEnumerable<TSource>>)
                            Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), seed.GetType())
                    select result;

                return q;
            };

            return go();
        }

        /// <summary>
        /// Runs <paramref name="condition"/> repeatedly as long as it returns <c>true</c>, and runs <paramref name="body"/> each time, collecting the results.<br />
        /// Equivalent to:
        /// <code>
        /// while (condition()) {
        ///     yield return body();
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TSource">The type of the seed/generated elements.</typeparam>
        /// <param name="condition">The monadic condition.</param>
        /// <param name="body">The monadic body.</param>
        public static IMonad<IEnumerable<TSource>> WhileM<TSource>(this IMonad<bool> condition, IMonad<TSource> body)
        {
            IMonad<IEnumerable<TSource>> go()
            {
                var q =
                    from conditionResult in condition
                    from results in conditionResult
                        ? from bodyResult in body
                          from rest in go()
                          select rest.Prepend(bodyResult)
                        : (IMonad<IEnumerable<TSource>>)
                            Applicative.PureUnsafe((IEnumerable<TSource>)Array.Empty<TSource>(), condition.GetType())
                    select results;

                return q;
            }

            return go();
        }

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="IMonad{TSource}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to an <see cref="IMonad{TSource}"/>.</param>
        public static IMonad<T> ToMonad<T>(this IFunctor<T> f) => (IMonad<T>)f;
    }
}
