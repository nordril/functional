using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Algebra
{
    /// <summary>
    /// A type which supports a partial order. A partial order has a "less then or equal" relation (<see cref="LeqPartial(T)"/> with the following properties:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Definition</description>
    ///     </listheader>
    ///     <item>
    ///         <term>reflexivity</term>
    ///         <description>[forall a] a.LeqPartial(a) == Maybe.Just(true)</description>
    ///     </item>
    ///     <item>
    ///         <term>antisymmetry</term>
    ///         <description>[forall a,b] if a.LeqPartial(b) == b.LeqPartial(a) == Maybe.Just(true) then a.Equals(b)</description>
    ///     </item>
    ///     <item>
    ///         <term>transitivity</term>
    ///         <description>[forall a,b,c] if a.LeqPartial(b) == b.LeqPartial(c) == Maybe.Just(true) then a.LeqPartial(c) == Maybe.Just(true)</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface IPartiallyOrdered<T>
        where T : IPartiallyOrdered<T>
    {
        /// <summary>
        /// The "less than or equals"-predicate which returns the comparison of two elements or <see cref="Maybe.Nothing{T}"/> if the two elements are incomparable.
        /// </summary>
        /// <param name="that">The second element to compare to the first.</param>
        /// <returns>Maybe.Just(true) iff the first element is less than or equal to the second. Maybe.Nothing if the elements are incomparable.</returns>
        Maybe<bool> LeqPartial(T that);
    }
}
