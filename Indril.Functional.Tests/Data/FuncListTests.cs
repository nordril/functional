using Indril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Indril.Functional.Tests.Data
{
    public static class FuncListTests
    {
        public static IEnumerable<object[]> Ap1Data()
        {
            yield return new object[] {
                FuncList.Make<Func<int, int>>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(),
                FuncList.Make(1),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make(4),
                FuncList.Make(5),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1),
                FuncList.Make(1, 2, 3),
                FuncList.Make(2, 3, 4),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncList.Make(5),
                FuncList.Make(6, 10),
            };

            yield return new object[] {
                FuncList.Make<Func<int, int>>(x => x + 1, x => x * 2),
                FuncList.Make(5, 6, 7),
                FuncList.Make(6, 7, 8, 10, 12, 14),
            };
        }

        public static IEnumerable<object[]> Ap2Data()
        {
            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make(1),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(),
                FuncList.Make(1),
                FuncList.Make(1),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
                FuncList.Make<int>(),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make(4),
                FuncList.Make(6),
                FuncList.Make(10),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y),
                FuncList.Make(1, 2, 3),
                FuncList.Make(7, 8, 9),
                FuncList.Make(8, 9, 10, 9, 10, 11, 10, 11, 12),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncList.Make(5),
                FuncList.Make(8),
                FuncList.Make(13, 40),
            };

            yield return new object[] {
                FuncList.Make<Func<int, Func<int, int>>>(x => y => x + y, x => y => x * y),
                FuncList.Make(5, 6, 7),
                FuncList.Make(10, 20, 30),
                FuncList.Make(15, 25, 35, 16, 26, 36, 17, 27, 37, 50, 100, 150, 60, 120, 180, 70, 140, 210),
            };
        }

        [Theory]
        [MemberData(nameof(Ap1Data))]
        public static void FuncListAp1(FuncList<Func<int, int>> funcs, FuncList<int> args, IEnumerable<int> expected)
        {
            var res = args.Ap(funcs);

            Assert.Equal(expected, res as IEnumerable<int>);
        }

        [Theory]
        [MemberData(nameof(Ap2Data))]
        public static void FuncListAp2(FuncList<Func<int, Func<int, int>>> funcs, FuncList<int> args1, FuncList<int> args2, IEnumerable<int> expected)
        {
            var res = args2.Ap(args1.Ap(funcs));

            Assert.Equal(expected, res as IEnumerable<int>);
        }
    }
}
