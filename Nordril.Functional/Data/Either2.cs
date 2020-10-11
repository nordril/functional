using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A sum type that can either be a left or a right, but not both. While left and right have no special semantics, per convention,
    /// the left is regarded as the "error-case", and the right is regarded as the "ok-case", if the either is used to model failure.
    /// Available as a data-source in LINQ-queries.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value in the either.</typeparam>
    /// <typeparam name="TRight">The type of the right value in the either.</typeparam>
    public struct Either<TLeft, TRight> 
        : IMonad<TRight>
        , IBifunctor<TLeft, TRight>
        , IEquatable<Either<TLeft, TRight>>
        , IEnumerable<TRight>
        , IAsyncMonad<TRight>
        , ICoproductFirst<TLeft>
        , ICoproductSecond<TRight>
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

        /// <inheritdoc />
        public bool IsFirst => IsLeft;

        /// <inheritdoc />
        public bool IsSecond => IsRight;

        /// <inheritdoc />
        public Maybe<TLeft> First
        {
            get { var ret = left; return Maybe.JustIf(IsLeft, () => ret); }
        }

        /// <inheritdoc />
        public Maybe<TRight> Second
        {
            get { var ret = right; return Maybe.JustIf(IsRight, () => ret); }
        }

        private Either(TLeft left, TRight right, EitherTag discriminator)
        {
            this.left = left;
            this.right = right;
            this.discriminator = discriminator;
        }

        /// <summary>
        /// Extends this coproduct with an additional component, returning the a copy of the original coproduct if <paramref name="value"/> is <see cref="Maybe.Nothing{T}"/> and a coproduct containing <paramref name="value"/> if it is <see cref="Maybe.Just{T}(T)"/>.
        /// </summary>
        /// <typeparam name="TNew">The type by which to extend this coproduct.</typeparam>
        /// <param name="value">The optional value by which to extend this coproduct.</param>
        public Either<TLeft, TRight, TNew> Extend<TNew>(Maybe<TNew> value)
        {
            var left = this.left;
            var right = this.right;
            var disc = discriminator;

            return value.ValueOrLazy(
                  x => new Either<TLeft, TRight, TNew>(new Either3<TNew>(x)),
                  () => disc switch
                  {
                      EitherTag.Left => new Either<TLeft, TRight, TNew>(new Either1<TLeft>(left)),
                      EitherTag.Right => new Either<TLeft, TRight, TNew>(new Either2<TRight>(right)),
                      _ => throw new ArgumentException("Unknown value for either-tag in Either`2, indicating a bug.")
                  });
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
        /// Constructors an <see cref="Either{TLeft, TRight}"/>-coproduct out of a first value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either1<TLeft> either) : this(either.Value, TagLeft.Value)
        {
        }

        /// <summary>
        /// Constructors an <see cref="Either{TLeft, TRight}"/>-coproduct out of a second value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either2<TRight> either) : this(either.Value, TagRight.Value)
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
            if (f is null || !(f is Either<TLeft, Func<TRight, TResult>>))
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
        public IApplicative<TResult> Pure<TResult>(TResult x) 
            => new Either<TLeft, TResult>(default, x, EitherTag.Right);

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<TRight, Task<IAsyncMonad<TResult>>> f)
            => IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left)
            : await f(right);

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => new Either<TLeft, TResult>(default, await x(), EitherTag.Right);

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<TRight, Task<TResult>>> f)
        {
            if (f == null || !(f is Either<TLeft, Func<TRight, Task<TResult>>>))
                throw new InvalidCastException();

            var fEither = (Either<TLeft, Func<TRight, Task<TResult>>>)f;

            return IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left)
                : fEither.IsLeft ? new Either<TLeft, TResult>(fEither.left, default, EitherTag.Left)
                : new Either<TLeft, TResult>(default, await fEither.right(right), EitherTag.Right);
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<TRight, Task<TResult>> f)
            => IsLeft ? new Either<TLeft, TResult>(left, default, EitherTag.Left) : new Either<TLeft, TResult>(default, await f(right), EitherTag.Right);

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
}
