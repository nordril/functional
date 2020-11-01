using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A monad. Monads, in addition to wrapping values (via <see cref="IApplicative{TSource}"/>
    /// and mapping over their contained values (via <see cref="IFunctor{TSource}"/> support temporarily
    /// unpacking their contained values and re-packing them via the <see cref="Bind(Func{TSource, T})"/>
    /// operator. Unlike <see cref="IApplicative{TSource}.Ap{TResult}(IApplicative{Func{TSource, TResult}})"/>,
    /// this allows dynamically controlling the computational path at runtime, instead of just applying a fixed
    /// function over one of more wrapped arguments.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    /// <typeparam name="TSource">The type of the values contained in the monad.</typeparam>
    public interface IMonoMonad<T, TSource> : IMonoFunctor<T, TSource>
        where T : IMonoMonad<T, TSource>
    {
        /// <summary>
        /// Wraps a value into an applicative. The this-value MUST NOT BE USED by implementors.
        /// </summary>
        /// <param name="x">The value to wrap.</param>
        T Pure(TSource x);

        /// <summary>
        /// Unpacks the value(s) contained in this monad and applies a function to them.
        /// Bind corresponds to chaining functions, with the addition of the monadic context.
        /// Implementors must fulfill the following laws:
        /// <code>
        ///     Pure(a).Bind(f) == f(a) (left identity of pure)
        ///     f.Bind(x => g(x).Bind(h)) == f.Bind(g).Bind(h) (associativity)
        /// </code>
        /// These laws are identical to the laws of <see cref="Algebra.IHasMonoid{TSource}"/>, except for the
        /// type variable in <see cref="IMonad{TSource}"/>.
        /// </summary>
        /// <param name="f">The function to apply.</param>
        T Bind(Func<TSource, T> f);
    }

    /// <summary>
    /// Extension methods for <see cref="IMonoMonad{T, TSource}"/>.
    /// </summary>
    public static class MonoMonadExtensions
    {
        /// <summary>
        /// A more readable alias to <see cref="IMonoMonad{T, TSource}.Bind(Func{TSource, T})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="a">The value containing the source.</param>
        /// <param name="f">The function to apply.</param>
        public static T ThenMono<T, TSource>(this IMonoMonad<T, TSource> a, Func<TSource, T> f)
            where T : IMonoMonad<T, TSource>
            => a.Bind(f);

        /// <summary>
        /// Chains two monad-producing functions by executing the first, then the second, gluing them together using
        /// <see cref="IMonoMonad{T, TSource}.Bind(Func{TSource, T})"/>. The monadic equivalent of
        /// <see cref="F.Then{TA, TB, TC}(Func{TA, TB}, Func{TB, TC})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TSource">The input of the first function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TSource, T> ThenMMono<T, TSource>(this Func<TSource, T> f, Func<TSource, T> g)
            where T : IMonoMonad<T, TSource>
            => x => f(x).Bind(g);

        /// <summary>
        /// Chains two monad-producing functions by executing the second, then the first, gluing them together using
        /// <see cref="IMonoMonad{T, TSource}"/>. The monadic equivalent of
        /// <see cref="F.After{TA, TB, TC}(Func{TB, TC}, Func{TA, TB})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TSource">The input of the second function.</typeparam>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        public static Func<TSource, T> AfterMMono<T, TSource>(this Func<TSource, T> f, Func<TSource, T> g)
            where T : IMonoMonad<T, TSource>
            => x => g(x).Bind(f);

        /// <summary>
        /// Extracts the value(s) from the source and replaces it with another one, ignoring the values of the source.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="a">The value containing the source.</param>
        /// <param name="b">The value containing the result.</param>
        public static T ThenMono<T, TSource>(this IMonoMonad<T, TSource> a, T b)
            where T : IMonoMonad<T, TSource>
            => a.Bind(x => b);

        /// <summary>
        /// Concatenates two mono-monad-functions.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="f">The first monad function.</param>
        /// <param name="g">The second monad function.</param>
        public static Func<TSource, T> CompMono<T, TSource>(this Func<TSource, T> f, Func<TSource, T> g)
            where T : IMonoMonad<T, TSource>
            => x => f(x).Bind(g);

        /// <summary>
        /// Wraps an object in an applicative via <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>.
        /// The applicative in question does not have to posess a parameterless constuctor; instead, a call to <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/> with the this-pointer being null is forced.
        /// If the requested applicative uses the this-pointer in <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/>, this will result in a <see cref="NullReferenceException"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the object to wrap.</typeparam>
        /// <typeparam name="T">The type of the applicative to return.</typeparam>
        /// <param name="x">The value to wrap.</param>
        /// <exception cref="NullReferenceException">If <see cref="IApplicative{TSource}.Pure{TResult}(TResult)"/> of <typeparamref name="T"/> uses the this-pointer.</exception>
        public static T PureUnsafeMono<TValue, T>(this TValue x)
            where T : IMonoMonad<T, TValue>
        {
            var instancePure = typeof(T).GetMember(
                "Pure",
                MemberTypes.Method,
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public);

            var pureMi = instancePure.Cast<MethodInfo>().First(mi =>
            {
                var gargs = mi.GetGenericArguments();
                return gargs.Length == 1
                && gargs[0].IsGenericParameter
                && mi.IsGenericMethod
                && mi.GetParameters().Length == 1;
            }).MakeGenericMethod(typeof(TValue));

            var pure = new DynamicMethod("pure", typeof(T), new Type[] { typeof(TValue) });
            var generator = pure.GetILGenerator();

            var applicativeThis = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Ldloca_S, 0); //push applicativeThis (index 0) onto the stack: [] -> [at]
            generator.Emit(OpCodes.Initobj, typeof(T)); //initialize at to null: at -> []
            generator.Emit(OpCodes.Ldloc_0); //put local variable 0 to the stack again: [] -> at
            generator.Emit(OpCodes.Box, typeof(T)); //box the value: [at:stack] -> [at:heap]
            generator.Emit(OpCodes.Ldarg_0); //load the first argument (x) onto the stack: [at:heap] -> [at:heap, x]
            generator.EmitCall(OpCodes.Call, pureMi, null); //call pureMi without null-checking: [at:heap, x] -> [ret]
            generator.Emit(OpCodes.Ret); //return: []

            var res = (T)pure.Invoke(null, new object[] { x });

            return res;
        }

        /// <summary>
        /// Lifts a binary function to take two <see cref="IMonoMonad{T, TSource}"/> arguments. See <see cref="Applicative.LiftA{T1, T2, TResult}(Func{T1, T2, TResult})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TValue">The type of the arguments.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<T, T, T>
            LiftMonoM<T, TValue>(this Func<TValue, TValue, TValue> f)
            where T : IMonoMonad<T, TValue>
            => (x, y) => x.Bind(x1 => y.Bind(y1 => PureUnsafeMono<TValue, T>(f(x1,y1))));

        /// <summary>
        /// Lifts a ternary function to take three <see cref="IMonoMonad{T, TSource}"/> arguments. See <see cref="LiftMonoM{T, TValue}(Func{TValue, TValue, TValue})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TValue">The type of the arguments.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<T, T, T, T>
            LiftMonoA<T, TValue>(this Func<TValue, TValue, TValue, TValue> f)
            where T : IMonoMonad<T, TValue>
            => (x, y, z) => x.Bind(x1 => y.Bind(y1 => z.Bind(z1 => PureUnsafeMono<TValue, T>(f(x1, y1, z1)))));

        /// <summary>
        /// Lifts a quaternary function to take three <see cref="IMonoMonad{T, TSource}"/> arguments. See <see cref="LiftMonoM{T, TValue}(Func{TValue, TValue, TValue})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IMonoMonad{T, TSource}"/>.</typeparam>
        /// <typeparam name="TValue">The type of the arguments.</typeparam>
        /// <param name="f">The function to lift.</param>
        public static Func<T, T, T, T, T>
            LiftMonoA<T, TValue>(this Func<TValue, TValue, TValue, TValue, TValue> f)
            where T : IMonoMonad<T, TValue>
            => (x, y, z, u) => x.Bind(x1 => y.Bind(y1 => z.Bind(z1 => u.Bind(u1 => PureUnsafeMono<TValue, T>(f(x1, y1, z1, u1))))));
    }
}
