using Nordril.Functional.Category;
using System;

namespace Nordril.Functional.Data
{
    /// <summary>
    /// A store-monad which starts with an initial store and allows repeated extraction of values,
    /// but not change to the store itself.
    /// Store is the opposite/dual of <see cref="State{TState, TValue}"/>, which requires an initial state
    /// and can produce new states.
    /// </summary>
    /// <typeparam name="TStore">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type value to be extracted.</typeparam>
    public class Store<TStore, TValue> : IComonad<TValue>
    {
        /// <summary>
        /// Runs the store.
        /// </summary>
        public Func<TStore, TValue> RunStore { get; private set; }

        /// <summary>
        /// The initial store which can be used to run the store.
        /// </summary>
        public TStore InitialStore { get; private set; }

        /// <summary>
        /// Creates a new store out for an extraction function and an initial state.
        /// </summary>
        /// <param name="runStore">The function to extract a value from the store.</param>
        /// <param name="initialStore">The initial state.</param>
        public Store(Func<TStore, TValue> runStore, TStore initialStore)
        {
            this.RunStore = runStore;
            this.InitialStore = initialStore;
        }

        /// <inheritdoc />
        public IComonad<TResult> Extend<TResult>(Func<IComonad<TValue>, TResult> f)
            => new Store<TStore, TResult>(s => f(new Store<TStore, TValue>(RunStore, s)), InitialStore);

        /// <inheritdoc />
        public TValue Extract() => RunStore(InitialStore);

        /// <inheritdoc />
        public IFunctor<TResult> Map<TResult>(Func<TValue, TResult> f) => new Store<TStore, TResult>(RunStore.Then(f), InitialStore);
    }

    /// <summary>
    /// Extension methods for <see cref="Store{TState, TValue}"/>.
    /// </summary>
    public static class Store
    {
        /// <summary>
        /// Extracts the store as a value.
        /// </summary>
        /// <typeparam name="TStore">The type type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation whose store to extract.</param>
        public static TStore GetStore<TStore, TValue>(this Store<TStore, TValue> s) => s.InitialStore;

        /// <summary>
        /// Replaces the initial store with a new one.
        /// Note that this does not mean replacing a state on the fly; rather, this function
        /// replaces the starting point of the compuation.
        /// </summary>
        /// <typeparam name="TStore">The type type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation whose initial store to replace.</param>
        /// <param name="newStore">The new initial store.</param>
        /// <returns></returns>
        public static Store<TStore, TValue> SetStore<TStore, TValue>(this Store<TStore, TValue> s, TStore newStore)
            => new Store<TStore, TValue>(s.RunStore, newStore);

        /// <summary>
        /// Replaces the initial store with a new one by modifiying it with a function.
        /// Note that this does not mean replacing a state on the fly; rather, this function
        /// replaces the starting point of the compuation.
        /// </summary>
        /// <typeparam name="TStore">The type type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation whose initial store to replace.</param>
        /// <param name="f">The function to apply to the store.</param>
        /// <returns></returns>
        public static Store<TStore, TValue> ModifyStore<TStore, TValue>(this Store<TStore, TValue> s, Func<TStore, TStore> f)
            => new Store<TStore, TValue>(s.RunStore, f(s.InitialStore));

        /// <summary>
        /// Returns the extracted value, if the initial store would be <paramref name="hypothetical"/>,
        /// without changing the store.
        /// </summary>
        /// <typeparam name="TStore">The type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation to run.</param>
        /// <param name="hypothetical">The hypothetical initial store to use.</param>
        public static TValue WhatIfStore<TStore, TValue>(this Store<TStore, TValue> s, TStore hypothetical)
            => s.RunStore(hypothetical);

        /// <summary>
        /// Returns the extracted value, if <paramref name="f"/> would be applied to the initial store,
        /// without changing the store.
        /// </summary>
        /// <typeparam name="TStore">The type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation to run.</param>
        /// <param name="f">The function to apply to the initial store.</param>
        public static TValue WhatIfFunc<TStore, TValue>(this Store<TStore, TValue> s, Func<TStore, TStore> f)
            => s.RunStore(f(s.InitialStore));

        /// <summary>
        /// Returns the extracted value, if <paramref name="f"/> would be applied to the initial store. The
        /// <see cref="Store{TStore, TValue}.RunStore"/> function is applied via
        /// <see cref="IFunctor{TSource}.Map{TResult}(Func{TSource, TResult})"/> to the functor-valued result.
        /// </summary>
        /// <typeparam name="TStore">The type of the store.</typeparam>
        /// <typeparam name="TValue">The type of the value extracted.</typeparam>
        /// <param name="s">The computation to run.</param>
        /// <param name="f">The functor-valued function to apply to the initial store.</param>
        public static IFunctor<TValue> WhatIfF<TStore, TValue>(this Store<TStore, TValue> s, Func<TStore, IFunctor<TStore>> f)
            => f(s.InitialStore).Map(s2 => s.RunStore(s2));
    }
}
