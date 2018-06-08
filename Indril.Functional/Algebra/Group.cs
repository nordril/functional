using System;

namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A value-level group.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class Group<T> : Monoid<T>
    {
        /// <summary>
        /// The inversion function.
        /// </summary>
        public Func<T, T> Invert { get; private set; }

        /// <summary>
        /// C
        /// </summary>
        /// <param name="neutral">The neutral element.</param>
        /// <param name="op">The binary operation.</param>
        /// <param name="invert">The inversion function.</param>
        public Group(T neutral, Func<T,T,T> op, Func<T,T> invert) : base(neutral, op)
        {
            Invert = invert;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Monoid{T}"/>.
    /// </summary>
    public static class Group
    {
        /// <summary>
        /// Reifies a type's <see cref="IGroup{T}"/> instance into its own <see cref="Group{T}"/>-object.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IGroup{T}"/>.</typeparam>
        /// <param name="instance">An object that is an instance of <see cref="IGroup{T}"/>.</param>
        public static Group<T> FromGroupInstance<T>(IGroup<T> instance) where T : IGroup<T>
            => new Group<T>(instance.Neutral, (x, y) => x.Op(y), x => x.Inverse);

        /// <summary>
        /// The (0,+,negate) group for <see cref="int"/>.
        /// </summary>
        public static readonly Group<int> IntAdd = new Group<int>(0, (x, y) => x + y, x => x*(-1));

        /// <summary>
        /// The (0,+,negate) group for <see cref="float"/>.
        /// </summary>
        public static readonly Group<float> FloatAdd = new Group<float>(0f, (x, y) => x + y, x => x * (-1f));

        /// <summary>
        /// The (0,+,negate) group for <see cref="double"/>.
        /// </summary>
        public static readonly Group<double> DoubleAdd = new Group<double>(0d, (x, y) => x + y, x => x * (-1d));

        /// <summary>
        /// The (0,+,negate) group for <see cref="decimal"/>.
        /// </summary>
        public static readonly Group<decimal> DecimalAdd = new Group<decimal>(0m, (x, y) => x + y, x => x * (-1m));

        /// <summary>
        /// The (1,*,x => 1/x) group for <see cref="float"/>.
        /// </summary>
        public static readonly Group<float> FloatMult = new Group<float>(1f, (x, y) => x * y, x => 1f / x);

        /// <summary>
        /// The (1,*,x => 1/x) group for <see cref="double"/>.
        /// </summary>
        public static readonly Group<double> DoubleMult = new Group<double>(1d, (x, y) => x * y, x => 1d / x);

        /// <summary>
        /// The (1,*,x => 1/x) group for <see cref="decimal"/>.
        /// </summary>
        public static readonly Group<decimal> DecimalMult = new Group<decimal>(1m, (x, y) => x * y, x => 1m / x);
    }
}
