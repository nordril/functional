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
        /// An equality-comparer which compares two dictionaries for structural equality, meaning based on the keys and values they contain.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        private class DictionaryEqualityComparerImpl<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
        {
            private readonly IEqualityComparer<TKey> keyComp;
            private readonly IEqualityComparer<TValue> valueComp;

            public DictionaryEqualityComparerImpl(IEqualityComparer<TKey> keyComp, IEqualityComparer<TValue> valueComp)
            {
                this.keyComp = keyComp;
                this.valueComp = valueComp;
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
                .Both(ks => ks.Select(keyComp.GetHashCode).HashElements(), vs => vs.Select(valueComp.GetHashCode).HashElements())
                .Apply(xy => new object().DefaultHash(xy.Item1, xy.Item2));
        }

        /// <summary>
        /// Creates a structural <see cref="IEqualityComparer{T}"/> for key- and value-types for which the caller provides <see cref="IEqualityComparer{T}"/>s.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        public static IEqualityComparer<IDictionary<TKey, TValue>> Make<TKey, TValue>(
            IEqualityComparer<TKey> keyComp,
            IEqualityComparer<TValue> valueComp)
            => new DictionaryEqualityComparerImpl<TKey, TValue>(keyComp, valueComp);

        /// <summary>
        /// Creates a structural <see cref="IEqualityComparer{T}"/> for key- and value-types which are <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        public static IEqualityComparer<IDictionary<TKey, TValue>> Make<TKey, TValue>()
            where TKey : IEquatable<TKey>
            where TValue : IEquatable<TValue>
            => Make(
                new FuncEqualityComparer<TKey>((x, y) => x.Equals(y), x => x.GetHashCode()),
                new FuncEqualityComparer<TValue>((x, y) => x.Equals(y), x => x.GetHashCode()));
    }
}
