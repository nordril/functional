using System;
using System.Collections.Generic;
using System.Text;
using Nordril.Functional.Algebra;
using Nordril.Functional.Results;

namespace Nordril.Functional.Composite
{
    /// <summary>
    /// A component which can be composed of sub-components of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the sub-components.</typeparam>
    /// <typeparam name="TName">The type of the (sub-)component's name.</typeparam>
    public interface IComposite<T, TName> : INamed<TName>
        where T : IComposite<T, TName>
        where TName : IEquatable<TName>
    {
        /// <summary>
        /// The set of sub-components. If this component is atomic, this method must return an empty set.
        /// </summary>
        ISet<T> Parts { get; }

        /// <summary>
        /// Replaces the given elements of <see cref="Parts"/> with the given <see cref="INamed{T}.Name"/> names, modulo the <see cref="ConflictResolution"/> <paramref name="resolution"/>.
        /// </summary>
        /// <param name="parts">The list of parts to replace, with their names.</param>
        /// <param name="resolution">The resolution strategy to use in case of conflict.</param>
        /// <param name="numReplaced">The number of replacements which occurred. &lt;= the count of <paramref name="parts"/>.</param>
        /// <returns>A copy of the callee, which might be different or identical (if not replacement occurred). The caller should not rely on the return value (not) being reference-equal to the callee.</returns>
        /// <exception cref="KeyAlreadyPresentException">If <see cref="ConflictResolution.Fail"/> was specified and the old component already existed.</exception>
        Result<IComposite<T, TName>> ReplaceParts(IEnumerable<T> parts, ConflictResolution resolution, out int numReplaced);
    }
}
