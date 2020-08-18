using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// An algebraic structure supporting a unary operation with no further guaranteed properties.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IUnary<T>
    {
        /// <summary>
        /// The unary operation. This operation should not change its operand.
        /// </summary>
        /// <param name="x">The operand.</param>
        T UnaryOp(T x);
    }

    /// <summary>
    /// An algebraic structure with a unary operation which is idempotent.
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IUnaryIdempotent<T> : IUnary<T>
    {
    }

    /// <summary>
    /// An algebraic structure with a unary operation which is an involution, i.e.
    /// <code>
    ///     m.UnaryOp(m.UnaryOp(X)) == X
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the carrier set.</typeparam>
    public interface IUnaryInvolution<T> : IUnary<T>
    {
    }
}
