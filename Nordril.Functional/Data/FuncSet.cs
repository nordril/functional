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
        public int Count => SetCoalesce().Count;

        /// <inheritdoc />
        public bool IsReadOnly => SetCoalesce().IsReadOnly;

        /// <summary>
        /// Creates a new <see cref="FuncSet{T}"/> from the elements of <paramref name="xs"/>.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        public FuncSet(IEnumerable<T> xs = null)
        {
            if (xs == null)
                set = new HashSet<T>();
            else
                set = new HashSet<T>(xs);
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
        public IFuncSet<T> DifferencePure(ISet<T> elem) => Copy().Set(x => x.ExceptWith(elem));

        /// <inheritdoc />
        public bool Equals(ISet<T> other) => Equals((object)other);

        /// <inheritdoc />
        public bool Equals(IFuncSet<T> other) => Equals((object)other);

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other) => SetCoalesce().ExceptWith(other);

        /// <inheritdoc />
        public IFuncSet<T> Filter(Func<T, bool> f) => new FuncSet<T>(SetCoalesce().Where(f));

        /// <inheritdoc />
        public T1 FoldMap<T1>(Monoid<T1> monoid, Func<T, T1> f)
            => SetCoalesce().Select(f).Msum(monoid);

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
            //We don't need AggregateRight here because the order of elements is undefined.
            //Aggregate has better stack-behavior.
             => SetCoalesce().Aggregate(accumulator, (acc, x) => f(x,acc));

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => SetCoalesce().GetEnumerator();

        /// <inheritdoc />
        public IFuncSet<T> IntersectionPure(ISet<T> elem) => Copy().Set(x => x.IntersectWith(elem));

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other) => SetCoalesce().IntersectWith(other);

        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other) => SetCoalesce().IsProperSubsetOf(other);

        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other) => SetCoalesce().IsProperSupersetOf(other);

        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other) => SetCoalesce().IsSubsetOf(other);

        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other) => SetCoalesce().IsSupersetOf(other);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new FuncSet<TResult>(SetCoalesce().Select(f));

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other) => SetCoalesce().Overlaps(other);

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
        public bool SetEquals(IEnumerable<T> other) => SetCoalesce().SetEquals(other);

        /// <inheritdoc />
        public IFuncSet<T> SymmetricDifferencePure(ISet<T> elem) => Copy().Set(x => x.SymmetricExceptWith(elem));

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other) => SetCoalesce().SymmetricExceptWith(other);

        /// <inheritdoc />
        public IFuncSet<T> UnionPure(ISet<T> elem) => Copy().Set(x => x.UnionWith(elem));

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other) => SetCoalesce().UnionWith(other);

        /// <inheritdoc />
        void ICollection<T>.Add(T item) => SetCoalesce().Add(item);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => SetCoalesce().GetEnumerator();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is ISet<T> that))
                return false;

            var sThis = SetCoalesce();

            return sThis.IsSubsetOf(that) && sThis.Count == that.Count;
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

            return new FuncComparer<ISet<T1>>(f);
        }
    }
}
