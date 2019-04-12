using Nordril.Functional.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Performs two nested aggregations: the outer one traverses <paramref name="xs"/>, the inner one traverses the result of <paramref name="selector"/> for each element of <paramref name="xs"/>. Thus, this function is the equivalent of running two nested loops, with <paramref name="selector"/> generating the elements for the inner loop. The accumulator is forwarded across each step.
        /// </summary>
        /// <typeparam name="T1">The type of element in the outer sequence.</typeparam>
        /// <typeparam name="T2">The type of element in the inner sequence.</typeparam>
        /// <typeparam name="TAcc">The type of the accumulator/result.</typeparam>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="acc">The initial accumulator.</param>
        /// <param name="selector">The selector which generates the inner sequence for each element of <paramref name="xs"/>.</param>
        /// <param name="f">The combining function for the accumulator and the current element of the outer sequence, as well as the current element of th e inner list.</param>
        /// <returns></returns>
        public static TAcc Aggregate2<T1, T2, TAcc>(
            this IEnumerable<T1> xs,
            TAcc acc,
            Func<T1, IEnumerable<T2>> selector,
            Func<TAcc, T1, T2, TAcc> f)
            => xs.Aggregate(acc, (a, x) => selector(x).Aggregate(a, (b, y) => f(b, x, y)));

        /// <summary>
        /// Performs a right-fold on a sequence. For associative combining functions <paramref name="f"/>, <see cref="AggregateRight{T, TResult}(IEnumerable{T}, Func{T, TResult, TResult}, TResult)"/> returns the same result as <see cref="Enumerable.Aggregate{TSource, TAccumulate}(IEnumerable{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate})"/>, though it will use heap memory linear in the length of <paramref name="xs"/>. The definition is:
        /// <code>
        ///     {x1,x2,...,xn}.AggregateRight(acc, f) = f(x1, f(x2, ... f(xn, acc)...)
        ///     for instance
        ///     {1, 2, 3, 4}.AggregateRight(0, (x, acc) =&gt; x + acc = 1 + (2 + (3 + (4 + 0)))
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result/accumulator.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="acc">The accumulator.</param>
        /// <param name="f">The combining function.</param>
        public static TResult AggregateRight<T, TResult>(this IEnumerable<T> xs, Func<T, TResult, TResult> f, TResult acc)
        {
            var stack = new Stack<T>();

            foreach (var x in xs)
                stack.Push(x);

            foreach (var x in stack)
                acc = f(x, acc);

            return acc;
        }

        /// <summary>
        /// Returns true iff all elements of <paramref name="xs"/> equal true.
        /// </summary>
        /// <param name="xs">The list to check.</param>
        public static bool All(this IEnumerable<bool> xs) => xs.All(x => x);

        /// <summary>
        /// Returns true iff at least one element of <paramref name="xs"/> equals true.
        /// </summary>
        /// <param name="xs">The list to check.</param>
        public static bool AnyTrue(this IEnumerable<bool> xs)
        {
            foreach (var x in xs)
                if (x)
                    return true;

            return false;
        }

        /// <summary>
        /// Concatenates a sequence of sequences.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence whose elements to concatenate.</param>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> xs)
        {
            foreach (var ys in xs)
                foreach (var y in ys)
                    yield return y;
        }

        /// <summary>
        /// Efficiently concatenates the elements of a sequence of strings into a single strings. An alias for <see cref="string.Join(string, IEnumerable{string})"/>.
        /// </summary>
        /// <param name="xs">The sequence whose elements to concatenate.</param>
        /// <param name="separator">The separator to put between each two elements.</param>
        /// <param name="prefix">The optional prefix to apply to the resultant string.</param>
        /// <param name="postfix">The optional postfix to apply to the resultant string.</param>
        public static string ConcatStrings(this IEnumerable<string> xs, string separator, string prefix = null, string postfix = null)
            => (prefix ?? "") + string.Join(separator, xs) + (postfix ?? "");

        /// <summary>
        /// Returns true iff the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The list to check.</param>
        public static bool Empty<T>(this IEnumerable<T> xs) => !xs.Any();

        /// <summary>
        /// Returns the first element of the sequence if it exists, or <see cref="Maybe.Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The list to check.</param>
        public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> xs)
        {
            using (var x = xs.GetEnumerator())
            {
                return Maybe.JustIf(x.MoveNext(), () => x.Current);
            }
        }

        /// <summary>
        /// Returns the first element of the sequence which fulfills a predicate if it exists, or <see cref="Maybe.Nothing{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The list to check.</param>
        /// <param name="predicate">The predicate the element has to fulfill.</param>
        public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> xs, Func<T, bool> predicate)
        {
            foreach (var x in xs)
            {
                if (predicate(x))
                    return Maybe.Just(x);
            }

            return Maybe.Nothing<T>();
        }

        /// <summary>
        /// Performs an action <paramref name="action"/> on each element of a sequence.
        /// </summary>
        /// <param name="xs">The list to traverse.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach(this IEnumerable xs, Action<object> action)
        {
            foreach (var x in xs)
                action(x);
        }

        /// <summary>
        /// Performs an action <paramref name="action"/> on each element of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The list to traverse.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T>(this IEnumerable<T> xs, Action<T> action)
        {
            foreach (var x in xs)
                action(x);
        }

        /// <summary>
        /// Performs an action <paramref name="action"/> on each element of two sequences which are traversed in parallel, until the end of the shorter one..
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first sequencde.</typeparam>
        /// <typeparam name="T2">The type of elements in the second sequencde.</typeparam>
        /// <param name="xs">The first sequence to traverse.</param>
        /// <param name="ys">The second sequence to traverse.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T1, T2>(this IEnumerable<T1> xs, IEnumerable<T2> ys, Action<T1, T2> action)
        {
            using (var x = xs.GetEnumerator())
            using (var y = ys.GetEnumerator())
            {
                while (x.MoveNext() && y.MoveNext())
                    action(x.Current, y.Current);
            }
        }

        /// <summary>
        /// Performs an action <paramref name="action"/> on each element of three sequences which are traversed in parallel, until the end of the shorter one..
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first sequencde.</typeparam>
        /// <typeparam name="T2">The type of elements in the second sequencde.</typeparam>
        /// <typeparam name="T3">The type of elements in the third sequence.</typeparam>
        /// <param name="xs">The first sequence to traverse.</param>
        /// <param name="ys">The second secquence to traverse.</param>
        /// <param name="zs">The third sequence to traverse.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T1, T2, T3>(
            this IEnumerable<T1> xs,
            IEnumerable<T2> ys,
            IEnumerable<T3> zs,
            Action<T1, T2, T3> action)
        {
            using (var x = xs.GetEnumerator())
            using (var y = ys.GetEnumerator())
            using (var z = zs.GetEnumerator())
            {
                while (x.MoveNext() && y.MoveNext() && z.MoveNext())
                    action(x.Current, y.Current, z.Current);
            }
        }

        /// <summary>
        /// Performs an action <paramref name="action"/> on each element of four sequences which are traversed in parallel, until the end of the shorter one..
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first sequencde.</typeparam>
        /// <typeparam name="T2">The type of elements in the second sequencde.</typeparam>
        /// <typeparam name="T3">The type of elements in the third sequence.</typeparam>
        /// <typeparam name="T4">The type of elements in the fourth sequence.</typeparam>
        /// <param name="xs">The first sequence to traverse.</param>
        /// <param name="ys">The second secquence to traverse.</param>
        /// <param name="us">The third sequence to traverse.</param>
        /// <param name="zs">The fourth sequence to traverse.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T1, T2, T3, T4>(
            this IEnumerable<T1> xs,
            IEnumerable<T2> ys,
            IEnumerable<T3> zs,
            IEnumerable<T4> us,
            Action<T1, T2, T3, T4> action)
        {
            using (var x = xs.GetEnumerator())
            using (var y = ys.GetEnumerator())
            using (var z = zs.GetEnumerator())
            using (var u = us.GetEnumerator())
            {
                while (x.MoveNext() && y.MoveNext() && z.MoveNext() && u.MoveNext())
                    action(x.Current, y.Current, z.Current, u.Current);
            }
        }

        /// <summary>
        /// Returns the index of the first occurrence of an element <paramref name="elem"/> in a sequence, or null.
        /// The search is linear and the runtime O(n).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to check.</param>
        /// <param name="elem">The element to search for.</param>
        public static int? IndexOf<T>(this IEnumerable<T> xs, T elem) where T : IEquatable<T>
        {
            int i = 0;
            using (var x = xs.GetEnumerator())
            {
                while (x.MoveNext())
                {
                    if (x.Current.Equals(elem))
                        return i;
                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Intersperses a sequence <paramref name="separator"/> between each two elements of a sequence <paramref name="xs"/>.
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="xs">The sequence to intersperse.</param>
        /// <param name="separator">The separator to intersperse between each two elements of <paramref name="xs"/>.</param>
        public static IEnumerable<T> Intercalate<T>(this IEnumerable<T> xs, IEnumerable<T> separator)
        {
            var first = true;

            foreach (var x in xs)
            {
                if (!first)
                    foreach (var s in separator)
                        yield return s;

                yield return x;

                if (first)
                    first = false;
            }
        }

        /// <summary>
        /// Intersperses an element <paramref name="separator"/> between each two elements of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of element in the sequence.</typeparam>
        /// <param name="xs">The sequence to intersperse.</param>
        /// <param name="separator">The separator to intersperse between each two elements of <paramref name="xs"/>.</param>
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> xs, T separator)
        {
            var first = true;

            foreach (var x in xs)
            {
                if (!first)
                    yield return separator;

                yield return x;

                if (first)
                    first = false;
            }
        }

        /// <summary>
        /// Iterates through a sequence and merges any two adjacent elements for which the function <paramref name="mergeIf"/> returns a value. Multiple adjacent elements are merges as well, with the return value of <paramref name="mergeIf"/> functioning as an accumulator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="xs">The list to merge.</param>
        /// <param name="mergeIf">The merging function. If two elements shouldn't be merged, it should return no value.</param>
        public static IEnumerable<T> MergeAdjacent<T>(this IEnumerable<T> xs, Func<T, T, Maybe<T>> mergeIf)
        {
            var first = true;
            var isMerging = false;
            var previous = default(T);
            var merging = default(T);

            foreach (var x in xs)
            {
                if (!first)
                {
                    var newMerging = mergeIf(isMerging ? merging : previous, x);
                    var shouldMerge = newMerging.HasValue;

                    if (!isMerging && !shouldMerge)
                        yield return previous;
                    else if (!isMerging && shouldMerge)
                    {
                        isMerging = true;
                        merging = newMerging.Value();
                    }
                    else if (isMerging && !shouldMerge)
                    {
                        yield return merging;
                        isMerging = false;
                    }
                    else if (isMerging && shouldMerge)
                        merging = newMerging.Value();
                }
                else
                    first = false;

                previous = x;
            }

            if (!first)
                yield return isMerging ? merging : previous;
        }

        /// <summary>
        /// Partitions a list into two lists based on a predicate <paramref name="putInFirst"/>. All elements which fulfill <paramref name="putInFirst"/> are put into the first list, and all others are put into the second.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="xs">The source list to split.</param>
        /// <param name="putInFirst">The splitting predicate.</param>
        public static (IEnumerable<T>, IEnumerable<T>) Partition<T>(this IEnumerable<T> xs, Func<T, bool> putInFirst)
            => xs.Aggregate((new List<T>(), new List<T>()), (acc, x) =>
            {
                if (putInFirst(x))
                    acc.Item1.Add(x);
                else
                    acc.Item2.Add(x);
                return acc;
            });

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static int Product(this IEnumerable<int> xs) => xs.Aggregate(1, (acc, x) => acc * x);

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static long Product(this IEnumerable<long> xs) => xs.Aggregate(1L, (acc, x) => acc * x);

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static float Product(this IEnumerable<float> xs) => xs.Aggregate(1f, (acc, x) => acc * x);

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static double Product(this IEnumerable<double> xs) => xs.Aggregate(1d, (acc, x) => acc * x);

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static decimal Product(this IEnumerable<decimal> xs) => xs.Aggregate(1m, (acc, x) => acc * x);

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="selector">The value selector.</param>
        public static int Product<T>(this IEnumerable<T> xs, Func<T, int> selector) => xs.Aggregate(1, (acc, x) => acc * selector(x));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="selector">The value selector.</param>
        public static long Product<T>(this IEnumerable<T> xs, Func<T, long> selector) => xs.Aggregate(1L, (acc, x) => acc * selector(x));


        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="selector">The value selector.</param>
        public static float Product<T>(this IEnumerable<T> xs, Func<T, float> selector) => xs.Aggregate(1f, (acc, x) => acc * selector(x));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="selector">The value selector.</param>
        public static double Product<T>(this IEnumerable<T> xs, Func<T, double> selector) => xs.Aggregate(1d, (acc, x) => acc * selector(x));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to aggregate.</param>
        /// <param name="selector">The value selector.</param>
        public static decimal Product<T>(this IEnumerable<T> xs, Func<T, decimal> selector) => xs.Aggregate(1m, (acc, x) => acc * selector(x));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty. null-values are ignored.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static int Product(this IEnumerable<int?> xs) => xs.Aggregate(1, (acc, x) => acc * (x ?? 1));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty. null-values are ignored.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static long Product(this IEnumerable<long?> xs) => xs.Aggregate(1L, (acc, x) => acc * (x ?? 1L));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty. null-values are ignored.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static float Product(this IEnumerable<float?> xs) => xs.Aggregate(1f, (acc, x) => acc * (x ?? 1f));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty. null-values are ignored.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static double Product(this IEnumerable<double?> xs) => xs.Aggregate(1d, (acc, x) => acc * (x ?? 1d));

        /// <summary>
        /// Calculates the product of the elements of a sequence, returning 1 if the sequence is empty. null-values are ignored.
        /// </summary>
        /// <param name="xs">The sequence to aggregate.</param>
        public static decimal Product(this IEnumerable<decimal?> xs) => xs.Aggregate(1m, (acc, x) => acc * (x ?? 1m));

        /// <summary>
        /// Applies a function at a specific, 0-based index at a sequence and returns the original elements of the sequence at all others. This method takes O(n) time.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The list to search.</param>
        /// <param name="point">The point at which to apply the function.</param>
        /// <param name="f">The function to apply at index <paramref name="point"/>.</param>
        public static IEnumerable<T> SelectAt<T>(this IEnumerable<T> xs, int point, Func<T, T> f)
            => xs.Select((x, i) => i == point ? f(x) : x);

        /// <summary>
        /// Projects each element of a dictionary of key-value-pairs into a new form.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <typeparam name="TResult">The type of the results.</typeparam>
        /// <param name="xs">The sequence to traverse.</param>
        /// <param name="f">The function to apply to each element.</param>
        public static IEnumerable<TResult> SelectKeyValue<TKey, TValue, TResult>(this IEnumerable<KeyValuePair<TKey, TValue>> xs, Func<TKey, TValue, TResult> f)
            => xs.Select(kv => f(kv.Key, kv.Value));

        /// <summary>
        /// Applies a function which produces a <see cref="Maybe{T}"/> to each element of a sequence and returns a sequence of those resultant elements for which <see cref="Maybe{T}.HasValue"/> is true. This is useful for appliying a function which may not have a results and filtering it for results in one go.
        /// <br />
        /// Semantically, the following holds for all xs:
        /// <code>
        ///    xs.SelectMaybe(f) == xs.Select(f).Where(x =&lt; x.HasValue).Select(x =&gt; x.Value());
        /// </code>
        /// </summary>
        /// <example>
        /// We compute the square roots of all elements in the sequence and return only those which were computed from positive numbers:
        /// <code>
        /// var xs = new [] {9, -10, -5, 25, 16, -144};
        /// 
        /// var ys = xs.SelectMaybe(x =&gt; Maybe.JustIf(x &gt;= 0, () =&gt; Math.Sqrt(x)));
        /// //ys = [3, 5, 4]
        /// </code>
        /// </example>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements ot generate.</typeparam>
        /// <param name="xs">The sequence to whose elements to apply the function.</param>
        /// <param name="f">The function to apply to each element of the sequence.</param>
        public static IEnumerable<TResult> SelectMaybe<T, TResult>(this IEnumerable<T> xs, Func<T, Maybe<TResult>>f)
        {
            using (var x = xs.GetEnumerator())
            {
                while (x.MoveNext())
                {
                    var y = f(x.Current);

                    if (y.HasValue)
                        yield return y.Value();
                }
            }
        }

        /// <summary>
        /// Creates an <see cref="IFuncList{T}"/> out of a sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to turn into a list.</param>
        public static IFuncList<T> ToFuncList<T>(this IEnumerable<T> xs) => new FuncList<T>(xs);

        /// <summary>
        /// Generates a (potentially infinite) sequence from a seed value <paramref name="seed"/> and a function to generate the next seed and element <paramref name="next"/>.
        /// </summary>
        /// <typeparam name="TSeed">The type of the seed value.</typeparam>
        /// <typeparam name="TResult">The type of elements to generate.</typeparam>
        /// <param name="seed">The initial seed value.</param>
        /// <param name="next">The function to generate the next seed, and the next element to yield. If the return value is <see cref="Maybe.Nothing{T}"/>, the sequence-generation stops.</param>
        public static IEnumerable<TResult> Unfold<TSeed, TResult>(this TSeed seed, Func<TSeed, Maybe<(TSeed, TResult)>> next)
        {
            while (true)
            {
                var x = next(seed);
                if (x.HasValue)
                {
                    var (nextSeed, nextValue) = x.Value();

                    yield return nextValue;
                    seed = nextSeed;
                }
                else
                    yield break;
            }
        }

        /// <summary>
        /// Returns just the unique elements of the sequence. Two elements <c>x</c> and <c>y</c> are regarded as equal iff <c>x.Equals(y)</c> is true. If two or more elements are equal, only the first element is returned.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="xs">The sequence to filter.</param>
        /// <returns>Those elements whose keys are the first occurrences in the sequence.</returns>
        public static IEnumerable<T> Unique<T>(
            this IEnumerable<T> xs)
            where T : IEquatable<T>
            => Unique(xs, x => x, (x, y) => x.Equals(y), x => x.GetHashCode());

        /// <summary>
        /// Returns just the unique elements of the sequence. Two elements <c>x</c> and <c>y</c> are regarded as equal iff <c>keySelector(x).Equals(keySelector(y))</c> is true. If two or more elements are equal, only the first element is returned.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">The keys of the elements.</typeparam>
        /// <param name="xs">The sequence to filter.</param>
        /// <param name="keySelector">The keys selection function.</param>
        /// <returns>Those elements whose keys are the first occurrences in the sequence.</returns>
        public static IEnumerable<T> Unique<T, TKey>(
            this IEnumerable<T> xs,
            Func<T, TKey> keySelector)
            where TKey : IEquatable<TKey>
            => Unique(xs, keySelector, (x, y) => x.Equals(y), x => x.GetHashCode());

        /// <summary>
        /// Returns just the unique elements of the sequence. Two elements <c>x</c> and <c>y</c> are regarded as equal iff <c>comparer.Equal(keySelector(x), keySelector(y))</c> is true. If two or more elements are equal, only the first element is returned.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">The keys of the elements.</typeparam>
        /// <param name="xs">The sequence to filter.</param>
        /// <param name="keySelector">The keys selection function.</param>
        /// <param name="comparer">The equality comparer to use to determine element equality.</param>
        /// <param name="hashFunc">The hash-function. The following must hold: <c>comparer(x,y) => hashFunc(x) == hashFunc(y)</c>.</param>
        /// <returns>Those elements whose keys are the first occurrences in the sequence.</returns>
        public static IEnumerable<T> Unique<T, TKey>(
            this IEnumerable<T> xs,
            Func<T, TKey> keySelector,
            Func<TKey, TKey, bool> comparer,
            Func<TKey, int> hashFunc)
        {
            var index = new HashSet<TKey>(new FuncEqualityComparer<TKey>(comparer, hashFunc));

            foreach (var x in xs)
            {
                if (index.Add(keySelector(x)))
                    yield return x;
            }
        }

        /// <summary>
        /// The oppostive of <see cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>, in that it takes a list <paramref name="xs"/> and splits it into two lists of the same size via a function <paramref name="f"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source list.</typeparam>
        /// <typeparam name="T1">The type of elements in the first result list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second result list.</typeparam>
        /// <param name="xs">The source list to split.</param>
        /// <param name="f">The splitting function.</param>
        /// <returns>Two lists of the same size as <paramref name="xs"/>, with <paramref name="f"/> having been applied to the element of <paramref name="xs"/> in order.</returns>
        public static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T, T1, T2>(this IEnumerable<T> xs, Func<T, (T1, T2)> f)
            => xs.Aggregate((new List<T1>(), new List<T2>()), (acc, x) =>
            {
                var (first, second) = f(x);
                acc.Item1.Add(first);
                acc.Item2.Add(second);
                return acc;
            });

        /// <summary>
        /// The oppostive of <see cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>, in that it takes a list <paramref name="xs"/> and splits it into two lists of the same size.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first result list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second result list.</typeparam>
        /// <param name="xs">The source list to split.</param>
        /// <returns>Two lists of the same size as <paramref name="xs"/>, with the same element order.</returns>
        public static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1, T2)> xs)
            => xs.Aggregate((new List<T1>(), new List<T2>()), (acc, x) =>
            {
                acc.Item1.Add(x.Item1);
                acc.Item2.Add(x.Item2);
                return acc;
            });

        /// <summary>
        /// The oppostive of <see cref="Zip{T1, T2, T3, TResult}(IEnumerable{T1}, IEnumerable{T2}, IEnumerable{T3}, Func{T1, T2, T3, TResult})"/>, in that it takes a list <paramref name="xs"/> and splits it into three lists of the same size.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first result list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second result list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third result list.</typeparam>
        /// <param name="xs">The source list to split.</param>
        /// <returns>Three lists of the same size as <paramref name="xs"/>, with the same element order.</returns>
        public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) Unzip<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> xs)
            => xs.Aggregate((new List<T1>(), new List<T2>(), new List<T3>()), (acc, x) =>
            {
                acc.Item1.Add(x.Item1);
                acc.Item2.Add(x.Item2);
                acc.Item3.Add(x.Item3);
                return acc;
            });

        /// <summary>
        /// The oppostive of <see cref="Zip{T1, T2, T3, T4, TResult}(IEnumerable{T1}, IEnumerable{T2}, IEnumerable{T3}, IEnumerable{T4}, Func{T1, T2, T3, T4, TResult})"/>, in that it takes a list <paramref name="xs"/> and splits it into fource lists of the same size.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first result list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second result list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third result list.</typeparam>
        /// <typeparam name="T4">The type of elements in the fourth result list.</typeparam>
        /// <param name="xs">The source list to split.</param>
        /// <returns>Three lists of the same size as <paramref name="xs"/>, with the same element order.</returns>
        public static (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) Unzip<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3, T4)> xs)
            => xs.Aggregate((new List<T1>(), new List<T2>(), new List<T3>(), new List<T4>()), (acc, x) =>
            {
                acc.Item1.Add(x.Item1);
                acc.Item2.Add(x.Item2);
                acc.Item3.Add(x.Item3);
                acc.Item4.Add(x.Item4);
                return acc;
            });

        /// <summary>
        /// Zips two sequences together, creating a sequence of tuples.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second list.</typeparam>
        /// <param name="xs">The first list.</param>
        /// <param name="ys">The second list.</param>
        public static IEnumerable<(T1, T2)> Zip<T1, T2>(this IEnumerable<T1> xs, IEnumerable<T2> ys)
            => xs.Zip(ys, (x, y) => (x, y));

        /// <summary>
        /// Zips three sequences together, creating a sequence of tuples.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third list.</typeparam>
        /// <param name="xs">The first list.</param>
        /// <param name="ys">The second list.</param>
        /// <param name="zs">The third list.</param>
        public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>(this IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs)
            => xs.Zip(ys, zs, (x, y, z) => (x, y, z));

        /// <summary>
        /// Zips three sequences together, creating a sequence of tuples.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third list.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="xs">The first list.</param>
        /// <param name="ys">The second list.</param>
        /// <param name="zs">The third list.</param>
        /// <param name="f">The combining function.</param>
        public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs, Func<T1, T2, T3, TResult> f)
            => Zip3Iterator(xs, ys, zs, f);

        /// <summary>
        /// Zips four sequences together, creating a sequence of tuples.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third list.</typeparam>
        /// <typeparam name="T4">The type of elements in the fourth list.</typeparam>
        /// <param name="xs">The first list.</param>
        /// <param name="ys">The second list.</param>
        /// <param name="zs">The third list.</param>
        /// <param name="us">The fourth list.</param>
        public static IEnumerable<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(this IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs, IEnumerable<T4> us)
            => xs.Zip(ys, zs, us, (x, y, z, u) => (x, y, z, u));

        /// <summary>
        /// Zips four sequences together, creating a sequence of tuples.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the first list.</typeparam>
        /// <typeparam name="T2">The type of elements in the second list.</typeparam>
        /// <typeparam name="T3">The type of elements in the third list.</typeparam>
        /// <typeparam name="T4">The type of elements in the fourth list.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="xs">The first list.</param>
        /// <param name="ys">The second list.</param>
        /// <param name="zs">The third list.</param>
        /// <param name="us">The fourth list.</param>
        /// <param name="f">The combining function.</param>
        public static IEnumerable<TResult> Zip<T1, T2, T3, T4, TResult>(this IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs, IEnumerable<T4> us, Func<T1, T2, T3, T4, TResult> f)
            => Zip4Iterator(xs, ys, zs, us, f);

        /// <summary>
        /// Zips a sequence of sequences together, iterating through all sequences in parallel, until the end of the longest one, omitting those sequences whose end has been reached from the input of <paramref name="f"/>. For each i, the list of elements at index i is combined into one result via <paramref name="f"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="xs">The sequences to zip.</param>
        /// <param name="f">The combining function.</param>
        public static IEnumerable<TResult> Zip<T, TResult>(this IEnumerable<IEnumerable<T>> xs, Func<IList<T>, TResult> f)
        {
            var xsList = xs.ToList();
            var iterators = new List<IEnumerator<T>>();
            var stillRunning = Enumerable.Repeat(true, xsList.Count).ToList();
            var stillRunningCount = xsList.Count;
            var res = new List<TResult>();

            try
            {
                //Get an iterator for each sequence.
                foreach (var x in xsList)
                    iterators.Add(x.GetEnumerator());

                //While any sequence still has unvisited elements
                while (stillRunningCount > 0)
                {
                    //Try to retrieve an element from each of the still-running sequences
                    //and put those elements into a list, which is fed to f.
                    var nextList = new List<T>();

                    for (int i =0;i<iterators.Count;i++)
                    {
                        if (stillRunning[i])
                        {
                            if (iterators[i].MoveNext())
                                nextList.Add(iterators[i].Current);
                            else
                            {
                                iterators[i].Dispose();
                                stillRunning[i] = false;
                                stillRunningCount--;
                            }
                        }
                    }

                    //If this is 0, it means we finished all sequences without reading anything -> don't call f.
                    if (stillRunningCount > 0)
                        res.Add(f(nextList));
                }

                return res;
            } finally
            {
                for (int i=0;i<iterators.Count;i++)
                {
                    try { if (stillRunning[i]) { iterators[i].Dispose(); } } catch { }
                }
            }
        }

        /// <summary>
        /// Zips a sequence of sequences together, iterating through all sequences in parallel, until the end of the shortest one. For each i, the list of elements at index i is combined into one result list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequences.</typeparam>
        /// <param name="xs">The sequences to zip.</param>
        public static IEnumerable<IList<T>> Zip<T>(this IEnumerable<IEnumerable<T>> xs)
            => xs.Zip(ys => ys);

        /// <summary>
        /// Zips a list with a stream. A steam is an infinite series comprising an initial element <paramref name="start"/> and a "next element"-function <paramref name="next"/>.
        /// </summary>
        /// <typeparam name="T1">The type of elements in the list.</typeparam>
        /// <typeparam name="T2">The type of elements in the stream.</typeparam>
        /// <param name="xs">The list.</param>
        /// <param name="start">The initial element of the stream.</param>
        /// <param name="next">The "next element"-function.</param>
        /// <returns>The elements of the first list, together with the created elements of the stream.</returns>
        public static IEnumerable<(T1, T2)> ZipWithStream<T1, T2>(this IEnumerable<T1> xs, T2 start, Func<T2, T2> next)
        {
            using (var iter = xs.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    yield return (iter.Current, start);
                    start = next(start);
                }
            }
        }

        /// <summary>
        /// Creates a hash value for a collection of elements by hashing the contained elements
        /// and adding them to a seed a multiplier. The hash code is computed by the formula:
        /// <code>
        ///    collection.foldl(487, (acc, cur) => acc*31 + cur.GetHashCode())
        /// </code>
        /// Users should NOT rely on this implementation detail.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to hash.</param>
        public static int HashElements<T>(this IEnumerable<T> collection)
        {
            //From https://stackoverflow.com/a/30758270
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return collection.Aggregate(seed, (current, item) => (current * modifier) + item.GetHashCode());
            }
        }

        static IEnumerable<TResult> Zip3Iterator<T1, T2, T3, TResult>(IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs, Func<T1, T2, T3, TResult> f)
        {
            using (IEnumerator<T1> x = xs.GetEnumerator())
            using (IEnumerator<T2> y = ys.GetEnumerator())
            using (IEnumerator<T3> z = zs.GetEnumerator())
                while (x.MoveNext() && y.MoveNext() && z.MoveNext())
                    yield return f(x.Current, y.Current, z.Current);
        }

        static IEnumerable<TResult> Zip4Iterator<T1, T2, T3, T4, TResult>(IEnumerable<T1> xs, IEnumerable<T2> ys, IEnumerable<T3> zs, IEnumerable<T4> us, Func<T1, T2, T3, T4, TResult> f)
        {
            using (IEnumerator<T1> x = xs.GetEnumerator())
            using (IEnumerator<T2> y = ys.GetEnumerator())
            using (IEnumerator<T3> z = zs.GetEnumerator())
            using (IEnumerator<T4> u = us.GetEnumerator())
                while (x.MoveNext() && y.MoveNext() && z.MoveNext() && u.MoveNext())
                    yield return f(x.Current, y.Current, z.Current, u.Current);
        }
    }
}