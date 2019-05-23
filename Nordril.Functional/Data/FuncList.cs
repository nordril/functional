using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A functional list. This class is a wrapper for <see cref="List{T}"/> which implements various functional interfaces.
    /// </summary>
    /// <remarks>
    /// Because an <see cref="IEnumerable{T}"/>-implementation is all we need for the operations of <see cref="IApplicative{TSource}"/>, <see cref="IMonad{TSource}"/>, etc. (thanks to LINQ), these operations have correspondingly relaxed constraints, and instead of <see cref="FuncList{T}"/> as their parameter, they only require <see cref="IEnumerable{T}"/>. This applies to <see cref="Ap{TResult}(IApplicative{Func{T, TResult}})"/>, <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>, and <see cref="Mplus(IMonadPlus{T})"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public struct FuncList<T> : IFuncList<T>
    {
        private List<T> list;
        private IEqualityComparer<IEnumerable<T>> comparer;

        /// <summary>
        /// Creates a new <see cref="FuncList{T}"/> from the elements of <paramref name="xs"/>, using <see cref="object.Equals(object)"/> for equality-comparisons among elements.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        public FuncList(IEnumerable<T> xs = null)
        {
            list = xs == null ? new List<T>() : new List<T>(xs);
            comparer = new ListEqualityComparer<T>((x, y) => x.Equals(y));
        }

        /// <summary>
        /// Creates a new <see cref="FuncList{T}"/> from the elements of <paramref name="xs"/>, using <paramref name="comparer"/> for equality-comparisons among elements, or <see cref="object.Equals(object)"/> is <paramref name="comparer"/> is null.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        /// <param name="comparer">The equality comparer for the list's elements.</param>
        public FuncList(IEqualityComparer<T> comparer, IEnumerable<T> xs = null)
        {
            list = xs == null ? new List<T>() : new List<T>(xs);
            this.comparer = comparer != null ? new ListEqualityComparer<T>((x,y) => comparer.Equals(x,y)) : new ListEqualityComparer<T>((x, y) => x.Equals(y));
        }

        /// <inheritdoc />
        public int IndexOf(T item) => ListCoalesce().IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, T item) => ListCoalesce().Insert(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => ListCoalesce().RemoveAt(index);

        /// <inheritdoc />
        public T this[int index] { get => ListCoalesce()[index]; set { ListCoalesce()[index] = value; } }

        /// <inheritdoc />
        public void Add(T item) => ListCoalesce().Add(item);

        /// <inheritdoc />
        public void Clear() => ListCoalesce().Clear();

        /// <inheritdoc />
        public bool Contains(T item) => ListCoalesce().Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => ListCoalesce().CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(T item) => ListCoalesce().Remove(item);

        /// <inheritdoc />
        public int Count => ListCoalesce().Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => ListCoalesce().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ListCoalesce().GetEnumerator();

        /// <inheritdoc />
        public IMonadPlus<T> Mplus(IMonadPlus<T> that)
        {
            if (that == null || !(that is IEnumerable<T>))
                throw new InvalidCastException();

            var thatXs = (IEnumerable<T>)that;

            return new FuncList<T>(ListCoalesce().Concat(thatXs));
        }

        /// <inheritdoc />
        public IMonadZero<T> Mzero() => new FuncList<T>();

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
            => new FuncList<TResult>(ListCoalesce().SelectMany(x => (IEnumerable<TResult>)f(x)));

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new FuncList<TResult> { x };

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is IEnumerable<Func<T, TResult>>))
                throw new InvalidCastException();

            var functions = (IEnumerable<Func<T, TResult>>)f;

            var ys = ListCoalesce();
            return new FuncList<TResult>(functions.SelectMany(fx => ys.Select(y => fx(y))));
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
            => new FuncList<TResult>(ListCoalesce().Select(x => f(x)));

        /// <inheritdoc />
        public T1 FoldMap<T1>(Monoid<T1> monoid, Func<T, T1> f)
            => ListCoalesce().Select(f).Msum(monoid);

        /// <inheritdoc />
        public IFuncList<T> Filter(Func<T, bool> f)
            => new FuncList<T>(ListCoalesce().Where(f));

        /// <inheritdoc />
        public Maybe<IFuncList<T>> Semifilter(Func<T, bool> f)
        {
            var ret = Filter(f);

            return Maybe.JustIf(ret.Count > 0, () => ret);
        }

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
            => ListCoalesce().AggregateRight(f, accumulator);

        /// <summary>
        /// Returns an empty list.
        /// </summary>
        public IAlternative<T> Empty() => new FuncList<T>();

        /// <summary>
        /// Concatenates two lists. <paramref name="x"/> only has to be an <see cref="IEnumerable{T}"/>, not a <see cref="IFuncList{T}"/>.
        /// </summary>
        /// <param name="x">The other sequence.</param>
        public IAlternative<T> Alt(IAlternative<T> x)
        {
            if (x == null || !(x is IEnumerable<T>))
                throw new InvalidCastException();

            var xList = (IEnumerable<T>)x;

            return new FuncList<T>(ListCoalesce().Concat(xList));
        }

        /// <inheritdoc />
        public bool Equals(IList<T> that)
        {
            return ComparerCoalesce().Equals(ListCoalesce(), that);
        }

        /// <inheritdoc />
        public IComparer<IList<T1>> GetComparer<T1>() where T1 : IComparable<T1>
        {
            return new LexicographicalComparer<T1>((x, y) => x.CompareTo(y));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is IList<T> that))
                return false;

            return Equals(that);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var elemsHash = ListCoalesce().HashElements();
            return this.DefaultHash<IList<T>>(elemsHash);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var s = ListCoalesce();

            if (s.Count > 10)
                return s.Take(10).Select(x => x.ToString()).ConcatStrings(", ", "[", ",...]");
            else
                return s.Take(10).Select(x => x.ToString()).ConcatStrings(", ", "[", "]");
        }

        /// <inheritdoc />
        public IFuncList<T> Copy() => new FuncList<T>(list);

        /// <summary>
        /// Retuns <see cref="list"/>. If <see cref="list"/> has not been assigned, it's set to a new, empty list first.
        /// </summary>
        /// <remarks>
        /// If two callers hold references to this struct and <see cref="list"/> has not been initialized yet, they will hold references to different lists after this method returns.
        /// </remarks>
        private List<T> ListCoalesce()
        {
            if (list == null)
                list = new List<T>();
            return list;
        }

        /// <summary>
        /// Retuns <see cref="comparer"/>. If <see cref="comparer"/> has not been assigned, it's set to a new, default comparer first
        /// </summary>
        /// <remarks>
        /// If two callers hold references to this struct and <see cref="comparer"/> has not been initialized yet, they will hold references to different lists after this method returns.
        /// </remarks>
        private IEqualityComparer<IEnumerable<T>> ComparerCoalesce()
        {
            if (comparer == null)
                comparer = new ListEqualityComparer<T>((x, y) => x.Equals(y));
            return comparer;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="FuncList{T}"/>.
    /// </summary>
    public static class FuncList
    {
        /// <summary>
        /// Creates a new <see cref="FuncList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="elements">The list of elements.</param>
        public static FuncList<T> Make<T>(params T[] elements)
            => new FuncList<T>(elements);

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to an <see cref="IFuncList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static IFuncList<T> ToFuncList<T>(this IFunctor<T> x) => (IFuncList<T>)x;
    }
}
