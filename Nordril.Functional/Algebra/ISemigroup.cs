namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A semigroup whose binary operation is associative.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface ISemigroup<T> : IMagma<T>
        where T : ISemigroup<T>
    {
    }
}
