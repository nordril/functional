using Nordril.Functional.Category;
using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Nordril.Functional.Algebra
{
    #region Magma
    /// <summary>
    /// A value-level monoid.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public struct Magma<T> : IMagma<T>
    {
        /// <summary>
        /// The binary operation.
        /// </summary>
        private readonly Func<T, T, T> op;

        /// <summary>
        /// Creates a new monoid.
        /// </summary>
        /// <param name="op">The binary operation.</param>
        public Magma(Func<T, T, T> op)
        {
            this.op = op;
        }

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }
    #endregion

    #region Semigroup
    /// <summary>
    /// A value-level semigroup.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public struct Semigroup<T> : ISemigroup<T>
    {
        /// <summary>
        /// The binary operation.
        /// </summary>
        private readonly Func<T, T, T> op;

        /// <summary>
        /// Creates a new monoid.
        /// </summary>
        /// <param name="op">The binary operation.</param>
        public Semigroup(Func<T, T, T> op)
        {
            this.op = op;
        }

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }

    /// <summary>
    /// Extension methods for <see cref="Semigroup{T}"/>.
    /// </summary>
    public static class Semigroup
    {
        /// <summary>
        /// The (^) semigroup for <see cref="bool"/>.
        /// </summary>
        public static readonly Semigroup<bool> BoolXor = new Semigroup<bool>((x, y) => x ^ y);

        /// <summary>
        /// The semigroup whose operation always returns the first element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static Semigroup<T> First<T>() => new Semigroup<T>((x, _) => x);

        /// <summary>
        /// The semigroup whose operation always returns the last element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static Semigroup<T> Last<T>() => new Semigroup<T>((_, y) => y);

        /// <summary>
        /// Lifts a <see cref="Semigroup{T}"/> into one which has positive infinity (<see cref="Maybe.Nothing{T}"/>) as a special element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        /// <param name="m">The monoid to lift.</param>
        public static Semigroup<Maybe<T>> LiftSemigroupWithInfinity<T>(this Semigroup<T> m)
        {
            Func<T, T, T> mOp = m.Op;

            return new Semigroup<Maybe<T>>((x, y) => mOp.LiftA()(x, y).ToMaybe());
        }
    }
    #endregion

    #region Monoid
    /// <summary>
    /// A value-level monoid.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public struct Monoid<T> : IMonoid<T>
    {
        /// <summary>
        /// The neutral element of the binary operation.
        /// </summary>
        public T Neutral { get; private set; }

        /// <summary>
        /// The binary operation.
        /// </summary>
        private readonly Func<T, T, T> op;

        /// <summary>
        /// Creates a new monoid.
        /// </summary>
        /// <param name="neutral">The neutral element.</param>
        /// <param name="op">The binary operation.</param>
        public Monoid(T neutral, Func<T, T, T> op)
        {
            this.op = op;
            Neutral = neutral;
        }

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }

    /// <summary>
    /// Extension methods for <see cref="Monoid{T}"/>.
    /// </summary>
    public static class Monoid
    {
        /// <summary>
        /// Returns <see cref="INeutralElement{T}.Neutral"/> of an <typeparamref name="TNeutralElement"/>. The type in question does not have to possess a parameterless constructor; instead, a call to <see cref="INeutralElement{T}.Neutral"/> with the this-pointer being null is forced. If <see cref="INeutralElement{T}.Neutral"/> of <typeparamref name="TNeutralElement"/> uses the this-pointer, a <see cref="NullReferenceException"/> will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of the neutral element.</typeparam>
        /// <typeparam name="TNeutralElement">The type of the <see cref="INeutralElement{T}"/>.</typeparam>
        /// <exception cref="NullReferenceException">If <see cref="INeutralElement{T}.Neutral"/> of <typeparamref name="TNeutralElement"/> uses the this-pointer.</exception>
        public static T NeutralUnsafe<T, TNeutralElement>()
            where TNeutralElement : INeutralElement<T>
        {
            var instanceNeutral = typeof(TNeutralElement).GetProperty(nameof(INeutralElement<object>.Neutral)).GetMethod;

            var neutral = new DynamicMethod("neutral", typeof(T), new Type[0]);
            var generator = neutral.GetILGenerator();

            var monoidThis = generator.DeclareLocal(typeof(TNeutralElement));
            generator.Emit(OpCodes.Ldloca_S, 0); //push monoidThis (index 0) onto the stack: [] -> [mt]
            generator.Emit(OpCodes.Initobj, typeof(TNeutralElement)); //initialize mt to null: mt -> []
            generator.Emit(OpCodes.Ldloc_0); //put local variable 0 to the stack again: [] -> mt
            generator.Emit(OpCodes.Box, typeof(TNeutralElement)); //box the value: [mt:stack] -> [mt:heap]
            generator.EmitCall(OpCodes.Call, instanceNeutral, null); //call neutral without null-checking: [mt:heap] -> [ret]
            generator.Emit(OpCodes.Ret); //return: []

            var res = (T)neutral.Invoke(null, new object[0]);

            return res;
        }

        /// <summary>
        /// Calls <see cref="IMagma{T}.Op(T, T)"/> of of an <typeparamref name="TMagma"/>. The type in question does not have to possess a parameterless constructor; instead, a call to <see cref="IMagma{T}.Op(T, T)"/> with the this-pointer being null is forced. If <see cref="IMagma{T}.Op(T, T)"/> of <typeparamref name="TMagma"/> uses the this-pointer, a <see cref="NullReferenceException"/> will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of the neutral element.</typeparam>
        /// <typeparam name="TMagma">The type of the <see cref="IMagma{T}"/>.</typeparam>
        /// <exception cref="NullReferenceException">If <see cref="IMagma{T}.Op(T, T)"/> of <typeparamref name="TMagma"/> uses the this-pointer.</exception>
        public static Func<T, T, T> OpUnsafe<T, TMagma>()
            where TMagma : IMagma<T>
        {
            var instanceOp = typeof(TMagma).GetMember(
                nameof(INeutralElement<object>.Op),
                MemberTypes.Method,
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public)
            .Cast<MethodInfo>()
            .First(mi =>
            {
                var pars = mi.GetParameters();

                return !mi.IsGenericMethod && pars.Length == 2
                && pars[0].ParameterType == typeof(T)
                && pars[1].ParameterType == typeof(T);
            });

            var op = new DynamicMethod("op", typeof(T), new Type[] { typeof(T), typeof(T) });
            var generator = op.GetILGenerator();

            var monoidThis = generator.DeclareLocal(typeof(TMagma));
            generator.Emit(OpCodes.Ldloca_S, 0); //push monoidThis (index 0) onto the stack: [] -> [mt]
            generator.Emit(OpCodes.Initobj, typeof(TMagma)); //initialize mt to null: mt -> []
            generator.Emit(OpCodes.Ldloc_0); //put local variable 0 to the stack again: [] -> mt
            generator.Emit(OpCodes.Box, typeof(TMagma)); //box the value: [mt:stack] -> [mt:heap]
            generator.Emit(OpCodes.Ldarg_0); //[mt:heap, x]
            generator.Emit(OpCodes.Ldarg_1); //[mt:heap, x, y]
            generator.EmitCall(OpCodes.Call, instanceOp, null); //call op without null-checking: [mt:heap, x, y] -> [ret]
            generator.Emit(OpCodes.Ret); //return: []

            var res = (Func<T, T, T>)op.CreateDelegate(typeof(Func<T, T, T>));

            return res;
        }

        /// <summary>
        /// Sums a list of monoid elements using the monoid operation. If the list is empty, the neutral element is returned. 
        /// </summary>
        /// <typeparam name="T">The monoid type.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        public static T Msum<T>(this IEnumerable<T> xs) where T : IHasMonoid<T>, new()
            => xs.Aggregate(new T().Neutral, (x, y) => x.Op(y));

        /// <summary>
        /// Sums a list of monoid elements using the monoid operation. If the list is empty, the neutral element is returned. 
        /// </summary>
        /// <typeparam name="T">The monoid type.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        /// <param name="empty">The neutral element.</param>
        public static T Msum<T>(this IEnumerable<T> xs, Func<T> empty) where T : IMonoid<T>
            => xs.Aggregate(empty(), (x, y) => x.Op(y));

        /// <summary>
        /// Lifts an <see cref="IMonoid{T}"/> into one which has positive infinity (<see cref="Maybe.Nothing{T}"/>) as a special element.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        /// <param name="m">The monoid to lift.</param>
        public static IMonoid<Maybe<T>> LiftMonoidWithInfinity<T>(this IMonoid<T> m)
        {
            Func<T, T, T> mOp = m.Op;

            return new Monoid<Maybe<T>>(Maybe.Just(m.Neutral), (x, y) => mOp.LiftA()(x, y).ToMaybe());
        }

        /// <summary>
        /// Sums a list of elements using a monoid instance. If the list is empty, the neutral element is returned.
        /// </summary>
        /// <typeparam name="T">The type of elements to sum.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        /// <param name="m">The monoid instance.</param>
        public static T Msum<T>(this IEnumerable<T> xs, IMonoid<T> m) => xs.Aggregate(m.Neutral, (x, y) => m.Op(x, y));

        /// <summary>
        /// The mutating ([],++) monoid for lists. The binary operation mutates the first list.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        public static IMonoid<List<T>> ListAppend<T>() => new ListAppendMonoid<T>();

        /// <summary>
        /// The non-mutating ([],++) monoid for lists, meaning that the inputs of <see cref="IMagma{T}.Op" /> aren't changed.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        public static IMonoid<IList<T>> ImmutableListAppend<T>() => new ListAppendImmutableMonoid<T>();

        /// <summary>
        /// The ("",+) monoid for <see cref="string"/>.
        /// BEWARE THAT THIS HAS VERY POOR PERFORMANCE.
        /// </summary>
        [Obsolete("StringAppend is deprecated. Use StringBuilderAppend instead.")]
        public static readonly IMonoid<string> StringAppend = new Monoid<string>(string.Empty, (x, y) => x + y);

        /// <summary>
        /// The ("",Append) monoid for <see cref="StringBuilder"/>. Appending mutates the first argument.
        /// </summary>
        public static readonly StringBuilderAppendMonoid StringBuilderAppend = new StringBuilderAppendMonoid();

        /// <summary>
        /// The (true,&amp;&amp;) monoid for <see cref="bool"/>.
        /// </summary>
        public static readonly BoolAndMonoid BoolAnd = new BoolAndMonoid();

        /// <summary>
        /// The (false,||) monoid for <see cref="bool"/>.
        /// </summary>
        public static readonly BoolOrMonoid BoolOr = new BoolOrMonoid();

        /// <summary>
        /// The (1,*) monoid for <see cref="int"/>.
        /// </summary>
        public static readonly IntMultMonoid IntMult = new IntMultMonoid();

        /// <summary>
        /// The monoid whose operation always returns the first element and which has a default-value.
        /// </summary>
        /// <param name="default">The neutral element of the monoid.</param>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static IMonoid<T> FirstOrDefault<T>(T @default) => new FirstOrDefaultMonoid<T>(@default);

        /// <summary>
        /// The semigroup whose operation always returns the last element.
        /// </summary>
        /// <param name="default">The neutral element of the monoid.</param>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static IMonoid<T> LastOrDefault<T>(T @default) => new LastOrDefaultMonoid<T>(@default);

        /// <summary>
        /// The mutating ([],++) monoid for lists. The binary operation mutates the first list.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        public class ListAppendMonoid<T> : IMonoid<List<T>>
        {
            /// <inheritdoc />
            public List<T> Neutral => new List<T>();

            /// <inheritdoc />
            public List<T> Op(List<T> x, List<T> y)
            {
                x.AddRange(y);
                return x;
            }
        }

        /// <summary>
        /// The non-mutating ([],++) monoid for lists, meaning that the inputs of <see cref="IMagma{T}.Op" /> aren't changed.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        public class ListAppendImmutableMonoid<T> : IMonoid<IList<T>>
        {
            /// <inheritdoc />
            public IList<T> Neutral => new FuncList<T>();

            /// <inheritdoc />
            public IList<T> Op(IList<T> x, IList<T> y) => new FuncList<T>(x.Concat(y));
        }

        /// <summary>
        /// The ("",Append) monoid for <see cref="StringBuilder"/>. Appending mutates the first argument.
        /// </summary>
        public class StringBuilderAppendMonoid : IMonoid<StringBuilder>
        {
            /// <inheritdoc />
            public StringBuilder Neutral => new StringBuilder();

            /// <inheritdoc />
            public StringBuilder Op(StringBuilder x, StringBuilder y)
            {
                x.EnsureCapacity(x.Length + y.Length);
                x.Append(y);
                return x;
            }
        }

        /// <summary>
        /// The (true,&amp;&amp;) monoid for <see cref="bool"/>.
        /// </summary>
        public class BoolAndMonoid : IMonoid<bool>
        {
            /// <inheritdoc />
            public bool Neutral => true;

            /// <inheritdoc />
            public bool Op(bool x, bool y) => x && y;
        }

        /// <summary>
        /// The (false,||) monoid for <see cref="bool"/>.
        /// </summary>
        public class BoolOrMonoid : IMonoid<bool>
        {
            /// <inheritdoc />
            public bool Neutral => false;

            /// <inheritdoc />
            public bool Op(bool x, bool y) => x || y;
        }

        /// <summary>
        /// The (1,*) monoid for <see cref="int"/>.
        /// </summary>
        public class IntMultMonoid : IMonoid<int>
        {
            /// <inheritdoc />
            public int Neutral => 1;

            /// <inheritdoc />
            public int Op(int x, int y) => x * y;
        }

        /// <summary>
        /// The monoid whose operation always returns the first element and which has a default-value.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public class FirstOrDefaultMonoid<T> : IMonoid<T>
        {
            /// <summary>
            /// Creates a new instance.
            /// </summary>
            /// <param name="default">The neutral element of the monoid.</param>
            public FirstOrDefaultMonoid(T @default)
            {
                Neutral = @default;
            }

            /// <inheritdoc />
            public T Neutral { get; private set; }

            /// <inheritdoc />
            public T Op(T x, T y) => x;
        }

        /// <summary>
        /// The monoid whose operation always returns the last element and which has a default-value.
        /// </summary>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public class LastOrDefaultMonoid<T> : IMonoid<T>
        {
            /// <summary>
            /// Creates a new instance.
            /// </summary>
            /// <param name="default">The neutral element of the monoid.</param>
            public LastOrDefaultMonoid(T @default)
            {
                Neutral = @default;
            }

            /// <inheritdoc />
            public T Neutral { get; private set; }

            /// <inheritdoc />
            public T Op(T x, T y) => y;
        }
    }
    #endregion

    #region Group
    /// <summary>
    /// A value-level group.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public struct Group<T> : IGroup<T>
    {
        /// <summary>
        /// The neutral element of the binary operation.
        /// </summary>
        public T Neutral { get; private set; }

        /// <summary>
        /// The binary operation.
        /// </summary>
        private readonly Func<T, T, T> op;

        /// <summary>
        /// The inversion function.
        /// </summary>
        private readonly Func<T, T> inverse;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="neutral">The neutral element.</param>
        /// <param name="op">The binary operation.</param>
        /// <param name="inverse">The inversion function.</param>
        public Group(T neutral, Func<T, T, T> op, Func<T, T> inverse)
        {
            Neutral = neutral;
            this.op = op;
            this.inverse = inverse;
        }

        /// <inheritdoc />
        public T Inverse(T x) => inverse(x);

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }

    /// <summary>
    /// Extension methods for <see cref="Monoid{T}"/>.
    /// </summary>
    public static class Group
    {
        /// <summary>
        /// Reifies a type's <see cref="IHasGroup{T}"/> instance into its own <see cref="Group{T}"/>-object.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IHasGroup{T}"/>.</typeparam>
        /// <param name="instance">An object that is an instance of <see cref="IHasGroup{T}"/>.</param>
        public static Group<T> FromGroupInstance<T>(IHasGroup<T> instance) where T : IHasGroup<T>
            => new Group<T>(instance.Neutral, (x, y) => x.Op(y), x => x.Inverse());

        /// <summary>
        /// The (0,+,negate) group for <see cref="int"/>.
        /// </summary>
        public static readonly IntAddGroup IntAdd = new IntAddGroup();

        /// <summary>
        /// The (0,+,negate) group for <see cref="long"/>.
        /// </summary>
        public static readonly LongAddGroup LongAdd = new LongAddGroup();

        /// <summary>
        /// The (0,+,negate) group for <see cref="float"/>.
        /// </summary>
        public static readonly FloatAddGroup FloatAdd = new FloatAddGroup();

        /// <summary>
        /// The (0,+,negate) group for <see cref="double"/>.
        /// </summary>
        public static readonly DoubleAddGroup DoubleAdd = new DoubleAddGroup();

        /// <summary>
        /// The (0,+,negate) group for <see cref="decimal"/>.
        /// </summary>
        public static readonly DecimalAddGroup DecimalAdd = new DecimalAddGroup();

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="float"/>.
        /// </summary>
        public static readonly FloatMultGroup FloatMult = new FloatMultGroup();

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="double"/>.
        /// </summary>
        public static readonly DoubleMultGroup DoubleMult = new DoubleMultGroup();

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="decimal"/>.
        /// </summary>
        public static readonly DecimalMultGroup DecimalMult = new DecimalMultGroup();

        /// <summary>
        /// The (0,+,negate) group for <see cref="int"/>.
        /// </summary>
        public class IntAddGroup : IGroup<int>
        {
            /// <inheritdoc />
            public int Neutral => 0;

            /// <inheritdoc />
            public int Inverse(int x) => x * -1;

            /// <inheritdoc />
            public int Op(int x, int y) => x + x;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="long"/>.
        /// </summary>
        public class LongAddGroup : IGroup<long>
        {
            /// <inheritdoc />
            public long Neutral => 0;

            /// <inheritdoc />
            public long Inverse(long x) => x * -1;

            /// <inheritdoc />
            public long Op(long x, long y) => x + x;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="float"/>.
        /// </summary>
        public class FloatAddGroup : IGroup<float>
        {
            /// <inheritdoc />
            public float Neutral => 0;

            /// <inheritdoc />
            public float Inverse(float x) => x * -1f;

            /// <inheritdoc />
            public float Op(float x, float y) => x + x;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="double"/>.
        /// </summary>
        public class DoubleAddGroup : IGroup<double>
        {
            /// <inheritdoc />
            public double Neutral => 0;

            /// <inheritdoc />
            public double Inverse(double x) => x * -1d;

            /// <inheritdoc />
            public double Op(double x, double y) => x + x;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="decimal"/>.
        /// </summary>
        public class DecimalAddGroup : IGroup<decimal>
        {
            /// <inheritdoc />
            public decimal Neutral => 0;

            /// <inheritdoc />
            public decimal Inverse(decimal x) => x * -1m;

            /// <inheritdoc />
            public decimal Op(decimal x, decimal y) => x + x;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="float"/>.
        /// </summary>
        public class FloatMultGroup : IGroup<float>
        {
            /// <inheritdoc />
            public float Neutral => 1f;

            /// <inheritdoc />
            public float Inverse(float x) => 1f / x;

            /// <inheritdoc />
            public float Op(float x, float y) => x + x;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="double"/>.
        /// </summary>
        public class DoubleMultGroup : IGroup<double>
        {
            /// <inheritdoc />
            public double Neutral => 1d;

            /// <inheritdoc />
            public double Inverse(double x) => 1d / x;

            /// <inheritdoc />
            public double Op(double x, double y) => x + x;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="decimal"/>.
        /// </summary>
        public class DecimalMultGroup : IGroup<decimal>
        {
            /// <inheritdoc />
            public decimal Neutral => 1m;

            /// <inheritdoc />
            public decimal Inverse(decimal x) => 1m / x;

            /// <inheritdoc />
            public decimal Op(decimal x, decimal y) => x + x;
        }
    }
    #endregion
}
