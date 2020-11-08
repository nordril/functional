using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Composite
{
    /// <summary>
    /// A conflict resolution strategy when two pieces of data clash.
    /// </summary>
    public enum ConflictResolution
    {
        /// <summary>
        /// Overwrite the old data with the new.
        /// </summary>
        UseNew,
        /// <summary>
        /// Leave the old data in place.
        /// </summary>
        UseExisting,
        /// <summary>
        /// Leave the old data in place and fail the process (usually by throwing an exception).
        /// </summary>
        Fail
    }
}
