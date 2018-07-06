using Indril.Functional.CategoryTheory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indril.Functional.Data
{
    /// <summary>
    /// A "functional list" which, in addition to supporting the operations <see cref="IList{T}"/>, implements <see cref="IMonadPlus{T}"/> (and parent interfaces), and <see cref="IFoldable{TSource}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IFuncList<T> : IList<T>, IMonadPlus<T>, IFoldable<T>
    {
    }
}
