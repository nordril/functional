namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A monoid which supports a unary inversion operator with respect to the associative binary operation.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IGroup<T> : IMonoid<T>
        where T : IGroup<T>
    {
        /// <summary>
        /// Returns the inverse of the object. The inverse must fulfill the following for all X:
        /// <code>
        ///     X.Inverse.Op(X) == X.Neutral (left-inverse)
        ///     X.Op(X.Inverse) == X.Neutral (right-inverse)
        /// </code>
        /// </summary>
        T Inverse { get; }
    }
}
