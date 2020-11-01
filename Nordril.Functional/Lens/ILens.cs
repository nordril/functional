using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    public interface ILens<S, T, A, B> : ITraversal<S, T, A, B>
    {
    }
}
