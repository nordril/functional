namespace Nordril.Functional.Category
{
    /// <summary>
    /// Monads that support choice (see <see cref="IMonadPlus{T}.Mplus(IMonadPlus{T})"/>) in addition to failure (see <see cref="IMonadZero{T}.Mzero"/>).
    /// </summary>
    /// <typeparam name="T">The type of the contained elements.</typeparam>
    public interface IMonadPlus<T> : IMonadZero<T>, IAlternative<T>
    {
        /// <summary>
        /// Adds two monad values. Implementors must fulfill the following for all X and a, b, c, and k:
        /// <code>
        ///     X.Mzero.Mplus(a) = a (left identity)<br />
        ///     a.Mplus(X.mzero) = a (right identity)<br />
        ///     a.Mplus(b).Mplus(c) = a.Mplus(b.Mplus(c)) (associativity)<br />
        ///     a.Mplus(b).Bind(k) = a.Bind(k).Mplus(b.Bind(k)) (left-distribution)<br />
        /// </code>
        /// </summary>
        /// <param name="that">The second argument.</param>
        IMonadPlus<T> Mplus(IMonadPlus<T> that);
    }
}
