using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Lens
{
    /// <summary>
    /// A monomorphic traversal.
    /// </summary>
    /// <typeparam name="S">The type of the structure.</typeparam>
    /// <typeparam name="A">The type of the elements in the structure.</typeparam>
    public interface IMonoTraversal<S, A> : ITraversal<S, S, A, A>, IFold<S, A>
    {
    }
}
