namespace Nordril.Functional.Category
{
    /// <summary>
    /// A phantom-functor which is both a functor and a contravariant functor.
    /// Since a (covariant) functor and a contravariant functor to "the opposite things",
    /// one containing and one consuming values of type <typeparamref name="T"/>, the only
    /// way for any class to implement both is to not contain <typeparamref name="T"/> at all - 
    /// hence, <typeparamref name="T"/> is only a <em>phantom.</em>
    /// </summary>
    /// <typeparam name="T">The type of the values in the functor.</typeparam>
    public interface IPhantomFunctor<T> : IFunctor<T>, IContravariant<T>
    {
    }
}
