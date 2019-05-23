using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A semiring, composed of a commutative monoid for addition and a monoid for multiplication.
    /// Implementors MUST obey the following:
    /// <code>
    ///     x.Mult(y.Plus(z)) == x.Mult(y).Plus(x.Mult(z)) (left-distribution)
    ///     (x.Mult(y)).Plus(z) == x.Mult(z).Plus(y.Mult(z)) (right-distribution)
    ///     x.Mult(Zero) == Zero.Mult(x) == Zero (annihilation)
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TPlus">The type-tag for the addition. Canonically, <see cref="TagPlus"/>.</typeparam>
    /// <typeparam name="TMult">The type-tag for the multiplication. Canonincally <see cref="TagMult"/>.</typeparam>
    public interface ISemiring<T, TPlus, TMult> : ITaggedCommutativeMonoid<T, TPlus>, ITaggedMonoid<T, TMult>
        where T : IMonoid<T>, ICommutative<T>
    {
        /// <summary>
        /// The additive operation. This must be an alias of <see cref="IMagma{T}.Op(T)"/> of the first monoid.
        /// </summary>
        /// <param name="y">The second operand.</param>
        T Plus(T y);

        /// <summary>
        /// The additive operation. This must be an alias of <see cref="IMagma{T}.Op(T)"/> of the second monoid.
        /// </summary>
        /// <param name="y">The second operand.</param>
        T Mult(T y);

        /// <summary>
        /// The multiplicative neutral element. This must be an alias of <see cref="IMonoid{T}.Neutral"/> of the second monoid.
        /// </summary>
        T One { get; }

        /// <summary>
        /// The additive neutral element. This must be an alias of <see cref="IMonoid{T}.Neutral"/> of the first monoid.
        /// </summary>
        T Zero { get; }
    }
}
