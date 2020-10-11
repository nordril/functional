using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// A coproduct having a first component.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    public interface ICoproductFirst<T1>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the first component.
        /// </summary>
        bool IsFirst { get; }

        /// <summary>
        /// Returns the first component.
        /// </summary>
        Maybe<T1> First { get; }
    }

    /// <summary>
    /// A coproduct having a second component.
    /// </summary>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    public interface ICoproductSecond<T2>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the second component.
        /// </summary>
        bool IsSecond { get; }

        /// <summary>
        /// Returns the second component.
        /// </summary>
        Maybe<T2> Second { get; }
    }

    /// <summary>
    /// A coproduct having a third component.
    /// </summary>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    public interface ICoproductThird<T3>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the third component.
        /// </summary>
        bool IsThird { get; }

        /// <summary>
        /// Returns the third component.
        /// </summary>
        Maybe<T3> Third { get; }
    }

    /// <summary>
    /// A coproduct having a fourth component.
    /// </summary>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    public interface ICoproductFourth<T4>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the fourth component.
        /// </summary>
        bool IsFourth { get; }

        /// <summary>
        /// Returns the fourth component.
        /// </summary>
        Maybe<T4> Fourth { get; }
    }

    /// <summary>
    /// A coproduct having a fifth component.
    /// </summary>
    /// <typeparam name="T5">The type of the fifth component.</typeparam>
    public interface ICoproductFifth<T5>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the fifth component.
        /// </summary>
        bool IsFifth { get; }

        /// <summary>
        /// Returns the fifth component.
        /// </summary>
        Maybe<T5> Fifth { get; }
    }

    /// <summary>
    /// A coproduct having a sixth component.
    /// </summary>
    /// <typeparam name="T6">The type of the sixth component.</typeparam>
    public interface ICoproductSixth<T6>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the sixth component.
        /// </summary>
        bool IsSixth { get; }

        /// <summary>
        /// Returns the sixth component.
        /// </summary>
        Maybe<T6> Sixth { get; }
    }

    /// <summary>
    /// A coproduct having a seventh component.
    /// </summary>
    /// <typeparam name="T7">The type of the seventh component.</typeparam>
    public interface ICoproductSeventh<T7>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the seventh component.
        /// </summary>
        bool IsSeventh { get; }

        /// <summary>
        /// Returns the seventh component.
        /// </summary>
        Maybe<T7> Seventh { get; }
    }

    /// <summary>
    /// A coproduct having an eigth component.
    /// </summary>
    /// <typeparam name="T8">The type of the eigth component.</typeparam>
    public interface ICoproductEigth<T8>
    {
        /// <summary>
        /// Returns true iff the coproduct contains the eigth component.
        /// </summary>
        bool IsEigth { get; }

        /// <summary>
        /// Returns the eigth component.
        /// </summary>
        Maybe<T8> Eigth { get; }
    }
}
