using Indril.Functional.CategoryTheory;
using Indril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Indril.Functional.Tests.Data
{
    public static class MaybeTests
    {
        public static IEnumerable<object[]> MaybeEqualities()
        {
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Just(5), false };
            yield return new object[] { Maybe.Just(7), Maybe.Nothing<int>(), false };
            yield return new object[] { Maybe.Just(7), Maybe.Just(5), false };
            yield return new object[] { Maybe.Nothing<int>(), Maybe.Nothing<int>(), true };
            yield return new object[] { Maybe.Just(8), Maybe.Just(8), true };
        }

        [Fact]
        public static void NothingHasNoValue()
        {
            var x = Maybe.Nothing<int>();
            var y = Maybe<int>.Nothing();

            Assert.False(x.HasValue);
            Assert.False(y.HasValue);
            Assert.True(x.IsNothing);
            Assert.True(y.IsNothing);
            Assert.Throws<PatternMatchException>(() => x.Value());
            Assert.Throws<PatternMatchException>(() => y.Value());
        }

        [Fact]
        public static void NoValueAfterClear()
        {
            var x = Maybe.Just(5);

            x.ClearValue();

            Assert.False(x.HasValue);
            Assert.True(x.IsNothing);
            Assert.Throws<PatternMatchException>(() => x.Value());
        }

        [Fact]
        public static void ValuePresentAfterSetOfNothing()
        {
            var x = Maybe.Nothing<int>();

            x.SetValue(7);

            Assert.True(x.HasValue);
            Assert.False(x.IsNothing);
            Assert.Equal(7, x.Value());
        }

        [Fact]
        public static void ValuePresentAfterSetOfValue()
        {
            var x = Maybe.Just(9);

            x.SetValue(7);

            Assert.True(x.HasValue);
            Assert.False(x.IsNothing);
            Assert.Equal(7, x.Value());
        }

        [Fact]
        public static void ValueOrOfNothing()
        {
            var x = Maybe.Nothing<int>();

            Assert.Equal(66, x.ValueOr(66));
            Assert.Equal(77, x.ValueOr(y => 9, 77));
            Assert.Equal(88, x.ValueOrLazy(() => 88));
            Assert.Equal(99, x.ValueOrLazy(y => 1, () => 99));
        }

        [Fact]
        public static void ValueOrOfJust()
        {
            var x = Maybe.Just(10);

            Assert.Equal(10, x.ValueOr(66));
            Assert.Equal(20, x.ValueOr(y => y*2, 77));
            Assert.Equal(10, x.ValueOrLazy(() => 88));
            Assert.Equal(30, x.ValueOrLazy(y => y*3, () => 99));
        }

        [Fact]
        public static void ValueOrOfJustDoesntEvaluateAlternative()
        {
            var x = Maybe.Just(10);

            Assert.Equal(10, x.ValueOrLazy(() => throw new Exception()));
            Assert.Equal(10, x.ValueOrLazy(y => y, () => throw new Exception()));
        }

        [Fact]
        public static void TryGetValueOfNothingReturnsFalse()
        {
            var x = Maybe.Nothing<int>();
            var valuePresent1 = x.TryGetValue(9, out var value1);
            var valuePresent2 = x.TryGetValueLazy(() => 10, out var value2);

            Assert.False(valuePresent1);
            Assert.False(valuePresent2);
            Assert.Equal(9, value1);
            Assert.Equal(10, value2);
        }

        [Fact]
        public static void TryGetValueOfJustReturnsTrue()
        {
            var x = Maybe.Just(15);
            var valuePresent1 = x.TryGetValue(9, out var value1);
            var valuePresent2 = x.TryGetValueLazy(() => 10, out var value2);

            Assert.True(valuePresent1);
            Assert.True(valuePresent2);
            Assert.Equal(15, value1);
            Assert.Equal(15, value2);
        }

        [Fact]
        public static void TryGetValueOfJustDoesntEvaluateAlternative()
        {
            var x = Maybe.Just(15);
            var valuePresent1 = x.TryGetValueLazy(() => throw new Exception(), out var value1);

            Assert.True(valuePresent1);
            Assert.Equal(15, value1);
        }

        [Fact]
        public static void MzeroIsNothing()
        {
            Assert.Equal(Maybe.Nothing<int>(), Maybe.Nothing<int>().Mzero());
        }

        [Fact]
        public static void MplusCombinesValues()
        {
            var x1 = Maybe.Nothing<int>();
            var x2 = Maybe.Just(6);
            var y1 = Maybe.Nothing<int>();
            var y2 = Maybe.Just(7);


            Assert.IsType<Maybe<int>>(x1.Mplus(y1));

            var r1 = x1.Mplus(y1).CastToMaybe();
            var r2 = x1.Mplus(y2).CastToMaybe();
            var r3 = x2.Mplus(y1).CastToMaybe();
            var r4 = x2.Mplus(y2).CastToMaybe();

            Assert.True(r1.IsNothing);
            Assert.True(r2.HasValue);
            Assert.True(r3.HasValue);
            Assert.True(r4.HasValue);

            Assert.Equal(7, r2.Value());
            Assert.Equal(6, r3.Value());
            Assert.Equal(6, r4.Value());
        }

        [Fact]
        public static void BindAppliesFunction()
        {
            var x = Maybe.Nothing<int>();
            var y = Maybe.Just(5);

            IMonad<int> f(int z) => Maybe.Nothing<int>();
            IMonad<int> g(int z) => Maybe.Just(z * 2);

            Assert.True(x.Bind(f).CastToMaybe().IsNothing);
            Assert.True(y.Bind(f).CastToMaybe().IsNothing);
            Assert.True(x.Bind(g).CastToMaybe().IsNothing);
            Assert.True(y.Bind(g).CastToMaybe().HasValue);
            Assert.Equal(5, y.Value());
            Assert.Equal(10, y.Bind(g).CastToMaybe().Value());
        }

        [Fact]
        public static void ApAppliesFunction()
        {
            var x = Maybe.Nothing<int>();
            var y = Maybe.Just(5);

            var f = Maybe.Nothing<Func<int, bool>>();
            var g = Maybe.Just<Func<int, bool>>(i => i % 2 == 0);

            Assert.True(x.Ap(f).CastToMaybe().IsNothing);
            Assert.True(x.Ap(g).CastToMaybe().IsNothing);
            Assert.True(y.Ap(f).CastToMaybe().IsNothing);
            Assert.True(y.Ap(g).CastToMaybe().HasValue);

            Assert.Equal(5, y.Value());
            Assert.False(y.Ap(g).CastToMaybe().Value());
        }

        [Fact]
        public static void MapAppliesFunction()
        {
            var x = Maybe.Nothing<int>();
            var y = Maybe.Just(5);

            Assert.True(x.Map(i => i*2).CastToMaybe().IsNothing);
            Assert.True(y.Map(i => i * 2).CastToMaybe().HasValue);
            Assert.Equal(10, y.Map(i => i * 2).CastToMaybe().Value());
        }

        [Fact]
        public static void PureWrapsValue()
        {
            var x = Maybe.Nothing<int>();
            var y = x.Pure(9).CastToMaybe();

            Assert.False(x.HasValue);
            Assert.Equal(9, y.Value());
        }

        [Theory]
        [MemberData(nameof(MaybeEqualities))]
        public static void MaybeEquals(Maybe<int> x1, Maybe<int> x2, bool isEqual)
        {
            Assert.Equal(x1.Equals(x2), isEqual);
        }

        [Fact]
        public static void MaybeNotEqualsToOtherTypes()
        {
            Assert.False(Maybe.Nothing<int>().Equals(5));
            Assert.False(Maybe.Nothing<int>().Equals(Maybe.Nothing<string>()));
        }
    }
}
