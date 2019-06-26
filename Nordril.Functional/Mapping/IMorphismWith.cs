using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordril.Functional.Mapping
{
    /// <summary>
    /// type which can convert objects of type <typeparamref name="TFrom"/> to objects of type <typeparamref name="TTo"/>., with the help of an environment-variable <typeparamref name="TEnv"/>.
    /// </summary>
    /// <typeparam name="TEnv">The environment.</typeparam>
    /// <typeparam name="TFrom">The from-type.</typeparam>
    /// <typeparam name="TTo">The to-type.</typeparam>
    public interface IMorphismWith<TEnv, TFrom, TTo>
    {
        /// <summary>
        /// Converts an object.
        /// </summary>
        /// <param name="env">The environment.</param>
        /// <param name="from">The object to convert.</param>
        TTo ConvertWith(TEnv env, TFrom from);
    }
}
