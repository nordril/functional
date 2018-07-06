using Indril.Functional.Algebra;
using Indril.Functional.CategoryTheory;
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
    /// <em>DO NOT USE THE PARAMETERLESS CONSTRUCTOR. Attempting to do so will result in a <see cref="NullReferenceException"/> if you attempt to use any method of the created object.</em>
    /// <br />
    /// Because an <see cref="IEnumerable{T}"/>-implementation is all we need for the operations of <see cref="IApplicative{TSource}"/>, <see cref="IMonad{TSource}"/>, etc. (thanks to LINQ), these operations have correspondingly relax constraints, and instead of <see cref="FuncList{T}"/> as their parameter, they only require <see cref="IEnumerable{T}"/>. This applies to <see cref="Ap{TResult}(IApplicative{Func{T, TResult}})"/>, <see cref="Bind{TResult}(Func{T, IMonad{TResult}})"/>, and <see cref="Mplus(IMonadPlus{T})"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public struct FuncList<T> : IFuncList<T>
    {
        private readonly List<T> list;

        /// <summary>
        /// Creates a new <see cref="FuncList{T}"/> from the elements of <paramref name="xs"/>.
        /// </summary>
        /// <param name="xs">The elements to store in the list.</param>
        public FuncList(IEnumerable<T> xs)
        {
            if (xs == null)
                list = new List<T>();
            else
                list = new List<T>(xs);
        }

        /// <inheritdoc />
        public int IndexOf(T item) => list.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, T item) => list.Insert(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => list.RemoveAt(index);

        /// <inheritdoc />
        public T this[int index] { get => list[index]; set { list[index] = value; } }

        /// <inheritdoc />
        public void Add(T item) => list.Add(item);

        /// <inheritdoc />
        public void Clear() => list.Clear();

        /// <inheritdoc />
        public bool Contains(T item) => list.Contains(item);

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(T item) => list.Remove(item);

        /// <inheritdoc />
        public int Count => list.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc />
        public IMonadPlus<T> Mplus(IMonadPlus<T> that)
        {
            if (that == null || !(that is IEnumerable<T>))
                throw new InvalidCastException();

            var thatXs = (IEnumerable<T>)that;

            return new FuncList<T>(list.Concat(thatXs));
        }

        /// <inheritdoc />
        public IMonadZero<T> Mzero() => new FuncList<T>();

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f) => new FuncList<TResult>(list.SelectMany(x => (IEnumerable<TResult>)f(x)));

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new FuncList<TResult> { x };

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is IEnumerable<Func<T, TResult>>))
                throw new InvalidCastException();

            var fXs = (IEnumerable<Func<T, TResult>>)f;

            var ys = this.list;

            return new FuncList<TResult>(fXs.SelectMany(fx => ys.Select(y => fx(y))));

        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => new FuncList<TResult>(list.Select(x => f(x)));

        /// <inheritdoc />
        public T1 FoldMap<T1>(Monoid<T1> monoid, Func<T, T1> f) => list.Aggregate(monoid.Neutral, (acc, x) => monoid.Op(acc, f(x)));

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
        {
            TResult ListFoldr(TResult acc, IEnumerable<T> xs)
            {
                if (!xs.Any())
                    return acc;
                else
                    return f(xs.First(), ListFoldr(acc, xs.Skip(1)));
            }

            return ListFoldr(accumulator, list);
        }
    }
}
