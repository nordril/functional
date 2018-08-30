namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A magma whose binary operation is commutative.
    /// The binary operation must fulfill the following for all X and Y:
    /// <code>
    ///     X.Op(Y) == Y.Op(X) (commutativity)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface ICommutative<T> : IMagma<T>
        where T : ICommutative<T>
    {
    }
}
