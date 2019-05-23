using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// Visitable structures which allow traversal of their elements, and the calling of a visit-function at each.
    /// For constructing visitors, see <see cref="VisitorFactory"/>.
    /// Implementors must obey the following:
    /// <list type="number">
    ///     <item>The order in which the elements of the structure are visited by <see cref="Visit(IVisitor{T})"/> is the same in which they would be folded.</item>
    /// </list>
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IVisitable<T, TResult>
    {
        /// <summary>
        /// Traverses the structure and calls the visitor at each element.
        /// </summary>
        /// <param name="visitor">The visitor to call.</param>
        /// <returns>The structure-specific result.</returns>
        TResult Visit(IVisitor<T> visitor);
    }

    /// <summary>
    /// Visitable structures which signal when the traversal moves forward. <see cref="IVisitable{T, TResult}.Visit(IVisitor{T})"/> only tells the visitor when an element is visited, but without any additional context. <see cref="VisitForward(IForwardVisitor{T, TTo})"/> tells the visitor between which two elements the traversal is moving. On its own, this interface is not very useful and <see cref="IBidirectionalVisitable{T, TFrom, TTo, TResult}"/> is more often appropriate, esp. with non-linear structures likes trees or graphs, where it might matter whence one comes and whither one moves.
    /// For constructing visitors, see <see cref="VisitorFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IForwardVisitable<T, TTo, TResult> : IVisitable<T, TResult>
    {
        /// <summary>
        /// Traverses the structure and calls the visitor at each element, and when moving forward from one element to the next.
        /// Implementors must make following operations identical (in the result and the observable effects, save for those caused by <see cref="IForwardVisitor{T, TTo}.Forward(T, TTo)"/>):
        /// <code>
        ///    x.Visit(v) and x.VisitForward(v)
        /// </code>
        /// </summary>
        /// <param name="visitor">The visitor to call.</param>
        TResult VisitForward(IForwardVisitor<T, TTo> visitor);
    }

    /// <summary>
    /// Visitable structures which signal when the traversal moves backward. <see cref="IVisitable{T, TResult}.Visit(IVisitor{T})"/> only tells the visitor when an element is visited, but without any additional context. <see cref="VisitBackward(IBackwardVisitor{T, TFrom})"/> tells the visitor between which two elements the traversal is moving. On its own, this interface is not very useful and <see cref="IBidirectionalVisitable{T, TFrom, TTo, TResult}"/> is more often appropriate, esp. with non-linear structures likes trees or graphs, where it might matter whence one comes and whither one moves.
    /// For constructing visitors, see <see cref="VisitorFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IBackwardVisitable<T, TFrom, TResult> : IVisitable<T, TResult>
    {
        /// <summary>
        /// Traverses the structure and calls the visitor at each element, and when moving backwards from one element to the previous.
        /// Implementors must make following operations identical (in the result and the observable effects, save for those caused by <see cref="IBackwardVisitor{T, TFrom}.Backward(TFrom)"/>):
        /// <code>
        ///    x.Visit(v) and x.Reverse.VisitBackward(v)
        /// </code>
        /// </summary>
        /// <param name="visitor">The visitor to call.</param>
        TResult VisitBackward(IBackwardVisitor<T, TFrom> visitor);
    }

    /// <summary>
    ///A data structure which is both <see cref="IForwardVisitable{T, TTo, TResult}"/> and <see cref="IBackwardVisitable{T, TFrom, TResult}"/>.
    /// For constructing visitors, see <see cref="VisitorFactory"/>.
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IBidirectionalVisitable<T, TFrom, TTo, TResult> : IForwardVisitable<T, TTo, TResult>, IBackwardVisitable<T, TFrom, TResult>
    {
        /// <summary>
        /// Traverses the structure calls the visitor at each element, and when moving forward from one element to the next, and when moving backwards from one element to the previous.
        /// </summary>
        /// <param name="visitor">The visitor to call.</param>
        TResult VisitBidirectional(IBidirectionalVisitor<T, TFrom, TTo> visitor);
    }

    /// <summary>
    /// Implementors of <see cref="IBidirectionalVisitable{T, TFrom, TTo, TResult}"/> which also support a custom visitation-order. The typical example are trees, which support pre- and post-order traversal.
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TOrder">The type of the traversal-order.</typeparam>
    public interface ITreeVisitable<T, TFrom, TTo, TResult, TOrder> : IBidirectionalVisitable<T, TFrom, TTo, TResult>
    {
        /// <summary>
        /// Traverses the structure calls the visitor at each element, and when moving forward from one element to the next, and when moving backwards from one element to the previous.
        /// </summary>
        /// <param name="visitor">The visitor to call.</param>
        /// <param name="order">The order of the traversal.</param>
        TResult VisitBidirectional(IBidirectionalVisitor<T, TFrom, TTo> visitor, TOrder order);
    }

    /// <summary>
    /// Extension methods for <see cref="ITreeVisitable{T, TFrom, TTo, TResult, TOrder}"/>.
    /// </summary>
    public static class TreeVisitableExtensions
    {
        /// <summary>
        /// Traverses the tree without performing any visitation-actions, just returning the result.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement.</typeparam>
        /// <typeparam name="TTo">The type of the element which indicates the target of a forward-movement.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="tree"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static TResult TraverseTree<T, TFrom, TTo, TResult, TOrder>(this ITreeVisitable<T, TFrom, TTo, TResult, TOrder> tree, TOrder order) => tree.VisitBidirectional(VisitorFactory.MakeVisitor<T, TFrom, TTo>(_ => { }, (_, __) => { }, _ => { }), order);
    }

    /// <summary>
    /// A visitor which can be used by an implementor of <see cref="IVisitable{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the element in the structure.</typeparam>
    public interface IVisitor<T>
    {
        /// <summary>
        /// The function to call when an element is visited.
        /// </summary>
        /// <param name="x">The element being visited.</param>
        void At(T x);
    }

    /// <summary>
    /// A visitor which can be used by an implementor of <see cref="IForwardVisitable{T, TTo, TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the element in the structure.</typeparam>
    /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    public interface IForwardVisitor<T, TTo> : IVisitor<T>
    {
        /// <summary>
        /// The function to call when moving from one element to the next.
        /// </summary>
        /// <param name="x">The element being visited.</param>
        /// <param name="to">The next element.</param>
        void Forward(T x, TTo to);
    }

    /// <summary>
    /// A visitor which can be used by an implementor of <see cref="IForwardVisitable{T, TTo, TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the element in the structure.</typeparam>
    /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    public interface IBackwardVisitor<T, TFrom> : IVisitor<T>
    {
        /// <summary>
        /// The function to call when moving from one element to the previous.
        /// </summary>
        /// <param name="from">The previous element.</param>
        void Backward(TFrom from);
    }

    /// <summary>
    /// A bi-directional visitor.
    /// </summary>
    /// <typeparam name="T">The type of element in the structure.</typeparam>
    /// <typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
    public interface IBidirectionalVisitor<T, TFrom, TTo> : IForwardVisitor<T, TTo>, IBackwardVisitor<T, TFrom>
    {

    }

    /// <summary>
    /// A factory for visitors.
    /// </summary>
    public class VisitorFactory
    {
        /// <summary>
        /// Makes an <see cref="IVisitor{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of element in the structure being visited.</typeparam>
        /// <param name="at">The function to call when an element is visited.</param>
        public static IVisitor<T> MakeVisitor<T>(Action<T> at) => new Visitor<T, object, object>(at, null, null);

        /// <summary>
        /// Makes an <see cref="IForwardVisitor{T, TTo}"/>.
        /// </summary>
        /// <typeparam name="T">The type of element in the structure being visited.</typeparam>
        /// <typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
        /// <param name="at">The function to call when an element is visited.</param>
        /// <param name="forward">The function to call when moving from one element to the next.</param>
        public static IForwardVisitor<T, TTo> MakeForwardVisitor<T, TTo>(Action<T> at, Action<T, TTo> forward) => new Visitor<T, object, TTo>(at, forward, null);

        /// <summary>
        /// Makes an <see cref="IBackwardVisitor{T, TFrom}"/>.
        /// </summary>
        /// <typeparam name="T">The type of element in the structure being visited.</typeparam>
        ///<typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
        /// <param name="at">The function to call when an element is visited.</param>
        /// <param name="backward">The function to call when moving from one element to the previous.</param>
        public static IBackwardVisitor<T, TFrom> MakeBackwardVisitor<T, TFrom>(Action<T> at, Action<TFrom> backward) => new Visitor<T, TFrom, object>(at, null, backward);

        /// <summary>
        /// Makes an <see cref="IBidirectionalVisitor{T, TFrom, TTo}"/>.
        /// </summary>
        /// <typeparam name="T">The type of element in the structure being visited.</typeparam>
        ///<typeparam name="TFrom">The type of element which indicates the target of a backward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
        ///<typeparam name="TTo">The type of element which indicates the target of a forward-movement. This might be the same as <typeparamref name="T"/>, or, say, a integer-index.</typeparam>
        /// <param name="at">The function to call when an element is visited.</param>
        /// <param name="forward">The function to call when moving from one element to the next.</param>
        /// <param name="backward">The function to call when moving from one element to the previous.</param>
        public static IBidirectionalVisitor<T, TFrom, TTo> MakeVisitor<T, TFrom, TTo>(Action<T> at, Action<T, TTo> forward, Action<TFrom> backward) => new Visitor<T, TFrom, TTo>(at, forward, backward);

        private class Visitor<T, TFrom, TTo> : IBidirectionalVisitor<T, TFrom, TTo>
        {
            private readonly Action<T> at;
            private readonly Action<T, TTo> forward;
            private readonly Action<TFrom> backward;

            public Visitor(Action<T> at, Action<T, TTo> forward, Action<TFrom> backward)
            {
                this.at = at;
                this.forward = forward;
                this.backward = backward;
            }

            public void At(T x) => at(x);
            public void Backward(TFrom from) => backward(from);
            public void Forward(T x, TTo to) => forward(x, to);
        }
    }
}
