using Indril.Functional.Algebra;
using Indril.Functional.Category;
using Indril.Functional.Collection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indril.Functional.Data
{
    /// <summary>
    /// A functional list. This class is a wrapper for <see cref="List{T}"/> which implements various functional interfaces.
    /// </summary>
    /// <remarks>
    /// Because an <see cref="IEnumerable{T}"/>-implementation is all we need for the operations of <see cref="IApplicative{TSource}"/>, <see cref="IMonad{TSource}"/>, etc. (thanks to LINQ), these operations have correspondingly relax constraints, and instead of <see cref="FuncList{T}"/> as their parameter, they only require <see cref="IEnumerable{T}"/>. This applies to <see cref="Ap{TResult}(IApplicative{Func{T, TResult}})"/>, <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>, and <see cref="Mplus(IMonadPlus{T})"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public struct FuncList<T> : IFuncList<T>
    {
        private List<T> list;

        /// <summary>
        /// Creates a new <see cref="FuncList{T}"/> from the elements of <paramref name="xs"/>.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        public FuncList(IEnumerable<T> xs = null)
        {
            if (xs == null)
                list = new List<T>();
            else
                list = new List<T>(xs);
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
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f) => new FuncList<TResult>(ListCoalesce().SelectMany(x => (IEnumerable<TResult>)f(x)));

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new FuncList<TResult> { x };

        //todo: check this for correctness. either this or whereAp is wrong
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
            => ListCoalesce().Aggregate(monoid.Neutral, (acc, x) => monoid.Op(acc, f(x)));

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator) => ListCoalesce().AggregateRight(f, accumulator);

        private List<T> ListCoalesce()
        {
            if (list == null)
                list = new List<T>();
            return list;
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
    }
}
