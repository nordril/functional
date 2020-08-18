using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A sum type that can either be a left or a right, but not both. While left and right have no special semantics, per convention,
    /// the left is regarded as the "error-case", and the right is regarded as the "ok-case", if the either is used to model failure.
    /// Available as a data-source in LINQ-queries.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value in the either.</typeparam>
    /// <typeparam name="TRight">The type of the right value in the either.</typeparam>
    public struct Either<TLeft, TRight> : IMonad<TRight>, IBifunctor<TLeft, TRight>, IEquatable<Either<TLeft, TRight>>, IEnumerable<TRight>
    {
        private EitherTag discriminator;
        private TLeft left;
        private TRight right;

        /// <summary>
        /// Returns true iff the either contains a left value.
        /// </summary>
        public bool IsLeft => discriminator == EitherTag.Left;
        /// <summary>
        /// Returns true iff the either contains a right value.
        /// </summary>
        public bool IsRight => discriminator == EitherTag.Right;

        /// <summary>
        /// Tries to get the left value in an either and throws an <see cref="PatternMatchException"/> if the either is a right.
        /// </summary>
        public TLeft Left() => IsLeft ? left : throw new PatternMatchException(nameof(Left), nameof(Either<object, object>), nameof(Right));
        /// <summary>
        /// Tries to get the right value in an either and throws an <see cref="PatternMatchException"/> if the either is a left.
        /// </summary>
        public TRight Right() => IsRight ? right : throw new PatternMatchException(nameof(Left), nameof(Either<object, object>), nameof(Right));

        private Either(TLeft left, TRight right, EitherTag discriminator)
        {
            this.left = left;
            this.right = right;
            this.discriminator = discriminator;
        }


        /// <summary>
        /// Creates a new left-<see cref="Either{TLeft, TRight}"/> from a value. The type-level tag is required to disambiguate between constructors.
        /// </summary>
        /// <param name="value">The value to store in the either.</param>
        /// <param name="discriminator">The discriminator tag.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "used for constructor discrimination.")]
        public Either(TLeft value, TagLeft discriminator) : this(value, default, EitherTag.Left)
        {
        }

        /// <summary>
        /// Creates a new right-<see cref="Either{TLeft, TRight}"/> from a value. The type-level tag is required to disambiguate between constructors.
        /// </summary>
        /// <param name="value">The value to store in the either.</param>
        /// <param name="discriminator">The discriminator tag.</param>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "used for constructor discrimination.")]
        public Either(TRight value, TagRight discriminator) : this(default, value, EitherTag.Right)
        {
        }

        /// <summary>
        /// Creates a left-either from a value.
        /// </summary>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromLeft(TLeft value) => new Either<TLeft, TRight>(value, default, EitherTag.Left);

        /// <summary>
        /// Creates a right-either from a value.
        /// </summary>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromRight(TRight value) => new Either<TLeft, TRight>(default, value, EitherTag.Right);

        /// <summary>
        /// Tuple deconstructor function.
        /// </summary>
        /// <param name="isLeft">True if this <see cref="Either{TLeft, TRight}"/> has a left-value, and false if it has a right-value.</param>
        /// <param name="left">The left-value, if present, or <c>default</c>.</param>
        /// <param name="right">The right-value, if present, or <c>default</c>.</param>
        public void Deconstruct(out bool isLeft, out TLeft left, out TRight right)
        {
            isLeft = IsLeft;
            left = IsLeft ? this.left : default;
            right = IsLeft ? default : this.right;
        }

        /// <summary>
        /// Sets the either to a left, clearing the right, if present.
        /// </summary>
        /// <param name="left">The value to put into the either.</param>
        public void SetLeft(TLeft left)
        {
            discriminator = EitherTag.Left;
            this.left = left;
            right = default;
        }

        /// <summary>
        /// Sets the either to a right, clearing the left, if present.
        /// </summary>
        /// <param name="right">The value to put into the either.</param>
        public void SetRight(TRight right)
        {
            discriminator = EitherTag.Right;
            left = default;
            this.right = right;
        }

        /// <summary>
        /// Returns whether this <see cref="Either{TLeft, TRight}"/> contains a left- or a right-value. If it contains a left-value, <paramref name="left"/> will contain the left-value and <paramref name="right"/> will be <c>default</c> and vice versa.
        /// </summary>
        /// <param name="left">The left-value, if it exists, otherwise <c>default</c>.</param>
        /// <param name="right">The right-value, if it exists, otherwise <c>default</c>.</param>
        public EitherTag TryGetValue(out TLeft left, out TRight right)
        {
            if (discriminator == EitherTag.Left)
            {
                left = this.left;
                right = default;
                return EitherTag.Left;
            }
            else
            {
                left = default;
                right = this.right;
                return EitherTag.Right;
            }
        }

        /// <summary>
        /// Turns a left-either into a right-either and vice versa.
        /// </summary>
        public Either<TRight, TLeft> Swap() => discriminator == EitherTag.Left ? Either.FromRight<TRight, TLeft>(left) : Either.FromLeft<TRight, TLeft>(right);

        /// <summary>
        /// A safe way to get a either's left value. If <see cref="IsLeft"/> is true, <see cref="Left"/>
        /// is returned, otherwise, <paramref name="alternative"/> is returned.
        /// </summary>
        /// <param name="alternative">The value to return if the either is right.</param>
        public TLeft LeftOr(TLeft alternative) => IsLeft ? left : alternative;

        /// <summary>
        /// A safe way to get a either's right value. If <see cref="IsRight"/> is true, <see cref="Right"/>
        /// is returned, otherwise, <paramref name="alternative"/> is returned.
        /// </summary>
        /// <param name="alternative">The value to return if the either is left.</param>
        public TRight RightOr(TRight alternative) => IsRight ? right : alternative;

        /// <summary>
        /// Coalesces an either to a single value. If the either is a left, the first function is applied. If it is a right,
        /// the second function is applied.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="f">The function to apply if the either is a left.</param>
        /// <param name="g">The function to apply if the either is a right.</param>
        public TResult Coalesce<TResult>(Func<TLeft, TResult> f, Func<TRight, TResult> g) => IsLeft ? f(left) : g(right);

        /// <summary>
        /// Compares two <see cref="Either{TLeft, TRight}"/>-objects based on their underlying values. This method returns true if both objects are left-eithers/right-eithers and if <see cref="Object.Equals(object)"/> returns true for the underlying <see cref="Left"/> or <see cref="Right"/>, respectively.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Either<TLeft, TRight>))
                return false;

            var thatEither = (Either<TLeft, TRight>)obj;

            if (IsLeft != thatEither.IsLeft)
                return false;
            else if (IsLeft)
                return left.Equals(thatEither.left);
            else
                return right.Equals(thatEither.right);
        }

        /// <inheritdoc />
        public override int GetHashCode() =>
            this.DefaultHash(
                discriminator,
                discriminator == EitherTag.Left ? left : default,
                discriminator == EitherTag.Right ? right : default);

        /// <inheritdoc />
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(Either<TLeft, TRight> other) => Equals((object)other);

        /// <inheritdoc />
        public IBifunctor<TLeftResult, TRightResult> BiMap<TLeftResult, TRightResult>(Func<TLeft, TLeftResult> f, Func<TRight, TRightResult> g)
            => IsLeft ? new Either<TLeftResult, TRightResult>(f(left), default, EitherTag.Left) : new Either<TLeftResult, TRightResult>(default, g(right), EitherTag.Right);

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<TRight, TResult>> f)
        {
            if (f == null || !(f is Either<TLeft, Func<TRight, TResult>>))
                throw new InvalidCastException();

            var fEither = (Either<TLeft, Func<TRight, TResult>>)f;

            return IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left)
                : fEither.IsLeft ? new Either<TLeft, TResult>(fEither.left, default, EitherTag.Left)
                : new Either<TLeft, TResult>(default, fEither.right(right), EitherTag.Right);
        }

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<TRight, IMonad<TResult>> f)
            => IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left)
            : f(right);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TRight, TResult> f)
            => IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left) : new Either<TLeft, TResult>(default, f(right), EitherTag.Right);

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x) => new Either<Unit, TResult>(new Unit(), x, EitherTag.Right);

        /// <inheritdoc />
        public IEnumerator<TRight> GetEnumerator()
        {
            if (IsRight)
                yield return right;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsRight)
                yield return right;
        }
    }

    /// <summary>
    /// Static methods for <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static class Either
    {
        /// <summary>
        /// Equivalent to <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/>, but restricted to <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with one <c>from</c>-clause.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        public static Either<TLeft, TResult> Select<TLeft, TSource, TResult>(this Either<TLeft, TSource> source, Func<TSource, TResult> f)
            => (Either<TLeft, TResult>)source.Map(f);

        /// <summary>
        /// Equivalent to <see cref="IMonad{TSource}"/>, but restricted to <see cref="Either{TLeft, TRight}"/>. Offers LINQ query support with multiple <c>from</c>-clauses.
        /// </summary>
        /// <typeparam name="TLeft">The type of the either's left-value.</typeparam>
        /// <typeparam name="TSource">The type of the source's value.</typeparam>
        /// <typeparam name="TMiddle">The type of the selector's result.</typeparam>
        /// <typeparam name="TResult">The type of the result's value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="f">The function to apply.</param>
        /// <param name="resultSelector">The result-selector.</param>
        public static Either<TLeft, TResult> SelectMany<TLeft, TSource, TMiddle, TResult>
            (this Either<TLeft, TSource> source,
             Func<TSource, Either<TLeft, TMiddle>> f,
             Func<TSource, TMiddle, TResult> resultSelector)
        {
            if (source.IsLeft)
                return new Either<TLeft, TResult>(source.Left(), TagLeft.Value);

            var right = source.Right();
            var midRes = f(right);

            if (midRes.IsLeft)
                return new Either<TLeft, TResult>(midRes.Left(), TagLeft.Value);


            return new Either<TLeft, TResult>(resultSelector(right, midRes.Right()), TagRight.Value);
        }

        /// <summary>
        /// Creates a left-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromLeft<TLeft, TRight>(TLeft value) => Either<TLeft, TRight>.FromLeft(value);

        /// <summary>
        /// Creates a right-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="value">The value to store in the either.</param>
        public static Either<TLeft, TRight> FromRight<TLeft, TRight>(TRight value) => Either<TLeft, TRight>.FromRight(value);

        /// <summary>
        /// Creates a right-either from a value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="isRight">If true, an <see cref="Either.FromRight{TLeft, TRight}(TRight)"/> will be created, and if false, an <see cref="Either.FromLeft{TLeft, TRight}(TLeft)"/>.</param>
        /// <param name="leftFactory">The left-value to store in the either.</param>
        /// <param name="rightFactory">The right-value to store in the either.</param>
        public static Either<TLeft, TRight> EitherIf<TLeft, TRight>(
            bool isRight,
            Func<TLeft> leftFactory,
            Func<TRight> rightFactory) => isRight ? 
                Either<TLeft, TRight>.FromRight(rightFactory())
                : Either<TLeft, TRight>.FromLeft(leftFactory());

        /// <summary>
        /// Tries to cast a generic bifunctor to an either via an explicit cast. Provided for convenience.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left-value.</typeparam>
        /// <typeparam name="TRight">The type of the right-value.</typeparam>
        /// <param name="f">The bifunctor.</param>
        public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this IBifunctor<TLeft, TRight> f) => (Either<TLeft, TRight>)f;
    }

    /// <summary>
    /// A tag indicating the state of an <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public enum EitherTag
    {
        /// <summary>
        /// Indicates that the either is a left.
        /// </summary>
        Left,
        /// <summary>
        /// Indicates that the either is a right.
        /// </summary>
        Right
    }

    /// <summary>
    /// A type-level tag indicating "left" or "right".
    /// </summary>
    public class TagLeftRight
    {
        /// <summary>
        /// Empty, protected constructor.
        /// </summary>
        protected TagLeftRight() { }
    }

    /// <summary>
    /// A type-level tag indicating "left" (as opposed to "right").
    /// </summary>
    public sealed class TagLeft : TagLeftRight
    {
        /// <summary>
        /// The tag's singleton value.
        /// </summary>
        public static readonly TagLeft Value = new TagLeft();

        private TagLeft() : base() { }
    }

    /// <summary>
    /// A type-level tag indicating "right" (as opposed to "left").
    /// </summary>
    public sealed class TagRight : TagLeftRight
    {
        /// <summary>
        /// The tag's singleton value.
        /// </summary>
        public static readonly TagRight Value = new TagRight();

        private TagRight() : base() { }
    }
}
