using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A setter which can also be used as a traversal.
    /// </summary>
    public interface ILens<S, T, A, B> : ITraversal<S, T, A, B>
    {
    }
}
