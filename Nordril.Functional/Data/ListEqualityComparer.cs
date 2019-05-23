using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which compares two sequences of elements for structural equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    public class ListEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        /// <summary>
        /// The underlying elementwise comparer.
        /// </summary>
        private readonly Func<T, T, bool> elementComparer;

        /// <summary>
        /// Creates a new comparer.
        /// </summary>
        /// <param name="elementComparer">The comparison function for two elements of the sequences.</param>
        public ListEqualityComparer(Func<T, T, bool> elementComparer)
        {
            this.elementComparer = elementComparer;
        }

        /// <inheritdoc />
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            using (var xEnum = x.GetEnumerator())
            using (var yEnum = y.GetEnumerator())
            {
                while (true)
                {
                    var xnext = xEnum.MoveNext();
                    var ynext = yEnum.MoveNext();

                    if (!xnext && !ynext)
                        return true;
                    if (xnext && !ynext)
                        return false;
                    else if (!xnext && ynext)
                        return false;
                    else if (xnext && ynext)
                    {
                        var elementComparison = elementComparer(xEnum.Current, yEnum.Current);

                        if (!elementComparison)
                            return elementComparison;
                    }
                }
            }
        }

        /// <inheritdoc />
        public int GetHashCode(IEnumerable<T> obj)
            => obj.HashElements();
    }
}
