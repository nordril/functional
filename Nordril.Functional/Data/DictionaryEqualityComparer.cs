using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A factory-class that creates <see cref="IEqualityComparer{T}"/>s which compare dictionaries for structural equality.
    /// </summary>
    public static class DictionaryEqualityComparer
    {
        /// <summary>
        /// Creates a structural <see cref="IEqualityComparer{T}"/> for key- and value-types for which the caller provides <see cref="IEqualityComparer{T}"/>s.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        public static DictionaryEqualityComparer<TKey, TValue> Make<TKey, TValue>(
            IComparer<TKey> keyComp,
            IEqualityComparer<TValue> valueComp)
            => new DictionaryEqualityComparer<TKey, TValue>(keyComp, valueComp);

        /// <summary>
        /// Creates a structural <see cref="IEqualityComparer{T}"/> for key- and value-types which are <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        public static DictionaryEqualityComparer<TKey, TValue> Make<TKey, TValue>()
            where TKey : IComparable<TKey>
            where TValue : IEquatable<TValue>
            => Make(
                new FuncComparer<TKey>((x, y) => x.CompareTo(y), x => x.GetHashCode()),
                new FuncEqualityComparer<TValue>((x, y) => x.Equals(y), x => x.GetHashCode()));
    }

    /// <summary>
    /// An equality-comparer which compares two dictionaries for structural equality, meaning based on the keys and values they contain.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        /// <summary>
        /// The key-comparer.
        /// </summary>
        public IComparer<TKey> KeyComp { get; private set; }

        /// <summary>
        /// The key-comparer, upcast to an <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public IEqualityComparer<TKey> KeyEqComp { get; private set; }

        /// <summary>
        /// The value-commparer.
        /// </summary>
        public IEqualityComparer<TValue> ValueComp { get; private set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyComp">The key-comparer. This must also implement <see cref="IEqualityComparer{T}"/>.</param>
        /// <param name="valueComp">The value-comparer.</param>
        public DictionaryEqualityComparer(IComparer<TKey> keyComp, IEqualityComparer<TValue> valueComp)
        {
            KeyComp = keyComp;
            KeyEqComp = (IEqualityComparer<TKey>)keyComp;
            ValueComp = valueComp;
        }

        /// <inheritdoc />
        public bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            var onlyOtherKeys = y.Keys.Except(x.Keys);

            if (onlyOtherKeys.Empty())
            {
                var thisSame = x.All(kv =>
                {
                    if (y.TryGetValue(kv.Key, out var otherValue))
                        return kv.Value.Equals(otherValue);
                    else
                        return false;
                });

                return thisSame;
            }
            else
                return false;
        }

        /// <inheritdoc />
        public int GetHashCode(IDictionary<TKey, TValue> obj)
            => obj
            .Unzip(kv => (kv.Key, kv.Value))
            .Both(ks => ks.Select(KeyEqComp.GetHashCode).HashElements(), vs => vs.Select(ValueComp.GetHashCode).HashElements())
            .Apply(xy => new object().DefaultHash(xy.Item1, xy.Item2));
    }
}
