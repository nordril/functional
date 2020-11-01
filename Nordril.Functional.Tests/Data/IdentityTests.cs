using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using Xunit;

namespace Nordril.Functional.Tests.Data
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

        [Fact]
        public static void FoldrTest()
        {
            Assert.Equal(5, Identity.Make(5).Foldr((i, acc) => i + acc, 0));
        }

        [Fact]
        public static void FoldMapTest()
        {
            Assert.Equal(9, Identity.Make(3).FoldMap(Monoid.IntMult, i => i * 3));
        }

        [Fact]
        public static void TraverseTest()
        {
            var m = Identity.Make(7);
            var actual1 = m.Traverse<FuncList<int>, int>(i => FuncList.Make(i + 1)).Map(x => x.ToIdentity()).ToFuncList();
            var actual2 = m.Map(x => (x + 1).PureUnsafe<int, FuncList<int>>()).ToIdentity().Traverse<FuncList<int>, int>().Map(x => x.ToIdentity()).ToFuncList();

            Assert.Single(actual1);
            Assert.Equal(8, actual1[0].Value);
            Assert.Single(actual2);
            Assert.Equal(8, actual2[0].Value);
        }
    }
}
