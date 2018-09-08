using Nordril.Functional.Data;
using System;
using System.Collections.Generic;

namespace Nordril.Functional.Tests
{
    public static partial class CollectionExtensionsTests
    {
        public static IEnumerable<object[]> Aggregate2Data()
        {
            yield return new object[] {
                new int[] { },
                0
            };

            yield return new object[] {
                new int[] { 1 },
                //1..9
                //sumUp    res   form  i    
                //0+1+0 -> 1     1*1   0
                //1+1+1 -> 3     1*3   1
                //3+1+2 -> 6     2*3   2    res = (⌊i/2⌋+1) * (2⌈i/2⌉+1)
                //6+1+3 -> 10    2*5   3        = ⌊i/2⌋ * (2⌈i/2⌉+1) + (2⌈i/2⌉+1)
                //10+1+4 -> 15   3*5   4        = ⌊i/2⌋ * 2⌈i/2⌉ + ⌊i/2⌋ + (2⌈i/2⌉+1)
                //15+1+5 -> 21   3*7   5        = 2⌊i/2⌋⌈i/2⌉ + ⌊i/2⌋ + 2⌈i/2⌉ + 1
                //21+1+6 -> 28   4*7   6
                //28+1+7 -> 36   4*9   7
                //36+1+8 -> 45   5*9   8   i=8 -> 2⌊i/2⌋⌈i/2⌉ + ⌊i/2⌋ + 2⌈i/2⌉ + 1 = 45
                //45+1+9 -> 55   5*11  9   i=9 -> 2*4*5 + 4 + 2*5 + 1 = 55
                55
            };

            yield return new object[] {
                new int[] { 1, 10 },
                //0...9; 10..29
                //sumUp    res   form  i    
                //first sum: 55
                //55+10+10 -> 75
                //75+10+11 -> 96
                //96+10+12 -> 118
                //118+10+13 -> 141
                //141+10+14 -> 165
                //165+10+15 -> 190
                //190+10+16 -> 216
                //216+10+17 -> 243
                //243+10+18 -> 271
                //271+10+19 -> 300
                //300+10+20 -> 330
                //330+10+21 -> 361
                //361+10+22 -> 393
                //393+10+23 -> 426
                //426+10+24 -> 460
                //460+10+25 -> 495
                //495+10+26 -> 531
                //531+10+27 -> 568
                //568+10+28 -> 606
                //606+10+29 -> 645
                645
            };
        }

        public static IEnumerable<object[]> ConcatTestData()
        {
            yield return new object[] {
                new int[][] { },
                new int [] { }
            };
            yield return new object[] {
                new int[][] { new int[] { } },
                new int [] { }
            };
            yield return new object[] {
                new int[][] { new int[] { 3 } },
                new int [] { 3 }
            };
            yield return new object[] {
                new int[][] { new int[] { 1 }, new int[] { 3 } },
                new int [] { 1, 3 }
            };
            yield return new object[] {
                new int[][] { new int[] { 1, 3 }, new int[] { 4 }, new int[] { }, new int[] { 5, 6 } },
                new int [] { 1, 3, 4, 5, 6}
            };
        }

        public static IEnumerable<object[]> FirstMaybeData()
        {
            yield return new object[] {
                new int[] { },
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 0 },
                Maybe.Just(0)
            };
            yield return new object[] {
                new int[] { 3 },
                Maybe.Just(3)
            };
            yield return new object[] {
                new int[] { 3, 2, 1 },
                Maybe.Just(3)
            };
        }

