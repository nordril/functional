using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class EitherTests
    {
        [Fact]
        public void EitherStoresLeft()
        {
            var id = new Either<int, bool>(99, TagLeft.Value);

            Assert.True(id.IsLeft);
            Assert.Equal(99, id.Left());
        }

        [Fact]
        public void EitherStoresRight()
        {
            var id = new Either<int, float>(3.5f, TagRight.Value);

            Assert.True(id.IsRight);
            Assert.Equal(3.5f, id.Right());
        }

        [Fact]
        public void EitherFromLeft()
        {
            var id = Either.FromLeft<int, float>(99);

            Assert.True(id.IsLeft);
            Assert.Equal(99, id.Left());
        }

        [Fact]
        public void TryGetValue()
        {
            var left = Either.FromLeft<int, float>(86);

            Assert.Equal(EitherTag.Left, left.TryGetValue(out var leftRes, out var rightRes));
            Assert.Equal(86, leftRes);
            Assert.Equal(default, rightRes);

            var right = Either.FromRight<int, float>(101f);

            Assert.Equal(EitherTag.Right, right.TryGetValue(out var leftRes2, out var rightRes2));
            Assert.Equal(default, leftRes2);
            Assert.Equal(101f, rightRes2);
        }

        [Fact]
        public void EitherFromRight()
        {
            var id = Either.FromRight<int, float>(10.8f);

            Assert.True(id.IsRight);
            Assert.Equal(10.8f, id.Right());
        }

        [Fact]
        public void EitherFunctorResult()
        {
            var id = new Either<int, float>(3.5f, TagRight.Value);
            var res = (Either<int, float>)id.Map(x => x + 1);

            Assert.True(id.IsRight);
            Assert.Equal(4.5f, res.Right());

            var err = new Either<int, float>(3, TagLeft.Value);
            res = (Either<int, float>)err.Map(x => x + 1);

            Assert.True(err.IsLeft);
            Assert.Equal(3, res.Left());
        }

        [Fact]
        public static void PureTest()
        {
            var either = ApplicativeExtensions.PureUnsafe<float, Either<Unit, float>>(3.5f);

            Assert.True(either.IsRight);
            Assert.Equal(3.5f, either.Right());
        }

        [Fact]
        public static async Task EitherAsyncFunctorResult()
        {
            var id = new Either<int, float>(3.5f, TagRight.Value);
            var res = (Either<int, float>)await id.MapAsync(async x => await Task.FromResult(x + 1));

            Assert.True(id.IsRight);
            Assert.Equal(4.5f, res.Right());

            var err = new Either<int, float>(3, TagLeft.Value);
            res = (Either<int, float>)await err.MapAsync(async x => await Task.FromResult(x + 1));

            Assert.True(err.IsLeft);
            Assert.Equal(3, res.Left());
        }

        [Fact]
        public static void EitherApTest()
        {
            var either = new Either<int, float>(3.5f, TagRight.Value);
            var f = new Either<int, Func<float, double>>(x => x*2, TagRight.Value);

            var res = (Either<int, double>)either.Ap(f);

            Assert.True(res.IsRight);
            Assert.Equal(7D, res.Right());

            either = new Either<int, float>(5, TagLeft.Value);
            f = new Either<int, Func<float, double>>(x => x * 2, TagRight.Value);

            res = (Either<int, double>)either.Ap(f);

            Assert.True(res.IsLeft);

            either = new Either<int, float>(3.5f, TagRight.Value);
            f = new Either<int, Func<float, double>>(7, TagLeft.Value);

            res = (Either<int, double>)either.Ap(f);

            Assert.True(res.IsLeft);

            either = new Either<int, float>(2, TagLeft.Value);
            f = new Either<int, Func<float, double>>(7, TagLeft.Value);

            res = (Either<int, double>)either.Ap(f);

            Assert.True(res.IsLeft);
        }

        [Fact]
        public static async Task EitherApAsyncTest()
        {
            var either = new Either<int, float>(3.5f, TagRight.Value);
            var f = new Either<int, Func<float, Task<double>>>(async x => await Task.FromResult(x * 2), TagRight.Value);

            var res = (Either<int, double>)await either.ApAsync(f);

            Assert.True(res.IsRight);
            Assert.Equal(7D, res.Right());

            either = new Either<int, float>(5, TagLeft.Value);
            f = new Either<int, Func<float, Task<double>>>(async x => await Task.FromResult(x * 2), TagRight.Value);

            res = (Either<int, double>)await either.ApAsync(f);

            Assert.True(res.IsLeft);

            either = new Either<int, float>(3.5f, TagRight.Value);
            f = new Either<int, Func<float, Task<double>>>(7, TagLeft.Value);

            res = (Either<int, double>)await either.ApAsync(f);

            Assert.True(res.IsLeft);

            either = new Either<int, float>(2, TagLeft.Value);
            f = new Either<int, Func<float, Task<double>>>(7, TagLeft.Value);

            res = (Either<int, double>)await either.ApAsync(f);

            Assert.True(res.IsLeft);
        }

        [Fact]
        public static void BindTest()
        {
            var either = new Either<int, float>(3.5f, TagRight.Value);
            Func<float, IMonad<double>> f = x => Either.EitherIf(x > 0, () => -1, () => (double)x*x);

            var res = (Either<int, double>)either.Bind(f);

            Assert.True(res.IsRight);
            Assert.Equal(3.5D * 3.5D, res.Right());

            either = new Either<int, float>(-3.5f, TagRight.Value);
            res = (Either<int, double>)either.Bind(f);

            Assert.True(res.IsLeft);
            Assert.Equal(-1, res.Left());

            either = new Either<int, float>(3, TagLeft.Value);
            res = (Either<int, double>)either.Bind(f);

            Assert.True(res.IsLeft);
            Assert.Equal(3, res.Left());
        }

        [Fact]
        public static async Task BindAsyncTest()
        {
            var either = new Either<int, float>(3.5f, TagRight.Value);
            Func<float, Task<IAsyncMonad<double>>> f = async x => await Task.FromResult(Either.EitherIf(x > 0, () => -1, () => (double)x * x));

            var res = (Either<int, double>)await either.BindAsync(f);

            Assert.True(res.IsRight);
            Assert.Equal(3.5D * 3.5D, res.Right());

            either = new Either<int, float>(-3.5f, TagRight.Value);
            res = (Either<int, double>)await either.BindAsync(f);

            Assert.True(res.IsLeft);
            Assert.Equal(-1, res.Left());

            either = new Either<int, float>(3, TagLeft.Value);
            res = (Either<int, double>)await either.BindAsync(f);

            Assert.True(res.IsLeft);
            Assert.Equal(3, res.Left());
        }
    }
}
