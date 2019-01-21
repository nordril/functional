using Nordril.Functional.Category;
using System;
using System.Collections.Generic;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A "functional list" which, in addition to supporting the operations <see cref="IList{T}"/>, implements <see cref="IMonadPlus{T}"/> (and parent interfaces), <see cref="IAlternative{TSource}"/>, and <see cref="IFoldable{TSource}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IFuncList<T> : IList<T>, IMonadPlus<T>, IFilterable<IFuncList<T>, T>, IAlternative<T>, IEquatable<IList<T>>, ICopyable<IFuncList<T>>
    {
        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> for <see cref="IList{T}"/>s if the contained elements of type <typeparamref name="T1"/> are comparable.
        /// </summary>
        /// <typeparam name="T1">The of element in the <see cref="IList{T}"/>.</typeparam>
        IComparer<IList<T1>> GetComparer<T1>() where T1 : IComparable<T1>;
    }
}
