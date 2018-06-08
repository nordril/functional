using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indril.Functional.Algebra
{
    /// <summary>
    /// A value-level monoid.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class Monoid<T> : Semigroup<T>
    {
        /// <summary>
        /// The neutral element of the binary operation.
        /// </summary>
        public T Neutral { get; private set; }

        /// <summary>
        /// Creates a new monoid.
        /// </summary>
        /// <param name="neutral">The neutral element.</param>
        /// <param name="op">The binary operation.</param>
        public Monoid(T neutral, Func<T,T,T> op) : base(op)
        {
            Neutral = neutral;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Monoid{T}"/>.
    /// </summary>
    public static class Monoid
    {
        /// <summary>
        /// Reifies a type's <see cref="IMonoid{T}"/> instance into its own <see cref="Monoid{T}"/>-object.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IMonoid{T}"/>.</typeparam>
        /// <param name="instance">An object that is an instance of <see cref="IMonoid{T}"/>.</param>
        public static Monoid<T> FromMonoidInstance<T>(IMonoid<T> instance) where T : IMonoid<T>
            => new Monoid<T>(instance.Neutral, (x, y) => x.Op(y));

        /// <summary>
        /// The ([],++) monoid for lists.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        public static Monoid<List<T>> ListAppend<T>()
            => new Monoid<List<T>>(new List<T>(), (x, y) => { x.AddRange(y); return x; });

        /// <summary>
        /// The ("",+) monoid for <see cref="string"/>.
        /// BEWARE THAT THIS HAS VERY POOR PERFORMANCE.
        /// </summary>
        public static readonly Monoid<string> StringAppend = new Monoid<string>(string.Empty, (x, y) => x + y);

        /// <summary>
        /// The ("",Append) monoid for <see cref="StringBuilder"/>.
        /// </summary>
        public static readonly Monoid<StringBuilder> StringBuilderAppend = new Monoid<StringBuilder>(new StringBuilder(), (x, y) => {
            x.EnsureCapacity(x.Length + y.Length);
            return x.Append(y);
        });

        /// <summary>
        /// The (true,&amp;&amp;) monoid for <see cref="bool"/>.
        /// </summary>
        public static readonly Monoid<bool> BoolAnd = new Monoid<bool>(true, (x, y) => x && y);

        /// <summary>
        /// The (true,||) monoid for <see cref="bool"/>.
        /// </summary>
        public static readonly Monoid<bool> BoolOr = new Monoid<bool>(false, (x, y) => x || y);

        /// <summary>
        /// The (0,+) monoid for <see cref="int"/>.
        /// </summary>
        public static readonly Monoid<int> IntMult = new Monoid<int>(0, (x, y) => x * y);

        /// <summary>
        /// Sums a list of elements using a monoid instance. If the list is empty, the neutral element is returned.
        /// </summary>
        /// <typeparam name="T">The type of elements to sum.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        /// <param name="m">The monoid instance.</param>
        public static T Msum<T>(this IEnumerable<T> xs, Monoid<T> m) => xs.Aggregate(m.Neutral, (x, y) => m.Op(x, y));
    }
}
