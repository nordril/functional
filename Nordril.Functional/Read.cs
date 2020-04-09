using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// Types which can be parsed from a character-sequence, e.g. a string
    /// </summary>
    /// <typeparam name="T">The type to parse.</typeparam>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    public interface IRead<T, TOptions>
        where T : IRead<T, TOptions>
    {
        /// <summary>
        /// Tries to parse a value of type <typeparamref name="T"/> from a character-sequence, using options of type <typeparamref name="TOptions"/>.
        /// Implementors MUST NOT use the <c>this</c>-pointer during this operation.
        /// </summary>
        /// <param name="source">The character-sequence to parse.</param>
        /// <param name="options">The options to use for the parsing.</param>
        /// <returns><see cref="Maybe.Just{T}(T)"/> if the parsing was successful, and <see cref="Maybe.Nothing{T}"/> otherwise.</returns>
        Maybe<T> TryParse(IEnumerable<char> source, TOptions options);
    }

    /// <summary>
    /// Extensions methods for <see cref="IRead{T, TOptions}"/>.
    /// </summary>
    public static class Read
    {
        /// <summary>
        /// Like <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/>, but without options.
        /// </summary>
        /// <typeparam name="T">The type to parse.</typeparam>
        /// <param name="instance">The instance on which to call <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/>.</param>
        /// <param name="source">The character-sequences to parse.</param>
        public static Maybe<T> TryParse<T>(this IRead<T, Unit> instance, IEnumerable<char> source)
            where T : IRead<T, Unit>
            => instance.TryParse(source, new Unit());

        /// <summary>
        /// Calls <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/> without an instance. This is safe to do if the implementor's <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/>-method does not use the this-pointer.
        /// </summary>
        /// <typeparam name="T">The type to parse.</typeparam>
        /// <param name="source">The character-sequences to parse.</param>
        public static Maybe<T> TryParseUnsafe<T>(IEnumerable<char> source)
            where T : IRead<T, Unit>
            => TryParseUnsafe<T, Unit>(source, new Unit());

        /// <summary>
        /// Calls <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/> without an instance. This is safe to do if the implementor's <see cref="IRead{T, TOptions}.TryParse(IEnumerable{char}, TOptions)"/>-method does not use the this-pointer.
        /// </summary>
        /// <typeparam name="T">The type to parse.</typeparam>
        /// <typeparam name="TOptions">The type of the options to use.</typeparam>
        /// <param name="source">The character-sequences to parse.</param>
        /// <param name="options">The options to use for the parsing.</param>
        public static Maybe<T> TryParseUnsafe<T, TOptions>(IEnumerable<char> source, TOptions options)
            where T : IRead<T, TOptions>
        {
            var instancePure = typeof(T).GetMember(
                nameof(IRead<ReadProxy, object>.TryParse),
                MemberTypes.Method,
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public);

            var tryParseMi = instancePure.Cast<MethodInfo>().First(mi =>
            {
                var gargs = mi.GetGenericArguments();
                return gargs.Length == 0
                && mi.GetParameters().Length == 2
                && !mi.IsGenericMethod;
            });

            var tryParse = new DynamicMethod("tryParse", typeof(Maybe<T>), new Type[] { typeof(IEnumerable<char>), typeof(TOptions) });
            var generator = tryParse.GetILGenerator();

            var readThis = generator.DeclareLocal(typeof(IRead<T, TOptions>));
            generator.Emit(OpCodes.Ldloca_S, 0); //push readThis (index 0) onto the stack: [] -> [rt]
            generator.Emit(OpCodes.Initobj, typeof(IRead<T, TOptions>)); //initialize at to null: rt -> []
            generator.Emit(OpCodes.Ldloc_0); //put local variable 0 to the stack again: [] -> rt
            generator.Emit(OpCodes.Box, typeof(IRead<T, TOptions>)); //box the value: [rt:stack] -> [rt:heap]
            generator.Emit(OpCodes.Ldarg_0); //load the first argument (source) onto the stack: [rt:heap] -> [rt:heap, source]
            generator.Emit(OpCodes.Ldarg_1); //load the second argument (options) onto the stack [rt:heap, source] -> [rt:heap, source, options)
            generator.EmitCall(OpCodes.Call, tryParseMi, null); //call tryParseMi without null-checking: [rt:heap, source , options] -> [ret]
            generator.Emit(OpCodes.Ret); //return: []

            var res = (Maybe<T>)tryParse.Invoke(null, new object[] { source, options });

            return res;
        }

        /// <summary>
        /// A proxy for <c>nameof</c>.
        /// </summary>
        private class ReadProxy : IRead<ReadProxy, object>
        {
            public Maybe<ReadProxy> TryParse(IEnumerable<char> source, object options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
