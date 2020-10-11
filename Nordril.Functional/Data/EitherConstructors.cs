using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The Either-constructor for <see cref="ICoproductFirst{T1}"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the value.</typeparam>
    public struct Either1<T1>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T1 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either1(T1 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductSecond{T1}"/>.
    /// </summary>
    /// <typeparam name="T2">The type of the value.</typeparam>
    public struct Either2<T2>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T2 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either2(T2 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductThird{T1}"/>.
    /// </summary>
    /// <typeparam name="T3">The type of the value.</typeparam>
    public struct Either3<T3>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T3 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either3(T3 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductFourth{T1}"/>.
    /// </summary>
    /// <typeparam name="T4">The type of the value.</typeparam>
    public struct Either4<T4>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T4 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either4(T4 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductFifth{T1}"/>.
    /// </summary>
    /// <typeparam name="T5">The type of the value.</typeparam>
    public struct Either5<T5>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T5 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either5(T5 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductSixth{T1}"/>.
    /// </summary>
    /// <typeparam name="T6">The type of the value.</typeparam>
    public struct Either6<T6>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T6 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either6(T6 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductSeventh{T1}"/>.
    /// </summary>
    /// <typeparam name="T7">The type of the value.</typeparam>
    public struct Either7<T7>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T7 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either7(T7 value) { Value = value; }
    }

    /// <summary>
    /// The Either-constructor for <see cref="ICoproductEigth{T1}"/>.
    /// </summary>
    /// <typeparam name="T8">The type of the value.</typeparam>
    public struct Either8<T8>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T8 Value { get; }

        /// <summary>
        /// Wraps a new value.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public Either8(T8 value) { Value = value; }
    }
}
