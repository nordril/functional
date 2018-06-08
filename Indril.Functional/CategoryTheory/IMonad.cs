using Indril.Functional.Function;
using System;

namespace Indril.Functional.CategoryTheory
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
    public interface IMonad<TSource> : IApplicative<TSource>
    {
        /// <summary>
        /// Unpacks the value(s) contained in this monad and applies a function to them.
        /// Bind corresponds to chaining functions, with the addition of the monadic context.
        /// Implementors must fulfill the following laws:
        /// <code>
        ///     Pure(a).Bind(f) == f(a) (left identity of pure)
        ///     f.Bind(x => g(x).Bind(h)) == f.Bind(g).Bind(h) (associativity)
        /// </code>
        /// These laws are identical to the laws of <see cref="Algebra.IMonoid{TSource}"/>, except for the
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
        /// <see cref="Functions.Then{TA, TB, TC}(Func{TA, TB}, Func{TB, TC})"/>.
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
        /// <see cref="Functions.After{TA, TB, TC}(Func{TB, TC}, Func{TA, TB})"/>.
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
        public static IMonad<T> Join<T>(IMonad<IMonad<T>> a) => a.Bind(x => x);
    }
}
