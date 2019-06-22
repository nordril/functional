using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An applicative functor which supports applying multi-argument functions to containers.
    /// Implementations must fulfill the following (assuming that Pure(a) == X.Pure(a) for readability)
    /// for all a, f, g, h.
    /// <code>
    ///     Pure(a).Ap(b => b) == Pure(a) (identity)
    ///     Pure(a).Ap(Pure(f)) == Pure(f(a)) (homomorphism)
    ///     Pure(a).Ap(f) == f.Ap(Pure(a => f(a))) (interchange)
    ///     h.Ap(g).Ap(f) == h.Ap(g.Ap(f.Ap(Pure(comp)))) (composition)
    ///     where
    ///        comp f2 f1 x = f2(f1(x)) 
    /// </code>
    /// </summary>
    /// <typeparam name="TSource">The data contained in the functor.</typeparam>
    public interface IApplicative<out TSource> : IFunctor<TSource>
    {
        /// <summary>
        /// Wraps a value into an applicative. The this-value MUST NOT BE USED by implementors.
        /// </summary>
        /// <typeparam name="TResult">The type of the value to wrap.</typeparam>
        /// <param name="x">The value to wrap.</param>
        IApplicative<TResult> Pure<TResult>(TResult x);

        /// <summary>
        /// Takes an applicative value and a function wrapped in an applicative value,
        /// and applies the function to this container.
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="f">The wrapped function to apply.</param>
        IApplicative<TResult> Ap<TResult>(IApplicative<Func<TSource, TResult>> f);
    }

    /// <summary>
    /// Extensions for <see cref="IApplicative{TSource}"/>.
    /// </summary>
    public static class ApplicativeExtensions
    {
#if NETFULL
        private static IEnumerable<T> Prepend<T>(this IEnumerable<T> xs, T x)
        {
            yield return x;
            foreach (var y in xs)
                yield return y;
        }
#endif
        /// <summary>
        /// <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/> with the arguments reversed,
        /// meaning that the function is applied to the argument, which reads more natural.
        /// </summary>
        /// <typeparam name="TSource">The argument type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="f">The applicative containing the function mapping the source type to the result type.</param>
        /// <param name="x">The applicative containing the source type.</param>
        public static IApplicative<TResult> ApF<TSource, TResult>(
            this IApplicative<Func<TSource, TResult>> f,
            IApplicative<TSource> x)
            => x.Ap(f);

        /// <summary>
        /// Wraps an object in an applicative via <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the object to wrap.</typeparam>
        /// <typeparam name="TResult">The type of the applicative to return.</typeparam>
        /// <param name="x">The value to wrap.</param>
        public static TResult Pure<TSource, TResult>(this TSource x)
            where TResult : IApplicative<TSource>, new() => (TResult)(new TResult().Pure(x));

        /// <summary>
        /// Wraps an object in an applicative via <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// The applicative in question does not have to posess a parameterless constuctor; instead, a call to <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/> with the this-pointer being null is forced.
        /// If the requested applicative uses the this-pointer in <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>, this will result in a <see cref="NullReferenceException"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the object to wrap.</typeparam>
        /// <typeparam name="TResult">The type of the applicative to return.</typeparam>
        /// <param name="x">The value to wrap.</param>
        /// <exception cref="NullReferenceException">If <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/> of <typeparamref name="TResult"/> uses the this-pointer.</exception>
        public static TResult PureUnsafe<TSource, TResult>(this TSource x)
            where TResult : IApplicative<TSource>
        {
            var instancePure = typeof(TResult).GetMember(
                nameof(IApplicative<object>.Pure),
                MemberTypes.Method,
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public);

            var pureMi = instancePure.Cast<MethodInfo>().First(mi =>
            {
                var gargs = mi.GetGenericArguments();
                return gargs.Length == 1
                && gargs[0].IsGenericParameter
                && mi.IsGenericMethod
                && mi.GetParameters().Length == 1;
            }).MakeGenericMethod(typeof(TSource));

            var pure = new DynamicMethod("pure", typeof(IApplicative<TSource>), new Type[] { typeof(TSource) });
            var generator = pure.GetILGenerator();

            var applicativeThis = generator.DeclareLocal(typeof(TResult));
            generator.Emit(OpCodes.Ldloca_S, 0); //push applicativeThis (index 0) onto the stack: [] -> [at]
            generator.Emit(OpCodes.Initobj, typeof(TResult)); //initialize at to null: at -> []
            generator.Emit(OpCodes.Ldloc_0); //put local variable 0 to the stack again: [] -> at
            generator.Emit(OpCodes.Box, typeof(TResult)); //box the value: [at:stack] -> [at:heap]
            generator.Emit(OpCodes.Ldarg_0); //load the first argument (x) onto the stack: [at:heap] -> [at:heap, x]
            generator.EmitCall(OpCodes.Call, pureMi, null); //call pureMi without null-checking: [at:heap, x] -> [ret]
            generator.Emit(OpCodes.Ret); //return: []

            var res = (TResult)pure.Invoke(null, new object[] { x });

            return res;
        }

        /// <summary>
        /// Lifts a binary function to take two applicative arguments.
        /// </summary>
        /// <example>
        /// <code>
        ///     var x = Maybe.Just(5);
        ///     var y = Maybe.Nothing&lt;int&gt;();
        ///     Func&lt;int,int,int&gt; f = (x,y) => x + y;
        ///     
        ///     //f takes integer arguments, but we can apply it to two maybe-arguments,
        ///     //with automatic unpacking and packing of the results, via liftA*.
        ///     var result = f.LiftA2()(x, y);
        /// </code>
        /// </example>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<TResult>>
            LiftA<T1, T2, TResult>(this Func<T1, T2, TResult> f)
        => (x, y) => (x.Map(f.Curry()) as IApplicative<Func<T2, TResult>>).ApF(y);

        /// <summary>
        /// Lifts a ternary function to take three applicative arguments. See <see cref="LiftA{T1, T2, TResult}(Func{T1, T2, TResult})"/>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<T3>, IApplicative<TResult>>
            LiftA<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> f)
        => (x, y, z) => (x.Map(f.Curry()) as IApplicative<Func<T2, Func<T3, TResult>>>).ApF(y).ApF(z);

        /// <summary>
        /// Lifts a quaternary function to take three applicative arguments. See <see cref="LiftA{T1, T2, TResult}(Func{T1, T2, TResult})"/>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the second argument.</typeparam>
        /// <typeparam name="T4">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<IApplicative<T1>, IApplicative<T2>, IApplicative<T3>, IApplicative<T4>, IApplicative<TResult>>
            LiftA<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> f)
        => (x, y, z, u) => (x.Map(f.Curry()) as IApplicative<Func<T2, Func<T3, Func<T4, TResult>>>>).ApF(y).ApF(z).ApF(u);

        /// <summary>
        /// An applicative/monadic filter operation which is a generalization of <see cref="Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>. If the applicative is <see cref="Identity{T}"/>, this function behaves identically to that in LINQ. The sequence is traversed via <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <example>
        ///     //Computing the powerset (interpreting a sequence as a set).
        ///     //Using the non-determinism monad of FuncList, every element is non-deterministically
        ///     //both excluded and included, and the result is a FuncList containing as elements all possible
        ///     //subsets of {1,2,3}.
        ///     var new List&lt;int&gt; { 1, 2, 3}.WhereAp(x => new FuncList&lt;bool&gt; {false, true});
        /// </example>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TPredicate">The applicate-type of the predicate.</typeparam>
        /// <typeparam name="TResult">The applicative-type of the result. This should agree with <typeparamref name="TPredicate"/>.</typeparam>
        /// <param name="xs">The sequence to filter.</param>
        /// <param name="f">The filtering predicate. Elements for which it returns true are included, otherwise they're excluded.</param>
        /// <returns>The sequence, traversed left to right, using the applicative combining operation at each step.</returns>
        /// <exception cref="NullReferenceException">If the applicative in question does not implement <see cref="Pure{TSource, TResult}(TSource)"/> correctly and uses the this-pointer.</exception>
        /// <exception cref="InvalidCastException">If <typeparamref name="TPredicate"/> and <typeparamref name="TResult"/> are incompatible.</exception>
        public static TResult WhereAp<T, TPredicate, TResult>(this IEnumerable<T> xs, Func<T, TPredicate> f)
            where TPredicate : IApplicative<bool>
            where TResult : IApplicative<IEnumerable<T>>
        {
            //The empty list as the base case.
            var pure = new List<T>().PureUnsafe<IEnumerable<T>, TResult>();

            //The "plain" combining function which takes the element to include/exclude (x),
            //the list so far, whether x fulfills should be included, and returns a new list
            //to which x is or isn't appended.
            Func<IEnumerable<T>, Func<bool, IEnumerable<T>>> g(T x) => acc => flag => flag ? acc.Prepend(x) : acc;

            //Iterate through the sequence, unpacking, at each step, the accumulated list and the result of the predicate,
            //and either adding or not adding the current element.
            return xs.AggregateRight((x, acc) => (TResult)f(x).Ap(acc.Map(g(x)) as IApplicative<Func<bool, IEnumerable<T>>>), pure);
        }

        /// <summary>
        /// An applicative/monadic <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> operation. If the applicative is <see cref="Identity{T}"/>, this function behaves identically to that in LINQ. The sequence is traversed via <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>.
        /// </summary>
        /// <example>
        ///     //Map each element to itself and its negation
        ///     //Using the non-determinism monad of FuncList, every element is non-deterministically
        ///     //mapped to both values, and the result is a FuncList containing as elements all possible
        ///     //combinations.
        ///     var new List&lt;int&gt; { 1, 2, 3}.SelectAp(x => new FuncList&lt;bool&gt; {x, x*(-1)});
        ///     
        ///     //result: [1,2,3], [1,2,-3], [1,-2,3], [1,-2,-3], [-1,2,3], [-1,2,-3], [-1,-2,3], [-1,-2,-3]
        /// </example>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result-elements.</typeparam>
        /// <typeparam name="TResultList">The applicative-type of the result. This should agree with <typeparamref name="TResult"/></typeparam>
        /// <param name="xs">The sequence to filter.</param>
        /// <param name="f">The filtering predicate. Elements for which it returns true are included, otherwise they're excluded.</param>
        /// <returns>The sequence, traversed left to right, using the applicative combining operation at each step.</returns>
        /// <exception cref="NullReferenceException">If the applicative in question does not implement <see cref="Pure{TSource, TResult}(TSource)"/> correctly and uses the this-pointer.</exception>
        /// <exception cref="InvalidCastException">If <typeparamref name="TResultList"/> and <typeparamref name="TResult"/> are incompatible.</exception>
        public static IApplicative<IEnumerable<TResult>> SelectAp<T, TResult, TResultList>(this IEnumerable<T> xs, Func<T, IApplicative<TResult>> f)
            where TResultList : IApplicative<IEnumerable<TResult>>
        {
            //The empty list as the base case.
            var pure = new List<TResult>().PureUnsafe<IEnumerable<TResult>, TResultList>();

            //The applicative-lifted prepend-function.
            Func<TResult, IEnumerable<TResult>, IEnumerable<TResult>> prepend = (a, b) => b.Prepend(a);
            var prependAp = prepend.LiftA();

            TResultList go(IEnumerable<T> ys)
            {
                if (ys.Empty())
                    return pure;
                else
                    return (TResultList)prependAp(f(ys.First()), go(ys.Skip(1)));
            }

            return go(xs);
        }
    }
}