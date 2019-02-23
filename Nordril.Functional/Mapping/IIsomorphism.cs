namespace Nordril.HedgingEngine.Logic.Mapping
{
    /// <summary>
    /// A type which can convert objects of type <typeparamref name="TFrom"/> to objects of type <typeparamref name="TTo"/>.
    /// </summary>
    /// <typeparam name="TFrom">The from-type.</typeparam>
    /// <typeparam name="TTo">The to-type.</typeparam>
    public interface IIsomorphism<TFrom, TTo> : IMorphism<TFrom, TTo>
    {
        /// <summary>
        /// Converts an object back from <typeparamref name="TTo"/> to <typeparamref name="TFrom"/>.
        /// The following holds for all <c>x</c>:
        /// <code>
        ///     ConvertBack(Convert(x)) == x
        /// </code>
        /// </summary>
        /// <param name="from">The object to convert.</param>
        TFrom ConvertBack(TTo from);
    }
}
