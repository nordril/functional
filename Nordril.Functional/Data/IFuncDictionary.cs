using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A dictionary which supports pure operations which do not modify the original.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public interface IFuncDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEquatable<IDictionary<TKey, TValue>>, IFunctor<TValue>, IMonoFunctor<IFuncDictionary<TKey, TValue>, TValue>
    {
        /// <summary>
        /// Returns a new dictionary to which the pair (<paramref name="key"/>, <paramref name="value"/>) has been added, leaving the original unmodified. <paramref name="success"/> is true if the key was not present in the original.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="success">true if the key was not present in the original, false otherwise.</param>
        IFuncDictionary<TKey, TValue> AddPure(TKey key, TValue value, out bool success);

        /// <summary>
        /// Returns a new dictionary from which <paramref name="key"/>has been removed, leaving the original unmodified. <paramref name="success"/> is true if the key was present in the original.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <param name="success">true if the key was present in the original, false otherwise.</param>
        IFuncDictionary<TKey, TValue> RemovePure(TKey key, out bool success);

        /// <summary>
        /// Returns a new dictionary to which the pair (<paramref name="key"/>, <paramref name="value"/>) has been added if the key was not present, or one where the value of the key <paramref name="key"/> has been overwritten by <paramref name="value"/> if it was, leaving the original unmodified.
        /// </summary>
        /// <param name="key">The key to add/update.</param>
        /// <param name="value">The value to add/update.</param>
        IFuncDictionary<TKey, TValue> UpsertPure(TKey key, TValue value);

        /// <summary>
        /// Returns a new dictionary in which the value of the key <paramref name="key"/> has been set to the result of <paramref name="f"/> if it was present, leaving the original unmodified.
        /// </summary>
        /// <param name="key">The key whose value to update.</param>
        /// <param name="f">The function whose result will be the new value. The first parameter is the key, the second is the old value.</param>
        IFuncDictionary<TKey, TValue> UpdatePure(TKey key, Func<TKey, TValue, TValue> f);
    }
}
