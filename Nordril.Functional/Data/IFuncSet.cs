using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A "functional set" which, in addition to supporting the operations <see cref="ISet{T}"/>, implements <see cref="IMonad{T}"/> (and parent interfaces).
    /// </summary>
    /// <remarks>Be aware that the equality comparer of the <em>left</em> set is used for determining element-equality for all set-theoretical operations. If the right set has a different equality comparer, this behavior will violate the expected theory of equivalence relations, e.g. you might have the situation where <c>S.Equals(T) != T.Equals(S)</c> or <c>S.Except(T).UnionPure(T.Except(S)) != S.SymmetricDifferencePure(T)</c>. This is a conscious design choice, intended to allow you to use the equality comparer of your choice, and you should interpret all set-theoretical operations as modulo the equality comparer of the left set.</remarks>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public interface IFuncSet<T> : ISet<T>, IMonad<T>, IAsyncMonad<T>, IEquatable<ISet<T>>, IEquatable<IFuncSet<T>>, IFoldable<T>, IFilterable<IFuncSet<T>, T>, ICopyable<IFuncSet<T>>
    {
        /// <summary>
        /// Returns the <see cref="IEqualityComparer{T}"/> of the set.
        /// </summary>
        IEqualityComparer<T> Comparer { get; }

        /// <summary>
        /// Returns a new set to which <paramref name="elem"/> has been added, while leaving this set unchanged.
        /// </summary>
        /// <param name="elem">The element to add.</param>
        IFuncSet<T> AddPure(T elem);

        /// <summary>
        /// Returns a new set from which <paramref name="elem"/> has been removed, while leaving this set unchanged.
        /// </summary>
        /// <param name="elem">The element to remove.</param>
        IFuncSet<T> RemovePure(T elem);

        /// <summary>
        /// Returns the union of this set and <paramref name="elem"/>, while leaving this set unchanged.
        /// </summary>
        /// <param name="elem">The second set.</param>
        IFuncSet<T> UnionPure(ISet<T> elem);

        /// <summary>
        /// Returns the intersection of this set and <paramref name="elem"/>, while leaving this set unchanged.
        /// </summary>
        /// <param name="elem">The second set.</param>
        IFuncSet<T> IntersectionPure(ISet<T> elem);

        /// <summary>
        /// Returns the difference between this set and <paramref name="elem"/>, while leaving this set unchanged.
        /// The difference is set the of those elements which are contained in this set but not in <paramref name="elem"/>.
        /// </summary>
        /// <param name="elem">The second set.</param>
        IFuncSet<T> DifferencePure(ISet<T> elem);

        /// <summary>
        /// Returns a new set which is the symmetric difference of this set and <paramref name="elem"/>, while leaving this set unchanged.
        /// The symmetric differnce is the set of those elements which are in one set but not both.
        /// </summary>
        /// <param name="elem">The second set.</param>
        IFuncSet<T> SymmetricDifferencePure(ISet<T> elem);

        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> for <see cref="ISet{T}"/>s if the contained elements of type <typeparamref name="T1"/> are comparable.
        /// </summary>
        /// <typeparam name="T1">The of element in the <see cref="ISet{T}"/>.</typeparam>
        IComparer<ISet<T1>> GetComparer<T1>() where T1 : IComparable<T1>;
    }
}
