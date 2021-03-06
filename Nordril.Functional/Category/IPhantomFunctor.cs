﻿namespace Nordril.Functional.Category
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

    /// <summary>
    /// Extension methods for <see cref="IPhantomFunctor{T}"/>.
    /// </summary>
    public static class PhantomFunctor
    {
        /// <summary>
        /// Maps the phantom type of an <see cref="IPhantomFunctor{T}"/> to a new type.
        /// </summary>
        /// <typeparam name="TSource">The source phantom type.</typeparam>
        /// <typeparam name="TResult">The result phantom type.</typeparam>
        /// <param name="f">The functor whose phantom type to map.</param>
        public static IPhantomFunctor<TResult> MapPhantom<TSource, TResult>(this IPhantomFunctor<TSource> f) => f.Map(_ => default(TResult)) as IPhantomFunctor<TResult>;
    }
}
