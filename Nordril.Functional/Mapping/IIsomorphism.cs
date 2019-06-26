using Nordril.Functional.Data;
using Nordril.Functional.Mapping;

namespace Nordril.Functional.Mapping
{
    /// <summary>
    /// A type which can convert objects of type <typeparamref name="TFrom"/> to objects of type <typeparamref name="TTo"/>.
    /// </summary>
    /// <typeparam name="TFrom">The from-type.</typeparam>
    /// <typeparam name="TTo">The to-type.</typeparam>
    public interface IIsomorphism<TFrom, TTo> : IIsomorphismWith<Unit, TFrom, TTo>
    {
    }

    /// <summary>
    /// Extension methods for <see cref="IIsomorphism{TFrom, TTo}"/>.
    /// </summary>
    public static class IsomorphismExtensions
    {
        /// <summary>
        /// Converts an object back from <typeparamref name="TTo"/> to <typeparamref name="TFrom"/>.
        /// </summary>
        /// <typeparam name="TFrom">The from-type.</typeparam>
        /// <typeparam name="TTo">The to-type.</typeparam>
        /// <param name="iso">The isomorphism.</param>
        /// <param name="from">The object to convert.</param>
        public static TFrom ConvertBack<TFrom, TTo>(this IIsomorphismWith<Unit, TFrom, TTo> iso, TTo from)
            => iso.ConvertBackWith(new Unit(), from);
    }
}
