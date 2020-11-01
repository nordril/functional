using Nordril.Functional.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// The composition of two functors, e.g. <c>Maybe(List(int))</c>.
    /// </summary>
    /// <typeparam name="TFOuter">The type of the outer functor.</typeparam>
    /// <typeparam name="TFInner">The type of the inner functor.</typeparam>
    /// <typeparam name="TSource">The type of the contained elements.</typeparam>
    public struct Compose<TFOuter, TFInner, TSource> : IFunctor<TSource>
        where TFOuter : IFunctor<TFInner>
        where TFInner : IFunctor<TSource>
    {
        /// <summary>
        /// The contained value.
        /// </summary>
        public TFOuter Value { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public Compose(TFOuter value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TSource, TResult> f)
        {
            var res = Value.Map(i => i.Map(f));

            Type tfInnerResType = res
                .GetType()
                .GetInterfaces()
                .Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IFunctor<>))
                .GetGenericArguments()[0];

            var resType = typeof(Compose<,,>).MakeGenericType(res.GetType(), tfInnerResType, typeof(TResult));
            return (IFunctor<TResult>)Activator.CreateInstance(resType, new object[] { res });
        }
    }
}
