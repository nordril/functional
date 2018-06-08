using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Indril.Functional.Data
{
    /// <summary>
    /// A rose tree, wherein each node has a single key and a list of children.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    [SuppressMessage("Microsoft.Design","CA1724", Justification="That's what they're called.")]
    public class Tree<T>
    {
        /// <summary>
        /// The node's key.
        /// </summary>
        public T Key { get; set; }
        /// <summary>
        /// The list of the node's children.
        /// </summary>
        public IList<Tree<T>> Children { get; set; }

        /// <summary>
        /// Whether the node is a leaf. A leaf can have no children, though a node without children is not necessarily a leaf
        /// (an example being a directory without files in it).
        /// </summary>
        public bool IsLeaf { get; private set; }

        private Tree()
        {
        }

        /// <summary>
        /// Sets the type of the node to a leaf.
        /// </summary>
        public void SetToLeaf()
        {
            IsLeaf = true;
            Children = null;
        }

        /// <summary>
        /// Sets the type of the node to an inner node and optionally gives it children.
        /// </summary>
        /// <param name="children">The children of the node.</param>
        public void SetToInner(IList<Tree<T>> children = null)
        {
            IsLeaf = true;
            Children = children ?? new List<Tree<T>>();
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
        public static Tree<T> MakeInner(T key, IList<Tree<T>> children = null)
        {
            var ret = new Tree<T>()
            {
                IsLeaf = false,
                Key = key,
                Children = children ?? new List<Tree<T>>()
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
        public static Tree<T> MakeInner<T>(T key, IList<Tree<T>> children = null) => Tree<T>.MakeInner(key, children);

        /// <summary>
        /// Creates a leaf.
        /// </summary>
        /// <typeparam name="T">The type of the keys.</typeparam>
        /// <param name="key">The key of the node.</param>
        public static Tree<T> MakeLeaf<T>(T key) => Tree<T>.MakeLeaf(key);
    }
}
