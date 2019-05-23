using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Composite
{
    /// <summary>
    /// Objects which possess a unique name of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INamed<T>
    {
        /// <summary>
        /// Gets the object's name.
        /// </summary>
        T Name { get; }
    }
}
