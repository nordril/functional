using System;

namespace Nordril.Functional
{
    /// <summary>
    /// Extension methods for <see cref="ValueTuple"/>.
    /// </summary>
    public static class TupleExtensions
    {
        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static ValueTuple<TR> First<T1, TR>(
            this ValueTuple<T1> t,
            Func<T1, TR> f)
            => new ValueTuple<TR>(f(t.Item1));

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2) First<T1, T2, TR>(
            this (T1, T2) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2);

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3) First<T1, T2, T3, TR>(
            this (T1, T2, T3) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2, t.Item3);

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3, T4) First<T1, T2, T3, T4, TR>(
            this (T1, T2, T3, T4) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2, t.Item3, t.Item4);

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3, T4, T5) First<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2, t.Item3, t.Item4, t.Item5);

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3, T4, T5, T6) First<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3, T4, T5, T6, T7) First<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T1, TR> f)
            => (f(t.Item1), t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);

        /// <summary>
        /// Applies a function to the first element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (TR, T2, T3, T4, T5, T6, T7, T8) First<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T1, TR> f)
            where T8 : struct
            => (f(t.Item1), t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR) Second<T1, T2, TR>(
            this (T1, T2) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2));

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3) Second<T1, T2, T3, TR>(
            this (T1, T2, T3) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2), t.Item3);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3, T4) Second<T1, T2, T3, T4, TR>(
            this (T1, T2, T3, T4) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2), t.Item3, t.Item4);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3, T4, T5) Second<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2), t.Item3, t.Item4, t.Item5);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3, T4, T5, T6) Second<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2), t.Item3, t.Item4, t.Item5, t.Item6);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3, T4, T5, T6, T7) Second<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T2, TR> f)
            => (t.Item1, f(t.Item2), t.Item3, t.Item4, t.Item5, t.Item6, t.Item7);

        /// <summary>
        /// Applies a function to the second element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, TR, T3, T4, T5, T6, T7, T8) Second<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T2, TR> f)
            where T8 : struct
            => (t.Item1, f(t.Item2), t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR) Third<T1, T2, T3, TR>(
            this (T1, T2, T3) t,
            Func<T3, TR> f)
            => (t.Item1, t.Item2, f(t.Item3));

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR, T4) Third<T1, T2, T3, T4, TR>(
            this (T1, T2, T3, T4) t,
            Func<T3, TR> f)
            => (t.Item1, t.Item2, f(t.Item3), t.Item4);

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR, T4, T5) Third<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) t,
            Func<T3, TR> f)
            => (t.Item1, t.Item2, f(t.Item3), t.Item4, t.Item5);

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR, T4, T5, T6) Third<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T3, TR> f)
            => (t.Item1, t.Item2, f(t.Item3), t.Item4, t.Item5, t.Item6);

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR, T4, T5, T6, T7) Third<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T3, TR> f)
            => (t.Item1, t.Item2, f(t.Item3), t.Item4, t.Item5, t.Item6, t.Item7);

        /// <summary>
        /// Applies a function to the third element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, TR, T4, T5, T6, T7, T8) Third<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T3, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, f(t.Item3), t.Item4, t.Item5, t.Item6, t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the fourth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, TR) Fourth<T1, T2, T3, T4, TR>(
            this (T1, T2, T3, T4) t,
            Func<T4, TR> f)
            => (t.Item1, t.Item2, t.Item3, f(t.Item4));

        /// <summary>
        /// Applies a function to the fourth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, TR, T5) Fourth<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) t,
            Func<T4, TR> f)
            => (t.Item1, t.Item2, t.Item3, f(t.Item4), t.Item5);

        /// <summary>
        /// Applies a function to the fourth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, TR, T5, T6) Fourth<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T4, TR> f)
            => (t.Item1, t.Item2, t.Item3, f(t.Item4), t.Item5, t.Item6);

        /// <summary>
        /// Applies a function to the fourth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, TR, T5, T6, T7) Fourth<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T4, TR> f)
            => (t.Item1, t.Item2, t.Item3, f(t.Item4), t.Item5, t.Item6, t.Item7);

        /// <summary>
        /// Applies a function to the fourth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, TR, T5, T6, T7, T8) Fourth<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T4, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, t.Item3, f(t.Item4), t.Item5, t.Item6, t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the fifth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, TR) Fifth<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) t,
            Func<T5, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, f(t.Item5));

        /// <summary>
        /// Applies a function to the fifth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, TR, T6) Fifth<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T5, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, f(t.Item5), t.Item6);

        /// <summary>
        /// Applies a function to the fifth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, TR, T6, T7) Fifth<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T5, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, f(t.Item5), t.Item6, t.Item7);

        /// <summary>
        /// Applies a function to the fifth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, TR, T6, T7, T8) Fifth<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T5, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, t.Item3, t.Item4, f(t.Item5), t.Item6, t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the sixth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, TR) Sixth<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T6, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, f(t.Item6));

        /// <summary>
        /// Applies a function to the sixth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, TR, T7) Sixth<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T6, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, f(t.Item6), t.Item7);

        /// <summary>
        /// Applies a function to the sixth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, TR, T7, T8) Sixth<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T6, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, f(t.Item6), t.Item7, t.Item8);

        /// <summary>
        /// Applies a function to the seventh element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, T6, TR) Seventh<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T7, TR> f)
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, f(t.Item7));

        /// <summary>
        /// Applies a function to the seventh element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, T6, TR, T8) Seventh<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T7, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, f(t.Item7), t.Item8);

        /// <summary>
        /// Applies a function to the eigth element of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TR">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static (T1, T2, T3, T4, T5, T6, T7, TR) Eighth<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T8, TR> f)
            where T8 : struct
            => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, f(t.Item8));

        /// <summary>
        /// Applies two functions to the two elements of the tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="TResult1">The result of the first function.</typeparam>
        /// <typeparam name="TResult2">The result of the second function.</typeparam>
        /// <param name="t">The tuple to which to apply the functions.</param>
        /// <param name="f">The first function.</param>
        /// <param name="g">The second function.</param>
        /// <returns></returns>
        public static (TResult1, TResult2) Both<T1, T2, TResult1, TResult2>(
            this (T1, T2) t,
            Func<T1, TResult1> f,
            Func<T2, TResult2> g)
            => (f(t.Item1), g(t.Item2));

        /// <summary>
        /// Applies a binary function to the two elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, TResult>(
            this (T1, T2) t,
            Func<T1, T2, TResult> f)
            => (f(t.Item1, t.Item2));

        /// <summary>
        /// Applies a ternary function to the three elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, TResult>(
            this (T1, T2, T3) t,
            Func<T1, T2, T3, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3));

        /// <summary>
        /// Applies a quaternary function to the four elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, T4, TResult>(
            this (T1, T2, T3, T4) t,
            Func<T1, T2, T3, T4, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3, t.Item4));

        /// <summary>
        /// Applies a quinary function to the five elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The five element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, T4, T5, TResult>(
            this (T1, T2, T3, T4, T5) t,
            Func<T1, T2, T3, T4, T5, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5));

        /// <summary>
        /// Applies a senary function to the six elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, T4, T5, T6, TResult>(
            this (T1, T2, T3, T4, T5, T6) t,
            Func<T1, T2, T3, T4, T5, T6, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6));

        /// <summary>
        /// Applies a septenary function to the seven elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this (T1, T2, T3, T4, T5, T6, T7) t,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7));

        /// <summary>
        /// Applies a octonary function to the seven elements of a tuple.
        /// </summary>
        /// <typeparam name="T1">The first element.</typeparam>
        /// <typeparam name="T2">The second element.</typeparam>
        /// <typeparam name="T3">The third element.</typeparam>
        /// <typeparam name="T4">The fourth element.</typeparam>
        /// <typeparam name="T5">The fifth element.</typeparam>
        /// <typeparam name="T6">The sixth element.</typeparam>
        /// <typeparam name="T7">The seventh element.</typeparam>
        /// <typeparam name="T8">The eighth element.</typeparam>
        /// <typeparam name="TResult">The result of the function.</typeparam>
        /// <param name="t">The tuple to which to apply the function.</param>
        /// <param name="f">The function to apply.</param>
        public static TResult ApplyToTuple<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) t,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> f)
            => (f(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Item8));
    }
}
