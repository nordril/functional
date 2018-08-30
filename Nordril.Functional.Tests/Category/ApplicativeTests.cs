using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Nordril.Functional.Tests.Category
{
    public static class ApplicativeTests
    {
        public static IEnumerable<object[]> WhereApListData()
        {
            yield return new object[] {
                new int[0],
                new int[][] { new int[0] }
            };

            yield return new object[] {
                new int[] { 1,2,3 },
                new int[][] {
                    new int[]{ },
                    new int[] { 1 }, new int[] { 2 }, new int[] { 3 },
                    new int[] { 1, 2 }, new int[] { 1, 3 }, new int[] { 2, 3 },
                    new int[] { 1, 2, 3}
                }
            };
        }

        [Theory]
        [MemberData(nameof(WhereApListData))]
        public static void WhereApForLists(IEnumerable<int> input, IEnumerable<IEnumerable<int>> output)
        {
            var res = input.WhereAp<int, FuncList<bool>, FuncList<IEnumerable<int>>>(_ => FuncList.Make(false, true))
                .Select(x => string.Join(',',x)).ToHashSet();

            var outputSet = output.Select(x => string.Join(',', x)).ToHashSet();

            Assert.Equal(outputSet, res);
        }
    }
}
