using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// An algebraic structure supporting a unary operation with no further guaranteed properties.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IHasUnary<T> : IUnary<T>
        where T : IHasUnary<T>
    {
    }

    /// <summary>
    /// An algebraic structure with a unary operation which is idempotent.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IHasUnaryIdempotent<T> : IUnaryIdempotent<T>
        where T : IHasUnaryIdempotent<T>
    {
    }

    /// <summary>
    /// An algebraic structure with a unary operation which is an involution, i.e.
    /// <code>
    ///     m.UnaryOp(m.UnaryOp(X)) == X
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IHasUnaryInvolution<T> : IUnaryInvolution<T>
        where T : IHasUnaryInvolution<T>
    {
    }
}
