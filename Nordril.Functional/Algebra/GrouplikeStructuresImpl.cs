using Nordril.Functional.Category;
using Nordril.Functional.Data;
using Sigil;
using Sigil.NonGeneric;
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
        /// Creates a new magma.
        /// </summary>
        /// <param name="op">The binary operation.</param>
        public Magma(Func<T, T, T> op)
        {
            this.op = op;
        }

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }

    /// <summary>
    /// A covariant product-wrapper around a grouplike structure.
    /// </summary>
    /// <typeparam name="T">The type of the contained grouplike.</typeparam>
    public struct ContainsGrouplike<T> : IContainsFirst<T>
    {
        /// <inheritdoc />
        public T First { get; }

        /// <summary>
        /// Creates a new instance. See also <see cref="Magma.AsProduct{T, TGrouplike}(TGrouplike)"/>.
        /// </summary>
        /// <param name="first">The contained grouplike.</param>
        public ContainsGrouplike(T first)
        {
            First = first;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="IMagma{T}"/>.
    /// </summary>
    public static class Magma
    {
        /// <summary>
        /// Turns an <see cref="IMagma{T}"/> into a binary relation which has a tuple of inputs on the left and an output on the right.
        /// </summary>
        /// <typeparam name="T">The type of element in this structure.</typeparam>
        /// <param name="m">The magma.</param>
        public static IFunctionRelation<(T, T), T> ToRelation<T>(this IMagma<T> m)
            where T : IEquatable<T>
            => new FunctionRelation<(T, T), T>(xy => m.Op(xy.Item1, xy.Item2));

        /// <summary>
        /// Wraps a grouplike structure in a single-element product so that it can be used in functions which expect products like <see cref="Ringlike.Zero{T, TFirst}(IContainsFirst{TFirst})"/>.
        /// </summary>
        /// <typeparam name="T">The type of the carrier set.</typeparam>
        /// <typeparam name="TGrouplike">The type of the grouplike to wrap.</typeparam>
        /// <param name="m">The grouplike to wrap.</param>
        public static ContainsGrouplike<TGrouplike> AsProduct<T, TGrouplike>(this TGrouplike m)
            where TGrouplike : IMagma<T>
            => new ContainsGrouplike<TGrouplike>(m);
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
        /// Creates a new semigroup.
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
            var ret = GetNeutralUnsafe<T, TNeutralElement>()();
            return ret;
        }

        /// <summary>
        /// Returns a function which, if called, returns <see cref="INeutralElement{T}.Neutral"/> of an <typeparamref name="TNeutralElement"/>. The type in question does not have to possess a parameterless constructor; instead, a call to <see cref="INeutralElement{T}.Neutral"/> with the this-pointer being null is forced. If <see cref="INeutralElement{T}.Neutral"/> of <typeparamref name="TNeutralElement"/> uses the this-pointer, a <see cref="NullReferenceException"/> will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of the neutral element.</typeparam>
        /// <typeparam name="TNeutralElement">The type of the <see cref="INeutralElement{T}"/>.</typeparam>
        /// <exception cref="NullReferenceException">If <see cref="INeutralElement{T}.Neutral"/> of <typeparamref name="TNeutralElement"/> uses the this-pointer.</exception>
        public static Func<T> GetNeutralUnsafe<T, TNeutralElement>()
        {
            var instanceNeutral = typeof(TNeutralElement).GetProperty(nameof(INeutralElement<object>.Neutral)).GetMethod;

            var n = Emit<Func<T>>.NewDynamicMethod("neutral");
            var mthis = n.DeclareLocal<TNeutralElement>("monoidThis");
            if (typeof(TNeutralElement).IsValueType)
            {
                n.LoadLocalAddress(mthis);
                n.InitializeObject<TNeutralElement>();
                n.LoadLocalAddress(mthis);
            }
            else if (!typeof(TNeutralElement).IsConstructedGenericType)
            {
                n.LoadNull();
                n.StoreLocal(mthis);
                n.LoadLocal(mthis);
            }
            else
            {
                n.NewObject<TNeutralElement>();
                n.StoreLocal(mthis);
                n.LoadLocal(mthis);
            }

            n.Call(instanceNeutral);
            n.Return();

            var ret = n.CreateDelegate();
            return ret;
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

            var op = Emit<Func<T, T, T>>.NewDynamicMethod("op");
            var mthis = op.DeclareLocal<TMagma>();

            if (typeof(TMagma).IsValueType)
            {
                op.LoadLocalAddress(mthis);
                op.InitializeObject<TMagma>();
                op.LoadLocalAddress(mthis);
            }
            else if (!typeof(TMagma).IsConstructedGenericType)
            {
                op.LoadNull();
                op.StoreLocal(mthis);
                op.LoadLocal(mthis);
            }
            else
            {
                op.NewObject<TMagma>();
                op.StoreLocal(mthis);
                op.LoadLocal(mthis);
            }

            op.LoadArgument(0);
            op.LoadArgument(1);
            op.Call(instanceOp);
            op.Return();

            return op.CreateDelegate();
        }

        /// <summary>
        /// Sums a list of monoid elements using the monoid operation. If the list is empty, the neutral element is returned. 
        /// </summary>
        /// <typeparam name="T">The monoid type.</typeparam>
        /// <param name="xs">The list of elements to sum.</param>
        public static T Msum<T>(this IEnumerable<T> xs) where T : IHasMonoid<T>
            => xs.Aggregate(Monoid.NeutralUnsafe<T, T>(), (x, y) => x.Op(y));

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
        public static FirstOrDefaultMonoid<T> FirstOrDefault<T>(T @default) => new FirstOrDefaultMonoid<T>(@default);

        /// <summary>
        /// The monoid whose operation always returns the last element.
        /// </summary>
        /// <param name="default">The neutral element of the monoid.</param>
        /// <typeparam name="T">The type of the element in the structure.</typeparam>
        public static LastOrDefaultMonoid<T> LastOrDefault<T>(T @default) => new LastOrDefaultMonoid<T>(@default);

        /// <summary>
        /// The monoid whose neutral element is <see cref="Maybe.Nothing{T}"/> and which always returns the first non-<see cref="Maybe.Nothing{T}"/> operand.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        public static FirstMonoid<T> First<T>() => new FirstMonoid<T>();

        /// <summary>
        /// The monoid of function composition, for <c>Func&lt;T,T&gt;</c>
        /// </summary>
        /// <typeparam name="T">The type of the input/output elements.</typeparam>
        /// <returns></returns>
        public static EndoMonoid<T> Endo<T>() => new EndoMonoid<T>();

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
            public IList<T> Neutral => new FuncList<T>(null);

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
        public class BoolAndMonoid : IMonoid<bool>, ICommutative<bool>, IIdempotent<bool>
        {
            /// <inheritdoc />
            public bool Neutral => true;

            /// <inheritdoc />
            public bool Op(bool x, bool y) => x && y;
        }

        /// <summary>
        /// The (false,||) monoid for <see cref="bool"/>.
        /// </summary>
        public class BoolOrMonoid : IMonoid<bool>, ICommutative<bool>, IIdempotent<bool>
        {
            /// <inheritdoc />
            public bool Neutral => false;

            /// <inheritdoc />
            public bool Op(bool x, bool y) => x || y;
        }

        /// <summary>
        /// The (1,*) monoid for <see cref="int"/>.
        /// </summary>
        public class IntMultMonoid : IMonoid<int>, ICommutative<int>
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

        /// <summary>
        /// The (Nothing, Alt)-monoid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class FirstMonoid<T> : IMonoid<Maybe<T>>
        {
            /// <inheritdoc />
            public Maybe<T> Neutral => Maybe.Nothing<T>();

            /// <inheritdoc />
            public Maybe<T> Op(Maybe<T> x, Maybe<T> y) => x.ValueOr(_ => x, y);
        }

        /// <summary>
        /// The (id, (.))-monoid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EndoMonoid<T> : IMonoid<Func<T,T>>
        {
            /// <inheritdoc />
            public Func<T, T> Neutral => F.Id<T>();

            /// <inheritdoc />
            public Func<T, T> Op(Func<T, T> x, Func<T, T> y) => x.After(y);
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
        public static Group<T> FromGroupInstance<T>() where T : IGroup<T>
        {
            var instance = Monoid.NeutralUnsafe<T, T>();
            return new Group<T>(instance.Neutral, (x, y) => x.Op(y), x => x.Inverse());
        }

        /// <summary>
        /// The (0,XOR,id) group for <see cref="bool"/>.
        /// </summary>
        public static readonly BoolXorGroup BoolXor = new BoolXorGroup();

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
        /// The (0,XOR,id) group for <see cref="bool"/>.
        /// </summary>
        public class BoolXorGroup : ICommutativeGroup<bool>
        {
            /// <inheritdoc />
            public bool Neutral => false;

            /// <inheritdoc />
            public bool Inverse(bool x) => x;

            /// <inheritdoc />
            public bool Op(bool x, bool y) => x ^ y;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="int"/>.
        /// </summary>
        public class IntAddGroup : ICommutativeGroup<int>
        {
            /// <inheritdoc />
            public int Neutral => 0;

            /// <inheritdoc />
            public int Inverse(int x) => x * -1;

            /// <inheritdoc />
            public int Op(int x, int y) => x + y;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="long"/>.
        /// </summary>
        public class LongAddGroup : ICommutativeGroup<long>
        {
            /// <inheritdoc />
            public long Neutral => 0;

            /// <inheritdoc />
            public long Inverse(long x) => x * -1;

            /// <inheritdoc />
            public long Op(long x, long y) => x + y;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="float"/>.
        /// </summary>
        public class FloatAddGroup : ICommutativeGroup<float>
        {
            /// <inheritdoc />
            public float Neutral => 0;

            /// <inheritdoc />
            public float Inverse(float x) => x * -1f;

            /// <inheritdoc />
            public float Op(float x, float y) => x + y;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="double"/>.
        /// </summary>
        public class DoubleAddGroup : ICommutativeGroup<double>
        {
            /// <inheritdoc />
            public double Neutral => 0;

            /// <inheritdoc />
            public double Inverse(double x) => x * -1d;

            /// <inheritdoc />
            public double Op(double x, double y) => x + y;
        }

        /// <summary>
        /// The (0,+,negate) group for <see cref="decimal"/>.
        /// </summary>
        public class DecimalAddGroup : ICommutativeGroup<decimal>
        {
            /// <inheritdoc />
            public decimal Neutral => 0;


            /// <inheritdoc />
            public decimal Inverse(decimal x) => x * -1m;

            /// <inheritdoc />
            public decimal Op(decimal x, decimal y) => x + y;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="float"/>.
        /// </summary>
        public class FloatMultGroup : ICommutativeGroup<float>
        {
            /// <inheritdoc />
            public float Neutral => 1f;

            /// <inheritdoc />
            public float Inverse(float x) => 1f / x;

            /// <inheritdoc />
            public float Op(float x, float y) => x * y;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="double"/>.
        /// </summary>
        public class DoubleMultGroup : ICommutativeGroup<double>
        {
            /// <inheritdoc />
            public double Neutral => 1d;

            /// <inheritdoc />
            public double Inverse(double x) => 1d / x;

            /// <inheritdoc />
            public double Op(double x, double y) => x * y;
        }

        /// <summary>
        /// The (1,*,x =&gt; 1/x) group for <see cref="decimal"/>.
        /// </summary>
        public class DecimalMultGroup : ICommutativeGroup<decimal>
        {
            /// <inheritdoc />
            public decimal Neutral => 1m;

            /// <inheritdoc />
            public decimal Inverse(decimal x) => 1m / x;

            /// <inheritdoc />
            public decimal Op(decimal x, decimal y) => x * y;
        }
    }
    #endregion

    #region Semilattices
    /// <summary>
    /// A value-level group.
    /// </summary>
    /// <typeparam name="T">The type of element in this structure.</typeparam>
    public struct Semilattice<T> : ISemilattice<T>
    {
        /// <summary>
        /// The binary operation.
        /// </summary>
        private readonly Func<T, T, T> op;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="op">The binary operation.</param>
        public Semilattice(Func<T, T, T> op)
        {
            this.op = op;
        }

        /// <inheritdoc />
        public T Op(T x, T y) => op(x, y);
    }

    /// <summary>
    /// Extension methods for <see cref="ISemilattice{T}"/>s.
    /// </summary>
    public static class Semilattice
    {
        /// <summary>
        /// A meet-semilattice whose operation creates the union of two sets.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the set.</typeparam>
        public class SetMeetSemilattice<T> : ISemilattice<ISet<T>>
            where T : IEquatable<T>
        {
            /// <inheritdoc />
            public ISet<T> Op(ISet<T> x, ISet<T> y) => x.Union(y).ToHashSet();
        }

        /// <summary>
        /// A join-semilattice whose operation creates the intersection of two sets.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the set.</typeparam>
        public class SetJoinSemilattice<T> : ISemilattice<ISet<T>>
            where T : IEquatable<T>
        {
            /// <inheritdoc />
            public ISet<T> Op(ISet<T> x, ISet<T> y) => x.Intersect(y).ToHashSet();
        }
    }
    #endregion
}
