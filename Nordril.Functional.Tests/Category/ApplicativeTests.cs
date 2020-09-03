using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Nordril.Functional.Tests.Category
{
    public static class ApplicativeTests
    {
        public static IEnumerable<object[]> WhereApForListsData()
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

        public static IEnumerable<object[]> SelectApForListsData()
        {
            yield return new object[] {
                new int[0],
                new int[][] { new int[0] }
            };

            yield return new object[] {
                new int[]{1 },
                new int[][] { new int[] { 1 }, new int[] { -1 } }
            };

            yield return new object[] {
                new int[] { 1,2,3 },
                new int[][] {
                    new int[] {1,2,3},
                    new int[] {1,2,-3},
                    new int[] {1,-2,3},
                    new int[] {1,-2,-3},
                    new int[] {-1,2,3},
                    new int[] {-1,2,-3},
                    new int[] {-1,-2,3},
                    new int[] {-1,-2,-3},
                }
            };
        }

        [Theory]
        [MemberData(nameof(WhereApForListsData))]
        public static void WhereApForLists(IEnumerable<int> input, IEnumerable<IEnumerable<int>> output)
        {
            var res = input.WhereAp<int, FuncList<bool>, FuncList<IEnumerable<int>>>(_ => FuncList.Make(false, true))
                .Select(x => string.Join(',',x)).ToHashSet();

            var outputSet = output.Select(x => string.Join(',', x)).ToHashSet();

            Assert.Equal(outputSet, res);
        }

        [Theory]
        [MemberData(nameof(WhereApForListsData))]
        public async static void WhereApAsyncForLists(IEnumerable<int> input, IEnumerable<IEnumerable<int>> output)
        {
            var res = (await input.WhereApAsync<int, FuncList<bool>, FuncList<IEnumerable<int>>>(async _ => { await Task.Delay(500); return FuncList.Make(false, true); }))
                .Select(x => string.Join(',', x)).ToHashSet();

            var outputSet = output.Select(x => string.Join(',', x)).ToHashSet();

            Assert.Equal(outputSet, res);
        }

        [Theory]
        [MemberData(nameof(SelectApForListsData))]
        public static void SelectApForLists(IEnumerable<int> input, IEnumerable<IEnumerable<int>> output)
        {
            var res = input.SelectAp<int, int, FuncList<IEnumerable<int>>>(x => FuncList.Make(x, x*(-1)))
                .ToFuncList()
                .Select(x => string.Join(',', x)).ToHashSet();

            var outputSet = output.Select(x => string.Join(',', x)).ToHashSet();

            Assert.Equal(outputSet, res);
        }

        [Theory]
        [MemberData(nameof(SelectApForListsData))]
        public async static void SelectApAsyncForLists(IEnumerable<int> input, IEnumerable<IEnumerable<int>> output)
        {
            var res = (await input.SelectApAsync<int, int, FuncList<IEnumerable<int>>>(async x => { await Task.Delay(500); return FuncList.Make(x, x * (-1)); }))
                .ToFuncList()
                .Select(x => string.Join(',', x)).ToHashSet();

            var outputSet = output.Select(x => string.Join(',', x)).ToHashSet();

            Assert.Equal(outputSet, res);
        }
    }
}
