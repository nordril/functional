using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A predicate which wraps a function that takes a <typeparamref name="T"/> and returns true or false.
    /// </summary>
    /// <typeparam name="T">The type of the input parameter.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1815", Justification = "Equality between functions cannot, in general, be determined.")]
    public struct Predicate<T> : IContravariant<T>, IMonoid<Predicate<T>>
    {
        /// <summary>
        /// The predicate function.
        /// </summary>
        public Func<T, bool> Func { get; private set; }

        /// <summary>
        /// The neutral element w.r.t the predicate's monoid. Creates a predicate that always returns true.
        /// </summary>
        public Predicate<T> Neutral => new Predicate<T>(_ => true);

        /// <summary>
        /// Creates a new predicate from a function.
        /// </summary>
        /// <param name="func"></param>
        public Predicate(Func<T, bool> func)
        {
            Func = func;
        }

        /// <summary>
        /// Runs the predicate with an argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public bool Run(T arg) => Func(arg);

        /// <inheritdoc />
        public IContravariant<TResult> ContraMap<TResult>(Func<TResult, T> f)
            => new Predicate<TResult>(f.Then(Func));

        /// <summary>
        /// Combines two predicates via logical AND.
        /// </summary>
        /// <param name="that">The other predicate.</param>
        public Predicate<T> Op(Predicate<T> that)
        {
            var f = Func;
            return new Predicate<T>(x => f(x) && that.Func(x));
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Predicate{T}"/>.
    /// </summary>
    public static class Pred
    {
        /// <summary>
        /// Creates a new <see cref="Predicate{T}"/> from a function <paramref name="f"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="f">The predicate function.</param>
        public static Predicate<T> Create<T>(Func<T, bool> f) => new Predicate<T>(f);

        /// <summary>
        /// Unsafely casts an <see cref="IContravariant{TSource}"/> to a <see cref="Predicate{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the input parameter.</typeparam>
        /// <param name="x">The object to cast.</param>
        public static Predicate<T> ToPredicate<T>(this IContravariant<T> x) => (Predicate<T>)x;
    }
}
