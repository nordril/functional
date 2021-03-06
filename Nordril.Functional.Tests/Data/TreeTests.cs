﻿using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Nordril.Functional.Tests.Data
{
    public class TreeTests
    {
        public static IEnumerable<object[]> MappedTreesRounded()
        {
            yield return new object[] {
                Tree.MakeLeaf(5f),
                Tree.MakeLeaf(5)
            };
            yield return new object[] {
                Tree.MakeLeaf(5.6f),
                Tree.MakeLeaf(6)
            };
            yield return new object[] {
                Tree.MakeLeaf(5.1f),
                Tree.MakeLeaf(5)
            };
            yield return new object[] {
                Tree.MakeInner(5.6f, new[] { Tree.MakeLeaf(3.1f), Tree.MakeLeaf(7.7f) } ),
                Tree.MakeInner(6, new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(8) } )
            };
            yield return new object[] {
                Tree.MakeInner(5.6f, new[] { Tree.MakeInner(3.1f, new[] { Tree.MakeLeaf(2.3f) }), Tree.MakeLeaf(7.7f) } ),
                Tree.MakeInner(6, new[] { Tree.MakeInner(3, new[] { Tree.MakeLeaf(2) }), Tree.MakeLeaf(8) } )
            };
        }

        public static IEnumerable<object[]> SampleTrees()
        {
            yield return new object[] {
                Tree.MakeLeaf(5)
            };
            yield return new object[] {
                Tree.MakeInner(6)
            };
            yield return new object[] {
                Tree.MakeInner(7, new[] { Tree.MakeLeaf(3) })
            };
            yield return new object[] {
                Tree.MakeInner(8, new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(7), Tree.MakeLeaf(9) })
            };
            yield return new object[] {
                Tree.MakeInner(9, new[] { Tree.MakeLeaf(3), Tree.MakeInner(12, new[] { Tree.MakeLeaf(9), Tree.MakeInner(22) }), Tree.MakeLeaf(87), Tree.MakeInner(76), Tree.MakeInner(143, new[] { Tree.MakeLeaf(96), Tree.MakeLeaf(11) }), Tree.MakeLeaf(88) })
            };
        }

        public static IEnumerable<object[]> TreeSums()
        {
            yield return new object[] {
                Tree.MakeLeaf(5),
                5
            };
            yield return new object[] {
                Tree.MakeInner(6),
                6
            };
            yield return new object[] {
                Tree.MakeInner(7, new[] { Tree.MakeLeaf(3) }),
                10
            };
            yield return new object[] {
                Tree.MakeInner(8, new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(7), Tree.MakeLeaf(9) }),
                27
            };
            yield return new object[] {
                Tree.MakeInner(9, new[] { Tree.MakeLeaf(3), Tree.MakeInner(12, new[] { Tree.MakeLeaf(9), Tree.MakeInner(22) }), Tree.MakeLeaf(87), Tree.MakeInner(76), Tree.MakeInner(143, new[] { Tree.MakeLeaf(96), Tree.MakeLeaf(11) }), Tree.MakeLeaf(88) }),
                556
            };
        }

        public static IEnumerable<object[]> TreeEnumFoldMapData()
        {
            yield return new object[] {
                Tree.MakeLeaf(5),
            };
            yield return new object[] {
                Tree.MakeInner(6),
            };
            yield return new object[] {
                Tree.MakeInner(7, new[] { Tree.MakeLeaf(3) }),
            };
            yield return new object[] {
                Tree.MakeInner(8, new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(7), Tree.MakeLeaf(9) }),
            };
            yield return new object[] {
                Tree.MakeInner(8, new[] { Tree.MakeInner(3, new[] { Tree.MakeLeaf(1), Tree.MakeLeaf(4) }), Tree.MakeInner(11, new[] { Tree.MakeLeaf(9), Tree.MakeLeaf(13) }) }),
            };
            yield return new object[] {
                Tree.MakeInner(9, new[] { Tree.MakeLeaf(3), Tree.MakeInner(12, new[] { Tree.MakeLeaf(9), Tree.MakeInner(22) }), Tree.MakeLeaf(87), Tree.MakeInner(76), Tree.MakeInner(143, new[] { Tree.MakeLeaf(96), Tree.MakeLeaf(11) }), Tree.MakeLeaf(88) }),
            };
        }

        public static IEnumerable<object[]> TreeStringConcats()
        {
            yield return new object[] {
                Tree.MakeLeaf("g"),
                "g"
            };
            yield return new object[] {
                Tree.MakeInner("g"),
                "g"
            };
            yield return new object[] {
                Tree.MakeInner("g", new[] { Tree.MakeLeaf("k") }),
                "gk"
            };
            yield return new object[] {
                Tree.MakeInner("g", new[] { Tree.MakeLeaf("b"), Tree.MakeLeaf("h"), Tree.MakeLeaf("l") }),
                "gbhl"
            };
            yield return new object[] {
                Tree.MakeInner("u", new[] { Tree.MakeLeaf("o"), Tree.MakeInner("p", new[] { Tree.MakeLeaf("n"), Tree.MakeInner("f") }), Tree.MakeLeaf("x"), Tree.MakeInner("y"), Tree.MakeInner("z", new[] { Tree.MakeLeaf("d"), Tree.MakeLeaf("a") }), Tree.MakeLeaf("r") }),
                "uopnfxyzdar"
            };
        }

        public static IEnumerable<object[]> TreeStringConcatsPostOrder()
        {
            yield return new object[] {
                Tree.MakeLeaf("g"),
                "g"
            };
            yield return new object[] {
                Tree.MakeInner("g"),
                "g"
            };
            yield return new object[] {
                Tree.MakeInner("g", new[] { Tree.MakeLeaf("k") }),
                "kg"
            };
            yield return new object[] {
                Tree.MakeInner("g", new[] { Tree.MakeLeaf("b"), Tree.MakeLeaf("h"), Tree.MakeLeaf("l") }),
                "bhlg"
            };
            yield return new object[] {
                Tree.MakeInner("u", new[] { Tree.MakeLeaf("o"), Tree.MakeInner("p", new[] { Tree.MakeLeaf("n"), Tree.MakeInner("f") }), Tree.MakeLeaf("x"), Tree.MakeInner("y"), Tree.MakeInner("z", new[] { Tree.MakeLeaf("d"), Tree.MakeLeaf("a") }), Tree.MakeLeaf("r") }),
                "onfpxydazru"
            };
        }

        public static IEnumerable<object[]> DifferentTrees()
        {
            //Different key.
            yield return new object[] {
                Tree.MakeLeaf(5),
                Tree.MakeLeaf(7)
            };
            //Different IsLeaf.
            yield return new object[] {
                Tree.MakeLeaf(5),
                Tree.MakeInner(5)
            };
            //Different number of children.
            yield return new object[] {
                Tree.MakeInner(5, new [] { Tree.MakeLeaf(9), Tree.MakeLeaf(10) }),
                Tree.MakeInner(5, new[] { Tree.MakeLeaf(9) })
            };
            //Different children.
            yield return new object[] {
                Tree.MakeInner(5, new [] { Tree.MakeLeaf(9), Tree.MakeLeaf(10) }),
                Tree.MakeInner(5, new[] { Tree.MakeLeaf(9), Tree.MakeLeaf(11) })
            };
            yield return new object[] {
                Tree.MakeInner(5, new [] { Tree.MakeLeaf(9), Tree.MakeInner(10, new[] { Tree.MakeLeaf(20) }) }),
                Tree.MakeInner(5, new [] { Tree.MakeLeaf(9), Tree.MakeInner(10, new[] { Tree.MakeLeaf(21) }) }),
            };
        }

        public static IEnumerable<object[]> TreeDifferencesFoldr()
        {
            yield return new object[] {
                Tree.MakeLeaf(5),
                5
            };
            yield return new object[] {
                Tree.MakeInner(6),
                6
            };
            yield return new object[] {
                Tree.MakeInner(7, new[] { Tree.MakeLeaf(3) }),
                4
            };
            yield return new object[] {
                Tree.MakeInner(8, new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(7), Tree.MakeLeaf(9) }),
                3
            };
        }

        [Fact]
        public static void CreateLeaf()
        {
            var ret = Tree.MakeLeaf(6);

            Assert.True(ret.IsLeaf);
            Assert.Equal(6, ret.Key);
        }

        [Fact]
        public static void CreateInner()
        {
            var ret = Tree.MakeInner(6, new[] { Tree.MakeLeaf(4), Tree.MakeLeaf(9) });
            var leftChild = ret.Children.Value()[0];
            var rightChild = ret.Children.Value()[1];

            Assert.True(ret.IsInner);
            Assert.Equal(2, ret.Children.Value().Count);
            Assert.Equal(4, leftChild.Key);
            Assert.Equal(9, rightChild.Key);
            Assert.True(leftChild.IsLeaf);
            Assert.True(rightChild.IsLeaf);
            Assert.Equal(6, ret.Key);
        }

        [Fact]
        public static void AddParentToLeaf()
        {
            var leaf = Tree.MakeLeaf(10);
            var parent = leaf.AddParent(6);

            Assert.Equal(6, parent.Key);
            Assert.True(parent.IsInner);
            Assert.Equal(1, parent.Children.Value().Count);
            Assert.True(parent.Children.Value()[0].IsLeaf);
            Assert.Equal(10, parent.Children.Value()[0].Key);
        }

        [Fact]
        public static void SetNodeToLeaf()
        {
            var leaf = Tree.MakeLeaf(7);
            var inner = Tree.MakeInner(8, new[] { Tree.MakeLeaf(5), Tree.MakeLeaf(11) });

            leaf.SetToLeaf();
            inner.SetToLeaf();

            Assert.True(leaf.IsLeaf);
            Assert.True(inner.IsLeaf);
            Assert.Equal(7, leaf.Key);
            Assert.Equal(8, inner.Key);
        }

        [Fact]
        public static void SetNodeToInner()
        {
            var leaf = Tree.MakeLeaf(7);
            var inner = Tree.MakeInner(8, new[] { Tree.MakeLeaf(5), Tree.MakeLeaf(11) });

            leaf.SetToInner(new[] { Tree.MakeLeaf(3), Tree.MakeLeaf(9) });
            inner.SetToInner(new[] { Tree.MakeLeaf(1), Tree.MakeLeaf(3), Tree.MakeLeaf(15) });

            Assert.True(leaf.IsInner);
            Assert.True(inner.IsInner);
            Assert.Equal(7, leaf.Key);
            Assert.Equal(8, inner.Key);
            Assert.Equal(2, leaf.Children.Value().Count);
            Assert.Equal(3, inner.Children.Value().Count);
            Assert.Equal(new[] { 3, 9 }, leaf.Children.Value().Select(x => x.Key));
            Assert.Equal(new[] { 1, 3, 15 }, inner.Children.Value().Select(x => x.Key));
        }

        [Theory]
        [MemberData(nameof(MappedTreesRounded))]
        public static void MapTree(Tree<float> inTree, Tree<int> outTree)
        {
            var res = inTree.Map(x => (int)Math.Round(x)) as Tree<int>;
            Assert.True(res.Equals(outTree));
        }

        [Theory]
        [MemberData(nameof(DifferentTrees))]
        public static void TreeNotEqualsToDifferentTrees(Tree<int> thisTree, Tree<int> thatTree)
        {
            Assert.False(thisTree.Equals(thatTree));
            Assert.False(thatTree.Equals(thisTree));
        }

        [Theory]
        [MemberData(nameof(SampleTrees))]
        public static void TreeEqualsReflexiveSymmetric(Tree<int> tree)
        {
            //Comparing with the same object.
            Assert.True(tree.Equals(tree));

            //Comparing with a copy.
            var copy = tree.Map(x => x) as Tree<int>;

            Assert.True(tree.Equals(copy));
            Assert.True(copy.Equals(tree));
        }

        [Theory]
        [MemberData(nameof(TreeSums))]
        public static void FoldMapTreeSum(Tree<int> t, int sum)
        {
            var treeSum = t.FoldMap(new Monoid<int>(0, (x, y) => x + y), x => x);

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeStringConcats))]
        public static void FoldMapTreeStringConcat(Tree<string> t, string sum)
        {
            var treeSum = t.FoldMap(new Monoid<string>("", (x, y) => x + y), x => x);

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeSums))]
        public static void FoldMapTreeSumDouble(Tree<int> t, int sum)
        {
            var treeSum = t.FoldMap(new Monoid<int>(0, (x, y) => x + y), x => x * 2);

            Assert.Equal(2 * sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeEnumFoldMapData))]
        public static void FoldMapTreeEnum(Tree<int> t)
        {
            var expected = t.Select(x => x * 2).ToList();
            var actual = t.FoldMap(Monoid.ListAppend<int>(), x => new List<int> { x * 2 });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(TreeStringConcats))]
        public static void FoldrStringConcat(Tree<string> t, string sum)
        {
            var treeSum = t.Foldr((x, acc) => x + acc, "");

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeDifferencesFoldr))]
        public static void FoldrDifference(Tree<int> t, int sum)
        {
            var treeSum = t.Foldr((x, acc) => x - acc, 0);

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeStringConcats))]
        public static void TraversePreOrder(Tree<string> t, string sum)
        {
            var treeSum = t.TraverseTree(TreeTraversal.PreOrder).Select(x => x.Item1).Aggregate("", (x, y) => x + y);

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeStringConcatsPostOrder))]
        public static void TraversePostOrder(Tree<string> t, string sum)
        {
            var treeSum = t.TraverseTree(TreeTraversal.PostOrder).Select(x => x.Item1).Aggregate("", (x, y) => x + y);

            Assert.Equal(sum, treeSum);
        }

        [Fact]
        public static void TraversePreOrderVisitors()
        {
            var tree = Tree.MakeInner(10, new[] {
                Tree.MakeInner(5, new [] {
                    Tree.MakeLeaf(2),
                    Tree.MakeLeaf(7) }),
                Tree.MakeInner(15, new [] {
                    Tree.MakeInner(12, new[] {
                        Tree.MakeLeaf(11) }),
                    Tree.MakeLeaf(14),
                    Tree.MakeLeaf(17)})
                });

            var visitorResultsExpected = new[] {
                "visit 10",
                "down 10 0",
                "visit 5",
                "down 5 0",
                "visit 2",
                "up 2",
                "down 5 1",
                "visit 7",
                "up 7",
                "up 5",
                "down 10 1",
                "visit 15",
                "down 15 0",
                "visit 12",
                "down 12 0",
                "visit 11",
                "up 11",
                "up 12",
                "down 15 1",
                "visit 14",
                "up 14",
                "down 15 2",
                "visit 17",
                "up 17",
                "up 15",
                "up 10"
            };

            var visitorResultsActual = new List<string>();

            tree.VisitBidirectional(VisitorFactory.MakeVisitor<Tree<int>, Tree<int>, int>(
                t => visitorResultsActual.Add($"visit {t.Key}"),
                (t, i) => visitorResultsActual.Add($"down {t.Key} {i}"),
                t => visitorResultsActual.Add($"up {t.Key}"))).ToList();

            Assert.Equal(visitorResultsExpected, visitorResultsActual);
        }

        [Theory]
        [InlineData(PathNameUsage.Always)]
        [InlineData(PathNameUsage.RootOnly)]
        [InlineData(PathNameUsage.Never)]
        public static void RetrieveDirectoryStructureOnDirectory(PathNameUsage usage)
        {
            var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                var expected = CreateTree(dir);

                if (usage == PathNameUsage.RootOnly || usage == PathNameUsage.Never)
                {
                    expected = (Tree<Either<string, string>>)expected.Map(e
                        => (Either<string, string>)e.BiMap(d => Path.GetDirectoryName(d), f => Path.GetFileName(f)));
                }
                if (usage == PathNameUsage.RootOnly)
                    expected.Key = (Either<string, string>)expected.Key.LeftMap(_ => dir);

                var actual = Tree.RetrieveDirectoryStructure(dir, usage);

                Assert.Equal(expected, actual);

            } finally
            {
                DeleteTree(dir);
            }
        }

        private static Tree<Either<string, string>> CreateTree(string dir)
        {
            Either<string, string> mkDir(params string[] d) => Either.FromLeft<string, string>(Path.Combine(d));
            Either<string, string> mkFile(params string[] d) => Either.FromRight<string, string>(Path.Combine(d));

            Directory.CreateDirectory(dir);

            //1st level
            Directory.CreateDirectory(Path.Combine(dir, "a"));
            Directory.CreateDirectory(Path.Combine(dir, "b"));
            Directory.CreateDirectory(Path.Combine(dir, "c"));

            File.Create(Path.Combine(dir, "f1")).Close();
            File.Create(Path.Combine(dir, "f2")).Close();
            File.Create(Path.Combine(dir, "f3")).Close();
            File.Create(Path.Combine(dir, "f4")).Close();

            //2nd level
            Directory.CreateDirectory(Path.Combine(dir, "a", "aa"));
            File.Create(Path.Combine(dir, "a", "af1")).Close();
            Directory.CreateDirectory(Path.Combine(dir, "b", "ba"));
            File.Create(Path.Combine(dir, "c", "cf1")).Close();
            File.Create(Path.Combine(dir, "c", "cf2")).Close();

            //3rd level
            Directory.CreateDirectory(Path.Combine(dir, "a", "aa", "aaa"));

            var expected = Tree.MakeInner(mkDir(dir), new[]
                {
                    Tree.MakeInner(mkDir(dir, "a"), new []
                    {
                        Tree.MakeInner(mkDir(dir, "a", "aa"), new [] {
                            Tree.MakeInner(mkDir(dir, "a", "aa", "aaa"))
                        }),
                        Tree.MakeLeaf(mkFile(dir, "a", "af1"))
                    }),
                    Tree.MakeInner(mkDir(dir, "b"), new [] {
                        Tree.MakeInner(mkDir(dir, "b", "ba"))
                    }),
                    Tree.MakeInner(mkDir(dir, "c"), new [] {
                        Tree.MakeLeaf(mkFile(dir, "c", "cf1")),
                        Tree.MakeLeaf(mkFile(dir, "c", "cf2"))
                    }),
                    Tree.MakeLeaf(mkFile(dir, "f1")),
                    Tree.MakeLeaf(mkFile(dir, "f2")),
                    Tree.MakeLeaf(mkFile(dir, "f3")),
                    Tree.MakeLeaf(mkFile(dir, "f4")),
                });

            return expected;
        }

        private static void DeleteTree(string dir)
        {
            Directory.CreateDirectory(dir);

            File.Delete(Path.Combine(dir, "f1"));
            File.Delete(Path.Combine(dir, "f2"));
            File.Delete(Path.Combine(dir, "f3"));
            File.Delete(Path.Combine(dir, "f4"));
            File.Delete(Path.Combine(dir, "a", "af1"));
            File.Delete(Path.Combine(dir, "c", "cf1"));
            File.Delete(Path.Combine(dir, "c", "cf2"));

            Directory.Delete(Path.Combine(dir, "a", "aa", "aaa"));
            Directory.Delete(Path.Combine(dir, "a", "aa"));
            Directory.Delete(Path.Combine(dir, "b", "ba"));
            Directory.Delete(Path.Combine(dir, "a"));
            Directory.Delete(Path.Combine(dir, "b"));
            Directory.Delete(Path.Combine(dir, "c"));
        }
    }
}
