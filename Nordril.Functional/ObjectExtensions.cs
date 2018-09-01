using System;
using System.Linq;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// Extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// A default hashing function which hashes all given fields, plus the full name of the object's type.
        /// <see cref="string"/>-fields are hashed with the option <see cref="StringComparison.InvariantCulture"/>.
        /// This hashing function is well-suited to hashing according to structural equality in complex objects like trees and dictionaries.
        /// See also 
        /// </summary>
        /// <param name="obj">The object for which to get the hash.</param>
        /// <param name="fields">The fields of the object.</param>
        public static int DefaultHash(this object obj, params object[] fields)
        {
            //From https://stackoverflow.com/a/263416, with modifications.
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + obj.GetType().GetGenericName(true).GetHashCode();

                foreach (var f in fields)
                    if (f != null)
                        hash = hash * 23 + (f.GetType() == typeof(string) ? ((string)f).GetHashCode() : f.GetHashCode());

                return hash;
            }
        }

        //The following methods are copied from the Nordril.TypeToolkit-package. This is quite ugly, but it avoids a circular dependency between
        //Nordril.TypeToolkit and Nordril.Functional.

        /// <summary>
        /// Gets the type's base name, i.e. its name without any generic parameters.
        /// </summary>
        /// <param name="type">The type.</param>
        private static string GetBaseName(this Type type) => type.Name.Split('`')[0];

        /// <summary>
        /// Gets the type's base full name, i.e. its full name (namespace-qualified) name without any generic parameters.
        /// </summary>
        /// <param name="type">The type.</param>
        private static string GetBaseFullName(this Type type) => type.FullName.Split('`')[0];

        /// <summary>
        /// Gets the type's generic name, which is equal to its base name for non-generic types,
        /// and equal to the angle-bracket-syntax in C#.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="useFullName">If true, the type's full (namespace-qualified) name will be used, otherwise the type's name.</param>
        public static string GetGenericName(this Type type, bool useFullName = false)
        {
            var ret = new StringBuilder();

            ret.Append(useFullName ? type.GetBaseFullName() : type.GetBaseName());

            if (type.IsGenericType)
            {
                ret.Append('<');
                ret.Append(string.Join(", ", type.GetGenericArguments().Select(a => a.GetGenericName(useFullName))));
                ret.Append('>');
            }

            return ret.ToString();
        }
    }
}
