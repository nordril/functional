using Nordril.Functional.Category;
using Nordril.Functional.Data;
using Nordril.Functional.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Results
{
    public sealed class ResultTests
    {
        public static IEnumerable<object[]> MapTestOkData()
        {
            yield return new object[]
            {
                Result.Ok(0),
                (Func<int, double>)(x => x*2D),
                0
            };
            yield return new object[]
            {
                Result.Ok(3),
                (Func<int, double>)(x => x*6D),
                18D
            };
        }

        public static IEnumerable<object[]> MapTestWithErrorsData()
        {
            yield return new object[]
            {
                Result.WithErrors<int>(new Error[] { }, ResultClass.DataConflict),
                (Func<int, double>)(x => x*2D),
            };
            yield return new object[]
            {
                Result.WithErrors<int>(new [] { new Error("uh-oh!", Code.Green, "that field", new ArgumentException("uh-oh!")) }, ResultClass.DataConflict),
                (Func<int, double>)(x => x*2D),
            };
            yield return new object[]
            {
                Result.WithErrors<int>(new [] {
                    new Error("uh-oh!", Code.Green, "that field", new ArgumentException("uh-oh!")),
                    new Error("yikes!", Code.Red, "that other field", new ArgumentException("yikes!"))

                }, ResultClass.DataConflict),
                (Func<int, double>)(x => x*2D),
            };
        }

        public static IEnumerable<object[]> ApTestData()
        {
            FuncList<int> mk(int x) => (FuncList<int>)Enumerable.Range(0, x).MakeFuncList();
            var errors = new[] {
                new Error("booh!", Code.Red, "some field", new ArgumentException("booh!")),
                new Error("ouch!", Code.Orange, "some other field", new ArgumentException("ouch!"))
            };
            var errors2 = new[] {
                new Error("shablam!", Code.Red, "some 3rd field", new ArgumentException("shablam!")),
                new Error("frigg!", Code.Orange, "some 4th field", new ArgumentException("frigg!"))
            };

            yield return new object[]
            {
                Result.Ok(5),
                Result.Ok<Func<int, FuncList<int>>>(mk),
                Result.Ok(mk(5))
            };

            yield return new object[]
            {
                Result.WithErrors<int>(errors, ResultClass.BadRequest),
                Result.Ok<Func<int, FuncList<int>>>(mk),
                Result.WithErrors<FuncList<int>>(errors, ResultClass.BadRequest)
            };

            yield return new object[]
            {
                Result.Ok(5),
                Result.WithErrors<Func<int, FuncList<int>>>(errors, ResultClass.BadRequest),
                Result.WithErrors<FuncList<int>>(errors, ResultClass.BadRequest)
            };

            yield return new object[]
            {
                Result.WithErrors<int>(errors, ResultClass.EditConflict),
                Result.WithErrors<Func<int, FuncList<int>>>(errors2, ResultClass.BadRequest),
                Result.WithErrors<FuncList<int>>(errors2, ResultClass.BadRequest)
            };
        }

        public static IEnumerable<object[]> BindTestData()
        {
            FuncList<int> mk(int x) => (FuncList<int>)Enumerable.Range(0, x).MakeFuncList();
            var errors = new[] {
                new Error("booh!", Code.Red, "some field", new ArgumentException("booh!")),
                new Error("ouch!", Code.Orange, "some other field", new ArgumentException("ouch!"))
            };
            var errors2 = new[] {
                new Error("shablam!", Code.Red, "some 3rd field", new ArgumentException("shablam!")),
                new Error("frigg!", Code.Orange, "some 4th field", new ArgumentException("frigg!"))
            };
            Func<int, IMonad<FuncList<int>>> ok = x => Result.Ok((FuncList<int>)Enumerable.Range(0, x).MakeFuncList());
            Func<int, IMonad<FuncList<int>>> err = x => Result.WithErrors<FuncList<int>>(errors2, ResultClass.BadRequest);

            yield return new object[]
            {
                Result.Ok(5),
                ok,
                Result.Ok(mk(5))
            };

            yield return new object[]
            {
                Result.WithErrors<int>(errors, ResultClass.BadRequest),
                ok,
                Result.WithErrors<FuncList<int>>(errors, ResultClass.BadRequest)
            };

            yield return new object[]
            {
                Result.Ok(5),
                err,
                Result.WithErrors<FuncList<int>>(errors2, ResultClass.BadRequest)
            };

            yield return new object[]
            {
                Result.WithErrors<int>(errors, ResultClass.DataConflict),
                err,
                Result.WithErrors<FuncList<int>>(errors, ResultClass.DataConflict)
            };
        }

        [Theory]
        [MemberData(nameof(MapTestOkData))]
        public static void MapTestOk(Result<int> r, Func<int, double> f, double expected)
        {
            var actual = r.Map(f);

            Assert.IsType<Result<double>>(actual);

            var actualRes = (Result<double>)actual;

            Assert.True(actualRes.IsOk);
            Assert.Equal(ResultClass.Ok, actualRes.ResultClass);
            Assert.False(ReferenceEquals(actualRes, r));
            Assert.Equal(expected, actualRes.Value());
        }

        [Theory]
        [MemberData(nameof(MapTestOkData))]
        public static async Task MapAsyncTestOk(Result<int> r, Func<int, double> f, double expected)
        {
            var actual = await r.MapAsync(async i => await Task.FromResult(f(i)));

            Assert.IsType<Result<double>>(actual);

            var actualRes = (Result<double>)actual;

            Assert.True(actualRes.IsOk);
            Assert.Equal(ResultClass.Ok, actualRes.ResultClass);
            Assert.False(ReferenceEquals(actualRes, r));
            Assert.Equal(expected, actualRes.Value());
        }

        [Theory]
        [MemberData(nameof(MapTestWithErrorsData))]
        public static void MapTestWithErrors(Result<int> r, Func<int, double> f)
        {
            var errors = r.Errors();
            var resClass = r.ResultClass;
            var actual = r.Map(f);

            Assert.IsType<Result<double>>(actual);

            var actualRes = (Result<double>)actual;

            Assert.False(actualRes.IsOk);
            Assert.Equal(resClass, actualRes.ResultClass);
            Assert.False(ReferenceEquals(actualRes, r));
            Assert.Equal(errors, actualRes.Errors());
        }

        [Theory]
        [MemberData(nameof(MapTestWithErrorsData))]
        public static async Task MapAsyncTestWithErrors(Result<int> r, Func<int, double> f)
        {
            var errors = r.Errors();
            var resClass = r.ResultClass;
            var actual = await r.MapAsync(async i => await Task.FromResult(f(i)));

            Assert.IsType<Result<double>>(actual);

            var actualRes = (Result<double>)actual;

            Assert.False(actualRes.IsOk);
            Assert.Equal(resClass, actualRes.ResultClass);
            Assert.False(ReferenceEquals(actualRes, r));
            Assert.Equal(errors, actualRes.Errors());
        }

        [Fact]
        public static void PureTest()
        {
            var res = 7.PureUnsafe<int, Result<int>>();

            Assert.True(res.IsOk);
            Assert.Equal(ResultClass.Ok, res.ResultClass);
            Assert.Equal(7, res.Value());
        }

        [Fact]
        public static async Task PureAsyncTest()
        {
            var res = (await Result.Ok<Unit>().PureAsync(async () => await Task.FromResult(7))).ToResult();

            Assert.True(res.IsOk);
            Assert.Equal(ResultClass.Ok, res.ResultClass);
            Assert.Equal(7, res.Value());
        }

        [Theory]
        [MemberData(nameof(ApTestData))]
        public static void ApTest(Result<int> r1, Result<Func<int, FuncList<int>>> r2, Result<FuncList<int>> expected)
        {
            var actual = r1.Ap(r2);

            Assert.IsType<Result<FuncList<int>>>(actual);

            var actualRes = (Result<FuncList<int>>)actual;

            Assert.Equal(expected.IsOk, actualRes.IsOk);
            Assert.Equal(expected.ResultClass, actualRes.ResultClass);

            Assert.False(ReferenceEquals(r1, actual));
            Assert.False(ReferenceEquals(r2, actual));

            if (expected.IsOk)
                Assert.Equal(expected.Value(), actualRes.Value());
            else
                Assert.Equal(expected.Errors(), actualRes.Errors());
        }

        [Theory]
        [MemberData(nameof(ApTestData))]
        public static async Task ApAsyncTest(Result<int> r1, Result<Func<int, FuncList<int>>> r2, Result<FuncList<int>> expected)
        {
            Func<Func<int, FuncList<int>>, Func<int, Task<FuncList<int>>>> fInner = f => async i => await Task.FromResult(f(i));
            var r2Async = r2.Map(fInner).ToResult();
            var actual = await r1.ApAsync(r2Async);

            Assert.IsType<Result<FuncList<int>>>(actual);

            var actualRes = (Result<FuncList<int>>)actual;

            Assert.Equal(expected.IsOk, actualRes.IsOk);
            Assert.Equal(expected.ResultClass, actualRes.ResultClass);

            Assert.False(ReferenceEquals(r1, actual));
            Assert.False(ReferenceEquals(r2, actual));

            if (expected.IsOk)
                Assert.Equal(expected.Value(), actualRes.Value());
            else
                Assert.Equal(expected.Errors(), actualRes.Errors());
        }

        [Theory]
        [MemberData(nameof(BindTestData))]
        public static void BindTest(Result<int> r1, Func<int, IMonad<FuncList<int>>> f, Result<FuncList<int>> expected)
        {
            var actual = r1.Bind(f);

            Assert.IsType<Result<FuncList<int>>>(actual);

            var actualRes = (Result<FuncList<int>>)actual;

            Assert.Equal(expected.IsOk, actualRes.IsOk);
            Assert.Equal(expected.ResultClass, actualRes.ResultClass);

            Assert.False(ReferenceEquals(r1, actual));

            if (expected.IsOk)
                Assert.Equal(expected.Value(), actualRes.Value());
            else
                Assert.Equal(expected.Errors(), actualRes.Errors());
        }

        [Theory]
        [MemberData(nameof(BindTestData))]
        public static async Task BindAsyncTest(Result<int> r1, Func<int, IMonad<FuncList<int>>> f, Result<FuncList<int>> expected)
        {
            Func<int, Task<IAsyncMonad<FuncList<int>>>> fAsync = async x => await Task.FromResult((IAsyncMonad<FuncList<int>>)f(x));

            var actual = await r1.BindAsync(fAsync);

            Assert.IsType<Result<FuncList<int>>>(actual);

            var actualRes = (Result<FuncList<int>>)actual;

            Assert.Equal(expected.IsOk, actualRes.IsOk);
            Assert.Equal(expected.ResultClass, actualRes.ResultClass);

            Assert.False(ReferenceEquals(r1, actual));

            if (expected.IsOk)
                Assert.Equal(expected.Value(), actualRes.Value());
            else
                Assert.Equal(expected.Errors(), actualRes.Errors());
        }

        private enum Code { Green, Yellow, Orange, Red }

        [Fact]
        public static void LinqQueryTest()
        {
            var res1 =
                from r1 in Result.Ok(32)
                select r1 * 2;

            Assert.True(res1.IsOk);
            Assert.Equal(32 * 2, res1.Value());

            var res2 =
                from r1 in Result.Ok(18)
                from r2 in Result.Ok(32)
                select r1 + r2;

            Assert.True(res2.IsOk);
            Assert.Equal(18 + 32, res2.Value());

            var res3 =
                from r1 in Result.WithErrors<int>(new Error[] { new Error("error1", Code.Orange) }, ResultClass.EditConflict)
                from r2 in Result.WithErrors<int>(new Error[] { new Error("error2", Code.Red) }, ResultClass.BadRequest)
                from r3 in Result.WithErrors<int>(new Error[] { new Error("error3", Code.Green) }, ResultClass.NotFound)
                select r1 + r2 + r3;

            Assert.True(!res3.IsOk);
            Assert.Equal(new Error[] { new Error("error1", Code.Orange) }, res3.Errors());
            Assert.Equal(ResultClass.EditConflict, res3.ResultClass);

            var res4 =
                from r1 in Result.Ok(5)
                from r2 in Result.WithErrors<int>(new Error[] { new Error("error2", Code.Red) }, ResultClass.BadRequest)
                from r3 in Result.WithErrors<int>(new Error[] { new Error("error3", Code.Green) }, ResultClass.NotFound)
                select r1 + r2 + r3;

            Assert.True(!res4.IsOk);
            Assert.Equal(new Error[] { new Error("error2", Code.Red) }, res4.Errors());
            Assert.Equal(ResultClass.BadRequest, res4.ResultClass);

            var res5 =
                from r1 in Result.WithErrors<int>(new Error[] { new Error("error1", Code.Orange) }, ResultClass.EditConflict)
                select r1 * 2;

            Assert.True(!res5.IsOk);
            Assert.Equal(new Error[] { new Error("error1", Code.Orange) }, res5.Errors());
            Assert.Equal(ResultClass.EditConflict, res5.ResultClass);
        }

        [Fact]
        public static async Task AsyncLinqQueryTest()
        {
            var res1 = await
                from r1 in Task.FromResult(Result.Ok(32))
                select r1 * 2;

            Assert.True(res1.IsOk);
            Assert.Equal(32 * 2, res1.Value());

            var res2 = await
                from r1 in Task.FromResult(Result.Ok(18))
                from r2 in Task.FromResult(Result.Ok(32))
                select r1 + r2;

            Assert.True(res2.IsOk);
            Assert.Equal(18 + 32, res2.Value());

            var res3 = await
                from r1 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error1", Code.Orange) }, ResultClass.EditConflict))
                from r2 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error2", Code.Red) }, ResultClass.BadRequest))
                from r3 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error3", Code.Green) }, ResultClass.NotFound))
                select r1 + r2 + r3;

            Assert.True(!res3.IsOk);
            Assert.Equal(new Error[] { new Error("error1", Code.Orange) }, res3.Errors());
            Assert.Equal(ResultClass.EditConflict, res3.ResultClass);

            var res4 = await
                from r1 in Task.FromResult(Result.Ok(5))
                from r2 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error2", Code.Red) }, ResultClass.BadRequest))
                from r3 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error3", Code.Green) }, ResultClass.NotFound))
                select r1 + r2 + r3;

            Assert.True(!res4.IsOk);
            Assert.Equal(new Error[] { new Error("error2", Code.Red) }, res4.Errors());
            Assert.Equal(ResultClass.BadRequest, res4.ResultClass);

            var res5 = await
                from r1 in Task.FromResult(Result.WithErrors<int>(new Error[] { new Error("error1", Code.Orange) }, ResultClass.EditConflict))
                select r1 * 2;

            Assert.True(!res5.IsOk);
            Assert.Equal(new Error[] { new Error("error1", Code.Orange) }, res5.Errors());
            Assert.Equal(ResultClass.EditConflict, res5.ResultClass);
        }

        [Fact]
        public static void WithErrorTest()
        {
            var res = Result.WithError<bool>(new Error("err 1", 4), ResultClass.AlreadyPresent);

            Assert.False(res.IsOk);
            Assert.Equal(new Error[] { new Error("err 1", 4) }, res.Errors());
            Assert.Equal(ResultClass.AlreadyPresent, res.ResultClass);
        }

        [Fact]
        public static void OkIfTest()
        {
            var res = Result.OkIf<bool>(false, () => { throw new NotImplementedException(); }, new Error[] { new Error("err 1", 6) }, ResultClass.BadRequest);

            Assert.False(res.IsOk);
            Assert.Equal(new Error[] { new Error("err 1", 6) }, res.Errors());
            Assert.Equal(ResultClass.BadRequest, res.ResultClass);

            res = Result.OkIf(true, () => true, new Error[] { new Error("err 1", 6) }, ResultClass.BadRequest);

            Assert.True(res.IsOk);
            Assert.True(res.Value());
            Assert.Equal(ResultClass.Ok, res.ResultClass);
        }
    }
}
