using Indril.TypeToolkit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indril.Functional
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// A default hashing function which hashes all given fields, plus the full name of the object's type.
        /// <see cref="string"/>-fields are hashed with the option <see cref="StringComparison.InvariantCulture"/>.
        /// </summary>
        /// <param name="obj">The object for which to get the hash.</param>
        /// <param name="fields">The fields of the object.</param>
        internal static int DefaultHash(this object obj, params object[] fields)
        {
            //From https://stackoverflow.com/a/263416, with modifications.
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + obj.GetType().GetGenericName(true).GetHashCode(StringComparison.InvariantCulture);

                foreach (var f in fields)
                    if (f != null)
                        hash = hash * 23 + (f.GetType() == typeof(string) ? ((string)f).GetHashCode(StringComparison.InvariantCulture) : f.GetHashCode());

                return hash;
            }
        }
    }
}
