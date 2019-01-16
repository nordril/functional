using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A value-level partial order.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public class PartialOrder<T>
    {

        /// <summary>
        /// The "less than or equals"-predicate. See <see cref="IPartiallyOrdered{T}.LeqPartial(T)"/>.
        /// </summary>
        public Func<T, T, Maybe<bool>> LeqPartial { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="leqPartial">The "less than or equals"-predicate.</param>
        public PartialOrder(Func<T, T, Maybe<bool>> leqPartial)
        {
            LeqPartial = leqPartial;
        }
    }
}
