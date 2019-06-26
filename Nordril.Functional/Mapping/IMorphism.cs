using Nordril.Functional.Data;

namespace Nordril.Functional.Mapping
{
    /// <summary>
    /// A type which can convert objects of type <typeparamref name="TFrom"/> to objects of type <typeparamref name="TTo"/>.
    /// </summary>
    /// <typeparam name="TFrom">The from-type.</typeparam>
    /// <typeparam name="TTo">The to-type.</typeparam>
    public interface IMorphism<TFrom, TTo> : IMorphismWith<Unit, TFrom, TTo>
    {
    }

    /// <summary>
    /// Extension methods for <see cref="IMorphism{TFrom, TTo}"/>.
    /// </summary>
    public static class IMorphismExtensions
    {
        /// <summary>
        /// Converts an object from <typeparamref name="TFrom"/> to <typeparamref name="TTo"/>.
        /// </summary>
        /// <typeparam name="TFrom">The from-type.</typeparam>
        /// <typeparam name="TTo">The to-type.</typeparam>
        /// <param name="iso">The isomorphism.</param>
        /// <param name="from">The object to convert.</param>
        public static TTo Convert<TFrom, TTo>(this IIsomorphismWith<Unit, TFrom, TTo> iso, TFrom from)
            => iso.ConvertWith(new Unit(), from);
    }
}
