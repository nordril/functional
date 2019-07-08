using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nordril.Functional.Algebra;
using Nordril.Functional.Category;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A functional set. This class is a wrapper for <see cref="HashSet{T}"/> which implements various functional interfaces.
    /// </summary>
    /// <remarks>
    /// Because an <see cref="IEnumerable{T}"/>-implementation is all we need for the operations of <see cref="IApplicative{TSource}"/>, <see cref="IMonad{TSource}"/>, etc. (thanks to LINQ), these operations have correspondingly relaxed constraints, and instead of <see cref="FuncList{T}"/> as their parameter, they only require <see cref="IEnumerable{T}"/>. This applies to <see cref="Ap{TResult}(IApplicative{Func{T, TResult}})"/>, <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public struct FuncSet<T> : IFuncSet<T>
    {
        private HashSet<T> set;

        /// <inheritdoc />
        public IEqualityComparer<T> Comparer => set.Comparer;

        /// <inheritdoc />
        public int Count => SetCoalesce().Count;

        /// <inheritdoc />
        public bool IsReadOnly => SetCoalesce().IsReadOnly;

        /// <summary>
        /// Creates a new <see cref="FuncSet{T}"/> from the elements of <paramref name="xs"/>, using <see cref="object.Equals(object)"/> for equality-comparisons among elements.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        public FuncSet(IEnumerable<T> xs = null)
        {
            set = xs == null ? new HashSet<T>() : new HashSet<T>(xs);
        }

        /// <summary>
        /// Creates a new <see cref="FuncSet{T}"/> from the elements of <paramref name="xs"/>, using <paramref name="comparer"/> for equality-comparisons among elements, or <see cref="object.Equals(object)"/> if <paramref name="comparer"/> is null.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        /// <param name="comparer">The equality comparer for the list's elements.</param>
        public FuncSet(IEqualityComparer<T> comparer, IEnumerable<T> xs = null)
        {
            comparer = comparer ?? new FuncEqualityComparer<T>((x, y) => x.Equals(y));
            set = xs == null ? new HashSet<T>(comparer) : new HashSet<T>(xs, comparer);
        }

        /// <inheritdoc />
        public bool Add(T item) => SetCoalesce().Add(item);

        /// <inheritdoc />
        public IFuncSet<T> AddPure(T elem) => Copy().Set(x => x.Add(elem));

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is IEnumerable<Func<T, TResult>>))
                throw new InvalidCastException();

            var functions = (IEnumerable<Func<T, TResult>>)f;

            var ys = SetCoalesce();
            return new FuncSet<TResult>(functions.SelectMany(fx => ys.Select(y => fx(y))));
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f)
            => new FuncSet<TResult>(SetCoalesce().SelectMany(x => (IEnumerable<TResult>)f(x)));

        /// <inheritdoc />
        public void Clear() => SetCoalesce().Clear();

        /// <inheritdoc />
        public bool Contains(T item) => SetCoalesce().Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => SetCoalesce().CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public IFuncSet<T> DifferencePure(ISet<T> elem)
        {
            var copy = Copy();
            copy.ExceptWith(ReplaceEqualityComparer(elem));
            return copy;
        }

        /// <inheritdoc />
        public bool Equals(ISet<T> other) => Equals((object)other);

        /// <inheritdoc />
        public bool Equals(IFuncSet<T> other) => Equals((object)other);

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other) => SetCoalesce().ExceptWith(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public IFuncSet<T> Filter(Func<T, bool> f) => new FuncSet<T>(SetCoalesce().Where(f));

        /// <inheritdoc />
        public T1 FoldMap<T1>(IMonoid<T1> monoid, Func<T, T1> f)
            => SetCoalesce().Select(f).Msum(monoid);

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
            //We don't need AggregateRight here because the order of elements is undefined.
            //Aggregate has better stack-behavior.
             => SetCoalesce().Aggregate(accumulator, (acc, x) => f(x,acc));

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => SetCoalesce().GetEnumerator();

        /// <inheritdoc />
        public IFuncSet<T> IntersectionPure(ISet<T> elem)
        {
            var copy = Copy();
            copy.IntersectWith(ReplaceEqualityComparer(elem));
            return copy;
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other) => SetCoalesce().IntersectWith(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other) => SetCoalesce().IsProperSubsetOf(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other) => SetCoalesce().IsProperSupersetOf(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other) => SetCoalesce().IsSubsetOf(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other) => SetCoalesce().IsSupersetOf(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new FuncSet<TResult>(SetCoalesce().Select(f));

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other) => SetCoalesce().Overlaps(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new FuncSet<TResult>(new[] { x });

        /// <inheritdoc />
        public bool Remove(T item) => SetCoalesce().Remove(item);

        /// <inheritdoc />
        public IFuncSet<T> RemovePure(T elem) => Copy().Set(x => x.Remove(elem));

        /// <inheritdoc />
        public Maybe<IFuncSet<T>> Semifilter(Func<T, bool> f)
        {
            var res = Filter(f);
            return Maybe.JustIf(res.Count > 0, () => res);
        }

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other) => SetCoalesce().SetEquals(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public IFuncSet<T> SymmetricDifferencePure(ISet<T> elem)
        {
            var copy = Copy();
            copy.SymmetricExceptWith(ReplaceEqualityComparer(elem));
            return copy;
        }

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other) => SetCoalesce().SymmetricExceptWith(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        public IFuncSet<T> UnionPure(ISet<T> elem)
        {
            var copy = Copy();
            copy.UnionWith(ReplaceEqualityComparer(elem));
            return copy;
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other) => SetCoalesce().UnionWith(ReplaceEqualityComparer(other));

        /// <inheritdoc />
        void ICollection<T>.Add(T item) => SetCoalesce().Add(item);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => SetCoalesce().GetEnumerator();

        /// <inheritdoc />
        public IComparer<ISet<T1>> GetComparer<T1>() where T1 : IComparable<T1>
        {
            int f(ISet<T1> xs, ISet<T1> ys)
            {
                var lexicographicalComparer = new LexicographicalComparer<T1>((x, y) => x.CompareTo(y));
                var xsOrd = xs.OrderBy(x => x);
                var ysOrd = ys.OrderBy(y => y);

                return lexicographicalComparer.Compare(xsOrd, ysOrd);
            };

            return new FuncComparer<ISet<T1>>(f, s => s.HashElements());
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is ISet<T> that))
                return false;

            var sThis = SetCoalesce();


            return sThis.IsSubsetOf(ReplaceEqualityComparer(that)) && sThis.Count == that.Count;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var elemsHash = SetCoalesce().HashElements();
            return this.DefaultHash<ISet<T>>(elemsHash);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var s = SetCoalesce();

            if (s.Count > 10)
                return s.Take(10).Select(x => x.ToString()).ConcatStrings(", ", "{", ",...}");
            else
                return s.Take(10).Select(x => x.ToString()).ConcatStrings(", ", "{", "}");
        }

        /// <inheritdoc />
        public IFuncSet<T> Copy() => new FuncSet<T>(set);

        private ISet<T> SetCoalesce()
        {
            if (set == null)
                set = new HashSet<T>();
            return set;
        }

        /// <summary>
        /// Conditionally returns either <paramref name="that"/> that or, if its <see cref="IEqualityComparer{T}"/> is not reference-equals to ours, a copy with an equality-comparer that is reference-equals to ours. The point of this is so that set-theoretical operations use our equality-comparer instead of defaulting to reference-equality for elements if the equality-comparer of <paramref name="that"/> differs from ours (this behavior is from the .NET reference source).
        /// See https://referencesource.microsoft.com/system.core/system/Collections/Generic/HashSet.cs.html (AreEqualityComparersEqual and the functions which use it).
        /// </summary>
        /// <param name="that">The set whose equality-comparer to replace.</param>
        /// <returns>Either <paramref name="that"/> or a copy with its equality-comparer replaced.</returns>
        private ISet<T> ReplaceEqualityComparer(IEnumerable<T> that)
        {
            //HashSet -> check the comparer.
            if (that is HashSet<T> hs)
            {
                if (!hs.Comparer.Equals(set.Comparer))
                    return new HashSet<T>(hs, set.Comparer);
                else
                    return hs;
            }
            //FuncSet -> check the comparer.
            else if (that is IFuncSet<T> fs)
            {
                if (!(fs.Comparer.Equals(set.Comparer)))
                    return new FuncSet<T>(set.Comparer, fs);
                else
                    return fs;
            }
            //Anything else -> always copy into a FuncSet, we don't know the comparer.
            else
            {
                return new FuncSet<T>(set.Comparer, that);
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="FuncSet{T}"/>.
    /// </summary>
    public static class FuncSet
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="FuncSet{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static FuncSet<TResult> Select<TSource, TResult>(this FuncSet<TSource> source, Func<TSource, TResult> f)
            => (FuncSet<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="FuncSet{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static FuncSet<TResult> SelectMany<TSource, TMiddle, TResult>
            (this FuncSet<TSource> source,
             Func<TSource, FuncSet<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
        {
            var enumSource = ((IEnumerable<TSource>)source);
            return new FuncSet<TResult>(enumSource.SelectMany(x => f(x), resultSelector));
        }

        /// <summary>
        /// Creates a new <see cref="FuncSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="elements">The set of elements.</param>
        public static FuncSet<T> Make<T>(params T[] elements)
            => new FuncSet<T>(elements);

        /// <summary>
        /// Unsafely casts an <see cref="IFunctor{TSource}"/> to an <see cref="IFuncSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static IFuncSet<T> ToFuncSet<T>(this IFunctor<T> x) => (IFuncSet<T>)x;
    }
}
