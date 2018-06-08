using System;

namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A value-level structure that offers a binary operation.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class Magma<T>
    {
        /// <summary>
        /// The binary operation.
        /// </summary>
        public Func<T,T,T> Op { get; private set; }

        /// <summary>
        /// Creates a new magma.
        /// </summary>
        /// <param name="f">The binary operation.</param>
        public Magma(Func<T,T,T> f)
        {
            Op = f;
        }
    }
}
