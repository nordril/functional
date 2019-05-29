using Nordril.Functional.Algebra;
using Nordril.Functional.Category;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A rose tree, wherein each node has a single key and a list of children.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    public class Tree<T> : IFunctor<T>, ISemifilterable<Tree<T>, T>, IEquatable<Tree<T>>, ITreeVisitable<Tree<T>, Tree<T>, int, IEnumerable<(T, bool)>, TreeTraversal>
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

        /// <inheritdoc />
        public IEnumerable<(T, bool)> VisitBidirectional(IBidirectionalVisitor<Tree<T>, Tree<T>, int> visitor, TreeTraversal order)
            => Traverse(order, visitor.At, visitor.Forward, visitor.Backward);

        /// <inheritdoc />
        public IEnumerable<(T, bool)> VisitBidirectional(IBidirectionalVisitor<Tree<T>, Tree<T>, int> visitor)
            => Traverse(TreeTraversal.PreOrder, visitor.At, visitor.Forward, visitor.Backward);

        /// <inheritdoc />
        public IEnumerable<(T, bool)> VisitForward(IForwardVisitor<Tree<T>, int> visitor)
            => Traverse(TreeTraversal.PreOrder, visitor.At, visitor.Forward, null);

        /// <inheritdoc />
        public IEnumerable<(T, bool)> VisitBackward(IBackwardVisitor<Tree<T>, Tree<T>> visitor)
            => Traverse(TreeTraversal.PreOrder, visitor.At, null, visitor.Backward);

        /// <inheritdoc />
        public IEnumerable<(T, bool)> Visit(IVisitor<Tree<T>> visitor)
            => Traverse(TreeTraversal.PreOrder, visitor.At, null, null);

        /// <summary>
        /// Traverses the tree in a certain order and yields the nodes.
        /// Uses the visitor pattern.
        /// </summary>
        /// <param name="traversal">The type of the traversal.</param>
        /// <param name="visit">The action to execute at each node, if any.</param>
        /// <param name="down">The action to execute when entering a node's children. The arguments are the current node and the index of the child node.</param>
        /// <param name="up">The action to execute when leaving a node. The argument is the node being left.</param>
        /// <returns>The nodes of the tree, and whether each node is a leaf.</returns>
        private IEnumerable<(T, bool)> Traverse(TreeTraversal traversal, Action<Tree<T>> visit = null, Action<Tree<T>, int> down = null, Action<Tree<T>> up = null)
        {
            IEnumerable<Tree<T>> trav(Tree<T> tree, Tree<T> parent, int? parentIndex)
            {
                if (traversal == TreeTraversal.PreOrder)
                {
                    if (parentIndex != null)
                        down?.Invoke(parent, parentIndex.Value);
                    visit?.Invoke(tree);
                    yield return tree;
                }

                if (tree.IsInner)
                {
                    foreach (var subResult in tree.Children.Value().Select((t, i) => trav(t, tree, i)))
                        foreach (var (node, index) in subResult.ZipWithStream(0, i => i + 1))
                            yield return node;
                }

                if (traversal == TreeTraversal.PostOrder)
                {
                    visit?.Invoke(tree);
                    yield return tree;
                }

                up?.Invoke(tree);
            };

            foreach (var n in trav(this, null, null))
                yield return (n.Key, n.IsLeaf);
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
        public TResult FoldMap<TResult>(IMonoid<TResult> monoid, Func<T, TResult> f)
        {
            return this.Aggregate(monoid.Neutral, (x, y) => monoid.Op(x, f(y)));
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

        /// <inheritdoc />
        public Maybe<Tree<T>> Semifilter(Func<T, bool> f)
        {
            if (!f(Key))
                return Maybe.Nothing<Tree<T>>();
            else if (IsLeaf)
                return Maybe.Just(Tree.MakeLeaf(Key));
            else
            {
                var children = Children.Value().Select(t => t.Semifilter(f)).Where(t => t.HasValue).Select(t => t.Value());

                return Maybe.Just(Tree.MakeInner(Key, children));
            }
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

        /// <summary>
        /// Takes a directory and returns a tree representing that directory as its root and the sub-directories and files as child-nodes, recursively. An <see cref="Either.FromLeft{TLeft, TRight}(TLeft)"/> represents a directory and will always be an inner-node, and an <see cref="Either.FromRight{TLeft, TRight}(TRight)"/> represents a file. No file will have a directory as a child.
        /// <br />
        /// This method uses direct recursion and thus required O(n) stack space, where n is the maximum depth of the directory structure. Symlinks are not traversed. As there are no filesystem-level locks, this method may fail if a file or directory is deleted partway through the computation.
        /// </summary>
        /// <param name="directory">The path to the directory.</param>
        /// <param name="fullName">Where to use the full names of filesystem-entries.</param>
        /// <param name="predicate">The optional predicate to which a path (except for the root-node) has to conform to be included as a node. The argument is always the full path of the filesystem-entry.</param>
        /// <exception cref="InvalidOperationException">If the directory <paramref name="directory"/> does not exist.</exception>
        /// <exception cref="DirectoryNotFoundException">A directory path is invalid, such as referring to an unmapped drive or having been deleted.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="StackOverflowException">If the maximum stack size has been exceeded.</exception>
        public static Tree<Either<string, string>> RetrieveDirectoryStructure(string directory, PathNameUsage fullName = PathNameUsage.Always, Func<string, bool> predicate = null)
        {
            predicate = predicate ?? (x => true);

            if (!Directory.Exists(directory))
                throw new InvalidOperationException();

            var key = (fullName == PathNameUsage.Always || fullName == PathNameUsage.RootOnly)
                ? directory
                : Path.GetDirectoryName(directory);
            var ret = MakeInner(Either.FromLeft<string, string>(key));
            var children = ret.Children.Value();
            var fullNameRec = fullName == PathNameUsage.Always ? fullName : PathNameUsage.Never;
            var useFullNameForFiles = fullName == PathNameUsage.Always;

            foreach (var entry in Directory.EnumerateDirectories(directory).Where(predicate))
                children.Add(RetrieveDirectoryStructure(entry, fullNameRec));

            foreach (var entry in Directory.EnumerateFiles(directory).Where(predicate))
                children.Add(
                    MakeLeaf(
                        Either.FromRight<string, string>(
                            useFullNameForFiles ? entry : Path.GetFileName(entry))));

            return ret;
        }
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

    /// <summary>
    /// Where the full names of filesystem-entries should be used.
    /// </summary>
    public enum PathNameUsage
    {
        /// <summary>
        /// The full name should always be used.
        /// </summary>
        Always,
        /// <summary>
        /// The full name should only be used at the root.
        /// </summary>
        RootOnly,
        /// <summary>
        /// The full name should be used nowhere.
        /// </summary>
        Never
    }
}
