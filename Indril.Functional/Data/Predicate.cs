using Indril.Functional.Algebra;
using Indril.Functional.CategoryTheory;
using Indril.Functional.Function;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Indril.Functional.Data
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
}
