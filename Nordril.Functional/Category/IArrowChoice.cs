using System;
using System.Collections.Generic;
using System.Text;

namespace Nordril.Functional.Category
{
    /// <summary>
    /// An <see cref="IArrow{TNeed, THave}"/> which also supports choosing a left or a right value.
    /// </summary>
    /// <typeparam name="TNeed">The type of values the arrow needs.</typeparam>
    /// <typeparam name="THave">The type of values the arrow contains.</typeparam>
    public interface IArrowChoice<TNeed, THave> : IArrow<TNeed, THave>, IChoice<TNeed, THave>
    {
    }
}
