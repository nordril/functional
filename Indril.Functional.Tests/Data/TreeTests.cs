using Indril.Functional.Algebra;
using Indril.Functional.CategoryTheory;
using Indril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Indril.Functional.Tests.Data
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
            var treeSum = t.FoldMap(new Monoid<int>(0, (x, y) => x + y), x => x*2);

            Assert.Equal(2*sum, treeSum);
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
            var treeSum = t.Traverse(TreeTraversal.PreOrder).Select(x => x.Item1).Aggregate("", (x, y) => x + y);

            Assert.Equal(sum, treeSum);
        }

        [Theory]
        [MemberData(nameof(TreeStringConcatsPostOrder))]
        public static void TraversePostOrder(Tree<string> t, string sum)
        {
            var treeSum = t.Traverse(TreeTraversal.PostOrder).Select(x => x.Item1).Aggregate("", (x, y) => x + y);

            Assert.Equal(sum, treeSum);
        }
    }
}
