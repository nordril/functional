using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which lexicographically compares two sequences of elements. If the elements in the shorter sequence are all equal according to the comparison, the longer sequence counts as the greater one.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    public class LexicographicalComparer<T> : IComparer<IEnumerable<T>>
    {
        /// <summary>
        /// The underlying elementwise comparer.
        /// </summary>
        private readonly Func<T, T, int> elementComparer;

        /// <summary>
        /// Creates a new comparer.
        /// </summary>
        /// <param name="elementComparer">The comparison function for two elements of the sequences.</param>
        public LexicographicalComparer(Func<T, T, int> elementComparer)
        {
            this.elementComparer = elementComparer;
        }

        /// <inheritdoc />
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            using (var xEnum = x.GetEnumerator())
            using (var yEnum = y.GetEnumerator())
            {
                while (true)
                {
                    var xnext = xEnum.MoveNext();
                    var ynext = yEnum.MoveNext();

                    if (!xnext && !ynext)
                        return 0;
                    if (xnext && !ynext)
                        return 1;
                    else if (!xnext && ynext)
                        return -1;
                    else if (xnext && ynext)
                    {
                        var elementComparison = elementComparer(xEnum.Current, yEnum.Current);

                        if (elementComparison != 0)
                            return elementComparison;
                    }
                }
            }
        }
    }
}
