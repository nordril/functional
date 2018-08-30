using System;

namespace Nordril.Functional
{
    /// <summary>
    /// A typed version of <see cref="ICloneable"/> which creates a deep copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of the implementor.</typeparam>
    public interface ICopyable<T>
        where T : ICopyable<T>
    {
        /// <summary>
        /// Creates a deep copy of the object that can be modified independently of the original.
        /// </summary>
        T Copy();
    }
}
