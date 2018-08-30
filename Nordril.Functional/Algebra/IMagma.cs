namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// An algebraic structure supporting a binary operation with no further guaranteed properties.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IMagma<T> where T : IMagma<T>
    {
        /// <summary>
        /// The binary operation.
        /// This operation should not change either operand.
        /// </summary>
        /// <param name="that">The second operand.</param>
        T Op(T that);
    }
}
