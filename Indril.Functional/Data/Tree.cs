using Indril.Functional.Algebra;
using Indril.Functional.CategoryTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Indril.Functional.Data
{
    /// <summary>
    /// A rose tree, wherein each node has a single key and a list of children.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    [SuppressMessage("Microsoft.Design","CA1724", Justification="That's what they're called.")]
    public class Tree<T> : IFunctor<T>, IFoldable<T>, IEquatable<Tree<T>>
    {
        /// <summary>
        /// The node's key.
        /// </summary>
        public T Key { get; set; }
        /// <summary>
        /// The list of the node's children, if the node is an inner node.
        /// </summary>
        public Maybe<IFuncList<Tree<T>>> Children { get; set; }

        /// <summary>
        /// Whether the node is a leaf. A leaf can have no children, though a node without children is not necessarily a leaf
        /// (an example being a directory without files in it).
        /// </summary>
        public bool IsLeaf { get; private set; }

        /// <summary>
        /// Whether the node is an inner node. This is the opposite of <see cref="IsLeaf"/>.
        /// </summary>
        public bool IsInner => !IsLeaf;

        private Tree()
        {
        }

        /// <summary>
        /// Sets the type of the node to a leaf.
        /// </summary>
        public void SetToLeaf()
        {
            IsLeaf = true;
            Children = Maybe.Nothing<IFuncList<Tree<T>>>();
        }

        /// <summary>
        /// Sets the type of the node to an inner node and optionally gives it children.
        /// </summary>
        /// <param name="children">The children of the node.</param>
        public void SetToInner(IEnumerable<Tree<T>> children = null)
        {
            IsLeaf = false;
            Children = Maybe.Just<IFuncList<Tree<T>>>(new FuncList<Tree<T>>(children));
        }

        /// <summary>
        /// Returns a new tree with <paramref name="newParent"/> as its root
        /// and this being its only child. The tree is not copied.
        /// </summary>
        /// <param name="newParent">The key that should be the new root of the tree.</param>
        public Tree<T> AddParent(T newParent) => MakeInner(newParent, new List<Tree<T>> { this });

        /// <summary>
        /// Creates an inner node.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        /// <param name="children">The children of the node.</param>
        public static Tree<T> MakeInner(T key, IEnumerable<Tree<T>> children = null)
        {
            var ret = new Tree<T>()
            {
                IsLeaf = false,
                Key = key,
                Children = Maybe.Just<IFuncList<Tree<T>>>(new FuncList<Tree<T>>(children))
            };
            return ret;
        }

        /// <summary>
        /// Creates a leaf.
        /// </summary>
        /// <param name="key">The key.</param>
        public static Tree<T> MakeLeaf(T key)
        {
            var ret = new Tree<T>()
            {
                IsLeaf = true,
                Key = key
            };
            return ret;
        }

        /// <summary>
        /// Traverses the tree in a certain order and yields the nodes.
        /// </summary>
        /// <param name="traversal">The type of the traversal.</param>
        /// <returns>The nodes of the tree, and whether each node is a leaf.</returns>
        public IEnumerable<(T, bool)> Traverse(TreeTraversal traversal)
        {
            IEnumerable<(T, bool)> trav(Tree<T> tree)
            {
                if (traversal == TreeTraversal.PreOrder)
                    yield return (tree.Key, tree.IsLeaf);

                if (tree.IsInner)
                    foreach (var subResult in tree.Children.Value().Select(trav))
                        foreach (var node in subResult)
                            yield return node;

                if (traversal == TreeTraversal.PostOrder)
                    yield return (tree.Key, tree.IsLeaf);
            };

            foreach (var n in trav(this))
                yield return n;
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<T, TResult> f)
        {
            if (IsLeaf)
                return Tree.MakeLeaf(f(Key));
            else
                return Tree.MakeInner(f(Key), Children.Value().Select(c => (Tree<TResult>)c.Map(f)).ToList());
        }
        
        /// <inheritdoc />
        public TResult FoldMap<TResult>(Monoid<TResult> monoid, Func<T, TResult> f)
        {
            if (IsLeaf)
                return f(Key);
            else
                return monoid.Op(f(Key), Children.Value().Select(c => c.FoldMap(monoid, f)).Msum(monoid));
        }

        /// <inheritdoc />
        public TResult Foldr<TResult>(Func<T, TResult, TResult> f, TResult accumulator)
        {
            if (IsLeaf || Children.Value().Count == 0)
                return f(Key, accumulator);
            else if (Children.Value().Count == 1)
                return f(Key, Children.Value()[0].Foldr(f, accumulator));
            else
            {
                var revChildren = Children.Value().Reverse();
                var head = revChildren.First();
                var tail = revChildren.Skip(1);

                return f(Key, Children.Value().Foldr((child, childAcc) => child.Foldr(f, childAcc), accumulator));
            }
        }

        private static TB ListFoldr<TA, TB>(Func<TA,TB,TB> f, TB acc, IEnumerable<TA> xs)
        {
            if (!xs.Any())
                return acc;
            else
                return f(xs.First(), ListFoldr(f, acc, xs.Skip(1)));
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => Traverse(TreeTraversal.PreOrder).Select(x => x.Item1).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Compares two trees structurally. Two trees are structurally equal if:
        /// <br />
        /// <list type="number">
        ///     <item><see cref="IsLeaf"/> has the same value for both,</item>,
        ///     <item>The keys are equal according to <see cref="object.Equals(object)"/>.</item>
        ///     <item><see cref="Children"/>, if <see cref="IsInner"/> is true, are of the same number and are pairwise equal according to <see cref="Equals(Tree{T})"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="other">The other tree.</param>
        public bool Equals(Tree<T> other)
        {
            if (other == null || IsLeaf != other.IsLeaf || !Key.Equals(other.Key))
                return false;

            if (IsLeaf)
                return true;
            else if (Children.Value().Count != other.Children.Value().Count)
                return false;
            else
                return Children.Value().Zip(other.Children.Value(), (x, y) => x.Equals(y)).All(x => x);
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Tree{T}"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1724", Justification = "That's what they're called.")]
    public static class Tree
    {
        /// <summary>
        /// Creates an inner node.
        /// </summary>
        /// <typeparam name="T">The type of the keys.</typeparam>
        /// <param name="key">The key of the node.</param>
        /// <param name="children">The children of the node.</param>
        public static Tree<T> MakeInner<T>(T key, IEnumerable<Tree<T>> children = null) => Tree<T>.MakeInner(key, children);

        /// <summary>
        /// Creates a leaf.
        /// </summary>
        /// <typeparam name="T">The type of the keys.</typeparam>
        /// <param name="key">The key of the node.</param>
        public static Tree<T> MakeLeaf<T>(T key) => Tree<T>.MakeLeaf(key);
    }

    /// <summary>
    /// A type of tree traversal.
    /// </summary>
    public enum TreeTraversal
    {
        /// <summary>
        /// First the node, then its children.
        /// </summary>
        PreOrder,
        /// <summary>
        /// First the children, then the node.
        /// </summary>
        PostOrder
    }
}
