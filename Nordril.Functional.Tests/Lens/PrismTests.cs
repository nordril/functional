using Nordril.Functional.Data;
using Nordril.Functional.Lens;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Lens
{
    public sealed class PrismTests
    {
        [Fact]
        public void PrismJustTest()
        {
            var m = Maybe.Just(5);
            var lens = L.Make.Just<int, int>();

            var actual = L.TryGet(lens, m);

            Assert.True(actual.HasValue);
            Assert.Equal(5, actual.Value());

            m = Maybe.Nothing<int>();
            actual = L.TryGet(lens, m);

            Assert.False(actual.HasValue);
        }

        [Fact]
        public void PrismEitherTest()
        {
            var left = L.Make.First<string, int, string>();
            var right = L.Make.Second<string, int, int>();

            Assert.Equal("xyz", L.TryGet(left, Either.EitherWith<string, int>(Either.One("xyz"))).Value());
            Assert.False(L.TryGet(right, Either.EitherWith<string, int>(Either.One("xyz"))).HasValue);

            Assert.Equal(25, L.TryGet(right, Either.EitherWith<string, int>(Either.Two(25))).Value());
            Assert.False(L.TryGet(left, Either.EitherWith<string, int>(Either.Two(25))).HasValue);
        }

        [Fact]
        public void PrismNestTest()
        {
            var left = L.Make.First<Either<string, bool>, Either<decimal, int>, Either<string, bool>>();
            var right = L.Make.Second<Either<string, bool>, Either<decimal, int>, Either<decimal, int>>();

            var left2 = L.Make.First<string, bool, string>();
            var right2 = L.Make.Second<string, bool, bool>();

            var leftLeft = left.Then(left2);

            Assert.Equal("xyz", L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.One(Either.EitherWith<string, bool>(Either.One("xyz"))))).Value());
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.One(Either.EitherWith<string, bool>(Either.Two(true))))).HasValue);
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.Two(Either.EitherWith<decimal, int>(Either.One(3.0M))))).HasValue);
            Assert.False(L.TryGet(leftLeft, Either.EitherWith<Either<string, bool>, Either<decimal, int>>(Either.Two(Either.EitherWith<decimal, int>(Either.Two(19))))).HasValue);
        }

        [Fact]
        public void PrismAssembleTest()
        {
            var expected = Either.EitherWith<string, bool>(Either.One("xyz"));

            var left = L.Make.First<string, bool, string>();
            var right = L.Make.Second<string, bool, bool>();

            var actual = L.Review(left, "abc");
            Assert.True(actual.IsLeft);
            Assert.Equal("abc", actual.Left());

            actual = L.Review(right, true);
            Assert.True(actual.IsRight);
            Assert.True(actual.Right());
        }
    }
}
