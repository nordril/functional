namespace Indril.Functional.Category
{
    /// <summary>
    /// Monads that support choice (see <see cref="IMonadPlus{T}.Mplus(IMonadPlus{T})"/>) in addition to failure (see <see cref="IMonadZero{T}.Mzero"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMonadPlus<T> : IMonadZero<T>, IAlternative<T>
    {
        /// <summary>
        /// Adds two monad values. Implementors must fulfill the following for all X and a, b, c, and k:
        /// <code>
        ///     X.Mzero.Mplus(a) = a (left identity)
        ///     a.Mplus(X.mzero) = a (right identity)
        ///     a.Mplus(b).Mplus(c) = a.Mplus(b.Mplus(c)) (associativity)
        ///     a.Mplus(b).Bind(k) = a.Bind(k).Mplus(b.Bind(k)) (left-distribution)
        /// </code>
        /// </summary>
        /// <param name="that">The second argument.</param>
        IMonadPlus<T> Mplus(IMonadPlus<T> that);
    }
}
