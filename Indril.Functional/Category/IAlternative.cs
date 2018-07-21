using Indril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Indril.Functional.Category
{
    /// <summary>
    /// A subset of <see cref="IApplicative{TSource}"/> that has a notion of "combining computations". It's up to the implementor to define how this combining works, but implementors should obey the following laws:
    /// <code>
    ///     x.Empty().Alt(a) == a (left-neutrality of Empty)
    ///     a.Alt(x.Empty()) == a (right-neutrality of Empty)
    ///     a.Alt(b).Alt(c)  == a.Alt(b.Alt(c)) (associativity of Alt)
    /// </code>
    /// <see cref="IAlternative{TSource}"/> is thus a <see cref="IMonoid{TSource}"/> on <see cref="IApplicative{TSource}"/>.
    /// Moreover, the implementor should obey
    /// <code>
    ///     x.Empty().Ap(a) == x.Empty() (guard)
    /// </code>
    /// Additionally, if the implementor implements <see cref="IMonadPlus{T}"/>, it should also obey:
    /// <code>
    ///     x.Empty().Bind(f) == x.Empty()
    /// </code>
    /// </summary>
    /// <remarks>
    /// <see cref="IAlternative{TSource}"/> serves two purposes: it functions as a monoid on instances of <see cref="IApplicative{TSource}"/> and it provides a "guard"-function which can stop a computation if it's not fulfilled. This is done via <see cref="Empty"/>.
    /// </remarks>
    /// <typeparam name="TSource">The data contained in the functor.</typeparam>
    public interface IAlternative<TSource> : IApplicative<TSource>
    {
        /// <summary>
        /// Returns a computation with zero results. The this-value MUST NOT BE USED by implementors.
        /// </summary>
        IAlternative<TSource> Empty();
        /// <summary>
        /// Combines two computations.
        /// </summary>
        /// <param name="x">The other computation.</param>
        IAlternative<TSource> Alt(IAlternative<TSource> x);
    }

    /// <summary>
    /// Extension methods for <see cref="IAlternative{TSource}"/>.
    /// </summary>
    public static class AlternativeExtensions
    {
        /// <summary>
        /// A guard-function with respect to <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/> and <see cref="IMonad{TSource}.Bind{TResult}(Func{TSource, IMonad{TResult}})"/>. If the condition is not fulfilled, the computation does not proceed.
        /// </summary>
        /// <typeparam name="TResult">The type of the resultant <see cref="IAlternative{TSource}"/>.</typeparam>
        /// <param name="condition">The condition.</param>
        /// <returns>Either Pure(Unit) or Empty.</returns>
        public static TResult Guard<TResult>(Func<bool> condition)
            where TResult : IAlternative<Unit>, new()
            => (TResult)(condition() ? new TResult().Pure(new Unit()) : new TResult().Empty());

        /// <summary>
        /// Aggregates a sequence of <see cref="IAlternative{TSource}"/>, combining the element from left to right via <see cref="IAlternative{TSource}.Alt(IAlternative{TSource})"/>. The accumulator is <see cref="IAlternative{TSource}.Empty"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of values in the <see cref="IAlternative{TSource}"/>.</typeparam>
        /// <typeparam name="TAlt">The type of the <see cref="IAlternative{TSource}"/>.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        public static TAlt AltSum<TResult, TAlt>(this IEnumerable<TAlt> xs)
            where TAlt : IAlternative<TResult>, new()
        => (TAlt)xs.Aggregate(new TAlt().Empty(), (acc, x) => acc.Alt(x));
    }
}
