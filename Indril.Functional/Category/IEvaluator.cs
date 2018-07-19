using Indril.Functional.Data;

namespace Indril.Functional.Category
{
    /// <summary>
    /// An evaluator which can run an evaluation on a value of type <typeparamref name="TSource"/> and return a result
    /// of type <typeparamref name="TResult"/>.
    /// An example would be an expression tree which is reduced to a value.
    /// </summary>
    /// <typeparam name="TSource">The type of objects which this evaluator can evaluate.</typeparam>
    /// <typeparam name="TResult">The type of the result, if the evaluation is successful.</typeparam>
    /// <typeparam name="TError">The type of the error, if the evaluation is unsuccessful.</typeparam>
    public interface IEvaluator<TSource, TResult, TError>
    {
        /// <summary>
        /// Evaluates a <typeparamref name="TSource"/> and returns a <typeparamref name="TResult"/>.
        /// The evaluation may fail, and in that case, the method must return a <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="arg">The value to evaluate.</param>
        Either<TError, TResult> Evaluate(TSource arg);
    }
}
