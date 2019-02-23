namespace Nordril.HedgingEngine.Logic.Mapping
{
    /// <summary>
    /// A type which can convert objects of type <typeparamref name="TFrom"/> to objects of type <typeparamref name="TTo"/>.
    /// </summary>
    /// <typeparam name="TFrom">The from-type.</typeparam>
    /// <typeparam name="TTo">The to-type.</typeparam>
    public interface IMorphism<TFrom, TTo>
    {
        /// <summary>
        /// Converts an object.
        /// </summary>
        /// <param name="from">The object to convert.</param>
        TTo Convert(TFrom from);
    }
}
