﻿using Nordril.Functional.Category;
using System;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A sum type that can either be a left or a right, but not both. While left and right have no special semantics, per convention,
    /// the left is regarded as the "error-case", and the right is regarded as the "ok-case", if the either is used to model failure.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value in the either.</typeparam>
    /// <typeparam name="TRight">The type of the right value in the either.</typeparam>
    public struct Either<TLeft, TRight> : IMonad<TRight>, IBifunctor<TLeft, TRight>, IEquatable<Either<TLeft, TRight>>
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
        public Either(TLeft value, TagLeft discriminator) : this(value, default, EitherTag.Left)
        {
        }

        /// <summary>
        /// Creates a new right-<see cref="Either{TLeft, TRight}"/> from a value. The type-level tag is required to disambiguate between constructors.
        /// </summary>
        /// <param name="value">The value to store in the either.</param>
        /// <param name="discriminator">The discriminator tag.</param>
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

        #region IBifunctor implementation
        /// <inheritdoc />
        public IBifunctor<TLeftResult, TRightResult> BiMap<TLeftResult, TRightResult>(Func<TLeft, TLeftResult> f, Func<TRight, TRightResult> g)
            => IsLeft ? new Either<TLeftResult, TRightResult>(f(left), default, EitherTag.Left) : new Either<TLeftResult, TRightResult>(default, g(right), EitherTag.Right);
        #endregion

        #region IMonad implementation
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
        #endregion
    }

    /// <summary>
    /// Static methods for <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static class Either
    {
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
        public static Either<TLeft, TRight> CastToEither<TLeft, TRight>(this IBifunctor<TLeft, TRight> f) => (Either<TLeft, TRight>)f;
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
