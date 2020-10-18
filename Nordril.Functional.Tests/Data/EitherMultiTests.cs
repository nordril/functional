using Nordril.Functional.Data;
using Nordril.TypeToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public static class EitherMultiTests
    {
        [Fact]
        public static void Either1LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int>(Either.One(5))
                select x + "d";

            Assert.True(res.IsFirst);
            Assert.Equal("5d", res.First.Value());
        }

        [Fact]
        public static void Either2LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string>(Either.One(5))
                select x + "d";

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);

            res =
                from x in Either.EitherWith<int, string>(Either.Two("abc"))
                select x + "d";

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.First.HasValue);
            Assert.Equal("abcd", res.Second.Value());
        }

        [Fact]
        public static void Either3LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float>(Either.One(5))
                select x+1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);

            res =
                from x in Either.EitherWith<int, string, float>(Either.Two("abc"))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);

            res =
                from x in Either.EitherWith<int, string, float>(Either.Three(3.5f))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(4.5f, res.Third.Value());
        }

        [Fact]
        public static void Either4LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, decimal>(Either.One(5))
                select x + 1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal>(Either.Two("abc"))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal>(Either.Three(3.5f))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal>(Either.Four(18M))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(19M, res.Fourth.Value());
        }

        [Fact]
        public static void Either5LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, decimal, int[]>(Either.One(5))
                select x.Append(13).ToArray();

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[]>(Either.Two("abc"))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[]>(Either.Three(3.5f))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[]>(Either.Four(18M))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[]>(Either.Five(new[] { 3, 4 }))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4, 13 }, res.Fifth.Value());
        }

        [Fact]
        public static void Either6LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.One(5))
                select !x;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Two("abc"))
                select !x;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Three(3.5f))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Four(18M))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Five(new[] { 3, 4 }))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Six(true))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.Value());
        }

        [Fact]
        public static void Either7LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.One(5))
                select x+1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Two("abc"))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Three(3.5f))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Four(18M))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Five(new[] { 3, 4 }))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Six(true))
                select x+1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.True(res.Sixth.Value());
            Assert.False(res.Seventh.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Seven('a'))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.True(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.Equal('a'+1, res.Seventh.Value());
        }

        [Fact]
        public static void Either8LinqSelectTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.One(5))
                select x.GetGenericName();

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Two("abc"))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Three(3.5f))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Four(18M))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Five(new[] { 3, 4 }))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Six(true))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.True(res.Sixth.Value());
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Seven('a'))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.True(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.Equal('a', res.Seventh.Value());
            Assert.False(res.Eigth.HasValue);

            res =
                from x in Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Eight(typeof(int)))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.True(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.Equal("Int32", res.Eigth.Value());
        }

        [Fact]
        public static void Either1LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int>(Either.One(5))
                from y in Either.EitherWith<int>(Either.One(4))
                select x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(9, res.First.Value());
        }

        [Fact]
        public static void Either2LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string>(Either.One(5))
                from y in Either.EitherWith<int, string>(Either.Two("xyz"))
                select x+y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string>(Either.Two("abc"))
                from y in Either.EitherWith<int, string>(Either.One(5))
                select x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string>(Either.Two("abc"))
                from y in Either.EitherWith<int, string>(Either.Two("xyz"))
                select x + y;

            Assert.True(res.IsSecond);
            Assert.Equal("abcxyz", res.Second.Value());
        }

        [Fact]
        public static void Either3LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float>(Either.One(5))
                from y in Either.EitherWith<int, string, decimal>(Either.Three(4M))
                select (decimal)x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float>(Either.Three(5f))
                from y in Either.EitherWith<int, string, decimal>(Either.Two("xyz"))
                select (decimal)x + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float>(Either.Three(5f))
                from y in Either.EitherWith<int, string, decimal>(Either.Three(4M))
                select (decimal)x + y;

            Assert.True(res.IsThird);
            Assert.Equal(9M, res.Third.Value());
        }

        [Fact]
        public static void Either4LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, Type>(Either.One(5))
                from y in Either.EitherWith<int, string, float, string>(Either.Four("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float, Type>(Either.Four(typeof(int)))
                from y in Either.EitherWith<int, string, float, string>(Either.Two("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float, Type>(Either.Four(typeof(int)))
                from y in Either.EitherWith<int, string, float, string>(Either.Four("___"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFourth);
            Assert.Equal("Int32___", res.Fourth.Value());
        }

        [Fact]
        public static void Either5LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, bool, Type>(Either.One(5))
                from y in Either.EitherWith<int, string, float, bool, string>(Either.Five("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, Type>(Either.Five(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, string>(Either.Two("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, Type>(Either.Five(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, string>(Either.Five("___"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFifth);
            Assert.Equal("Int32___", res.Fifth.Value());
        }

        [Fact]
        public static void Either6LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, bool, int[], Type>(Either.One(5))
                from y in Either.EitherWith<int, string, float, bool, int[], string>(Either.Six("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], Type>(Either.Six(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], string>(Either.Two("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], Type>(Either.Six(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], string>(Either.Six("___"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSixth);
            Assert.Equal("Int32___", res.Sixth.Value());
        }

        [Fact]
        public static void Either7LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.One(5))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Seven("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.Seven(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Two("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.Seven(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Seven("___"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSeventh);
            Assert.Equal("Int32___", res.Seventh.Value());
        }

        [Fact]
        public static void Either8LinqSelectManyTest()
        {
            var res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.One(5))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Eight("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.Eight(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Two("xyz"))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res =
                from x in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.Eight(typeof(int)))
                from y in Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Eight("___"))
                select x.GetGenericName() + y;

            Assert.True(res.IsEigth);
            Assert.Equal("Int32___", res.Eigth.Value());
        }

        [Fact]
        public static async Task Either1LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int>(Either.One(5)))
                select x + "d";

            Assert.True(res.IsFirst);
            Assert.Equal("5d", res.First.Value());
        }

        [Fact]
        public static async Task Either2LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string>(Either.One(5)))
                select x + "d";

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);

            res = await 
                from x in Task.FromResult(Either.EitherWith<int, string>(Either.Two("abc")))
                select x + "d";

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.First.HasValue);
            Assert.Equal("abcd", res.Second.Value());
        }

        [Fact]
        public static async Task Either3LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.One(5)))
                select x + 1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.Two("abc")))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.Three(3.5f)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(4.5f, res.Third.Value());
        }

        [Fact]
        public static async Task Either4LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal>(Either.One(5)))
                select x + 1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal>(Either.Two("abc")))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal>(Either.Three(3.5f)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal>(Either.Four(18M)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(19M, res.Fourth.Value());
        }

        [Fact]
        public static async Task Either5LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[]>(Either.One(5)))
                select x.Append(13).ToArray();

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[]>(Either.Two("abc")))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[]>(Either.Three(3.5f)))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[]>(Either.Four(18M)))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[]>(Either.Five(new[] { 3, 4 })))
                select x.Append(13).ToArray();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4, 13 }, res.Fifth.Value());
        }

        [Fact]
        public static async Task Either6LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.One(5)))
                select !x;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Two("abc")))
                select !x;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Three(3.5f)))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Four(18M)))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Five(new[] { 3, 4 })))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool>(Either.Six(true)))
                select !x;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.Value());
        }

        [Fact]
        public static async Task Either7LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.One(5)))
                select x + 1;

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Two("abc")))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Three(3.5f)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Four(18M)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Five(new[] { 3, 4 })))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Six(true)))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.True(res.Sixth.Value());
            Assert.False(res.Seventh.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char>(Either.Seven('a')))
                select x + 1;

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.True(res.IsSeventh);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.Equal('a' + 1, res.Seventh.Value());
        }

        [Fact]
        public static async Task Either8LinqSelectAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.One(5)))
                select x.GetGenericName();

            Assert.True(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.Equal(5, res.First.Value());
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Two("abc")))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.True(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.Equal("abc", res.Second.Value());
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Three(3.5f)))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.True(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.Equal(3.5f, res.Third.Value());
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Four(18M)))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.True(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.Equal(18M, res.Fourth.Value());
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Five(new[] { 3, 4 })))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.True(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.Equal(new int[] { 3, 4 }, res.Fifth.Value());
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Six(true)))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.True(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.True(res.Sixth.Value());
            Assert.False(res.Seventh.HasValue);
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Seven('a')))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.True(res.IsSeventh);
            Assert.False(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.Equal('a', res.Seventh.Value());
            Assert.False(res.Eigth.HasValue);

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, decimal, int[], bool, char, Type>(Either.Eight(typeof(int))))
                select x.GetGenericName();

            Assert.False(res.IsFirst);
            Assert.False(res.IsSecond);
            Assert.False(res.IsThird);
            Assert.False(res.IsFourth);
            Assert.False(res.IsFifth);
            Assert.False(res.IsSixth);
            Assert.False(res.IsSeventh);
            Assert.True(res.IsEigth);
            Assert.False(res.First.HasValue);
            Assert.False(res.Second.HasValue);
            Assert.False(res.Third.HasValue);
            Assert.False(res.Fourth.HasValue);
            Assert.False(res.Fifth.HasValue);
            Assert.False(res.Sixth.HasValue);
            Assert.False(res.Seventh.HasValue);
            Assert.Equal("Int32", res.Eigth.Value());
        }

        [Fact]
        public static async Task Either1LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int>(Either.One(4)))
                select x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(9, res.First.Value());
        }

        [Fact]
        public static async Task Either2LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string>(Either.Two("xyz")))
                select x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string>(Either.Two("abc")))
                from y in Task.FromResult(Either.EitherWith<int, string>(Either.One(5)))
                select x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string>(Either.Two("abc")))
                from y in Task.FromResult(Either.EitherWith<int, string>(Either.Two("xyz")))
                select x + y;

            Assert.True(res.IsSecond);
            Assert.Equal("abcxyz", res.Second.Value());
        }

        [Fact]
        public static async Task Either3LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, decimal>(Either.Three(4M)))
                select (decimal)x + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.Three(5f)))
                from y in Task.FromResult(Either.EitherWith<int, string, decimal>(Either.Two("xyz")))
                select (decimal)x + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float>(Either.Three(5f)))
                from y in Task.FromResult(Either.EitherWith<int, string, decimal>(Either.Three(4M)))
                select (decimal)x + y;

            Assert.True(res.IsThird);
            Assert.Equal(9M, res.Third.Value());
        }

        [Fact]
        public static async Task Either4LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, Type>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, float, string>(Either.Four("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, Type>(Either.Four(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, string>(Either.Two("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, Type>(Either.Four(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, string>(Either.Four("___")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFourth);
            Assert.Equal("Int32___", res.Fourth.Value());
        }

        [Fact]
        public static async Task Either5LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, Type>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, string>(Either.Five("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, Type>(Either.Five(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, string>(Either.Two("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, Type>(Either.Five(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, string>(Either.Five("___")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFifth);
            Assert.Equal("Int32___", res.Fifth.Value());
        }

        [Fact]
        public static async Task Either6LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], Type>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], string>(Either.Six("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], Type>(Either.Six(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], string>(Either.Two("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], Type>(Either.Six(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], string>(Either.Six("___")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSixth);
            Assert.Equal("Int32___", res.Sixth.Value());
        }

        [Fact]
        public static async Task Either7LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Seven("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.Seven(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Two("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], Type>(Either.Seven(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], string>(Either.Seven("___")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSeventh);
            Assert.Equal("Int32___", res.Seventh.Value());
        }

        [Fact]
        public static async Task Either8LinqSelectManyAsyncTest()
        {
            var res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.One(5)))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Eight("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsFirst);
            Assert.Equal(5, res.First.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.Eight(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Two("xyz")))
                select x.GetGenericName() + y;

            Assert.True(res.IsSecond);
            Assert.Equal("xyz", res.Second.Value());

            res = await
                from x in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, Type>(Either.Eight(typeof(int))))
                from y in Task.FromResult(Either.EitherWith<int, string, float, bool, int[], bool[], List<string>, string>(Either.Eight("___")))
                select x.GetGenericName() + y;

            Assert.True(res.IsEigth);
            Assert.Equal("Int32___", res.Eigth.Value());
        }

        [Fact]
        public static void ExtendTest()
        {
            var e1 = new Identity<int>(5);
            var e2 = e1.Extend(Maybe.Nothing<float>());
            var e3 = e2.Extend(Maybe.Nothing<bool>());
            var e4 = e3.Extend(Maybe.Nothing<int[]>());
            var e5 = e4.Extend(Maybe.Nothing<Type>());
            var e6 = e5.Extend(Maybe.Nothing<decimal>());
            var e7 = e6.Extend(Maybe.Nothing<string>());
            var e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsFirst);
            Assert.Equal(5, e8.First.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Just<float>(3.3f));
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsSecond);
            Assert.Equal(3.3f, e8.Second.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Just<bool>(true));
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsThird);
            Assert.True(e8.Third.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Just<int[]>(new int[] { 4, 8 }));
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsFourth);
            Assert.Equal(new int[] { 4, 8 }, e8.Fourth.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Just<Type>(typeof(string)));
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsFifth);
            Assert.Equal(typeof(string), e8.Fifth.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Just<decimal>(4.1M));
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsSixth);
            Assert.Equal(4.1M, e8.Sixth.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Just<string>("abc"));
            e8 = e7.Extend(Maybe.Nothing<char>());

            Assert.True(e8.IsSeventh);
            Assert.Equal("abc", e8.Seventh.Value());

            e1 = new Identity<int>(5);
            e2 = e1.Extend(Maybe.Nothing<float>());
            e3 = e2.Extend(Maybe.Nothing<bool>());
            e4 = e3.Extend(Maybe.Nothing<int[]>());
            e5 = e4.Extend(Maybe.Nothing<Type>());
            e6 = e5.Extend(Maybe.Nothing<decimal>());
            e7 = e6.Extend(Maybe.Nothing<string>());
            e8 = e7.Extend(Maybe.Just<char>('g'));

            Assert.True(e8.IsEigth);
            Assert.Equal('g', e8.Eigth.Value());
        }

        [Fact]
        public static void Equals3Test()
        {
            var x = Either.EitherWith<int, string, bool>(Either.One(5));
            var y = Either.EitherWith<int, string, bool>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool>(Either.Three(false));
            y = Either.EitherWith<int, string, bool>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool>(Either.One(5));
            y = Either.EitherWith<int, string, bool>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool>(Either.Three(false));
            y = Either.EitherWith<int, string, bool>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool>(Either.One(5));
            y = Either.EitherWith<int, string, bool>(Either.Two("abc"));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void Equals4Test()
        {
            var x = Either.EitherWith<int, string, bool, float>(Either.One(5));
            var y = Either.EitherWith<int, string, bool, float>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.Four(3.2f));
            y = Either.EitherWith<int, string, bool, float>(Either.Four(3.1f));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float>(Either.Two("abc"));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void Equals5Test()
        {
            var x = Either.EitherWith<int, string, bool, float, Type>(Either.One(5));
            var y = Either.EitherWith<int, string, bool, float, Type>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Five(typeof(bool)));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Five(typeof(bool)));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.2f));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3.1f));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Two("abc"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type>(Either.Five(typeof(int)));
            y = Either.EitherWith<int, string, bool, float, Type>(Either.Five(typeof(bool)));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void Equals6Test()
        {
            var x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(5));
            var y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Five(typeof(bool)));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Five(typeof(bool)));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Six((byte)43));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.2f));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3.1f));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("abc"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Five(typeof(int)));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Five(typeof(bool)));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Six((byte)42));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void Equals7Test()
        {
            var x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(5));
            var y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Five(typeof(bool)));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Five(typeof(bool)));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Six((byte)43));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Seven((sbyte)48));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Seven((sbyte)48));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.2f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3.1f));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("abc"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Five(typeof(int)));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Five(typeof(bool)));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Six((byte)42));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Seven((sbyte)48));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Seven((sbyte)32));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void Equals8Test()
        {
            var x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(5));
            var y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(5));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("a"));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Three(false));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.1f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.1f));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Five(typeof(bool)));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Five(typeof(bool)));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Six((byte)43));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Seven((sbyte)48));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Seven((sbyte)48));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Eight(48L));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Eight(48L));
            Assert.Equal(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(7));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("a"));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("b"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Three(false));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Three(true));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.2f));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3.1f));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(5));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("abc"));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Five(typeof(int)));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Five(typeof(bool)));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Six((byte)43));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Six((byte)42));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Seven((sbyte)48));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Seven((sbyte)32));
            Assert.NotEqual(x, y);

            x = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Eight(48L));
            y = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Eight(47L));
            Assert.NotEqual(x, y);
        }

        [Fact]
        public static void CoalesceTest()
        {
            Func<int, string> f1 = x => x.ToString() + "__";
            Func<string, string> f2 = x => x + "__";
            Func<bool, string> f3 = x => x.ToString() + "__";
            Func<float, string> f4 = x => x.ToString() + "__";
            Func<Type, string> f5 = x => x.GetGenericName() + "__";
            Func<byte, string> f6 = x => x.ToString() + "__";
            Func<sbyte, string> f7 = x => x.ToString() + "__";
            Func<long, string> f8 = x => x.ToString() + "__";

            var res = "";

            //Two
            res = Either.EitherWith<int, string>(Either.One(4)).Coalesce(f1, f2);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string>(Either.Two("abc")).Coalesce(f1, f2);
            Assert.Equal("abc__", res);

            //Three
            res = Either.EitherWith<int, string, bool>(Either.One(4)).Coalesce(f1, f2, f3);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool>(Either.Two("abc")).Coalesce(f1, f2, f3);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool>(Either.Three(true)).Coalesce(f1, f2, f3);
            Assert.Equal("True__", res);

            //Four
            res = Either.EitherWith<int, string, bool, float>(Either.One(4)).Coalesce(f1, f2, f3, f4);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool, float>(Either.Two("abc")).Coalesce(f1, f2, f3, f4);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool, float>(Either.Three(true)).Coalesce(f1, f2, f3, f4);
            Assert.Equal("True__", res);

            res = Either.EitherWith<int, string, bool, float>(Either.Four(3F)).Coalesce(f1, f2, f3, f4);
            Assert.Equal("3__", res);

            //Five
            res = Either.EitherWith<int, string, bool, float, Type>(Either.One(4)).Coalesce(f1, f2, f3, f4, f5);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool, float, Type>(Either.Two("abc")).Coalesce(f1, f2, f3, f4, f5);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool, float, Type>(Either.Three(true)).Coalesce(f1, f2, f3, f4, f5);
            Assert.Equal("True__", res);

            res = Either.EitherWith<int, string, bool, float, Type>(Either.Four(3F)).Coalesce(f1, f2, f3, f4, f5);
            Assert.Equal("3__", res);

            res = Either.EitherWith<int, string, bool, float, Type>(Either.Five(typeof(int))).Coalesce(f1, f2, f3, f4, f5);
            Assert.Equal("Int32__", res);

            //Six
            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.One(4)).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Two("abc")).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Three(true)).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("True__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Four(3F)).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("3__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Five(typeof(int))).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("Int32__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte>(Either.Six((byte)34)).Coalesce(f1, f2, f3, f4, f5, f6);
            Assert.Equal("34__", res);

            //Seven
            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.One(4)).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Two("abc")).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Three(true)).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("True__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Four(3F)).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("3__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Five(typeof(int))).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("Int32__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Six((byte)34)).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("34__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte>(Either.Seven((sbyte)35)).Coalesce(f1, f2, f3, f4, f5, f6, f7);
            Assert.Equal("35__", res);

            //Eight
            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.One(4)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("4__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Two("abc")).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("abc__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Three(true)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("True__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Four(3F)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("3__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Five(typeof(int))).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("Int32__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Six((byte)34)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("34__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Seven((sbyte)35)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("35__", res);

            res = Either.EitherWith<int, string, bool, float, Type, byte, sbyte, long>(Either.Eight(500L)).Coalesce(f1, f2, f3, f4, f5, f6, f7, f8);
            Assert.Equal("500__", res);
        }
    }
}
