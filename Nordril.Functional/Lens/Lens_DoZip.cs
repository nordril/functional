using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public static partial class L
    {
        public static partial class Do
        {
            #region Zip
            /// <summary>
            /// Combines 2 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            public static IMonoLens<(TA1, TA2), (TB1, TB2)> Zip<TA1, TA2, TB1, TB2>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2)
                => L.Make.Lens<(TA1, TA2), (TB1, TB2)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2)));

            /// <summary>
            /// Combines 3 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            public static IMonoLens<(TA1, TA2, TA3), (TB1, TB2, TB3)> Zip<TA1, TA2, TA3, TB1, TB2, TB3>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3)
                => L.Make.Lens<(TA1, TA2, TA3), (TB1, TB2, TB3)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3)));

            /// <summary>
            /// Combines 4 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TA4">The type of the fourth input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <typeparam name="TB4">The type of the fourth output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            /// <param name="l4">The fourth lens.</param>
            public static IMonoLens<(TA1, TA2, TA3, TA4), (TB1, TB2, TB3, TB4)> Zip<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3,
                IMonoLens<TA4, TB4> l4)
                => L.Make.Lens<(TA1, TA2, TA3, TA4), (TB1, TB2, TB3, TB4)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3), l4.Get(t.Item4)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3),
                        l4.Set(ta.Item4, tb.Item4)));

            /// <summary>
            /// Combines 5 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TA4">The type of the fourth input.</typeparam>
            /// <typeparam name="TA5">The type of the fifth input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <typeparam name="TB4">The type of the fourth output.</typeparam>
            /// <typeparam name="TB5">The type of the fifth output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            /// <param name="l4">The fourth lens.</param>
            /// <param name="l5">The fifth lens.</param>
            public static IMonoLens<(TA1, TA2, TA3, TA4, TA5), (TB1, TB2, TB3, TB4, TB5)> Zip<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3,
                IMonoLens<TA4, TB4> l4,
                IMonoLens<TA5, TB5> l5)
                => L.Make.Lens<(TA1, TA2, TA3, TA4, TA5), (TB1, TB2, TB3, TB4, TB5)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3), l4.Get(t.Item4), l5.Get(t.Item5)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3),
                        l4.Set(ta.Item4, tb.Item4),
                        l5.Set(ta.Item5, tb.Item5)));

            /// <summary>
            /// Combines 6 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TA4">The type of the fourth input.</typeparam>
            /// <typeparam name="TA5">The type of the fifth input.</typeparam>
            /// <typeparam name="TA6">The type of the sixth input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <typeparam name="TB4">The type of the fourth output.</typeparam>
            /// <typeparam name="TB5">The type of the fifth output.</typeparam>
            /// <typeparam name="TB6">The type of the sixth output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            /// <param name="l4">The fourth lens.</param>
            /// <param name="l5">The fifth lens.</param>
            /// <param name="l6">The sixth lens.</param>
            public static IMonoLens<(TA1, TA2, TA3, TA4, TA5, TA6), (TB1, TB2, TB3, TB4, TB5, TB6)> Zip<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3,
                IMonoLens<TA4, TB4> l4,
                IMonoLens<TA5, TB5> l5,
                IMonoLens<TA6, TB6> l6)
                => L.Make.Lens<(TA1, TA2, TA3, TA4, TA5, TA6), (TB1, TB2, TB3, TB4, TB5, TB6)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3), l4.Get(t.Item4), l5.Get(t.Item5), l6.Get(t.Item6)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3),
                        l4.Set(ta.Item4, tb.Item4),
                        l5.Set(ta.Item5, tb.Item5),
                        l6.Set(ta.Item6, tb.Item6)));

            /// <summary>
            /// Combines 7 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TA4">The type of the fourth input.</typeparam>
            /// <typeparam name="TA5">The type of the fifth input.</typeparam>
            /// <typeparam name="TA6">The type of the sixth input.</typeparam>
            /// <typeparam name="TA7">The type of the seventh input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <typeparam name="TB4">The type of the fourth output.</typeparam>
            /// <typeparam name="TB5">The type of the fifth output.</typeparam>
            /// <typeparam name="TB6">The type of the sixth output.</typeparam>
            /// <typeparam name="TB7">The type of the seventh output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            /// <param name="l4">The fourth lens.</param>
            /// <param name="l5">The fifth lens.</param>
            /// <param name="l6">The sixth lens.</param>
            /// <param name="l7">The seventh lens.</param>
            public static IMonoLens<(TA1, TA2, TA3, TA4, TA5, TA6, TA7), (TB1, TB2, TB3, TB4, TB5, TB6, TB7)> Zip<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3,
                IMonoLens<TA4, TB4> l4,
                IMonoLens<TA5, TB5> l5,
                IMonoLens<TA6, TB6> l6,
                IMonoLens<TA7, TB7> l7)
                => L.Make.Lens<(TA1, TA2, TA3, TA4, TA5, TA6, TA7), (TB1, TB2, TB3, TB4, TB5, TB6, TB7)>(
                    t => (l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3), l4.Get(t.Item4), l5.Get(t.Item5), l6.Get(t.Item6), l7.Get(t.Item7)),
                    (ta, tb) => (
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3),
                        l4.Set(ta.Item4, tb.Item4),
                        l5.Set(ta.Item5, tb.Item5),
                        l6.Set(ta.Item6, tb.Item6),
                        l7.Set(ta.Item7, tb.Item7)));

            /// <summary>
            /// Combines 8 lenses and into one tuple-lens.
            /// </summary>
            /// <typeparam name="TA1">The type of the first input.</typeparam>
            /// <typeparam name="TA2">The type of the second input.</typeparam>
            /// <typeparam name="TA3">The type of the third input.</typeparam>
            /// <typeparam name="TA4">The type of the fourth input.</typeparam>
            /// <typeparam name="TA5">The type of the fifth input.</typeparam>
            /// <typeparam name="TA6">The type of the sixth input.</typeparam>
            /// <typeparam name="TA7">The type of the seventh input.</typeparam>
            /// <typeparam name="TARest">The type of the rest of the input.</typeparam>
            /// <typeparam name="TB1">The type of the first output.</typeparam>
            /// <typeparam name="TB2">The type of the second output.</typeparam>
            /// <typeparam name="TB3">The type of the third output.</typeparam>
            /// <typeparam name="TB4">The type of the fourth output.</typeparam>
            /// <typeparam name="TB5">The type of the fifth output.</typeparam>
            /// <typeparam name="TB6">The type of the sixth output.</typeparam>
            /// <typeparam name="TB7">The type of the seventh output.</typeparam>
            /// <typeparam name="TBRest">The type of the rest of the output.</typeparam>
            /// <param name="l1">The first lens.</param>
            /// <param name="l2">The second lens.</param>
            /// <param name="l3">The third lens.</param>
            /// <param name="l4">The fourth lens.</param>
            /// <param name="l5">The fifth lens.</param>
            /// <param name="l6">The sixth lens.</param>
            /// <param name="l7">The seventh lens.</param>
            /// <param name="lRest">The lens for thr rest.</param>
            public static IMonoLens<ValueTuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TARest>, ValueTuple<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TBRest>> Zip<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TARest, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TBRest>(
                IMonoLens<TA1, TB1> l1,
                IMonoLens<TA2, TB2> l2,
                IMonoLens<TA3, TB3> l3,
                IMonoLens<TA4, TB4> l4,
                IMonoLens<TA5, TB5> l5,
                IMonoLens<TA6, TB6> l6,
                IMonoLens<TA7, TB7> l7,
                IMonoLens<TARest, TBRest> lRest)
                where TARest : struct
                where TBRest : struct
                => L.Make.Lens<ValueTuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TARest>, ValueTuple<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TBRest>>(
                    t => new ValueTuple<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TBRest>(l1.Get(t.Item1), l2.Get(t.Item2), l3.Get(t.Item3), l4.Get(t.Item4), l5.Get(t.Item5), l6.Get(t.Item6), l7.Get(t.Item7), lRest.Get(t.Rest)),
                    (ta, tb) => new ValueTuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TARest>(
                        l1.Set(ta.Item1, tb.Item1),
                        l2.Set(ta.Item2, tb.Item2),
                        l3.Set(ta.Item3, tb.Item3),
                        l4.Set(ta.Item4, tb.Item4),
                        l5.Set(ta.Item5, tb.Item5),
                        l6.Set(ta.Item6, tb.Item6),
                        l7.Set(ta.Item7, tb.Item7),
                        lRest.Set(ta.Rest, tb.Rest)));
            #endregion
        }
    }
}
