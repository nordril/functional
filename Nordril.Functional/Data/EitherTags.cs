using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A tag indicating the state of an <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public enum EitherTag
    {
        /// <summary>
        /// Indicates that the either is a left.
        /// </summary>
        Left,
        /// <summary>
        /// Indicates that the either is a right.
        /// </summary>
        Right
    }

    /// <summary>
    /// The 8-valued either-tag.
    /// </summary>
    internal enum EitherTag8 { First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eigth }

    /// <summary>
    /// A type-level tag indicating "left" or "right".
    /// </summary>
    public class TagLeftRight
    {
        /// <summary>
        /// Empty, protected constructor.
        /// </summary>
        protected TagLeftRight() { }
    }

    /// <summary>
    /// A type-level tag indicating "left" (as opposed to "right").
    /// </summary>
    public sealed class TagLeft : TagLeftRight
    {
        /// <summary>
        /// The tag's singleton value.
        /// </summary>
        public static readonly TagLeft Value = new TagLeft();

        private TagLeft() : base() { }
    }

    /// <summary>
    /// A type-level tag indicating "right" (as opposed to "left").
    /// </summary>
    public sealed class TagRight : TagLeftRight
    {
        /// <summary>
        /// The tag's singleton value.
        /// </summary>
        public static readonly TagRight Value = new TagRight();

        private TagRight() : base() { }
    }
}
