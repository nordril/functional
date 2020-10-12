using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A six-component <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    /// <typeparam name="T3">The third component.</typeparam>
    /// <typeparam name="T4">The fourth component.</typeparam>
    /// <typeparam name="T5">The fifth component.</typeparam>
    /// <typeparam name="T6">The sixth component.</typeparam>
    public struct Either<T1, T2, T3, T4, T5, T6>
        : ICoproductFirst<T1>
        , ICoproductSecond<T2>
        , ICoproductThird<T3>
        , ICoproductFourth<T4>
        , ICoproductFifth<T5>
        , ICoproductSixth<T6>
        , IAsyncMonad<T6>
        , IEquatable<Either<T1, T2, T3, T4, T5, T6>>
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

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6, T7}"/>-coproduct out of a first value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either1<T1> either)
        {
            discriminator = EitherTag8.First;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-coproduct out of a second value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either2<T2> either)
        {
            discriminator = EitherTag8.Second;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-coproduct out of a third value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either3<T3> either)
        {
            discriminator = EitherTag8.Third;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-coproduct out of a fourth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either4<T4> either)
        {
            discriminator = EitherTag8.Fourth;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-coproduct out of a fifth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either5<T5> either)
        {
            discriminator = EitherTag8.Fifth;
            Value = either.Value;
        }

        /// <summary>
        /// Constructors an <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-coproduct out of a sixth value.
        /// </summary>
        /// <param name="either">The value to wrap</param>
        public Either(Either6<T6> either)
        {
            discriminator = EitherTag8.Sixth;
            Value = either.Value;
        }

        private Either<T1, T2, T3, T4, T5, TResult> CopyError<TResult>()
        {
            if (IsFirst)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either1<T1>((T1)Value));
            else if (IsSecond)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either2<T2>((T2)Value));
            else if (IsThird)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either3<T3>((T3)Value));
            else if (IsFourth)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either4<T4>((T4)Value));
            else
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either5<T5>((T5)Value));
        }

        /// <summary>
        /// Compares two <see cref="Either{T1, T2, T3, T4, T5, T6}"/>-objects based on their underlying values.
        /// </summary>
        /// <param name="obj">The other object.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Either<T1, T2, T3, T4, T5, T6>))
                return false;

            var thatEither = (Either<T1, T2, T3, T4, T5, T6>)obj;

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
            else
                return ((T6)Value).Equals((T6)thatEither.Value);
        }

        /// <inheritdoc />
        public bool Equals(Either<T1, T2, T3, T4, T5, T6> other) => Equals((object)other);

        /// <summary>
        /// Extends this coproduct with an additional component, returning the a copy of the original coproduct if <paramref name="value"/> is <see cref="Maybe.Nothing{T}"/> and a coproduct containing <paramref name="value"/> if it is <see cref="Maybe.Just{T}(T)"/>.
        /// </summary>
        /// <typeparam name="TNew">The type by which to extend this coproduct.</typeparam>
        /// <param name="value">The optional value by which to extend this coproduct.</param>
        public Either<T1, T2, T3, T4, T5, T6, TNew> Extend<TNew>(Maybe<TNew> value)
        {
            var y = Value;
            var disc = discriminator;

            return value.ValueOrLazy(
                  x => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either7<TNew>(x)),
                  () => disc switch
                  {
                      EitherTag8.First => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either1<T1>((T1)y)),
                      EitherTag8.Second => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either2<T2>((T2)y)),
                      EitherTag8.Third => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either3<T3>((T3)y)),
                      EitherTag8.Fourth => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either4<T4>((T4)y)),
                      EitherTag8.Fifth => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either5<T5>((T5)y)),
                      _ => new Either<T1, T2, T3, T4, T5, T6, TNew>(new Either6<T6>((T6)y))
                  });
        }

        /// <inheritdoc />
        public async Task<IAsyncMonad<TResult>> BindAsync<TResult>(Func<T6, Task<IAsyncMonad<TResult>>> f)
            => !IsSixth ? CopyError<TResult>() : await f((T6)Value);

        /// <inheritdoc />
        public IMonad<TResult> Bind<TResult>(Func<T6, IMonad<TResult>> f)
            => !IsSixth ? CopyError<TResult>() : f((T6)Value);

        /// <inheritdoc />
        public async Task<IApplicative<TResult>> PureAsync<TResult>(Func<Task<TResult>> x)
            => new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(await x()));

        /// <inheritdoc />
        public async Task<IAsyncApplicative<TResult>> ApAsync<TResult>(IApplicative<Func<T6, Task<TResult>>> f)
        {
            if (f is null || !(f is Either<T1, T2, T3, T4, T5, Func<T6, Task<TResult>>> that))
                throw new InvalidCastException();

            if (IsSixth && that.IsSixth)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(await ((Func<T6, Task<TResult>>)that.Value)((T6)Value)));
            else if (!IsSixth)
                return CopyError<TResult>();
            else
                return that.CopyError<TResult>();
        }

        /// <inheritdoc />
        public IApplicative<TResult> Pure<TResult>(TResult x)
            => new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(x));

        /// <inheritdoc />
        public IApplicative<TResult> Ap<TResult>(IApplicative<Func<T6, TResult>> f)
        {
            if (f is null || !(f is Either<T1, T2, T3, T4, T5, Func<T6, TResult>> that))
                throw new InvalidCastException();

            if (IsSixth && that.IsSixth)
                return new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(((Func<T6, TResult>)that.Value)((T6)Value)));
            else if (!IsSixth)
                return CopyError<TResult>();
            else
                return that.CopyError<TResult>();
        }

        /// <inheritdoc />
        public async Task<IAsyncFunctor<TResult>> MapAsync<TResult>(Func<T6, Task<TResult>> f)
            => !IsSixth ? CopyError<TResult>()
            : new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(await f((T6)Value)));

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T6, TResult> f)
            => !IsSixth ? CopyError<TResult>()
            : new Either<T1, T2, T3, T4, T5, TResult>(new Either6<TResult>(f((T6)Value)));
    }
}
