using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional
{
    /// <summary>
    /// Functional extension methods for function expressions and for metaprogramming.
    /// </summary>
    public static class FE
    {
        /// <summary>
        /// Returns the identity function expression for a given type.
        /// See <see cref="F.Id{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the identity function.</typeparam>
        public static Expression<Func<T, T>> Id<T>()
        {
            var p = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, T>>(p, p);
        }

        /// <summary>
        /// Composes two function expressions. The second function is run with the result of the first.
        /// See <see cref="F.Then{TA, TB, TC}(Func{TA, TB}, Func{TB, TC})"/>.
        /// </summary>
        /// <typeparam name="TA">The input of the first function.</typeparam>
        /// <typeparam name="TB">The output of the first function and input of the second function.</typeparam>
        /// <typeparam name="TC">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<TA, TC>> Then<TA, TB, TC>(this Expression<Func<TA, TB>> f, Expression<Func<TB, TC>> g)
        {
            var (fParam, fBody) = SplitParameterAndBody(f);
            var (gParam, gBody) = SplitParameterAndBody(g);

            gBody = Beta(gBody, gParam, fBody);

            var ret = Expression.Lambda<Func<TA, TC>>(gBody, fParam);

            return ret;
        }

        /// <summary>
        /// Composes two function expressions. The first function is run with the result of thesecond. This is "traditional" function chaining.
        /// See <see cref="F.Then{TA, TB, TC}(Func{TA, TB}, Func{TB, TC})"/>.
        /// </summary>
        /// <typeparam name="TA">The input of the first function.</typeparam>
        /// <typeparam name="TB">The output of the first function and input of the second function.</typeparam>
        /// <typeparam name="TC">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<TA, TC>> After<TA, TB, TC>(this Expression<Func<TB, TC>> f, Expression<Func<TA, TB>> g)
        {
            var (fParam, fBody) = SplitParameterAndBody(f);
            var (gParam, gBody) = SplitParameterAndBody(g);

            fBody = Beta(fBody, fParam, gBody);

            var ret = Expression.Lambda<Func<TA, TC>>(fBody, gParam);

            return ret;
        }

        /// <summary>
        /// Takes a one-parameter function expression and splits the parameter from the function-body.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <param name="expr">The function expression.</param>
        /// <returns>The parameter of the function expression and its open body.</returns>
        public static (ParameterExpression, Expression) SplitParameterAndBody<TA, TB>(this Expression<Func<TA, TB>> expr)
        {
            var l = (LambdaExpression)expr;
            return (l.Parameters[0], expr.Body);
        }

        /// <summary>
        /// Takes a two-parameter function expression and splits the parameter from the function-body.
        /// </summary>
        /// <typeparam name="TA">The first input of the function.</typeparam>
        /// <typeparam name="TB">The second input of the function.</typeparam>
        /// <typeparam name="TC">The output of the function.</typeparam>
        /// <param name="expr">The function expression.</param>
        /// <returns>The parameters of the function expression and its open body.</returns>
        public static (ParameterExpression, ParameterExpression, Expression) SplitParameterAndBody<TA, TB, TC>(this Expression<Func<TA, TB, TC>> expr)
        {
            var l = (LambdaExpression)expr;
            return (l.Parameters[0], l.Parameters[1], expr.Body);
        }

        /// <summary>
        /// Takes a three-parameter function expression and splits the parameter from the function-body.
        /// </summary>
        /// <typeparam name="TA">The first input of the function.</typeparam>
        /// <typeparam name="TB">The second input of the function.</typeparam>
        /// <typeparam name="TC">The third input of the function.</typeparam>
        /// <typeparam name="TD">The output of the function.</typeparam>
        /// <param name="expr">The function expression.</param>
        /// <returns>The parameters of the function expression and its open body.</returns>
        public static (ParameterExpression, ParameterExpression, ParameterExpression, Expression) SplitParameterAndBody<TA, TB, TC, TD>(this Expression<Func<TA, TB, TC, TD>> expr)
        {
            var l = (LambdaExpression)expr;
            return (l.Parameters[0], l.Parameters[1], l.Parameters[2], expr.Body);
        }

        /// <summary>
        /// Takes a three-parameter function expression and splits the parameter from the function-body.
        /// </summary>
        /// <typeparam name="TA">The first input of the function.</typeparam>
        /// <typeparam name="TB">The second input of the function.</typeparam>
        /// <typeparam name="TC">The third input of the function.</typeparam>
        /// <typeparam name="TD">The fourth input of the function.</typeparam>
        /// <typeparam name="TE">The output of the function.</typeparam>
        /// <param name="expr">The function expression.</param>
        /// <returns>The parameters of the function expression and its open body.</returns>
        public static (ParameterExpression, ParameterExpression, ParameterExpression, ParameterExpression, Expression) SplitParameterAndBody<TA, TB, TC, TD, TE>(this Expression<Func<TA, TB, TC, TD, TE>> expr)
        {
            var l = (LambdaExpression)expr;
            return (l.Parameters[0], l.Parameters[1], l.Parameters[2], l.Parameters[3], expr.Body);
        }

        /// <summary>
        /// Replaces a parameter in a function expression with a new expression, performing beta-reduction.
        /// </summary>
        /// <param name="expr">The expression containing the parameter.</param>
        /// <param name="value">The value with which to replace the parameter.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="value"/> isn't a subtype of the type of the parameter of <paramref name="expr"/>.</exception>
        public static Expression<Func<TB>> Beta<TA, TB>(this Expression<Func<TA, TB>> expr, Expression value)
        {
            var l = (LambdaExpression)expr;
            var oldParam = l.Parameters[0];

            if (!oldParam.Type.IsAssignableFrom(value.Type))
                throw new ArgumentException($"The type {value.Type} is not a subtype of {oldParam.Type}", nameof(value));

            var visitor = new ParameterExpressionVisitor(oldParam, _ => value);
            var ret = visitor.VisitAndConvert(expr.Body, nameof(Beta));

            return Expression.Lambda<Func<TB>>(ret);
        }

        /// <summary>
        /// Replaces a parameter in a function expression with a new expression, performing beta-reduction.
        /// </summary>
        /// <param name="expr">The expression containing the parameter.</param>
        /// <param name="param">The parameter to replace.</param>
        /// <param name="value">The value with which to replace the parameter.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="value"/> isn't a subtype of the type of <paramref name="param"/>.</exception>
        public static Expression Beta(this Expression expr, ParameterExpression param, Expression value)
        {
            if (!param.Type.IsAssignableFrom(value.Type))
                throw new ArgumentException($"The type {value.Type} is not a subtype of {param.Type}", nameof(value));

            var visitor = new ParameterExpressionVisitor(param, _ => value);
            var ret = visitor.Visit(expr);

            return ret;
        }

        /// <summary>
        /// Returns a function expression whose return type is a supertype of the original function expression.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <typeparam name="TBSuper">The output of the returned function, which is a supertype of <typeparamref name="TB"/>.</typeparam>
        /// <param name="expr">The function expression.</param>
        public static Expression<Func<TA, TBSuper>> CastReturnType<TA, TB, TBSuper>(this Expression<Func<TA, TB>> expr)
            where TB : TBSuper
        {
            Expression converted = Expression.Convert(expr.Body, typeof(TBSuper));
            return Expression.Lambda<Func<TA, TBSuper>>(converted, expr.Parameters);
        }

        /// <summary>
        /// Returns a function expression whose input type is a subtype of the original function expression.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TASub">The input of the returned function, which is a subtype of <typeparamref name="TA"/>.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <param name="expr">The function expression.</param>
        public static Expression<Func<TASub, TB>> CastInputType<TA, TASub, TB>(this Expression<Func<TA, TB>> expr)
            where TASub : TA
        {
            var p = Expression.Parameter(typeof(TASub), expr.Parameters[0].Name);
            Expression converted = new ParameterExpressionVisitor(expr.Parameters[0], _ => p).Visit(expr.Body);
            return Expression.Lambda<Func<TASub, TB>>(converted, p);
        }

        /// <summary>
        /// A visitor which applies a function to a <see cref="ParameterExpression"/>, replacing it with the function's result.
        /// </summary>
        private class ParameterExpressionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression source;
            private readonly Func<ParameterExpression, Expression> f;

            public ParameterExpressionVisitor(ParameterExpression source, Func<ParameterExpression, Expression> f)
            {
                this.source = source;
                this.f = f;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Equals(source))
                    return f(node);
                else
                    return base.VisitParameter(node);
            }
        }
    }
}
