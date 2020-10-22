using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// An eight-component <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    /// <typeparam name="T3">The third component.</typeparam>
    /// <typeparam name="T4">The fourth component.</typeparam>
    /// <typeparam name="T5">The fifth component.</typeparam>
    /// <typeparam name="T6">The sixth component.</typeparam>
    /// <typeparam name="T7">The seventh component.</typeparam>
    /// <typeparam name="T8">The eigth component.</typeparam>
    public struct Either<T1, T2, T3, T4, T5, T6, T7, T8>
        : ICoproductFirst<T1>
        , ICoproductSecond<T2>
        , ICoproductThird<T3>
        , ICoproductFourth<T4>
        , ICoproductFifth<T5>
        , ICoproductSixth<T6>
        , ICoproductSeventh<T7>
        , ICoproductEigth<T8>
        , IAsyncMonad<T8>
        , IEquatable<Either<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        private readonly EitherTag8 discriminator;
        private readonly object Value;

        /// <inheritdoc />
        public bool IsFirst => discriminator == EitherTag8.First;
        /// <inheritdoc />
        public bool IsSecond => discriminator == EitherTag8.Second;
        /// <inheritdoc />
        public bool IsThird => discriminator == EitherTag8.Third;
        /// <inheritdoc />
        public bool IsFourth => discriminator == EitherTag8.Fourth;
        /// <inheritdoc />
        public bool IsFifth => discriminator == EitherTag8.Fifth;
        /// <inheritdoc />
        public bool IsSixth => discriminator == EitherTag8.Sixth;
        /// <inheritdoc />
        public bool IsSeventh => discriminator == EitherTag8.Seventh;
        /// <inheritdoc />
        public bool IsEigth => discriminator == EitherTag8.Eigth;

        /// <inheritdoc />
        public Maybe<T1> First
        {
            get { var ret = Value; return Maybe.JustIf(IsFirst, () => (T1)ret); }
        }

        /// <inheritdoc />
        public Maybe<T2> Second
        {
            get { var ret = Value; return Maybe.JustIf(IsSecond, () => (T2)ret); }
        }

        /// <inheritdoc />
        public Maybe<T3> Third
        {
            get { var ret = Value; return Maybe.JustIf(IsThird, () => (T3)ret); }
        }

        /// <inheritdoc />
        public Maybe<T4> Fourth
        {
            get { var ret = Value; return Maybe.JustIf(IsFourth, () => (T4)ret); }
        }

        /// <inheritdoc />
        public Maybe<T5> Fifth
        {
            get { var ret = Value; return Maybe.JustIf(IsFifth, () => (T5)ret); }
        }

        /// <inheritdoc />
        public Maybe<T6> Sixth
        {
            get { var ret = Value; return Maybe.JustIf(IsSixth, () => (T6)ret); }
        }

        /// <inheritdoc />
        public Maybe<T7> Seventh
        {
            get { var ret = Value; return Maybe.JustIf(IsSeventh, () => (T7)ret); }
        }

        /// <inheritdoc />
        public Maybe<T8> Eigth
        {
            get { var ret = Value; return Maybe.JustIf(IsEigth, () => (T8)ret); }
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a first value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either1<T1> either)
        {
            discriminator = EitherTag8.First;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a second value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either2<T2> either)
        {
            discriminator = EitherTag8.Second;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a third value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either3<T3> either)
        {
            discriminator = EitherTag8.Third;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a fourth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either4<T4> either)
        {
            discriminator = EitherTag8.Fourth;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a fifth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either5<T5> either)
        {
            discriminator = EitherTag8.Fifth;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a sixth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either6<T6> either)
        {
            discriminator = EitherTag8.Sixth;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a seventh value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either7<T7> either)
        {
            discriminator = EitherTag8.Seventh;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-coproduct out of a eigth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either8<T8> either)
        {
            discriminator = EitherTag8.Eigth;
            Value = either.Value;
        }

        private Either<T1, T2, T3, T4, T5, T6, T7, TResult> CopyError<TResult>()
        {
            if (IsFirst)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either1<T1>((T1)Value));
            else if (IsSecond)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either2<T2>((T2)Value));
            else if (IsThird)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either3<T3>((T3)Value));
            else if (IsFourth)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either4<T4>((T4)Value));
            else if (IsFifth)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either5<T5>((T5)Value));
            else if (IsSixth)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either6<T6>((T6)Value));
            else
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either7<T7>((T7)Value));
        }

        /// <summary>
        /// Compares two <see cref="Either{T1, T2, T3, T4, T5, T6, T7, T8}"/>-objects based on their underlying values.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Either<T1, T2, T3, T4, T5, T6, T7, T8>))
                return false;

            var thatEither = (Either<T1, T2, T3, T4, T5, T6, T7, T8>)obj;

            if (discriminator != thatEither.discriminator)
                return false;
            else if (IsFirst)
                return ((T1)Value).Equals((T1)thatEither.Value);
            else if (IsSecond)
                return ((T2)Value).Equals((T2)thatEither.Value);
            else if (IsThird)
                return ((T3)Value).Equals((T3)thatEither.Value);
            else if (IsFourth)
                return ((T4)Value).Equals((T4)thatEither.Value);
            else if (IsFifth)
                return ((T5)Value).Equals((T5)thatEither.Value);
            else if (IsSixth)
                return ((T6)Value).Equals((T6)thatEither.Value);
            else if (IsSeventh)
                return ((T7)Value).Equals((T7)thatEither.Value);
            else
                return ((T8)Value).Equals((T8)thatEither.Value);
        }

        /// <inheritdoc />
        public bool Equals(Either<T1, T2, T3, T4, T5, T6, T7, T8> other) => Equals((object)other);

        /// <inheritdoc />
        public override int GetHashCode() => this.DefaultHash(discriminator, Value);

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T8, Task<IAsyncMonad<TResult>>> f)
            => !IsEigth ? CopyError<TResult>() : await f((T8)Value);

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T8, IMonad<TResult>> f)
            => !IsEigth ? CopyError<TResult>() : f((T8)Value);

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(await x()));

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T8, Task<TResult>>> f)
        {
            if (f is null || !(f is Either<T1, T2, T3, T4, T5, T6, T7, Func<T8, Task<TResult>>> that))
                throw new InvalidCastException();

            if (IsEigth && that.IsEigth)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(await ((Func<T8, Task<TResult>>)that.Value)((T8)Value)));
            else if (!IsEigth)
                return CopyError<TResult>();
            else
                return that.CopyError<TResult>();
        }

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(x));

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T8, TResult>> f)
        {
            if (f is null || !(f is Either<T1, T2, T3, T4, T5, T6, T8, Func<T8, TResult>> that))
                throw new InvalidCastException();

            if (IsEigth && that.IsEigth)
                return new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(((Func<T8, TResult>)that.Value)((T8)Value)));
            else if (!IsEigth)
                return CopyError<TResult>();
            else
                return that.CopyError<TResult>();
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T8, Task<TResult>> f)
            => !IsEigth ? CopyError<TResult>()
            : new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(await f((T8)Value)));

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T8, TResult> f)
            => !IsEigth ? CopyError<TResult>()
            : new Either<T1, T2, T3, T4, T5, T6, T7, TResult>(new Either8<TResult>(f((T8)Value)));

        /// <inheritdoc />
        public static bool operator ==(Either<T1, T2, T3, T4, T5, T6, T7, T8> left, Either<T1, T2, T3, T4, T5, T6, T7, T8> right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(Either<T1, T2, T3, T4, T5, T6, T7, T8> left, Either<T1, T2, T3, T4, T5, T6, T7, T8> right)
        {
            return !(left == right);
        }
    }
}
