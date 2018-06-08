namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A group which is also commutative.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    interface ICommutativeGroup<T> : IGroup<T>, ICommutative<T>
        where T : ICommutativeGroup<T>
    {
    }
}
