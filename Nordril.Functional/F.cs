using System;
using System.Linq.Expressions;

namespace Nordril.Functional
{
    /// <summary>
    /// Functional extension methods for functions.
    /// </summary>
    public static class F
    {
        /// <summary>
        /// Returns the identity function for a given type.
        /// </summary>
        /// <typeparam name="T">The type of the identity function.</typeparam>
        public static Func<T,T> Id<T>() => x => x;

        /// <summary>
        /// Composes two functions. The second function is run with the result of the first.
        /// </summary>
        /// <typeparam name="TA">The input of the first function.</typeparam>
        /// <typeparam name="TB">The output of the first function and input of the second function.</typeparam>
        /// <typeparam name="TC">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TA, TC> Then<TA, TB, TC>(this Func<TA, TB> f, Func<TB, TC> g)
            => x => g(f(x));

        /// <summary>
        /// Composes two functions. The first function is run with the result of the second.
        /// This is "traditional" function chaining.
        /// </summary>
        /// <typeparam name="TA">The input of the second function.</typeparam>
        /// <typeparam name="TB">The output of the first function and input of the second function.</typeparam>
        /// <typeparam name="TC">The output of the first function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TA, TC> After<TA, TB, TC>(this Func<TB, TC> f, Func<TA, TB> g)
            => x => f(g(x));

        /// <summary>
        /// Curries a binary function, allowing it to take its arguments one by one.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The output of the function.</typeparam>
        /// <param name="f">The function to curry.</param>
        public static Func<TA, Func<TB, TC>> Curry<TA, TB, TC>(this Func<TA, TB, TC> f)
            => x => y => f(x, y);

        /// <summary>
        /// Curries a ternary function, allowing it to take its arguments one by one.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The third argument of the function.</typeparam>
        /// <typeparam name="TD">The output of the function.</typeparam>
        /// <param name="f">The function to curry.</param>
        public static Func<TA, Func<TB, Func<TC, TD>>> Curry<TA, TB, TC, TD>(this Func<TA, TB, TC, TD> f)
            => x => y => z => f(x, y, z);

        /// <summary>
        /// Curries a quaternary function, allowing it to take its arguments one by one.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The third argument of the function.</typeparam>
        /// <typeparam name="TD">The fourth argument of the function.</typeparam>
        /// <typeparam name="TE">The output of the function.</typeparam>
        /// <param name="f">The function to curry.</param>
        public static Func<TA, Func<TB, Func<TC, Func<TD, TE>>>> Curry<TA, TB, TC, TD, TE>(this Func<TA, TB, TC, TD, TE> f)
            => x => y => z => u => f(x, y, z, u);

        /// <summary>
        /// Uncurries a binary function, requiring it to take its arguments all at once.
        /// The inverse of <see cref="Curry{TIn1, TIn2, TOut}(Func{TIn1, TIn2, TOut})"/>.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The output of the function.</typeparam>
        /// <param name="f">The curried function.</param>
        public static Func<TA, TB, TC> Uncurry<TA, TB, TC>(this Func<TA, Func<TB, TC>> f)
            => (x, y) => f(x)(y);

        /// <summary>
        /// Uncurries a ternary function, requiring it to take its arguments all at once.
        /// The inverse of <see cref="Curry{TIn1, TIn2, TIn3, TOut}(Func{TIn1, TIn2, TIn3, TOut})"/>.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The third argument of the function.</typeparam>
        /// <typeparam name="TD">The output of the function.</typeparam>
        /// <param name="f">The curried function.</param>
        public static Func<TA, TB, TC, TD> Uncurry<TA, TB, TC, TD>(this Func<TA, Func<TB, Func<TC, TD>>> f)
            => (x, y, z) => f(x)(y)(z);

        /// <summary>
        /// Uncurries a quaternary function, requiring it to take its arguments all at once.
        /// The inverse of <see cref="Curry{TIn1, TIn2, TIn3, TIn4, TOut}(Func{TIn1, TIn2, TIn3, TIn4, TOut})"/>.
        /// </summary>
        /// <typeparam name="TA">The first argument of the function.</typeparam>
        /// <typeparam name="TB">The second argument of the function.</typeparam>
        /// <typeparam name="TC">The third argument of the function.</typeparam>
        /// <typeparam name="TD">The fourth argument of the function.</typeparam>
        /// <typeparam name="TE">The output of the function.</typeparam>
        /// <param name="f">The curried function.</param>
        public static Func<TA, TB, TC, TD, TE> Uncurry<TA, TB, TC, TD, TE>(this Func<TA, Func<TB, Func<TC, Func<TD, TE>>>> f)
            => (x, y, z, u) => f(x)(y)(z)(u);

        /// <summary>
        /// Applies an action to an object and returns the object.
        /// Mostly a convenience method for executing setters.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to modify.</param>
        /// <param name="a">The action to apply.</param>
        public static T Set<T>(this T obj, Action<T> a)
        {
            a(obj);
            return obj;
        }

        /// <summary>
        /// Applies a function to an object postfix.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="f">The function to apply to the object.</param>
        public static TResult Apply<T, TResult>(this T obj, Func<T, TResult> f) => f(obj);
    }
}
