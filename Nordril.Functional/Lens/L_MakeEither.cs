using Nordril.Functional.Data;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            /// <summary>
            /// Makes a prism that returns the value of a <see cref="Maybe{T}"/>, if it exists.
            /// </summary>
            /// <typeparam name="A">The type of the contained value.</typeparam>
            /// <typeparam name="B">The type of the output value.</typeparam>
            public static IPrism<Maybe<A>, Maybe<B>, A, B> Just<A, B>()
                => Prism<Maybe<A>, Maybe<B>, A, B>(
                    x => Maybe.Just(x),
                    x => x.ValueOrLazy(y => Either.FromRight<Maybe<B>, A>(y), () => Either.FromLeft<Maybe<B>, A>(Maybe.Nothing<B>())));

            /// <summary>
            /// Makes a prism which returns the first value of an Either, if it exists.
            /// </summary>
            /// <typeparam name="A">The type of the input value</typeparam>
            /// <typeparam name="B">The type of the output value</typeparam>
            /// <typeparam name="T2">The type of the second value.</typeparam>
            public static IPrism<Either<A, T2>, Either<B, T2>, A, B> First<A, T2, B>()
                => Prism<Either<A, T2>, Either<B, T2>, A, B>(
                    x => Either.EitherWith<B, T2>(Either.One(x)),
                    x => x.First.ValueOrLazy(y =>
                        Either.EitherWith<Either<B, T2>, A>(Either.Two(y)),
                        () => Either.EitherWith<Either<B, T2>, A>(Either.One(Either.EitherWith<B, T2>(Either.Two(x.Second.Value()))))));

            /// <summary>
            /// Makes a prism which returns the second value of an Either, if it exists.
            /// </summary>
            /// <typeparam name="A">The type of the input value</typeparam>
            /// <typeparam name="B">The type of the output value</typeparam>
            /// <typeparam name="T1">The type of the first value.</typeparam>
            public static IPrism<Either<T1, A>, Either<T1, B>, A, B> Second<T1, A, B>()
                => Prism<Either<T1, A>, Either<T1, B>, A, B>(
                    x => Either.EitherWith<T1, B>(Either.Two(x)),
                    x => x.Second.ValueOrLazy(y =>
                        Either.EitherWith<Either<T1, B>, A>(Either.Two(y)),
                        () => Either.EitherWith<Either<T1, B>, A>(Either.One(Either.EitherWith<T1, B>(Either.One(x.First.Value()))))));

            //todo: the rest of all of these. god...
        }
    }
}
