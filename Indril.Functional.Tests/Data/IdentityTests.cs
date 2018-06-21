﻿using Indril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Indril.Functional.Tests.Data
{
    public class IdentityTests
    {
        /// <summary>
        /// <see cref="Identity{T}"/> should faithfully store its value.
        /// </summary>
        [Fact]
        public void IdentityStoresValue()
        {
            var id = new Identity<int>(5);

            Assert.Equal(5, id.Value);
        }

        [Fact]
        public void IdentityFunctorResult()
        {
            var id = new Identity<int>(5);
            var res = (Identity<int>)id.Map(x => x + 1);

            Assert.Equal(6, res.Value);
        }
    }
}
