using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A thread-safe, immutable list supporting cheap deconstruction/slicing/appending.
    /// </summary>
    /// <typeparam name="T">The type of the contents.</typeparam>
    public sealed class Lst<T>
        : IEnumerable<T>
        , IMonadPlus<T>
        , IAsyncMonad<T>
        , IFilterable<Lst<T>, T>
        , IKeyedFunctor<int, T>
        , IAlternative<T>
        , IEquatable<IEnumerable<T>>
        , ISliceable<Lst<T>>
        , IDisposable
    {
        private readonly ConcurrentList<T> list;
        private readonly int startIncl = 0;
        private readonly int endExcl = 0;
        private readonly IEqualityComparer<IEnumerable<T>> comparer;

        /// <summary>
        /// The handler for when this list is disposed, with the slice that the list had of the underlying list.
        /// </summary>
        /// <param name="startIncl">The start-index of this list's slice.</param>
        /// <param name="endExcl">The exclusive end-index of this list's slice.</param>
        internal delegate void LstDisposeHandler(int startIncl, int endExcl);

        /// <summary>
        /// Gets called when this list is disposed.
        /// </summary>
        internal event LstDisposeHandler OnDisposing;

        /// <summary>
        /// Creates a new, immutable list from a shallow copy of <paramref name="xs"/>, using the comparer <paramref name="comparer"/>.
        /// </summary>
        /// <param name="xs">The items to shallowly copy.</param>
        /// <param name="comparer">The comparer. If null, <see cref="object.Equals(object?, object?)"/> will be used for element-comparison.</param>
        public Lst(IEnumerable<T> xs, IEqualityComparer<T> comparer = null)
        {
            list = new ConcurrentList<T>(xs == null ? new List<T>() : xs.ToList());
            list.AddUser(this, 0, list.Count);
            endExcl = list.Count;
            this.comparer = comparer != null ? new ListEqualityComparer<T>((x, y) => comparer.Equals(x, y)) : new ListEqualityComparer<T>((x, y) => x.Equals(y));
        }

        private Lst(ConcurrentList<T> xs, int startIncl, int endExcl, IEqualityComparer<IEnumerable<T>> comparer)
        {
            list = xs;
            list.AddUser(this, startIncl, endExcl);
            this.startIncl = startIncl;
            this.endExcl = endExcl;
            this.comparer = comparer ?? new ListEqualityComparer<T>((x, y) => x.Equals(y));
        }

        /// <summary>
        /// Disposes the list and, perhaps, compacts the underlying list.
        /// </summary>
        public void Dispose()
        {
            OnDisposing?.Invoke(startIncl, endExcl);
        }

        /// <summary>
        /// Gets the first element of the list, if it exists.
        /// O(1).
        /// </summary>
        public Maybe<T> Head
            => Maybe.JustIf(startIncl < list.Count, () => list.Get(startIncl));

        /// <summary>
        /// Gets the list excluding its first element, if it has at least one element.
        /// O(1).
        /// </summary>
        public Maybe<Lst<T>> Tail
            => Maybe.JustIf(startIncl < list.Count, () => new Lst<T>(list, startIncl + 1, endExcl, comparer));

        /// <summary>
        /// Unsafely gets the first element of a list.
        /// O(1).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If the list if empty.</exception>
        public T HeadUnsafe
            => list.Get(startIncl);

        /// <summary>
        /// Unsafely gets the list excluding its first element, if it has at least one element.
        /// O(1).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If the list if empty.</exception>
        public Lst<T> TailUnsafe
            => startIncl < endExcl
            ? new Lst<T>(list, startIncl + 1, endExcl, comparer)
            : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Gets the number of elements in the list.
        /// O(1).
        /// </summary>
        public int Count => endExcl - startIncl;

        internal int UnderlyingListCount => list.Count;

        /// <summary>
        /// Gets the element at the index <paramref name="index"/>, if it exists.
        /// O(1).
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        public Maybe<T> ElementAt(int index) => Maybe.JustIf(index >= 0 && index < Count, () => list.Get(startIncl + index));

        /// <summary>
        /// Appends an element at the end of the list.
        /// O(n). Ω(1) if this list is not a slice/tail of a larger list.
        /// </summary>
        /// <param name="item">The item to append.</param>
        public Lst<T> Append(T item)
        {
            //We go to the end of the underlying list -> it's safe to append, since no other Lst sees any value past us.
            if (endExcl >= list.Count)
            {
                list.Add(item);
                return new Lst<T>(list, startIncl, endExcl + 1, comparer);
            }
            //We don't -> we have to copy our elements.
            else
            {
                var list = new List<T>(Count + 1);
                list.AddRange(LocalView());
                list.Add(item);
                return new Lst<T>(new ConcurrentList<T>(list), 0, list.Count, comparer);
            }
        }

        /// <summary>
        /// Appends a sequence of elements at the end of the list.
        /// O(n+m). Ω(m) where m is the number of elements to append if this list is not a slice/tail of a larger list.
        /// </summary>
        /// <param name="item">The elements to append.</param>
        public Lst<T> AppendRange(IEnumerable<T> item)
        {
            var itemList = item.ToList();
            //We go to the end of the underlying list -> it's safe to append, since no other Lst sees any value past us.
            if (endExcl >= list.Count)
            {
                list.AddRange(item);
                return new Lst<T>(list, startIncl, endExcl + itemList.Count, comparer);
            }
            //We don't -> we have to copy our elements.
            else
            {
                var list = new List<T>(Count + itemList.Count);
                list.AddRange(LocalView());
                list.AddRange(itemList);
                return new Lst<T>(new ConcurrentList<T>(list), 0, list.Count, comparer);
            }
        }

        /// <summary>
        /// Prepends an element to the beginning of the list.
        /// O(n).
        /// </summary>
        /// <param name="item">The element to prepend.</param>
        public Lst<T> Prepend(T item)
        {
            var list = new List<T>(Count + 1) { item };
            list.AddRange(LocalView());

            return new Lst<T>(new ConcurrentList<T>(list), 0, list.Count, comparer);
        }

        /// <summary>
        /// Prepends an element to the beginning of the list.
        /// O(n+m), where m is the number of elements to prepend.
        /// </summary>
        /// <param name="item">The element to prepend.</param>
        public Lst<T> PrependRange(IEnumerable<T> item)
        {
            var itemCount = item.Count();
            var list = new List<T>(Count + itemCount);
            list.AddRange(item);
            list.AddRange(LocalView());

            return new Lst<T>(new ConcurrentList<T>(list), 0, list.Count, comparer);
        }

        /// <summary>
        /// Gets the list starting with the index <paramref name="index"/>.
        /// O(1).
        /// </summary>
        /// <param name="index">The index of the first element to get.</param>
        /// <exception cref="IndexOutOfRangeException">If <paramref name="index"/> lies outside of the list.</exception>
        public Lst<T> Slice(int index)
            => Slice(index, endExcl - startIncl);

        /// <summary>
        /// Gets the list starting with the index <paramref name="index"/>, containing <paramref name="count"/> elements.
        /// O(1).
        /// </summary>
        /// <param name="index">The index of the first element to get.</param>
        /// <param name="count">The number of elements, starting with the element at index <paramref name="index"/>, to include.</param>
        /// <exception cref="IndexOutOfRangeException">If <paramref name="index"/> lies outside of the list or if the list does not have <paramref name="count"/> elements started from <paramref name="index"/>.</exception>
        public Lst<T> Slice(int index, int count)
        {
            if (0 > startIncl || index + count > Count)
                throw new IndexOutOfRangeException();

            return new Lst<T>(list, startIncl + index, startIncl + index + count, comparer);
        }

        private Lst<T> LocalView() => Slice(0, Count);

        /// <inheritdoc />
        public IMonadZero<T> Mzero() => new Lst<T>(new ConcurrentList<T>(new List<T>()), 0, 0, comparer);

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new Lst<TResult>(new ConcurrentList<TResult>(new List<TResult>{ x }), 0, 1, null);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
            => new Lst<TResult>(new ConcurrentList<TResult>(Enumerable.Select(LocalView(), x => f(x)).ToList()), 0, Count, null);

        /// <inheritdoc />
        public IKeyedFunctor<int, TResult> MapWithKey<TResult>(Func<int, T, TResult> f)
            => new Lst<TResult>(new ConcurrentList<TResult>(Enumerable.Select(LocalView(), (x, i) => f(i, x)).ToList()), 0, Count, null);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is IEnumerable<Func<T, TResult>>))
                throw new InvalidCastException();

            var functions = (IEnumerable<Func<T, TResult>>)f;

            var ys = LocalView();
            return new Lst<TResult>(functions.SelectMany(fx => Enumerable.Select(ys, y => fx(y))));
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
            => new Lst<TResult>(LocalView().SelectMany(x => (IEnumerable<TResult>)f(x)));

        /// <inheritdoc />
        public IMonadPlus<T> Mplus(IMonadPlus<T> that)
        {
            if (that == null || !(that is IEnumerable<T>))
                throw new InvalidCastException();

            var thatXs = (IEnumerable<T>)that;


            return AppendRange(thatXs);
        }

        /// <inheritdoc />
        public Lst<T> Filter(Func<T, bool> f)
            => new Lst<T>(LocalView().Where(f));

        /// <inheritdoc />
        public Maybe<Lst<T>> Semifilter(Func<T, bool> f)
        {
            var ret = Filter(f);

            return Maybe.JustIf(ret.Count > 0, () => ret);
        }

        /// <inheritdoc />
        public IAlternative<T> Empty() => new Lst<T>(new List<T>());

        /// <inheritdoc />
        public IAlternative<T> Alt(IAlternative<T> x)
        {
            if (x == null || !(x is IEnumerable<T> xList))
                throw new InvalidCastException();

            return AppendRange(xList);
        }

        /// <inheritdoc />
        public T1 FoldMap<T1>(IMonoid<T1> monoid, Func<T, T1> f)
            => LocalView().Select(f).Msum(monoid);

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
            => LocalView().AggregateRight(f, accumulator);

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T, Task<IAsyncMonad<TResult>>> f)
            => new Lst<TResult>(((
                await Task.WhenAll(Enumerable.Select(LocalView(), x => f(x)))).Select(x => (IEnumerable<TResult>)x)).SelectMany(xs => xs));

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => Pure(await x());

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T, Task<TResult>>> f)
        {
            if (f == null || !(f is IEnumerable<Func<T, Task<TResult>>> functions))
                throw new InvalidCastException();

            var ys = LocalView();
            return new Lst<TResult>(await Task.WhenAll(functions.SelectMany(fx => ys.Select(y => fx(y)))));
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> f)
            => new Lst<TResult>(await Task.WhenAll(LocalView().Select(x => f(x))));

        /// <inheritdoc />
        public bool Equals(IEnumerable<T> that)
        {
            return comparer.Equals(LocalView(), that);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => new ListEnumerator(list, startIncl, endExcl);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => new ListEnumerator(list, startIncl, endExcl);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var elemsHash = LocalView().HashElements();
            return this.DefaultHash<Lst<T>>(elemsHash);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is IList<T> that))
                return false;

            return Equals(that);
        }

        private class ListEnumerator : IEnumerator<T>
        {
            private readonly ConcurrentList<T> list;
            private readonly int startIncl;
            private readonly int endExcl;
            private int current;

            public ListEnumerator(ConcurrentList<T> list, int start, int end)
            {
                this.list = list;
                this.startIncl = start;
                this.endExcl = end;
                current = start-1;

                this.list.AcquireReadLock();
            }

            public T Current
                => current >= startIncl && current < endExcl ? list.Get(current) : throw new InvalidOperationException();

            object System.Collections.IEnumerator.Current => Current;

            public void Dispose()
            {
                list.ReleaseReadLock();
            }

            public bool MoveNext()
            {
                if (current < endExcl - 1)
                {
                    current++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                current = startIncl - 1;
            }
        }
    }

    internal class ConcurrentList<T> : IEnumerable<T>
    {
        private readonly ReaderWriterLock @lock = new ReaderWriterLock();
        private readonly object usersLock = new object();
        private readonly SortedDictionary<int, int> startIncls = new SortedDictionary<int, int>();
        private readonly SortedDictionary<int, int> endExcls = new SortedDictionary<int, int>();

        private readonly List<T> list;

        public ConcurrentList(List<T> list)
        {
            this.list = list;
        }

        public void AcquireReadLock()
        {
            @lock.AcquireReaderLock(int.MaxValue);
        }

        public void ReleaseReadLock()
        {
            @lock.ReleaseReaderLock();
        }

        public void AddUser(Lst<T> user, int startIncl, int endExcl)
        {
            user.OnDisposing += RemoveUserSlice;

            lock (usersLock)
            {
                AddOrUpdate(startIncls, startIncl, 1, i => i + 1);
                AddOrUpdate(endExcls, endExcl, 1, i => i + 1);
            }
        }

        private void RemoveUserSlice(int startIncl, int endExcl)
        {
            lock (usersLock)
            {
                if (startIncls.Count > 0 && endExcls.Count > 0)
                {
                    var curStartIncl = startIncls.Keys.First();
                    var curEndExcl = endExcls.Keys.Last();

                    RemoveOrUpdate(startIncls, startIncl, i => i <= 0, i => i - 1);
                    RemoveOrUpdate(endExcls, endExcl, i => i <= 0, i => i - 1);

                    //The counts should either bother be 0 or non-0, respectively.
                    //If we have count=0, we have no more users and GC will clean up this list.
                    if (startIncls.Count > 0 && endExcls.Count > 0)
                    {
                        var newStartIncl = startIncls.Keys.First();
                        var newEndExcl = endExcls.Keys.Last();

                        if (newStartIncl != curStartIncl || newEndExcl != curEndExcl)
                            SetRangeFromTo(newStartIncl, newEndExcl);
                    }
                }
            }
        }

        private static void AddOrUpdate<K, V>(SortedDictionary<K, V> dict, K key, V value, Func<V, V> update)
        {
            if (dict.TryGetValue(key, out var curValue))
                dict[key] = update(curValue);
            else
                dict[key] = value;
        }

        private static void RemoveOrUpdate<K, V>(SortedDictionary<K, V> dict, K key, Func<V, bool> removeAfterIf, Func<V, V> update)
        {
            if (dict.TryGetValue(key, out var value))
            {
                var newValue = update(value);

                if (removeAfterIf(newValue))
                    dict.Remove(key);
                else
                    dict[key] = newValue;
            }
        }

        public int Count
        {
            get
            {
                @lock.AcquireReaderLock(int.MaxValue);

                try
                {
                    return list.Count;
                }
                finally
                {
                    @lock.ReleaseReaderLock();
                }
            }
        }

        public void SetRangeFromTo(int index, int endExcl)
        {
            @lock.AcquireWriterLock(int.MaxValue);

            try
            {
                //0 1 [2 3 4] 5 6 7 8
                //index: 2
                //endExcl: 5
                //endIndex: 5 -> ok
                //endCount: 9 - 5 = 4 -> ok
                //0 1 [2 3 4]
                //begIndex: 0 -> ok
                //begCount: 2 -> ok
                //[2 3 4]
                list.RemoveRange(endExcl, list.Count - endExcl);
                list.RemoveRange(0, index);
            }
            finally
            {
                @lock.ReleaseWriterLock();
            }
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        public T Get(int index)
        {
            @lock.AcquireReaderLock(int.MaxValue);

            try
            {
                return list[index];
            } finally
            {
                @lock.ReleaseReaderLock();
            }
        }

        public void Add(T item)
        {
            @lock.AcquireWriterLock(int.MaxValue);

            try
            {
                list.Add(item);
            }
            finally
            {
                @lock.ReleaseWriterLock();
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            @lock.AcquireWriterLock(int.MaxValue);

            try
            {
                list.AddRange(items);
            }
            finally
            {
                @lock.ReleaseWriterLock();
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Lst{T}"/>.
    /// </summary>
    public static class Lst
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Lst{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Lst<TResult> Select<TSource, TResult>(this Lst<TSource> source, Func<TSource, TResult> f)
            => (Lst<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Lst{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Lst<TResult> SelectMany<TSource, TMiddle, TResult>
            (this Lst<TSource> source,
             Func<TSource, Lst<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Lst<TResult>)source.Bind(x => (Lst<TResult>)f(x).Map(y => resultSelector(x, y)));

        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to asynchronous <see cref="Lst{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static async Task<Lst<TResult>> Select<TSource, TResult>(
            this Task<Lst<TSource>> source, Func<TSource, TResult> f)
            => Select(await source, f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to asynchronous <see cref="Lst{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static async Task<Lst<TResult>> SelectMany<TSource, TMiddle, TResult>
            (this Task<Lst<TSource>> source,
             Func<TSource, Task<Lst<TMiddle>>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => (Lst<TResult>)(await (await source).BindAsync(async x => {
                var xy = await f(x);
                var mapped = xy.Map(y => resultSelector(x, y));
                var am = (IAsyncMonad<TResult>)mapped;
                return am;
                }));

        /// <summary>
        /// Creates a new <see cref="Lst{T}"/> out of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="xs">The elements of the sequence.</param>
        public static Lst<T> MakeLst<T>(this IEnumerable<T> xs) => new Lst<T>(xs);

        /// <summary>
        /// Creates a new <see cref="Lst{T}"/> out of a sequence, with a custom <see cref="IEqualityComparer{T}"/> for equality-checking.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="xs">The elements of the sequence.</param>
        /// <param name="comparer">The comparer to use.</param>
        public static Lst<T> MakeLst<T>(this IEnumerable<T> xs, IEqualityComparer<T> comparer) => new Lst<T>(xs, comparer);

        /// <summary>
        /// Creates a new <see cref="Lst{T}"/> out of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="xs">The elements of the sequence.</param>
        public static Lst<T> Make<T>(params T[] xs) => new Lst<T>(xs);

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to an <see cref="Lst{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Lst<T> ToLst<T>(this IFunctor<T> x) => (Lst<T>)x;
    }
}
