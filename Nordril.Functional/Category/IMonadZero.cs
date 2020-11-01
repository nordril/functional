namespace Nordril.Functional.Category
{
    /// <summary>
    /// A monad with a zero, which corresponds to the zero of an algebraic ring,
    /// being, in essence, a "fail"-element.
    /// </summary>
    /// <typeparam name="T">The type of value contained in the monad.</typeparam>
    public interface IMonadZero<out T> : IMonad<T>
    {
        /// <summary>
        /// The zero of the monad. The zero must fulfill the following for any X:
        /// <code>
        ///     X.Bind(X.MZero) == X.MZero (left zero)<br />
        ///     X.MZero.Bind(f) == X.MZero (right zero)<br />
        /// </code>
        /// </summary>
        IMonadZero<T> Mzero();
    }
}
