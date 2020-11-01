using Nordril.Functional.Lens;
using System;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A functor that is contravariant in one type argument (meaning it needs values of that type)
    /// and covariant in its second type argument (meaning it contains values of that type).
    /// Implementors must fulfill the following for all X and f,g:
    /// <code>
    ///     X.Promap(f, x => x) = X.Contramap(f)<br />
    ///     X.Promap(x => x, f) = X.Map(f)<br />
    /// </code>
    /// </summary>
    /// <typeparam name="TNeed">The type of values the functor needs.</typeparam>
    /// <typeparam name="THave">The type of values the functor contains.</typeparam>
    public interface IProfunctor<in TNeed, out THave> : IContravariant<TNeed>, IFunctor<THave>
    {
        /// <summary>
        /// Applies a function to the contravariant part of the functor and another over its covariant one.
        /// If the profunctor taks A and produces B, then we apply two functions:
        /// one from NewA to A, and one from B to NewB. The result is a profunctor that takes NewA and produces NewB.
        /// </summary>
        /// <typeparam name="TNeedResult">The type of the new values the profunctor will need.</typeparam>
        /// <typeparam name="THaveResult">The type of the new values the profunctor will contain.</typeparam>
        /// <param name="in">The function to apply to the contravariant values.</param>
        /// <param name="out">The function to apply to the covariant values.</param>
        IProfunctor<TNeedResult, THaveResult> Promap<TNeedResult, THaveResult>(Func<TNeedResult, TNeed> @in, Func<THave, THaveResult> @out);
    }

    /// <summary>
    /// Extension methods for profunctors.
    /// </summary>
    public static class Profunctor
    {
        /// <summary>
        /// Wrap a <see cref="Func{T, TResult}"/> into a profunctor-instance.
        /// </summary>
        /// <typeparam name="TNeed">The type of the input.</typeparam>
        /// <typeparam name="THave">The type of the output.</typeparam>
        /// <param name="f">The function to wrap.</param>
        public static ProfunctorFunc<TNeed, THave> FuncToProfunctor<TNeed, THave>(this Func<TNeed, THave> f)
            => new ProfunctorFunc<TNeed, THave>(f);

        /// <summary>
        /// Unsafely casts an <see cref="IProfunctor{TNeed, THave}"/> to a <see cref="ProfunctorFunc{TIn, TOut}"/> and returns its underlying function.
        /// </summary>
        /// <typeparam name="TIn">The type of the input.</typeparam>
        /// <typeparam name="TOut">The type of the output.</typeparam>
        /// <param name="pf">The profunctor to cast.</param>
        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IProfunctor<TIn, TOut> pf) => ((ProfunctorFunc<TIn, TOut>)pf).Func;
    }
}
