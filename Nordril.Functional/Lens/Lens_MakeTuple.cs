using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Make
        {
            //Item1
            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            public static IMonoLens<(T1, T2), T1> Item1<T1, T2>()
                => L.Make.Lens<(T1, T2), T1>(t => t.Item1, (t, x) => (x, t.Item2));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            public static IMonoLens<(T1, T2, T3), T1> Item1<T1, T2, T3>()
                => L.Make.Lens<(T1, T2, T3), T1>(t => t.Item1, (t, x) => (x, t.Item2, t.Item3));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4), T1> Item1<T1, T2, T3, T4>()
                => L.Make.Lens<(T1, T2, T3, T4), T1>(t => t.Item1, (t, x) => (x, t.Item2, t.Item3, t.Item4));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5), T1> Item1<T1, T2, T3, T4, T5>()
                => L.Make.Lens<(T1, T2, T3, T4, T5), T1>(t => t.Item1, (t, x) => (x, t.Item2, t.Item3, t.Item4, t.Item5));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T1> Item1<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T1>(t => t.Item1, (t, x) => (x, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T1> Item1<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T1>(t => t.Item1, (t, x) => (x, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7));

            /// <summary>
            /// Makes a lens for the first element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T1> Item1<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T1>(t => t.Item1, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(x, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Rest));

            //Item2
            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            public static IMonoLens<(T1, T2), T2> Item2<T1, T2>()
                => L.Make.Lens<(T1, T2), T2>(t => t.Item2, (t, x) => (t.Item1, x));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            public static IMonoLens<(T1, T2, T3), T2> Item2<T1, T2, T3>()
                => L.Make.Lens<(T1, T2, T3), T2>(t => t.Item2, (t, x) => (t.Item1, x, t.Item3));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4), T2> Item2<T1, T2, T3, T4>()
                => L.Make.Lens<(T1, T2, T3, T4), T2>(t => t.Item2, (t, x) => (t.Item1, x, t.Item3, t.Item4));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5), T2> Item2<T1, T2, T3, T4, T5>()
                => L.Make.Lens<(T1, T2, T3, T4, T5), T2>(t => t.Item2, (t, x) => (t.Item1, x, t.Item3, t.Item4, t.Item5));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T2> Item2<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T2>(t => t.Item2, (t, x) => (t.Item1, x, t.Item3, t.Item4, t.Item5, t.Item6));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T2> Item2<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T2>(t => t.Item2, (t, x) => (t.Item1, x, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7));

            /// <summary>
            /// Makes a lens for the second element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T2> Item2<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T2>(t => t.Item2, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, x, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Rest));

            //Item3

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            public static IMonoLens<(T1, T2, T3), T3> Item3<T1, T2, T3>()
                => L.Make.Lens<(T1, T2, T3), T3>(t => t.Item3, (t, x) => (t.Item1, t.Item2, x));

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4), T3> Item3<T1, T2, T3, T4>()
                => L.Make.Lens<(T1, T2, T3, T4), T3>(t => t.Item3, (t, x) => (t.Item1, t.Item2, x, t.Item4));

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5), T3> Item3<T1, T2, T3, T4, T5>()
                => L.Make.Lens<(T1, T2, T3, T4, T5), T3>(t => t.Item3, (t, x) => (t.Item1, t.Item2, x, t.Item4, t.Item5));

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T3> Item3<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T3>(t => t.Item3, (t, x) => (t.Item1, t.Item2, x, t.Item4, t.Item5, t.Item6));

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T3> Item3<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T3>(t => t.Item3, (t, x) => (t.Item1, t.Item2, x, t.Item4, t.Item5, t.Item6, t.Item7));

            /// <summary>
            /// Makes a lens for the third element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T3> Item3<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T3>(t => t.Item3, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, x, t.Item4, t.Item5, t.Item6, t.Item7, t.Rest));

            //Item4
            /// <summary>
            /// Makes a lens for the fourth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4), T4> Item4<T1, T2, T3, T4>()
                => L.Make.Lens<(T1, T2, T3, T4), T4>(t => t.Item4, (t, x) => (t.Item1, t.Item2, t.Item3, x));

            /// <summary>
            /// Makes a lens for the fourth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5), T4> Item4<T1, T2, T3, T4, T5>()
                => L.Make.Lens<(T1, T2, T3, T4, T5), T4>(t => t.Item4, (t, x) => (t.Item1, t.Item2, t.Item3, x, t.Item5));

            /// <summary>
            /// Makes a lens for the fourth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T4> Item4<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T4>(t => t.Item4, (t, x) => (t.Item1, t.Item2, t.Item3, x, t.Item5, t.Item6));

            /// <summary>
            /// Makes a lens for the fourth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T4> Item4<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T4>(t => t.Item4, (t, x) => (t.Item1, t.Item2, t.Item3, x, t.Item5, t.Item6, t.Item7));

            /// <summary>
            /// Makes a lens for the fourth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T4> Item4<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T4>(t => t.Item4, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, t.Item3, x, t.Item5, t.Item6, t.Item7, t.Rest));

            //Item5
            /// <summary>
            /// Makes a lens for the fifth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5), T5> Item5<T1, T2, T3, T4, T5>()
                => L.Make.Lens<(T1, T2, T3, T4, T5), T5>(t => t.Item5, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, x));

            /// <summary>
            /// Makes a lens for the fifth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T5> Item5<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T5>(t => t.Item5, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, x, t.Item6));

            /// <summary>
            /// Makes a lens for the fifth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T5> Item5<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T5>(t => t.Item5, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, x, t.Item6, t.Item7));

            /// <summary>
            /// Makes a lens for the fifth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T5> Item5<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T5>(t => t.Item5, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, t.Item3, t.Item4, x, t.Item6, t.Item7, t.Rest));

            //Item6
            /// <summary>
            /// Makes a lens for the sixth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6), T6> Item6<T1, T2, T3, T4, T5, T6>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6), T6>(t => t.Item6, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, x));

            /// <summary>
            /// Makes a lens for the sixth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T6> Item6<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T6>(t => t.Item6, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, x, t.Item7));

            /// <summary>
            /// Makes a lens for the sixth element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T6> Item6<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T6>(t => t.Item6, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, x, t.Item7, t.Rest));

            //Item7

            /// <summary>
            /// Makes a lens for the seventh element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            public static IMonoLens<(T1, T2, T3, T4, T5, T6, T7), T7> Item7<T1, T2, T3, T4, T5, T6, T7>()
                => L.Make.Lens<(T1, T2, T3, T4, T5, T6, T7), T7>(t => t.Item7, (t, x) => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, x));

            /// <summary>
            /// Makes a lens for theseventh element of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T7> Item7<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, T7>(t => t.Item7, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, x, t.Rest));

            //Rest

            /// <summary>
            /// Makes a lens for the rest of a tuple.
            /// </summary>
            /// <typeparam name="T1">The type of the first element.</typeparam>
            /// <typeparam name="T2">The type of the second element.</typeparam>
            /// <typeparam name="T3">The type of the third element.</typeparam>
            /// <typeparam name="T4">The type of the fourth element.</typeparam>
            /// <typeparam name="T5">The type of the fifth element.</typeparam>
            /// <typeparam name="T6">The type of the sixth element.</typeparam>
            /// <typeparam name="T7">The type of the seventh element.</typeparam>
            /// <typeparam name="TRest">The type of the rest.</typeparam>
            public static IMonoLens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, TRest> Rest<T1, T2, T3, T4, T5, T6, T7, TRest>()
                where TRest : struct
                => L.Make.Lens<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>, TRest>(t => t.Rest, (t, x) => new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, x));
        }
    }
}
