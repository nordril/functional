using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional
{
    /// <summary>
    /// Functional extension methods for function expressions and for metaprogramming.
    /// </summary>
    /// <remarks>
    /// <see cref="Expression"/>, with the functions
    /// <list type="number">
    ///     <item><see cref="Id{T}"/>, </item>
    ///     <item><see cref="LiftToExpression{TA, TB}(Func{TA, TB})"/>, </item>
    ///     <item><see cref="Then{TA, TB, TC}(Expression{Func{TA, TB}}, Expression{Func{TB, TC}})"/>, </item>
    ///     <item><see cref="First{TA, TB, TIgnored}(Expression{Func{TA, TB}})"/>, </item>
    ///     <item><see cref="Both{TA1, TA2, TB1, TB2}(Expression{Func{TA1, TB1}}, Expression{Func{TA2, TB2}})"/> </item>
    /// </list>
    /// forms a category and an arrow. Arrows are composable computations.
    /// See http://hackage.haskell.org/package/base-4.12.0.0/docs/Control-Arrow.html.
    /// </remarks>
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
        /// Lifts a function into a function expression. Note that the returned function expression will simply invoke the given function, which will still be a black box, inaccessible to, e.g. LINQ.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Expression<Func<TA, TB>> LiftToExpression<TA, TB>(this Func<TA, TB> f)
        {
            Delegate fd = f;

            var x = Expression.Parameter(typeof(TA), "x");
            var body = Expression.Call(Expression.Constant(f.Target), f.Method, x);
            return Expression.Lambda<Func<TA, TB>>(body, x);
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
        /// Takes two function expressions and returns a new function expression which runs the two function expressions and combines their results via a third, binary function expression. The typical usage is to have two predicates which are combined via a binary operator like <see cref="Expression.AndAlso(Expression, Expression)"/>.
        /// </summary>
        /// <typeparam name="TA">The input of the functions.</typeparam>
        /// <typeparam name="TB">The output of the first function and the first input of the combining function.</typeparam>
        /// <typeparam name="TC">The output of the second function and the second input of the combining function.</typeparam>
        /// <typeparam name="TD">The output of the binary combining function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        /// <param name="combine">The combining function.</param>
        /// <returns>A function expression which runs <paramref name="f"/> and <paramref name="g"/> and combines their results via <paramref name="combine"/>.</returns>
        public static Expression<Func<TA, TB>> Binary<TA, TB, TC, TD>(this Expression<Func<TA, TB>> f, Expression<Func<TA, TC>> g, Expression<Func<TB, TC, TD>> combine)
        {
            var (fParam, fBody) = f.SplitParameterAndBody();
            var (gParam, gBody) = g.SplitParameterAndBody();
            var (cParam1, cParam2, cBody) = combine.SplitParameterAndBody();

            var x = Expression.Parameter(typeof(TA), "x");
            fBody = fBody.Beta(fParam, x);
            gBody = gBody.Beta(gParam, x);
            cBody = cBody.Beta(cParam1, cParam2, fBody, gBody);

            var ret = Expression.Lambda<Func<TA, TB>>(cBody, x);

            return ret;
        }

        /// <summary>
        /// Returns a function expression which applies <paramref name="expr"/> to the first element of the tuple and leaves the second tuple element unchanged.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <typeparam name="TIgnored">The second input of the returned function, which is returned unchanged.</typeparam>
        /// <param name="expr">The function to lift.</param>
        public static Expression<Func<(TA, TIgnored), (TB, TIgnored)>> First<TA, TB, TIgnored>(this Expression<Func<TA, TB>> expr)
        {
            var (fParam, fBody) = expr.SplitParameterAndBody();
            var x = Expression.Parameter(typeof(ValueTuple<TA, TIgnored>));

            var body = Expression.New(
                typeof(ValueTuple<TB, TIgnored>).GetConstructor(new[] { typeof(TB), typeof(TIgnored)}),
                Expression.Invoke(expr, Expression.Field(x, nameof(ValueTuple<TA, TIgnored>.Item1))),
                Expression.Field(x, nameof(ValueTuple<TA, TIgnored>.Item2)));

            return Expression.Lambda<Func<ValueTuple<TA, TIgnored>, ValueTuple<TB, TIgnored>>>(body, x);
        }

        /// <summary>
        /// Returns a function expression which applies <paramref name="expr"/> to the second element of the tuple and leaves the first tuple element unchanged.
        /// </summary>
        /// <typeparam name="TA">The input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <typeparam name="TIgnored">The first input of the returned function, which is returned unchanged.</typeparam>
        /// <param name="expr">The function to lift.</param>
        public static Expression<Func<(TIgnored, TA), (TIgnored, TB)>> Second<TA, TB, TIgnored>(this Expression<Func<TA, TB>> expr)
        {
            var (fParam, fBody) = expr.SplitParameterAndBody();
            var x = Expression.Parameter(typeof(ValueTuple<TIgnored, TA>));

            var body = Expression.New(
                typeof(ValueTuple<TIgnored, TB>).GetConstructor(new[] { typeof(TIgnored), typeof(TB) }),
                Expression.Field(x, nameof(ValueTuple<TIgnored, TA>.Item1)),
                Expression.Invoke(expr, Expression.Field(x, nameof(ValueTuple<TIgnored, TA>.Item2))));

            return Expression.Lambda<Func<ValueTuple<TIgnored, TA>, ValueTuple<TIgnored, TB>>>(body, x);
        }

        /// <summary>
        /// Takes two function expressions and returns one which executes both, taking and returning two values. Also known as <c>(***)</c>.
        /// </summary>
        /// <typeparam name="TA1">The input of the first function.</typeparam>
        /// <typeparam name="TA2">The input of the second function.</typeparam>
        /// <typeparam name="TB1">The output of the first function.</typeparam>
        /// <typeparam name="TB2">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<(TA1, TA2), (TB1, TB2)>> Both<TA1, TA2, TB1, TB2>(this Expression<Func<TA1, TB1>> f, Expression<Func<TA2, TB2>> g)
        {
            var x = Expression.Parameter(typeof((TA1, TA2)), "x");

            var newBody = Expression.New(
                    typeof(ValueTuple<TB1, TB2>).GetConstructor(new[] { typeof(TB1), typeof(TB2) }),
                        Expression.Invoke(f, Expression.Field(x, nameof(ValueTuple<object, object>.Item1))),
                        Expression.Invoke(g, Expression.Field(x, nameof(ValueTuple<object, object>.Item2))));

            var ret = Expression.Lambda<Func<(TA1, TA2), (TB1, TB2)>>(newBody, x);

            return ret;
        }

        /// <summary>
        /// Takes two function expressions and returns one which executes both with the same argument, returning two values. Also known as <c>(&amp;&amp;&amp;)</c>.
        /// </summary>
        /// <typeparam name="TA">The input of the functions.</typeparam>
        /// <typeparam name="TB">The output of the first function.</typeparam>
        /// <typeparam name="TC">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<TA, (TB, TC)>> Fanout<TA, TB, TC>(this Expression<Func<TA, TB>> f, Expression<Func<TA, TC>> g)
        {
            var x = Expression.Parameter(typeof(TA), "x");

            var newBody = Expression.New(
                    typeof(ValueTuple<TB, TC>).GetConstructor(new[] { typeof(TB), typeof(TC) }),
                        Expression.Invoke(f, x),
                        Expression.Invoke(g, x));

            var ret = Expression.Lambda<Func<TA, (TB, TC)>>(newBody, x);

            return ret;
        }

        /// <summary>
        /// Takes a function expression and lifts it into a function expression taking and returning an <see cref="Either{TLeft, TRight}"/> which applies the original function to the <see cref="Either.FromLeft{TLeft, TRight}(TLeft)"/>, leaving the <see cref="Either.FromRight{TLeft, TRight}(TRight)"/> unchanged.
        /// </summary>
        /// <typeparam name="TA">Rhe input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <typeparam name="TC">The right value of the <see cref="Either{TLeft, TRight}"/>, which is left unchanged.</typeparam>
        /// <param name="f">The function.</param>
        public static Expression<Func<Either<TA, TC>, Either<TB, TC>>> Left<TA, TB, TC>(this Expression<Func<TA, TB>> f)
            => EitherOr(f, (TC x) => x);

        /// <summary>
        /// Takes a function expression and lifts it into a function expression taking and returning an <see cref="Either{TLeft, TRight}"/> which applies the original function to the <see cref="Either.FromRight{TLeft, TRight}(TRight)"/>, leaving the <see cref="Either.FromLeft{TLeft, TRight}(TLeft)"/> unchanged.
        /// </summary>
        /// <typeparam name="TA">Rhe input of the function.</typeparam>
        /// <typeparam name="TB">The output of the function.</typeparam>
        /// <typeparam name="TC">The left value of the <see cref="Either{TLeft, TRight}"/>, which is left unchanged.</typeparam>
        /// <param name="f">The function.</param>
        public static Expression<Func<Either<TC, TA>, Either<TC, TB>>> Right<TA, TB, TC>(this Expression<Func<TA, TB>> f)
            => EitherOr((TC x) => x, f);

        /// <summary>
        /// Takes two function expressions and lifts them into a function which take take &amp; produce the input/output of either. Also known as <c>(+++)</c>.
        /// </summary>
        /// <typeparam name="TA">Rhe input of the first function.</typeparam>
        /// <typeparam name="TB">The output of the first function.</typeparam>
        /// <typeparam name="TC">Rhe input of the second function.</typeparam>
        /// <typeparam name="TD">The output of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<Either<TA, TB>, Either<TC, TD>>> EitherOr<TA, TB, TC, TD>(this Expression<Func<TA, TC>> f, Expression<Func<TB, TD>> g)
        {
            var x = Expression.Parameter(typeof(Either<TA, TB>), "x");
            var mapMethod = typeof(IBifunctor<TA, TB>)
                .GetMethod(nameof(IBifunctor<TA, TB>.BiMap), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod)
                .MakeGenericMethod(typeof(TC), typeof(TD));
            var toEitherMethod = typeof(Either)
                .GetMethod(nameof(Either.ToEither), BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod)
                .MakeGenericMethod(typeof(TC), typeof(TD));

            var body = Expression.Call(
                toEitherMethod,
                Expression.Call(x, mapMethod, f, g));

            var ret = Expression.Lambda<Func<Either<TA, TB>, Either<TC, TD>>>(body, x);

            return ret;
        }

        /// <summary>
        /// Takes two function expressions and returns one which takes the inputs of either and merges the result. Also known as <c>|||</c>.
        /// </summary>
        /// <typeparam name="TA">The input of the first function.</typeparam>
        /// <typeparam name="TB">The input of the second function.</typeparam>
        /// <typeparam name="TC">The outpu of the two functions.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Expression<Func<Either<TA,TB>, TC>> Fanin<TA, TB, TC>(this Expression<Func<TA, TC>> f, Expression<Func<TB, TC>> g)
        {
            var x = Expression.Parameter(typeof(Either<TA, TB>), "x");

            var coalesceMethod = typeof(Either<TA, TB>).GetMethod(nameof(Either<TA, TB>.Coalesce), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod).MakeGenericMethod(typeof(TC));

            var body = Expression.Call(x, coalesceMethod, f, g);

            var ret = Expression.Lambda<Func<Either<TA, TB>, TC>>(body, x);

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

            var visitor = new ParameterExpressionVisitor(x => x.Equals(oldParam), _ => value);
            var ret = visitor.VisitAndConvert(expr.Body, nameof(Beta));

            return Expression.Lambda<Func<TB>>(ret);
        }

        /// <summary>
        /// Replaces a parameter in a function expression with a new expression, performing beta-reduction.
        /// </summary>
        /// <param name="expr">The expression containing the parameters.</param>
        /// <param name="value1">The value with which to replace the first parameter.</param>
        /// <param name="value2">The value with which to replace the second parameter.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="value1"/>/<paramref name="value2"/> isn't a subtype of the type of the first/second parameter of <paramref name="expr"/>.</exception>
        public static Expression<Func<TC>> Beta<TA, TB, TC>(this Expression<Func<TA, TB, TC>> expr, Expression value1, Expression value2)
        {
            var l = (LambdaExpression)expr;
            var oldParam1 = l.Parameters[0];
            var oldParam2 = l.Parameters[1];

            if (!oldParam1.Type.IsAssignableFrom(value1.Type))
                throw new ArgumentException($"The type {value1.Type} is not a subtype of {oldParam1.Type}", nameof(value1));
            if (!oldParam2.Type.IsAssignableFrom(value2.Type))
                throw new ArgumentException($"The type {value2.Type} is not a subtype of {oldParam2.Type}", nameof(value2));

            var visitor = new ParameterExpressionVisitor(x => x.Equals(oldParam1) || x.Equals(oldParam2), x => x.Equals(oldParam1) ? value1 : value2);
            var ret = visitor.VisitAndConvert(expr.Body, nameof(Beta));

            return Expression.Lambda<Func<TC>>(ret);
        }

        /// <summary>
        /// Replaces two parameter in a function expression with new expressions, performing binary beta-reduction.
        /// </summary>
        /// <param name="expr">The expression containing the parameter.</param>
        /// <param name="param">The parameter to replace.</param>
        /// <param name="value">The value with which to replace the parameter.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="value"/> isn't a subtype of the type of <paramref name="param"/>.</exception>
        public static Expression Beta(this Expression expr, ParameterExpression param, Expression value)
        {
            if (!param.Type.IsAssignableFrom(value.Type))
                throw new ArgumentException($"The type {value.Type} is not a subtype of {param.Type}", nameof(value));

            var visitor = new ParameterExpressionVisitor(x => x.Equals(param), _ => value);
            var ret = visitor.Visit(expr);

            return ret;
        }

        /// <summary>
        /// Replaces a parameter in a function expression with a new expression, performing beta-reduction.
        /// </summary>
        /// <param name="expr">The expression containing the parameters.</param>
        /// <param name="param1">The first parameter to replace.</param>
        /// <param name="param2">The second parameter to replace.</param>
        /// <param name="value1">The value with which to replace the first parameter.</param>
        /// <param name="value2">The value with which to replace the second parameter.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="value1"/>/<paramref name="value2"/> isn't a subtype of the type of <paramref name="param1"/>/<paramref name="param2"/>.</exception>
        public static Expression Beta(this Expression expr, ParameterExpression param1, ParameterExpression param2, Expression value1, Expression value2)
        {
            if (!param1.Type.IsAssignableFrom(value1.Type))
                throw new ArgumentException($"The type {value1.Type} is not a subtype of {param1.Type}", nameof(value1));
            if (!param2.Type.IsAssignableFrom(value2.Type))
                throw new ArgumentException($"The type {value2.Type} is not a subtype of {param2.Type}", nameof(value2));

            var visitor = new ParameterExpressionVisitor(x => x.Equals(param1) || x.Equals(param2), x => x.Equals(param1) ? value1 : value2);
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
            Expression converted = new ParameterExpressionVisitor(x => x.Equals(expr.Parameters[0]), _ => p).Visit(expr.Body);
            return Expression.Lambda<Func<TASub, TB>>(converted, p);
        }

        /// <summary>
        /// Compiles a closed expression to a function returning <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expression's return value.</typeparam>
        /// <param name="expr">The expression to compile.</param>
        /// <exception cref="ArgumentException">If the type of <paramref name="expr"/> isn't <typeparamref name="T"/>.</exception>
        public static Func<T> Compile<T>(this Expression expr)
        {
            if (expr.Type != typeof(T))
                throw new ArgumentException($"The type of {nameof(expr)} isn't {typeof(T)}.");

            var e = Expression.Lambda<Func<T>>(expr);

            return e.Compile();
        }

        /// <summary>
        /// A visitor which applies a function to a <see cref="ParameterExpression"/>, replacing it with the function's result.
        /// </summary>
        private class ParameterExpressionVisitor : ExpressionVisitor
        {
            private readonly Func<ParameterExpression, bool> source;
            private readonly Func<ParameterExpression, Expression> f;

            public ParameterExpressionVisitor(Func<ParameterExpression, bool> source, Func<ParameterExpression, Expression> f)
            {
                this.source = source;
                this.f = f;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (source(node))
                    return f(node);
                else
                    return base.VisitParameter(node);
            }
        }
    }
}