        public static IEnumerable<object[]> FirstMaybePredicateData()
        {
            Func<int, bool> isEven = i => i % 2 == 0;

            yield return new object[] {
                new int[] { },
                isEven,
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 0 },
                isEven,
                Maybe.Just(0)
            };
            yield return new object[] {
                new int[] { 3 },
                isEven,
                Maybe.Nothing<int>()
            };
            yield return new object[] {
                new int[] { 3, 2, 1 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 2, 4, 1 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 2, 1, 6 },
                isEven,
                Maybe.Just(2)
            };
            yield return new object[] {
                new int[] { 3, 5, 5, 6 },
                isEven,
                Maybe.Just(6)
            };
            yield return new object[] {
                new int[] { 3, 3, 5, 7 },
                isEven,
                Maybe.Nothing<int>()
            };
        }

        public static IEnumerable<object[]> ProductDecimalTest1Data()
        {
            yield return new object[] { new decimal[] { }, 1 };
            yield return new object[] { new decimal[] { 1 }, 1 };
            yield return new object[] { new decimal[] { 2 }, 2 };
            yield return new object[] { new decimal[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal[] { 2, 3, 3.4M, 4 }, 81.6M };
        }

        public static IEnumerable<object[]> ProductIntTest2Data()
        {
            yield return new object[] { new(string, int)[] { }, 1 };
            yield return new object[] { new(string, int)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, int)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, int)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, int)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
        }

        public static IEnumerable<object[]> ProductLongTest2Data()
        {
            yield return new object[] { new(string, long)[] { }, 1 };
            yield return new object[] { new(string, long)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, long)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, long)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, long)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
        }

        public static IEnumerable<object[]> ProductFloatTest2Data()
        {
            yield return new object[] { new(string, float)[] { }, 1 };
            yield return new object[] { new(string, float)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, float)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, float)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, float)[] { ("b", 2), ("c", 3), ("c4", 3.4F), ("d", 4) }, 81.6F };
        }

        public static IEnumerable<object[]> ProductDoubleTest2Data()
        {
            yield return new object[] { new(string, double)[] { }, 1 };
            yield return new object[] { new(string, double)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, double)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, double)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, double)[] { ("b", 2), ("c", 3), ("c4", 3.4D), ("d", 4) }, 81.6D };
        }

        public static IEnumerable<object[]> ProductDecimalTest2Data()
        {
            yield return new object[] { new(string, decimal)[] { }, 1 };
            yield return new object[] { new(string, decimal)[] { ("a", 1) }, 1 };
            yield return new object[] { new(string, decimal)[] { ("b", 2) }, 2 };
            yield return new object[] { new(string, decimal)[] { ("b", 2), ("c", 3), ("d", 4) }, 24 };
            yield return new object[] { new(string, decimal)[] { ("b", 2), ("c", 3), ("c4", 3.4M), ("d", 4) }, 81.6M };
        }

        public static IEnumerable<object[]> ProductIntTest3Data()
        {
            yield return new object[] { new int?[] { }, 1 };
            yield return new object[] { new int?[] { null }, 1 };
            yield return new object[] { new int?[] { 1 }, 1 };
            yield return new object[] { new int?[] { 2 }, 2 };
            yield return new object[] { new int?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new int?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new int?[] { 2, 3, null, null, 4, null }, 24 };
        }

        public static IEnumerable<object[]> ProductLongTest3Data()
        {
            yield return new object[] { new long?[] { }, 1 };
            yield return new object[] { new long?[] { null }, 1 };
            yield return new object[] { new long?[] { 1 }, 1 };
            yield return new object[] { new long?[] { 2 }, 2 };
            yield return new object[] { new long?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new long?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new long?[] { 2, 3, null, null, 4, null }, 24 };
        }

        public static IEnumerable<object[]> ProductFloatTest3Data()
        {
            yield return new object[] { new float?[] { }, 1 };
            yield return new object[] { new float?[] { null }, 1 };
            yield return new object[] { new float?[] { 1 }, 1 };
            yield return new object[] { new float?[] { 2 }, 2 };
            yield return new object[] { new float?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new float?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new float?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new float?[] { 2, 3, 1.2F, null, 3.5F, null, 4, null }, 100.8F };
        }

        public static IEnumerable<object[]> ProductDoubleTest3Data()
        {
            yield return new object[] { new double?[] { }, 1 };
            yield return new object[] { new double?[] { null }, 1 };
            yield return new object[] { new double?[] { 1 }, 1 };
            yield return new object[] { new double?[] { 2 }, 2 };
            yield return new object[] { new double?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new double?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new double?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new double?[] { 2, 3, 1.2D, null, 3.5D, null, 4, null }, 100.8D };
        }

        public static IEnumerable<object[]> ProductDecimalTest3Data()
        {
            yield return new object[] { new decimal?[] { }, 1 };
            yield return new object[] { new decimal?[] { null }, 1 };
            yield return new object[] { new decimal?[] { 1 }, 1 };
            yield return new object[] { new decimal?[] { 2 }, 2 };
            yield return new object[] { new decimal?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, 4 }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, null, null, 4, null }, 24 };
            yield return new object[] { new decimal?[] { 2, 3, 1.2M, null, 3.5M, null, 4, null }, 100.8M };
        }

        public static IEnumerable<object[]> Unzip1Data()
        {
            yield return new object[] {
                new Person[] { },
                new string[] { },
                new int[] { }
            };

            yield return new object[] {
                new [] { new Person("alice", 15 )},
                new [] { "alice" },
                new [] { 15 }
            };

            yield return new object[] {
                new [] { new Person("alice", 15 ), new Person("bob", 30), new Person("cecil", 12 )},
                new [] { "alice", "bob", "cecil" },
                new [] { 15, 30, 12 }
            };
        }

        public static IEnumerable<object[]> UnzipTupleData()
        {
            yield return new object[] {
                new (string, int)[] { },
                new string[] { },
                new int[] { }
            };

            yield return new object[] {
                new [] { ("alice", 15 )},
                new [] { "alice" },
                new [] { 15 }
            };

            yield return new object[] {
                new [] { ("alice", 15 ), ("bob", 30), ("cecil", 12 )},
                new [] { "alice", "bob", "cecil" },
                new [] { 15, 30, 12 }
            };
        }

        public static IEnumerable<object[]> UnzipTuple3Data()
        {
            yield return new object[] {
                new (string, int, float)[] { },
                new string[] { },
                new int[] { },
                new float[] { }
            };

            yield return new object[] {
                new [] { ("alice", 15, 165f )},
                new [] { "alice" },
                new [] { 15 },
                new [] { 165f }
            };

            yield return new object[] {
                new [] { ("alice", 15, 165f ), ("bob", 30, 178f ), ("cecil", 12, 154f )},
                new [] { "alice", "bob", "cecil" },
                new [] { 15, 30, 12 },
                new [] { 165f, 178f, 154f }
            };
        }

        public static IEnumerable<object[]> UnzipTuple4Data()
        {
            yield return new object[] {
                new (string, int, float, bool)[] { },
                new string[] { },
                new int[] { },
                new float[] { },
                new bool[] { }
            };

            yield return new object[] {
                new [] { ("alice", 15, 165f, true )},
                new [] { "alice" },
                new [] { 15 },
                new [] { 165f },
                new [] { true }
            };

            yield return new object[] {
                new [] { ("alice", 15, 165f, true ), ("bob", 30, 178f, false ), ("cecil", 12, 154f, false )},
                new [] { "alice", "bob", "cecil" },
                new [] { 15, 30, 12 },
                new [] { 165f, 178f, 154f },
                new [] { true, false, false }
            };
        }

        public static IEnumerable<object[]> ZipTupleData()
        {
            yield return new object[] {
                new int[] { },
                new bool[] { },
                new (int, bool)[] { }
            };

            yield return new object[] {
                new int[] { 3 },
                new bool[] { },
                new (int, bool)[] { }
            };

            yield return new object[] {
                new int[] { 3, 5, 6 },
                new bool[] { },
                new (int, bool)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { true },
                new (int, bool)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { false, true, true },
                new (int, bool)[] { }
            };

            yield return new object[] {
                new int[] { 3 },
                new bool[] { false },
                new (int, bool)[] { (3, false) }
            };

            yield return new object[] {
                new int[] { 3, 5, 6 },
                new bool[] { false, true, false },
                new (int, bool)[] { (3, false), (5, true), (6, false) }
            };

            yield return new object[] {
                new int[] { 3, 5, 6, 7 },
                new bool[] { false, true, false },
                new (int, bool)[] { (3, false), (5, true), (6, false) }
            };

            yield return new object[] {
                new int[] { 3, 5, 6 },
                new bool[] { false, true, false, true, true },
                new (int, bool)[] { (3, false), (5, true), (6, false) }
            };
        }

        public static IEnumerable<object[]> ZipTuple3Data()
        {
            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { false },
                new string[] { },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { "bla" },
                new (int, bool, string)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { "bla" },
                new (int, bool, string)[] { (5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { false },
                new string[] { "bla" },
                new (int, bool, string)[] { (5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false, true },
                new string[] { "bla" },
                new (int, bool, string)[] { (5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { "bla", "blu" },
                new (int, bool, string)[] { (5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { false, true },
                new string[] { "bla", "blu" },
                new (int, bool, string)[] { (5, false, "bla"), (6, true, "blu") }
            };
        }

        public static IEnumerable<object[]> ZipTuple4Data()
        {
            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { false },
                new string[] { },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { "bla" },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] {  },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] {  },
                new bool[] { true },
                new string[] { },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { "bla" },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d },
                new (int, bool, string, double)[] { }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d },
                new (int, bool, string, double)[] { (5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true, false },
                new string[] { "bla" },
                new double[] { 6d },
                new (int, bool, string, double)[] { (5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla", "blu" },
                new double[] { 6d },
                new (int, bool, string, double)[] { (5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d, 7d },
                new (int, bool, string, double)[] { (5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { true, false },
                new string[] { "bla", "blu" },
                new double[] { 6d, 19d },
                new (int, bool, string, double)[] { (5, true, "bla", 6d), (6, false, "blu", 19d) }
            };
        }

        public static IEnumerable<object[]> Zip3Data()
        {
            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { false },
                new string[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { "bla" },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { "bla" },
                new ZipRecord[] { new ZipRecord(5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { false },
                new string[] { "bla" },
                new ZipRecord[] { new ZipRecord(5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false, true },
                new string[] { "bla" },
                new ZipRecord[] { new ZipRecord(5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { "bla", "blu" },
                new ZipRecord[] { new ZipRecord(5, false, "bla") }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { false, true },
                new string[] { "bla", "blu" },
                new ZipRecord[] { new ZipRecord(5, false, "bla"), new ZipRecord(6, true, "blu") }
            };
        }

        public static IEnumerable<object[]> Zip4Data()
        {
            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { false },
                new string[] { },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { false },
                new string[] { },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { "bla" },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5  },
                new bool[] { },
                new string[] { },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] {  },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] {  },
                new bool[] { true },
                new string[] { },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { },
                new string[] { "bla" },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { },
                new string[] { "bla" },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d },
                new ZipRecord[] { }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d },
                new ZipRecord[] { new ZipRecord(5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true, false },
                new string[] { "bla" },
                new double[] { 6d },
                new ZipRecord[] { new ZipRecord(5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla", "blu" },
                new double[] { 6d },
                new ZipRecord[] { new ZipRecord(5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5 },
                new bool[] { true },
                new string[] { "bla" },
                new double[] { 6d, 7d },
                new ZipRecord[] { new ZipRecord(5, true, "bla", 6d) }
            };

            yield return new object[] {
                new int[] { 5, 6 },
                new bool[] { true, false },
                new string[] { "bla", "blu" },
                new double[] { 6d, 19d },
                new ZipRecord[] { new ZipRecord(5, true, "bla", 6d), new ZipRecord(6, false, "blu", 19d) }
            };
        }

        public static IEnumerable<object[]> ZipManyData()
        {
            yield return new object[] {
                new int[][]
                {
                },
                new int[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[0]
                },
                new int[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[0],
                    new int[0],
                    new int[0]
                },
                new int[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[] {3},
                    new int[] {5},
                    new int[0]
                },
                new int[] { 8 }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[] {3,4,5},
                    new int[0],
                    new int[] {5,6,7,8}
                },
                new int[] { 8, 10, 12, 8}
            };
        }

        public static IEnumerable<object[]> ZipManyListsData()
        {
            yield return new object[] {
                new int[][]
                {
                },
                new List<int>[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[0]
                },
                new List<int>[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[0],
                    new int[0],
                    new int[0]
                },
                new List<int>[] { }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[] {3},
                    new int[] {5},
                    new int[0]
                },
                new List<int>[] { new List<int> { 3, 5 } }
            };

            yield return new object[] {
                new int[][]
                {
                    new int[] {3,4,5},
                    new int[0],
                    new int[] {5,6,7,8}
                },
                new List<int>[] { new List<int> { 3,5 }, new List<int> { 4, 6 }, new List<int> { 5, 7 }, new List<int> { 8 } }
            };
        }

        public static IEnumerable<object[]> ZipWithStreamData()
        {
            yield return new object[] {
                new string[] { },
                new (string, int)[] { }
            };

            yield return new object[] {
                new string[] { "a" },
                new (string, int)[] { ("a", 5) }
            };

            yield return new object[] {
                new string[] { "a", "b", "c" },
                new (string, int)[] { ("a", 5), ("b", 7), ("c", 9) }
            };
        }
    }
}
