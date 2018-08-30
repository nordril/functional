﻿using Nordril.Functional.Data;
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

            Assert.Equal(4.5f, res.Right());
        }
    }
}