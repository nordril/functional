using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// Extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns all values of <typeparamref name="T"/> which fall into the open interval bounded by <paramref name="from"/> and <paramref name="to"/>. O(n) in the number of all values in <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="from">The first element to return.</param>
        /// <param name="to">The last element to return.</param>
        public static IEnumerable<T> To<T>(this T from, T to) where T : Enum
        {
            var cur = from;
            var fromInt = (int)(object)from;
            var toInt = (int)(object)to;

            foreach (var v in Enum.GetValues(typeof(T)))
            {
                var vi = (int)v;

                if (vi >= fromInt && vi <= toInt)
                    yield return (T)v;
            }
        }
    }
}
