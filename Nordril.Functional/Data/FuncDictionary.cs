﻿using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A wrapper around an arbitrary <see cref="IDictionary{TKey, TValue}"/> which can determine equality based on the contained keys and values instead of by reference, and which implements various functional interfaces. Two <see cref="FuncDictionary{TKey, TValue}"/>-object are equal if they contain the same set of keys and, for every contained key, <see cref="object.Equals(object)"/> of the corresponding values returns true.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public struct FuncDictionary<TKey, TValue> : IFuncDictionary<TKey, TValue>
    {
        /// <summary>
        /// The underlying dictionary field.
        /// </summary>
        private IDictionary<TKey, TValue> dict;

        /// <summary>
        /// The comparer.
        /// </summary>
        private DictionaryEqualityComparer<TKey, TValue> comparer;

        /// <inheritdoc />
        public void Add(TKey key, TValue value) => DictCoalesce().Add(key, value);

        /// <inheritdoc />
        public bool ContainsKey(TKey key) => DictCoalesce().ContainsKey(key);

        /// <inheritdoc />
        public bool Remove(TKey key) => DictCoalesce().Remove(key);

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) => DictCoalesce().TryGetValue(key, out value);

        /// <inheritdoc />
        public TValue this[TKey key] { get => DictCoalesce()[key]; set { DictCoalesce()[key] = value; } }

        /// <summary>
        /// Creates a new <see cref="FuncDictionary{TKey, TValue}"/> from a pair of keys and values.
        /// </summary>
        /// <param name="keyComparer">A comparer for the keys.</param>
        /// <param name="pairs">The pairs to put into the dictionary.</param>
        public FuncDictionary(IComparer<TKey> keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            pairs ??= Array.Empty<KeyValuePair<TKey, TValue>>();

            dict = new Dictionary<TKey, TValue>(pairs);

            comparer = DictionaryEqualityComparer.Make(
                keyComparer,
                new FuncEqualityComparer<TValue>((x,y) => x.Equals(y)));
        }

        /// <summary>
        /// Creates a new <see cref="FuncDictionary{TKey, TValue}"/> from a pair of keys and values, and a custom comparer.
        /// </summary>
        /// <param name="pairs">The pairs to put into the dictionary.</param>
        /// <param name="keyComparer">The comparer for the keys. This must also implement <see cref="IEqualityComparer{T}"/>.</param>
        /// <param name="valueComparer">The equality comparer for the values.</param>
        public FuncDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            pairs ??= Array.Empty<KeyValuePair<TKey, TValue>>();

            dict = new Dictionary<TKey, TValue>(pairs);

            valueComparer ??= new FuncEqualityComparer<TValue>((x, y) => x.Equals(y));

            comparer = DictionaryEqualityComparer.Make(keyComparer, valueComparer);
        }

        /// <summary>
        /// Creates a new <see cref="FuncDictionary{TKey, TValue}"/> from a pair of keys and values.
        /// </summary>
        /// <param name="keyComparer">The comparer for the keys. This must also implement <see cref="IEqualityComparer{T}"/>.</param>
        /// <param name="pairs">The pairs to put into the dictionary.</param>
        public FuncDictionary(IComparer<TKey> keyComparer, IEnumerable<(TKey, TValue)> pairs) : this(keyComparer, pairs.Select(p => new KeyValuePair<TKey, TValue>(p.Item1, p.Item2)))
        {
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => DictCoalesce().Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values => DictCoalesce().Values;

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item) => DictCoalesce().Add(item);

        /// <inheritdoc />
        public void Clear() => DictCoalesce().Clear();

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item) => DictCoalesce().Contains(item);

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => DictCoalesce().CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item) => DictCoalesce().Remove(item);

        /// <inheritdoc />
        public int Count => DictCoalesce().Count;

        /// <inheritdoc />
        public bool IsReadOnly => DictCoalesce().IsReadOnly;

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => DictCoalesce().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => DictCoalesce().GetEnumerator();

        /// <summary>
        /// Determines equality based on the contents of two <see cref="FuncDictionary{TKey, TValue}"/>-objects. Two objects are equal if they have the same set of keys and if, for every contained key, <see cref="IEquatable{T}.Equals(T)"/> of the values returns true.
        /// </summary>
        /// <param name="other">The other <see cref="FuncDictionary{TKey, TValue}"/>.</param>
        public bool Equals(IDictionary<TKey, TValue> other)
        {
            return ComparerCoalesce().Equals(DictCoalesce(), other);
        }

        /// <summary>
        /// See <see cref="Equals(IDictionary{TKey, TValue})"/>.
        /// </summary>
        /// <param name="obj">The other oject.</param>
        public override bool Equals(object obj)
            => obj is FuncDictionary<TKey, TValue> dictionary && Equals(dictionary);

        /// <summary>
        /// See <see cref="Equals(IDictionary{TKey, TValue})"/>.
        /// </summary>
        /// <param name="left">The first object.</param>
        /// <param name="right">The second object.</param>
        public static bool operator ==(FuncDictionary<TKey, TValue> left, FuncDictionary<TKey, TValue> right)
            => left.Equals(right);

        /// <summary>
        /// See <see cref="Equals(IDictionary{TKey, TValue})"/>.
        /// </summary>
        /// <param name="left">The first object.</param>
        /// <param name="right">The second object.</param>
        public static bool operator !=(FuncDictionary<TKey, TValue> left, FuncDictionary<TKey, TValue> right)
            => !(left == right);

        /// <summary>
        /// Computes the hash based on <see cref="Functional.CollectionExtensions.HashElements{T}(IEnumerable{T})"/>.
        /// </summary>
        public override int GetHashCode() => this.OrderBy(x => x.Key, comparer.KeyComp).HashElements();

        /// <inheritdoc />
        public IFuncDictionary<TKey, TValue> AddPure(TKey key, TValue value, out bool success)
        {
            if (DictCoalesce().ContainsKey(key))
            {
                success = false;
                return this;
            }
            else
            {
                success = true;
                var ret = new FuncDictionary<TKey, TValue>(DictCoalesce(), ComparerCoalesce().KeyComp, ComparerCoalesce().ValueComp)
                {
                    { key, value }
                };

                return ret;
            }
        }

        /// <inheritdoc />
        public IFuncDictionary<TKey, TValue> RemovePure(TKey key, out bool success)
        {
            if (!DictCoalesce().ContainsKey(key))
            {
                success = false;
                return this;
            }
            else
            {
                success = true;
                var ret = new FuncDictionary<TKey, TValue>(DictCoalesce(), ComparerCoalesce().KeyComp, ComparerCoalesce().ValueComp);
                ret.Remove(key);

                return ret;
            }
        }

        /// <inheritdoc />
        public IFuncDictionary<TKey, TValue> UpsertPure(TKey key, TValue value)
        {
            var ret = new FuncDictionary<TKey, TValue>(DictCoalesce(), ComparerCoalesce().KeyComp, ComparerCoalesce().ValueComp);
            if (ret.ContainsKey(key))
                ret[key] = value;
            else
                ret.Add(key, value);

            return ret;
        }

        /// <inheritdoc />
        public IFuncDictionary<TKey, TValue> UpdatePure(TKey key, Func<TKey, TValue, TValue> f)
        {
            if (!DictCoalesce().ContainsKey(key))
                return this;

            var ret = new FuncDictionary<TKey, TValue>(DictCoalesce(), ComparerCoalesce().KeyComp, ComparerCoalesce().ValueComp);
            ret[key] = f(key, ret[key]);

            return ret;
        }

        /// <summary>
        /// Retuns <see cref="dict"/>. If <see cref="dict"/> has not been assigned, it's set to a new, empty dictionary first.
        /// </summary>
        /// <remarks>
        /// If two callers hold references to this struct and <see cref="dict"/> has not been initialized yet, they will hold references to different dictionaries after this method returns.
        /// </remarks>
        private IDictionary<TKey, TValue> DictCoalesce()
        {
            if (dict == null)
                dict = new Dictionary<TKey, TValue>();
            return dict;
        }

        /// <summary>
        /// Retuns <see cref="comparer"/>. If <see cref="comparer"/> has not been assigned, it's set to a new, default comparer first
        /// </summary>
        /// <remarks>
        /// If two callers hold references to this struct and <see cref="comparer"/> has not been initialized yet, they will hold references to different lists after this method returns.
        /// </remarks>
        private DictionaryEqualityComparer<TKey, TValue> ComparerCoalesce()
        {
            Func<TKey, TKey, int> keyComparer;

            if (typeof(TKey) is IComparable<TKey>)
                keyComparer = (x, y) => ((IComparable<TKey>)x).CompareTo(y);
            else
                keyComparer = (x, y) => x.GetHashCode().CompareTo(y.GetHashCode());

            if (comparer == null)
                comparer = DictionaryEqualityComparer.Make(
                new FuncComparer<TKey>(keyComparer, x => x.GetHashCode()),
                new FuncEqualityComparer<TValue>((x, y) => x.Equals(y)));
            return comparer;
        }

        /// <summary>
        /// Applies a function to the values of the dictionary and returns the new dictionary, leaving the old one unchanged. If <typeparamref name="TValue"/> is different from <typeparamref name="TResult"/>, the value-comparer is downgraded to <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the new values.</typeparam>
        /// <param name="f">The function to apply to the values.</param>
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f)
        {
            var oldValueComp = ComparerCoalesce().ValueComp;

            var valueComp = new FuncEqualityComparer<TResult>((x, y) => oldValueComp.Equals((TValue)(object)x, (TValue)(object)y));

            if (typeof(TValue) != typeof(TResult))
                valueComp = new FuncEqualityComparer<TResult>((x, y) => x.Equals(y));

            return new FuncDictionary<TKey, TResult>(DictCoalesce().Select(kv => new KeyValuePair<TKey, TResult>(kv.Key, f(kv.Value))), ComparerCoalesce().KeyComp, valueComp);
        }

        /// <inheritdoc />
        public IKeyedFunctor<TKey, TResult> MapWithKey<TResult>(Func<TKey, TValue, TResult> f)
        {
            var oldValueComp = ComparerCoalesce().ValueComp;

            var valueComp = new FuncEqualityComparer<TResult>((x, y) => oldValueComp.Equals((TValue)(object)x, (TValue)(object)y));

            if (typeof(TValue) != typeof(TResult))
                valueComp = new FuncEqualityComparer<TResult>((x, y) => x.Equals(y));

            return new FuncDictionary<TKey, TResult>(DictCoalesce().Select(kv => new KeyValuePair<TKey, TResult>(kv.Key, f(kv.Key, kv.Value))), ComparerCoalesce().KeyComp, valueComp);
        }

        /// <inheritdoc />
        public IFuncDictionary<TKey, TValue> MonoMap(Func<TValue, TValue> f)
        {
            return new FuncDictionary<TKey, TValue>(DictCoalesce().Select(kv => new KeyValuePair<TKey, TValue>(kv.Key, f(kv.Value))), ComparerCoalesce().KeyComp, ComparerCoalesce().ValueComp);
        }
    }

    /// <summary>
    /// Extension methods for <see cref="FuncDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class FuncDictionary
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="FuncDictionary{TKey, TValue}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TKey">The type of the source's key.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static FuncDictionary<TKey, TResult> Select<TKey, TSource, TResult>(this FuncDictionary<TKey, TSource> source, Func<TSource, TResult> f)
            => (FuncDictionary<TKey, TResult>)source.Map(f);

        /// <summary>
        /// Creates a new <see cref="FuncDictionary{TKey, TValue}"/> based on the <see cref="IEquatable{T}"/>-instances of the keys and values.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static FuncDictionary<TKey, TValue> Make<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> xs = null)
            where TKey : IComparable<TKey>
            where TValue : IEquatable<TValue>
        {
            return new FuncDictionary<TKey, TValue>(xs, new FuncComparer<TKey>((x,y) => x.CompareTo(y), x => x.GetHashCode()), new FuncEqualityComparer<TValue>((x, y) => x.Equals(y)));
        }

        /// <summary>
        /// Creates a new <see cref="FuncDictionary{TKey, TValue}"/> based on the <see cref="IEquatable{T}"/>-instances of the keys and values.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="xs"></param>
        /// <returns></returns>
        public static FuncDictionary<TKey, TValue> Make<TKey, TValue>(IEnumerable<(TKey, TValue)> xs = null)
            where TKey : IComparable<TKey>
            where TValue : IEquatable<TValue>
        {
            return new FuncDictionary<TKey, TValue>(xs.Select(x => new KeyValuePair<TKey, TValue>(x.Item1,x.Item2)), new FuncComparer<TKey>((x, y) => x.CompareTo(y), x => x.GetHashCode()), new FuncEqualityComparer<TValue>((x, y) => x.Equals(y)));
        }
    }
}
