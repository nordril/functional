using System;

namespace Indril.Functional.Category
{
    /// <summary>
    /// A bifunctor that contains both <typeparamref name="TLeft"/> values and <typeparamref name="TRight"/> values.
    /// An example is a tuple.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left values.</typeparam>
    /// <typeparam name="TRight">The type of the right values.</typeparam>
    public interface IBifunctor<TLeft, TRight>
    {
        /// <summary>
        /// Applies a function to the functor and returns a new functor without changing the original functor.
        /// Implementors must fulfill the following for all X and functions f and g:
        /// <code>
        ///     X.BiMap(a => a, b => b) == X (identity)
        ///     X.BiMap(x => f(g(x)), x => h(i(x))) == X.BiMap(g, i).BiMap(f, h) (homomorphism)
        /// </code>
        /// </summary>
        /// <typeparam name="TLeftResult">The type of the left result.</typeparam>
        /// <typeparam name="TRightResult">The type of the right result.</typeparam>
        /// <param name="f">The left function to apply to the functor.</param>
        /// <param name="g">The right function to apply to the functor.</param>
        IBifunctor<TLeftResult, TRightResult> BiMap<TLeftResult, TRightResult>(Func<TLeft, TLeftResult> f, Func<TRight, TRightResult> g);
    }

    /// <summary>
    /// Extensions for <see cref="IBifunctor{TLeft, TRight}"/>.
    /// </summary>
    public static class BifunctorExtensions
    {
        /// <summary>
        /// Maps over just the left part of a <see cref="IBifunctor{TLeft, TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left values.</typeparam>
        /// <typeparam name="TRight">The type of the right values.</typeparam>
        /// <typeparam name="TLeftResult">The type of the left result.</typeparam>
        /// <param name="this">The bifunctor to apply over.</param>
        /// <param name="f">The function to apply.</param>
        public static IBifunctor<TLeftResult, TRight> LeftMap<TLeft, TRight, TLeftResult>(this IBifunctor<TLeft, TRight> @this, Func<TLeft, TLeftResult> f)
            => @this.BiMap(f, x => x);

        /// <summary>
        /// Applies an action to the left part of a <see cref="IBifunctor{TLeft, TRight}"/> and returns it.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left values.</typeparam>
        /// <typeparam name="TRight">The type of the right values.</typeparam>
        /// <param name="this">The bifunctor to apply over.</param>
        /// <param name="f">The function to apply.</param>
        public static IBifunctor<TLeft, TRight> LeftMap<TLeft, TRight>(this IBifunctor<TLeft, TRight> @this, Action<TLeft> f)
            => @this.BiMap(y => { f(y); return y; }, x => x);

        /// <summary>
        /// Maps over just the right part of a <see cref="IBifunctor{TLeft, TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left values.</typeparam>
        /// <typeparam name="TRight">The type of the right values.</typeparam>
        /// <typeparam name="TRightResult">The type of the right result.</typeparam>
        /// <param name="this">The bifunctor to apply over.</param>
        /// <param name="f">The function to apply.</param>
        public static IBifunctor<TLeft, TRightResult> RightMap<TLeft, TRight, TRightResult>(this IBifunctor<TLeft, TRight> @this, Func<TRight, TRightResult> f)
            => @this.BiMap(x => x, f);

        /// <summary>
        /// Applies an action to the right part of a <see cref="IBifunctor{TLeft, TRight}"/> and returns it.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left values.</typeparam>
        /// <typeparam name="TRight">The type of the right values.</typeparam>
        /// <param name="this">The bifunctor to apply over.</param>
        /// <param name="f">The function to apply.</param>
        public static IBifunctor<TLeft, TRight> RightMap<TLeft, TRight>(this IBifunctor<TLeft, TRight> @this, Action<TRight> f)
            => @this.BiMap(x => x, y => { f(y); return y; });
    }
}
