using Nordril.Functional.Category;
using Nordril.Functional.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// An optional type that works for both reference and value types.
    /// Also known as "Option". Available as a data-source in LINQ-queries.
    /// </summary>
    public struct Maybe<T> : IMonadPlus<T>, IAlternative<T>, IEquatable<Maybe<T>>, IEnumerable<T>
    {
        private T value;

        /// <summary>
        /// Returns the value, if it exists. Otherwise, a <see cref="PatternMatchException"/> is thrown.
        /// </summary>
        public T Value() => HasValue ? value : throw new PatternMatchException(nameof(Value), nameof(Maybe<object>), nameof(IsNothing));

        /// <summary>
        /// Returns true iff the maybe has a value.
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Returns true iff the maybe does not have a value.
        /// </summary>
        public bool IsNothing => !HasValue;

        private Maybe(bool hasValue, T value)
        {
            HasValue = hasValue;
            this.value = value;
        }

        /// <summary>
        /// Clears the value in-place. <see cref="HasValue"/> will be false.
        /// </summary>
        public void ClearValue()
        {
            HasValue = false;
            value = default;
        }

        /// <summary>
        /// Sets the value in-place. <see cref="HasValue"/> will be true.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(T value)
        {
            HasValue = true;
            this.value = value;
        }

        /// <summary>
        /// A safe way to get a maybe's value. If <see cref="HasValue"/> is true, <see cref="Value"/>
        /// is returned, otherwise, <paramref name="alternative"/> is returned.
        /// </summary>
        /// <param name="alternative">The value to return if the maybe has no value.</param>
        public T ValueOr(T alternative) => HasValue ? value : alternative;

        /// <summary>
        /// A safe way to get a maybe's value. If <see cref="HasValue"/> is true, the function <paramref name="f"/> is applied to <see cref="Value"/> and returned, otherwise <paramref name="alternative"/> is returned.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to apply to the value, if present.</param>
        /// <param name="alternative">The value to return if the maybe has no value.</param>
        public TResult ValueOr<TResult>(Func<T, TResult> f, TResult alternative) => HasValue ? f(value) : alternative;

        /// <summary>
        /// A safe way to get a maybe's value. If <see cref="HasValue"/> is true, <see cref="Value"/>
        /// is returned, otherwise, <paramref name="alternative"/> is returned. This version allows lazy evaluation of <paramref name="alternative"/>.
        /// </summary>
        /// <param name="alternative">The value to return if the maybe has no value. Wrapped in a lambda to permit lazy evaluation.</param>
        public T ValueOrLazy(Func<T> alternative) => HasValue ? value : alternative();

        /// <summary>
        /// A safe way to get a maybe's value. If <see cref="HasValue"/> is true, the function <paramref name="f"/> is applied to <see cref="Value"/> and returned, otherwise, <paramref name="alternative"/> is returned. This version allows lazy evaluation of <paramref name="alternative"/>.
        /// </summary>
        /// <param name="f">The function to apply to the value, if present.</param>
        /// <param name="alternative">The value to return if the maybe has no value. Wrapped in a lambda to permit lazy evaluation.</param>
        public TResult ValueOrLazy<TResult>(Func<T, TResult> f, Func<TResult> alternative) => HasValue ? f(value) : alternative();

        /// <summary>
        /// A safe way to get a maybe's value. The return value is <see cref="HasValue"/>. If <see cref="HasValue"/> is true, <paramref name="result"/> will be set to <see cref="Value"/>, otherwise, it will be set to <paramref name="alternative"/>.
        /// </summary>
        /// <param name="alternative">The value to return if the maybe has no value.</param>
        /// <param name="result"><see cref="Value"/> if <see cref="HasValue"/> is true, and <paramref name="alternative"/> otherwise.</param>
        /// <returns></returns>
        public bool TryGetValue(T alternative, out T result)
        {
            if (HasValue)
            {
                result = value;
                return true;
            }
            else
            {
                result = alternative;
                return false;
            }
        }

        /// <summary>
        /// A safe way to get a maybe's value. The return value is <see cref="HasValue"/>. If <see cref="HasValue"/> is true, <paramref name="result"/> will be set to <see cref="Value"/>, otherwise, it will be set to <paramref name="alternativeFactory"/>. This version allows lazy evaluation.
        /// </summary>
        /// <param name="alternativeFactory">The value to return if the maybe has no value. Wrapped in a lambda to permit lazy evaluation.</param>
        /// <param name="result"><see cref="Value"/> if <see cref="HasValue"/> is true, and <paramref name="alternativeFactory"/> otherwise.</param>
        /// <returns></returns>
        public bool TryGetValueLazy(Func<T> alternativeFactory, out T result)
        {
            if (HasValue)
            {
                result = value;
                return true;
            }
            else
            {
                result = alternativeFactory();
                return false;
            }
        }

        /// <summary>
        /// Returns a new maybe containing new value.
        /// </summary>
        public static Maybe<T> Nothing() => new Maybe<T>(false, default);

        /// <summary>
        /// Returns a new maybe containing a value.
        /// </summary>
        /// <param name="value">The value to store in the maybe.</param>
        public static Maybe<T> Just(T value) => new Maybe<T>(true, value);

        /// <inheritdoc />
        public IMonadZero<T> Mzero() => Nothing();

        /// <inheritdoc />
        public IMonadPlus<T> Mplus(IMonadPlus<T> that)
        {
            if (that == null || !(that is Maybe<T>))
                throw new InvalidCastException();

            return HasValue ? this : that;
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T, IMonad<TResult>> f) => HasValue ? f(value) : Maybe<TResult>.Nothing();

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => Maybe<TResult>.Just(x);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T, TResult>> f)
        {
            if (f == null || !(f is Maybe<Func<T, TResult>>))
                throw new InvalidCastException();

            var fMaybe = (Maybe<Func<T, TResult>>)f;

            return (HasValue && fMaybe.HasValue) ? Maybe<TResult>.Just(fMaybe.Value()(value)) : Maybe<TResult>.Nothing();
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f) => HasValue ? Maybe<TResult>.Just(f(value)) : Maybe<TResult>.Nothing();

        /// <summary>
        /// Returns <see cref="Maybe.Nothing{T}"/>.
        /// </summary>
        public IAlternative<T> Empty() => new Maybe<T>();

        /// <summary>
        /// Returns a copy of this, if it has a value, or a copy of <paramref name="x"/>.
        /// </summary>
        /// <param name="x">The other value.</param>
        public IAlternative<T> Alt(IAlternative<T> x)
        {
            if (x == null || !(x is Maybe<T>))
                throw new InvalidCastException();

            var xMaybe = (Maybe<T>)x;

            return HasValue ? new Maybe<T>(true, value) : new Maybe<T>(xMaybe.HasValue, xMaybe.value);
        }

        /// <summary>
        /// Compares two <see cref="Maybe{T}"/>-objects based on their values, if present. The comparison returns true if both values lack a value or if both have one and the values are equal based on their <see cref="Object.Equals(object)"/>-method.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Maybe<T>))
                return false;

            var thatMaybe = (Maybe<T>)obj;

            if (HasValue != thatMaybe.HasValue)
                return false;
            else if (HasValue == false)
                return true;
            else
                return value.Equals(thatMaybe.value);
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(HasValue, HasValue ? value : default);

        /// <inheritdoc />
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Maybe<T> other) => Equals((object)other);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue)
                yield return value;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (HasValue)
                yield return value;
        }
    }

    /// <summary>
    /// Static methods and extension for <see cref="Maybe{T}"/>.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Maybe{T}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> f)
            => (Maybe<TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Maybe{T}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>
            (this Maybe<TSource> source,
             Func<TSource, Maybe<TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
            => source.ValueOr(s => f(s).ValueOr(m => Just(resultSelector(s, m)), Nothing<TResult>()), Nothing<TResult>());

        /// <summary>
        /// An isomorphism between <see cref="Maybe{T}"/> and reference types, where <c>default</c> is mapped to <see cref="Maybe.Nothing{T}"/> and all other values to <see cref="Maybe.Just{T}(T)"/>.
        /// </summary>
        /// <typeparam name="T">The underlying type to wrap.</typeparam>
        private class MaybeIso<T> : IIsomorphism<T, Maybe<T>>
            where T : class
        {
            public T ConvertBackWith(Unit _, Maybe<T> from) => from.ValueOr(default);

            public Maybe<T> ConvertWith(Unit _, T from) => JustIf(from != default, () => from);
        }

        /// <summary>
        /// Creates a new maybe which contains a value if <paramref name="isJust"/> is true,
        /// and Nothing otherwise.
        /// Useful as a shorthand case distinction.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="isJust">The test.</param>
        /// <param name="factory">The function to create a value. This wrapper is there to ensure that the
        /// value isn't evaluated if it isn't needed.</param>
        /// <returns></returns>
        public static Maybe<T> JustIf<T>(bool isJust, Func<T> factory) => isJust ? Maybe<T>.Just(factory()) : Maybe<T>.Nothing();

        /// <summary>
        /// Returns a new maybe containing a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to store.</param>
        public static Maybe<T> Just<T>(T value) => Maybe<T>.Just(value);

        /// <summary>
        /// Returns an isomorphism between a reference type (which has null) and <see cref="Maybe{T}"/>, which can be used to wrap/unwrap the <see cref="Maybe{T}"/>.
        /// null of the underlying type is mapped to <see cref="Maybe.Nothing{T}"/> and all other values are mapped to <see cref="Maybe.Just{T}(T)"/>.
        /// </summary>
        /// <typeparam name="T">The underlying type contained in the <see cref="Maybe{T}"/>.</typeparam>
        public static IIsomorphism<T, Maybe<T>> Iso<T>() where T : class => new MaybeIso<T>();

        /// <summary>
        /// Returns the value of the <see cref="Maybe{T}"/> or null if it has no value. Shorthand for <c>m.ValueOr(null)</c>.
        /// </summary>
        /// <typeparam name="T">The underlying type contained in the <see cref="Maybe{T}"/>.</typeparam>
        /// <param name="maybe">The <see cref="Maybe{T}"/> to unwrap.</param>
        public static T Unwrap<T>(this Maybe<T> maybe) where T : class => maybe.ValueOr(null);

        /// <summary>
        /// Returns a new maybe containing no value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        public static Maybe<T> Nothing<T>() => Maybe<T>.Nothing();

        /// <summary>
        /// Tries to cast a <see cref="IFunctor{TSource}"/> to a <see cref="Maybe{T}"/> via an explicit cast.
        /// Convenience method.
        /// </summary>
        /// <typeparam name="T">The type of the value contained in the functor.</typeparam>
        /// <param name="f">The functor to cast to a maybe.</param>
        public static Maybe<T> ToMaybe<T>(this IFunctor<T> f) => (Maybe<T>)f;

        /// <summary>
        /// Tries to get a value from a dictionary and applies a function if present, or returns an alternative value if not.
        /// This does not change the dictionary. The semantics are the same as <see cref="Maybe{T}.ValueOr{TResult}(Func{T, TResult}, TResult)"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary's key.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary's value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="dict">The dictionary from which to get the value.</param>
        /// <param name="key">The key whose corresponding value to get.</param>
        /// <param name="f">The function to apply to the key's value if the key is present.</param>
        /// <param name="alternative">The alternative value to return if the key is not present.</param>
        public static TResult ValueOr<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue, TResult> f, TResult alternative)
            => (dict.TryGetValue(key, out var value)) ? f(value) : alternative;
    }
}
