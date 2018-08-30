namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A algebraic ring, which is the combination of a commutative group (<see cref="IRing{T}.Plus(T)"/>, <see cref="IRing{T}.Zero"/>) and a monoid (<see cref="IRing{T}.Mult(T)"/>, <see cref="IRing{T}.One"/>), and where the following holds:
    /// <br />
    /// <code>
    ///    a.Mult(b.Plus(c)) == a.Mult(b).Plus(a.Mult(c)) (left-distributivity)
    ///    b.Plus(c).Mult(a) == b.Mult(a).Plus(c.Mult(a) (right-distributivity)
    /// </code>
    /// <br />
    /// In standard mathematical notation, with Plus being '+' and Mult being '*':
    /// <br />
    /// <code>
    ///    a * (b + c) == (a * b) + (a * c)
    ///    (b + c) * a == (b * a) + (c * a)
    /// </code>
    /// <code>
    /// </code>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IRing<T> where T : IRing<T>
    {
        /// <summary>
        /// The neutral element of <see cref="Plus(T)"/>.
        /// </summary>
        T Zero { get; }
        /// <summary>
        /// The neutral element of <see cref="Mult(T)"/>.
        /// </summary>
        T One { get; }
        /// <summary>
        /// The addition-operation which forms a commutative group with <see cref="Zero"/>.
        /// </summary>
        /// <param name="that">The second operand.</param>
        T Plus(T that);
        /// <summary>
        /// The multiplication-operation which forms a monoid with <see cref="One"/>.
        /// </summary>
        /// <param name="that">The second operand.</param>
        T Mult(T that);
    }
}
