using System;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// Monofunctors. Monofunctors are like functors (see <see cref="IFunctor{TSource}"/>), but
    /// they only support one specific type of element. An example would be a string, which is a monofunctor
    /// over char.
    /// Implementations have to obey the same laws as those of <see cref="IFunctor{TSource}"/> (for all X and f,g):
    /// <code>
    ///     X.Map(a => a) == X (identity)<br />
    ///     X.Map(a => g(f(a))) == X.Map(f).Map(g) (homomorphism)<br />
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TElem">The type of element contained in the monofunctor.</typeparam>
    public interface IMonoFunctor<T, TElem> where T : IMonoFunctor<T, TElem>
    {
        /// <summary>
        /// Applies a function to the values contained within the monofunctor
        /// and returns the result, without modifying the original object.
        /// </summary>
        /// <param name="f">The function to apply.</param>
        T MonoMap(Func<TElem, TElem> f);
    }
}
