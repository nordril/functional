using Nordril.Functional.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Algebra
{

    /// <summary>
    /// A value-level binary relation.
    /// </summary>
    /// <typeparam name="T1">The type of first elements contained in the relation.</typeparam>
    /// <typeparam name="T2">The type of second elements contained in the relation.</typeparam>
    internal struct BinaryRelation<T1, T2> : IBinaryRelation<T1, T2>
    {
        private readonly Func<T1, T2, bool> contains;

        /// <summary>
        /// Creates a new binary relation.
        /// </summary>
        /// <param name="contains">The "contains"-function.</param>
        public BinaryRelation(Func<T1, T2, bool> contains)
        {
            this.contains = contains;
        }

        /// <inheritdoc />
        public bool Contains(T1 x, T2 y) => contains(x, y);
    }

    /// <summary>
    /// A value-level partial order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the relation.</typeparam>
    internal struct PartialOrder<T> : IPartialOrder<T>
    {
        private readonly Func<T, T, bool> contains;

        /// <summary>
        /// Creates a new partial order from a partial "less than or equal to"-function.
        /// </summary>
        /// <param name="leqPartial">The partial "less than or equal to"-function.</param>
        public PartialOrder(Func<T, T, Maybe<bool>> leqPartial)
        {
            contains = (x, y) => { leqPartial(x, y).TryGetValue(false, out var res); return res; } ;
        }

        /// <inheritdoc />
        public bool Contains(T x, T y) => contains(x, y);
    }

    /// <summary>
    /// A value-level total order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the relation.</typeparam>
    internal struct TotalOrder<T> : ITotalOrder<T>
    {
        private readonly Func<T, T, short> comparison;

        /// <summary>
        /// Creates a new total order from a comparison-function.
        /// </summary>
        /// <param name="comparison">The comparison-function.</param>
        public TotalOrder(Func<T, T, short> comparison)
        {
            this.comparison = comparison;
        }

        /// <summary>
        /// Creates a total order from a "less than or equal to"-function.
        /// </summary>
        /// <param name="leq">The "less than or equal to"-function.</param>
        public TotalOrder(Func<T, T, bool> leq)
        {
            comparison = (x, y) =>
                leq(x, y)
                ? (leq(y, x) ? (short)0 : (short)-1)
                : (short)1;
        }

        /// <inheritdoc />
        public short Compare(T x, T y) => comparison(x, y);

        /// <inheritdoc />
        public bool Contains(T x, T y) => comparison(x, y) <= 0;
    }

    /// <summary>
    /// A value-level function-relation.
    /// </summary>
    /// <typeparam name="T1">The type of the inputs.</typeparam>
    /// <typeparam name="T2">The type of the outputs.</typeparam>
    internal struct FunctionRelation<T1, T2> : IFunctionRelation<T1, T2>
        where T2 : IEquatable<T2>
    {
        private readonly Func<T1, T2> f;

        /// <summary>
        /// Creates a new relation.
        /// </summary>
        /// <param name="f">The underlying function.</param>
        public FunctionRelation(Func<T1, T2> f)
        {
            this.f = f;
        }

        /// <inheritdoc />
        public bool Contains(T1 x, T2 y)
            => f(x).Equals(y);

        /// <inheritdoc />
        public Maybe<T2> MaybeResult(T1 x) => Maybe.Just(Result(x));

        /// <inheritdoc />
        public T2 Result(T1 x) => f(x);
    }

    /// <summary>
    /// A value-level, finite one-to-one relation (a partial bijection).
    /// </summary>
    /// <typeparam name="T1">The type of the inputs.</typeparam>
    /// <typeparam name="T2">The type of the outputs.</typeparam>
    internal struct OneToOneRelation<T1, T2>
        : IOneToOneRelation<T1, T2>
        , IExtensionalBinaryRelation<T1, T2>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        private readonly IDictionary<T1, T2> toDict;
        private readonly IDictionary<T2, T1> fromDict;

        /// <summary>
        /// Creates a new relation.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        /// <exception cref="KeyAlreadyPresentException">A left- or right-key occurred twice.</exception>
        public OneToOneRelation(IEnumerable<KeyValuePair<T1, T2>> pairs)
        {
            toDict = new Dictionary<T1, T2>();
            fromDict = new Dictionary<T2, T1>();

            foreach (var (k,v) in pairs ?? Array.Empty<KeyValuePair<T1, T2>>())
            {
                if (toDict.ContainsKey(k))
                    throw new KeyAlreadyPresentException($"The left-key {k} occurred twice in the list of pairs.");

                toDict[k] = v;

                if (fromDict.ContainsKey(v))
                    throw new KeyAlreadyPresentException($"The right-key {k} occurred twice in the list of pairs.");

                fromDict[v] = k;
            }
        }

        /// <inheritdoc />
        public bool Contains(T1 x, T2 y)
            => toDict.TryGetValue(x, out var xRes) && xRes.Equals(y);

        /// <inheritdoc />
        public IEnumerator<(T1, T2)> GetEnumerator()
            => toDict.Select(kv => (kv.Key, kv.Value)).GetEnumerator();

        /// <inheritdoc />
        public Maybe<T1> GetMaybeLeft(T2 x)
            => Maybe.JustIf(fromDict.TryGetValue(x, out var xRes), () => xRes);

        /// <inheritdoc />
        public Maybe<T2> GetMaybeRight(T1 x)
            => Maybe.JustIf(toDict.TryGetValue(x, out var xRes), () => xRes);

        /// <inheritdoc />
        public Maybe<T2> MaybeResult(T1 x) => GetMaybeRight(x);

        public IEnumerable<(T1, T2)> Elements => toDict.Select(kv => (kv.Key, kv.Value));
    }

    /// <summary>
    /// A value-level (total) bijective relation.
    /// </summary>
    /// <typeparam name="T1">The type of the inputs.</typeparam>
    /// <typeparam name="T2">The type of the outputs.</typeparam>
    internal struct BijectiveRelation<T1, T2> : IBijectiveRelation<T1, T2>
        where T2 : IEquatable<T2>
    {
        private readonly Func<T1, T2> to;
        private readonly Func<T2, T1> from;

        /// <summary>
        /// Creates a new relation out of two functions.
        /// </summary>
        /// <param name="to">The function which converts from the codomain to the domain.</param>
        /// <param name="from">The function which converts from the domain to the codomain</param>
        public BijectiveRelation(Func<T1, T2> to, Func<T2, T1> from)
        {
            this.to = to;
            this.from = from;
        }

        /// <inheritdoc />
        public bool Contains(T1 x, T2 y) => to(x).Equals(y);

        /// <inheritdoc />
        public T1 GetLeft(T2 x) => from(x);

        /// <inheritdoc />
        public T2 GetRight(T1 x) => to(x);

        /// <inheritdoc />
        public Maybe<T2> MaybeResult(T1 x) => Maybe.Just(to(x));

        /// <inheritdoc />
        public Maybe<T1> GetMaybeLeft(T2 x) => Maybe.Just(from(x));

        /// <inheritdoc />
        public Maybe<T2> GetMaybeRight(T1 x) => Maybe.Just(to(x));
    }

    /// <summary>
    /// A value-level dictionary-relation.
    /// </summary>
    /// <typeparam name="T1">The type of the keys.</typeparam>
    /// <typeparam name="T2">The type of the values.</typeparam>
    internal struct DictionaryRelation<T1, T2> : IDictionaryRelation<T1, T2>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        private readonly IDictionary<T1, T2> dict;

        public DictionaryRelation(IDictionary<T1, T2> dict)
        {
            this.dict = dict.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <inheritdoc />
        public T2 this[T1 key] => dict[key];

        /// <inheritdoc />
        public IEnumerable<T1> Keys => dict.Keys;

        /// <inheritdoc />
        public IEnumerable<T2> Values => dict.Values;

        /// <inheritdoc />
        public int Count => dict.Count;

        /// <inheritdoc />
        public bool Contains(T1 x, T2 y)
            => dict.TryGetValue(x, out var value) && value.Equals(y);

        /// <inheritdoc />
        public bool ContainsKey(T1 key)
            => dict.ContainsKey(key);

        /// <inheritdoc />
        public Maybe<T2> MaybeResult(T1 x)
            => Maybe.JustIf(dict.TryGetValue(x, out var value), () => value);

        /// <inheritdoc />
        public bool TryGetValue(T1 key, out T2 value)
            => dict.TryGetValue(key, out value);

        /// <inheritdoc />
        IEnumerator<KeyValuePair<T1, T2>> IEnumerable<KeyValuePair<T1, T2>>.GetEnumerator()
            => dict.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => dict.GetEnumerator();

        public bool Equals([AllowNull] IDictionaryRelation<T1, T2> other)
        {
            if (other is null || dict is null)
                return false;

            foreach (var (k,v) in dict)
            {
                if (!other.TryGetValue(k, out var vOther) || !v.Equals(vOther))
                    return false;
            }

            foreach (var (k, vOther) in other)
            {
                if (!dict.TryGetValue(k, out var v) || !v.Equals(vOther))
                    return false;
            }

            return true;
        }

        /// <inheritdoc />
        public IEnumerable<(T1, T2)> Elements
            => dict.Select(kv => (kv.Key, kv.Value));
    }

    /// <summary>
    /// Extension methods for <see cref="IPartialOrder{T}"/>s.
    /// </summary>
    public static class PartialOrder
    {
        /// <summary>
        /// Creates a new partial order from a partial "less than or equal to"-function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="leqPartial">The partial "less than or equal to"-function.</param>
        public static IPartialOrder<T> Make<T>(Func<T, T, Maybe<bool>> leqPartial)
            => new PartialOrder<T>(leqPartial);

        /// <summary>
        /// Returns true iff <paramref name="x"/> is smaller than or equal to <paramref name="y"/> in the partial order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The partial  order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Leq<T>(this IPartialOrder<T> order, T x, T y)
            => order.Contains(x, y);

        /// <summary>
        /// Returns true iff <paramref name="x"/> is smaller than or equal to <paramref name="y"/> in the partial order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Leq<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : IPartialOrder<T>
            => order.Order.Contains(x, y);

        /// <summary>
        /// Creates a partial order and an equivalence relation from an <see cref="IEquatable{T}"/> type.
        /// The resultant partial order only captures the notion of equality, not "less than", so the following might occur:
        /// <code>
        ///     X.CompareTo(Y) &lt; 0 != r.Leq(X,Y)
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        public static IPartialOrder<T> FromEquatable<T>()
            where T : IEquatable<T>
            => new PartialOrder<T>((x, y) => Maybe.JustIf(x.Equals(y), () => true));
    }

    /// <summary>
    /// Extension methods for <see cref="ITotalOrder{T}"/>s.
    /// </summary>
    public static class TotalOrder
    {
        /// <summary>
        /// Creates a total order out of a comparison-function.
        /// </summary>
        /// <typeparam name="T">The type of elements to compare.</typeparam>
        /// <param name="comparison">The comparison-function.</param>
        public static ITotalOrder<T> Make<T>(Func<T, T, short> comparison)
            => new TotalOrder<T>(comparison);

        /// <summary>
        /// Creates a total order out of a comparison-function.
        /// </summary>
        /// <typeparam name="T">The type of elements to compare.</typeparam>
        /// <param name="comparison">The comparison-function.</param>
        public static ITotalOrder<T> Make<T>(Func<T, T, int> comparison)
            => new TotalOrder<T>((x,y) => (short)comparison(x,y));

        /// <summary>
        /// Creates a total order out of a "less than or equal to"-function.
        /// </summary>
        /// <typeparam name="T">The type of elements to compare.</typeparam>
        /// <param name="leq">The "less then or equal to"-function.</param>
        public static ITotalOrder<T> Make<T>(Func<T, T, bool> leq)
            => new TotalOrder<T>(leq);

        /// <summary>
        /// Returns true iff <paramref name="x"/> is strictly smaller than <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Le<T>(this ITotalOrder<T> order, T x, T y)
            => order.Compare(x, y) < 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is strictly smaller than <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Le<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : ITotalOrder<T>
            => order.Order.Compare(x, y) < 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Eq<T>(this ITotalOrder<T> order, T x, T y)
            => order.Compare(x, y) == 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Eq<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : ITotalOrder<T>
            => order.Order.Compare(x, y) == 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is strictly not equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Neq<T>(this ITotalOrder<T> order, T x, T y)
            => order.Compare(x, y) != 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is not equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Neq<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : ITotalOrder<T>
            => order.Order.Compare(x, y) != 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is greater than or equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Geq<T>(this ITotalOrder<T> order, T x, T y)
            => order.Compare(x, y) >= 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is greater than or equal to <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Geq<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : ITotalOrder<T>
            => order.Order.Compare(x, y) >= 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is strictly greater than <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Ge<T>(this ITotalOrder<T> order, T x, T y)
            => order.Compare(x, y) > 0;

        /// <summary>
        /// Returns true iff <paramref name="x"/> is stricle greater than <paramref name="y"/> in the total order <paramref name="order"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        /// <typeparam name="TOrder">The type of the order.</typeparam>
        /// <param name="order">The structure which has a total order.</param>
        /// <param name="x">The first element to compare.</param>
        /// <param name="y">The second element to compare.</param>
        public static bool Ge<T, TOrder>(this IContainsOrder<TOrder> order, T x, T y)
            where TOrder : ITotalOrder<T>
            => order.Order.Compare(x, y) > 0;

        /// <summary>
        /// Creates a total order from an <see cref="IComparable{T}"/> type.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the relation.</typeparam>
        public static ITotalOrder<T> FromComparable<T>()
            where T : IComparable<T>
            => new TotalOrder<T>((x, y) => (short)x.CompareTo(y));

        /// <summary>
        /// Adds positive infinity, modeled via <see cref="Maybe.Nothing{T}"/> to a total order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the relation.</typeparam>
        /// <param name="order">The total order.</param>
        public static ITotalOrder<Maybe<T>> LiftTotalOrderWithInfinity<T>(this ITotalOrder<T> order)
            => new TotalOrder<Maybe<T>>((x, y) =>
            {
                var comp = x.HasValue.CompareTo(y.HasValue);

                //Exactly the first one is infinite -> the first one is greater
                if (comp < 0)
                    return false;
                //Exactly the second is infinite -> the second one is greater
                else if (comp > 0)
                    return true;
                //Neither have a value <-> both are infinite <-> true
                else if (!x.HasValue)
                    return true;
                //Both have a value <-> neither are infinite <-> use underlying order
                else
                    return order.Leq(x.Value(), y.Value());
            });
    }

    /// <summary>
    /// Extension methods for relations.
    /// </summary>
    public static class Relations
    {
        /// <summary>
        /// Creates a new intensional binary relation with no further guaranteed properties.
        /// </summary>
        /// <typeparam name="T1">The type of the first elements.</typeparam>
        /// <typeparam name="T2">The type of the second elements.</typeparam>
        /// <param name="contains">The contains-predicate.</param>
        public static IBinaryRelation<T1, T2> Make<T1, T2>(Func<T1, T2, bool> contains)
            => new BinaryRelation<T1, T2>(contains);

        /// <summary>
        /// Returns a total function as an <see cref="IFunctionRelation{T1, T2}"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the input.</typeparam>
        /// <typeparam name="T2">The type of the output.</typeparam>
        /// <param name="f">The function to transform into a relation.</param>
        public static IFunctionRelation<T1, T2> AsRelation<T1, T2>(Func<T1, T2> f)
            where T2 : IEquatable<T2>
            => new FunctionRelation<T1, T2>(f);

        /// <summary>
        /// Returns an <see cref="IDictionaryRelation{T1, T2}"/> from an <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the keys.</typeparam>
        /// <typeparam name="T2">The type of the values.</typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static IDictionaryRelation<T1, T2> AsRelation<T1, T2>(IDictionary<T1, T2> dict)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            => new DictionaryRelation<T1, T2>(dict);

        /// <summary>
        /// Returns an <see cref="IFunctionRelation{T1, T2}"/> as a total function.
        /// </summary>
        /// <typeparam name="T1">The type of the input.</typeparam>
        /// <typeparam name="T2">The type of the output.</typeparam>
        /// <param name="r">The relation to transform into a function.</param>
        public static Func<T1, T2> FromRelation<T1, T2>(IFunctionRelation<T1, T2> r)
        {
            return (x) => r.Result(x);
        }

        /// <summary>
        /// Creates a one-to-one relation (a bijective relation that is not total) from a list of key-value-pairs.
        /// </summary>
        /// <typeparam name="T1">The type of the left-keys.</typeparam>
        /// <typeparam name="T2">The type of the right-keys.</typeparam>
        /// <param name="pairs">The pairs of keys.</param>
        /// <exception cref="KeyAlreadyPresentException">A left- or right-key occurred twice.</exception>
        public static IOneToOneRelation<T1, T2> AsBijective<T1, T2>(IEnumerable<(T1, T2)> pairs)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            => new OneToOneRelation<T1, T2>(pairs.Select(p => KeyValuePair.Create(p.Item1, p.Item2)));

        /// <summary>
        /// Creates a one-to-one relation (a bijective relation that is not total) from a list of key-value-pairs.
        /// </summary>
        /// <typeparam name="T1">The type of the left-keys.</typeparam>
        /// <typeparam name="T2">The type of the right-keys.</typeparam>
        /// <param name="pairs">The pairs of keys.</param>
        /// <exception cref="KeyAlreadyPresentException">A left- or right-key occurred twice.</exception>
        public static IOneToOneRelation<T1, T2> AsBijective<T1, T2>(IEnumerable<KeyValuePair<T1, T2>> pairs)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            => new OneToOneRelation<T1, T2>(pairs);

        /// <summary>
        /// Creates a bijection from two functions. It is the responsibility of the caller to ensure that the two function actually induce a bijection.
        /// </summary>
        /// <typeparam name="T1">The type of the left-keys.</typeparam>
        /// <typeparam name="T2">The type of the right-keys.</typeparam>
        /// <param name="to">The function which converts from the codomain to the domain.</param>
        /// <param name="from">The function which converts from the domain to the codomain</param>
        public static IBijectiveRelation<T1, T2> AsBijective<T1, T2>(Func<T1, T2> to, Func<T2, T1> from)
            where T2 : IEquatable<T2>
            => new BijectiveRelation<T1, T2>(to, from);
    }
}
