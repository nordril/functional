using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A type-tagged commutative <see cref="IMonoid{T}"/> which notes its operation as a type-level tag <typeparamref name="TOp"/>.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TOp">The type-tag of the operation.</typeparam>
    public interface ITaggedCommutativeMonoid<T, TOp> : IMonoid<T>, ICommutative<T>
        where T : IMonoid<T>, ICommutative<T>
    {
    }
}
