using System;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A functor that is contravariant in one type argument (meaning it needs values of that type)
    /// and covariant in its second type argument (meaning it contains values of that type).
    /// Implementors must fulfill the following for all X and f,g:
    /// <code>
    ///     X.Promap(f, x => x) = X.Contramap(f)
    ///     X.Promap(x => x, f) = X.Map(f)
    /// </code>
    /// </summary>
    /// <typeparam name="TNeed">The type of values the functor needs.</typeparam>
    /// <typeparam name="THave">The type of values the functor contains.</typeparam>
    public interface IProfunctor<TNeed, THave> : IContravariant<TNeed>, IFunctor<THave>
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
}
