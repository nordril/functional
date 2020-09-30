using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    using M = MaybeT<Identity<int>, Identity<Maybe<int>>, Maybe<int>, int>;
    using MDouble = MaybeT<Identity<double>, Identity<Maybe<double>>, Maybe<double>, double>;
    using MString = MaybeT<Identity<string>, Identity<Maybe<string>>, Maybe<string>, string>;
    using MF = MaybeT<Identity<Func<int, string>>, Identity<Maybe<Func<int, string>>>, Maybe<Func<int, string>>, Func<int, string>>;

    public static class MaybeTTests
    {
        [Fact]
        public static void MaybeTLiftedTest()
        {
            var maybe = new M();

            var x = new Identity<int>(5);
            var lifted = (M)maybe.Lift(x);

            Assert.Equal(5, lifted.Run.Value.Value());
        }

        [Fact]
        public static void MaybeTPureTest()
        {
            var maybe = new M();

            var pure = (M)maybe.PureT<Identity<int>, Identity<Maybe<int>>, Maybe<int>, int>(3);

            Assert.Equal(3, pure.Run.Value.Value());
        }

        [Fact]
        public static void MaybeTMapTest()
        {
            var maybe = new M();

            var pure = (M)maybe.PureT<Identity<int>, Identity<Maybe<int>>, Maybe<int>, int>(3);
            var bound = (MDouble)pure.MapT<Identity<double>, Identity<Maybe<double>>, Maybe<double>, double>(x => (double)x * 2);

            Assert.Equal(6D, bound.Run.Value.Value());

            var nothing = new M(new Identity<Maybe<int>>(Maybe.Nothing<int>()));
            bound = (MDouble)nothing.MapT<Identity<double>, Identity<Maybe<double>>, Maybe<double>, double>(x => (double)x*2);

            Assert.False(bound.Run.Value.HasValue);
        }

        [Fact]
        public static void MaybeTBindTest()
        {
            var maybe = new M();

            var nothing = M.Nothing();
            var bound = (M)nothing
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 4))));

            Assert.False(bound.Run.Value.HasValue);

            var pure = (M)maybe.PureT<Identity<int>, Identity<Maybe<int>>, Maybe<int>, int>(3);
            bound = (M)pure
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 4))))
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 8))));

            Assert.Equal(3+4+8, bound.Run.Value.Value());

            nothing = new M(new Identity<Maybe<int>>(Maybe.Nothing<int>()));
            bound = (M)nothing
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 4))))
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 8))));

            Assert.False(bound.Run.Value.HasValue);

            bound = (M)pure
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Nothing<int>())))
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 8))));

            Assert.False(bound.Run.Value.HasValue);

            bound = (M)pure
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Just(x + 4))))
                .BindT(x => new M(new Identity<Maybe<int>>(Maybe.Nothing<int>())));

            Assert.False(bound.Run.Value.HasValue);
        }

        [Fact]
        public static void MaybeTApTTest()
        {
            var maybe = new M();

            var pure = (M)maybe.Pure(3);
            var pureF = (MF)maybe.Pure<Func<int, string>>((int x) => x + "");
            var f = (MF)pureF.Pure<Func<int, string>>(x => x + "");
            var bound = (MString)pure.ApT<Identity<Func<int, string>>, Identity<Maybe<Func<int, string>>>, Maybe<Func<int, string>>, Identity<string>, Identity<Maybe<string>>, Maybe<string>, string>(f);

            Assert.Equal("3", bound.Run.Value.Value());
        }

        [Fact]
        public static void MaybeTFunctorMapTest()
        {
            var maybe = new M();

            var pure = (M)maybe.PureT<Identity<int>, Identity<Maybe<int>>, Maybe<int>, int>(3);
            var bound = (MDouble)pure.Map(x => (double)x * 2);

            Assert.Equal(6D, bound.Run.Value.Value());

            var nothing = new M(new Identity<Maybe<int>>(Maybe.Nothing<int>()));
            bound = (MDouble)nothing.Map(x => (double)x * 2);

            Assert.False(bound.Run.Value.HasValue);
        }

        [Fact]
        public static void MaybeTApplicativePureApTest()
        {
            var maybe = new M();

            var pure = (M)maybe.Pure(3);
            var f = (MF)pure.Pure<Func<int, string>>(x => x + "");
            var bound = (MString)pure.Ap(f);

            Assert.Equal("3", bound.Run.Value.Value());
        }

        [Fact]
        public static void MaybeTMonadBindTest()
        {
            var maybe = new M();

            var pure = (M)maybe.Pure(3);
            var bound = (MString)pure
                .Bind(x => new MString(new Identity<Maybe<string>>(Maybe.Just(x + ""))))
                .Bind(x => new MString(new Identity<Maybe<string>>(Maybe.Just(x + "..."))));

            Assert.Equal("3...", bound.Run.Value.Value());

            bound = (MString)pure
                .Bind(x => new MString(new Identity<Maybe<string>>(Maybe.Nothing<string>())))
                .Bind(x => new MString(new Identity<Maybe<string>>(Maybe.Just(x + "..."))));

            Assert.False(bound.Run.Value.HasValue);
        }
    }
}
