using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// Data structures which can be cheaply sliced into sub-data-structures.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface ISliceable<T>
    {
        /// <summary>
        /// Gets a slice of the data structure, starting with the index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The first index which should be included in the slice.</param>
        /// <returns></returns>
        T Slice(int index);

        /// <summary>
        /// Gets a slice of the data structure, starting with the index <paramref name="index"/> and containing <paramref name="count"/> elements.
        /// </summary>
        /// <param name="index">The first index which should be included in the slice.</param>
        /// <param name="count">The number of elements, starting from <paramref name="index"/>, to include.</param>
        /// <exception cref="IndexOutOfRangeException">If the data structure does not contain enough elements.</exception>
        T Slice(int index, int count);
    }
}
