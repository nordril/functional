using System;
using System.Collections.Generic;
using System.Text;

/*namespace Nordril.Functional.Data
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which compares two sets of elements for structural equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    public class SetEqualityComparer<T> : IEqualityComparer<ISet<T>>
    {
        /// <summary>
        /// The underlying elementwise comparer.
        /// </summary>
        private readonly Func<T, T, bool> elementComparer;

        /// <summary>
        /// Creates a new comparer.
        /// </summary>
        /// <param name="elementComparer">The comparison function for two elements of the sequences.</param>
        public SetEqualityComparer(Func<T, T, bool> elementComparer)
        {
            this.elementComparer = elementComparer;
        }

        /// <inheritdoc />
        public bool Equals(ISet<T> x, ISet<T> y)
        {
            if (x == null && y == null)
                return true;
            else if ((x == null) != (y == null))
                return false;
            else if (x.Count != y.Count)
                return false;

            //Optimization: use hashing if available.
            //This is taken from System.Collections.HashSet.HashSetEquals, mostly.
            if (x is HashSet<T> hsX && y is HashSet<T> hsY)
            {
                foreach (var xx in x)
                    if (!hsY.Contains(xx))
                        return false;

                return true;
            }
            //
            else
            {

            }

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
        public int GetHashCode(ISet<T> obj)
            => obj.HashElements();
    }
}*/