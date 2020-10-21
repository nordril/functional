using Nordril.Functional.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nordril.Functional
{
    /// <summary>
    /// A constructor for a first-class pattern.
    /// </summary>
    /// <remarks>
    /// Using a <see cref="Pattern"/> consists of two stages:
    /// <list type="number">
    ///     <item>Constructing a list of cases which are checked in-order, and</item>
    ///     <item>running the pattern against an input object.</item>
    /// </list>
    /// Constructing a case-list is a safe operation in that none of the cases are evaluated, though be advised that the <see cref="Pattern{TIn, TOut}"/>-object is not thread-safe and <see cref="ICopyable{T}.Copy"/> should be used prior to sharing it across threads. Each case consists of a predicate which has to evaluate to true if the case is to apply, and an action which is run on the input object if the case applies.
    /// <br />
    /// Cases can be added via a fluent-API, all at once or at different times. <see cref="Pattern{TIn, TOut}"/>s, being first-class objects, can be passed in as arguments or be returned by functions.
    /// <br />
    /// The second stage is the exection: via <see cref="Pattern{TIn, TOut}.Run(TIn)"/>, and input-object is passed in and the previously registered cases are evaluated in order of their addition. The first case which evaluates to true is applied and result of its action returned; if no cases applies, the default pattern is applied, which can be set via <see cref="Pattern{TIn, TOut}.WithDefault(Func{TIn, TOut})"/>. If no default pattern was set and no case applies, a <see cref="PatternMatchException"/> is thrown.
    /// <br />
    /// It is considered good form to always specify a default pattern unless one is absolutely sure that the disjunction of the registered cases all possible values of the input object.
    /// </remarks>
    /// <example>
    /// Below we create a sign-pattern which returns 1 for postive numbers, -1 for negative ones, and 0 for 0. The <see cref="int"/> type annotation in the first case disambiguates the type of the input object.
    /// <code>
    /// var pat = Pattern
    ///    .Match((int x) => x &gt; 0, _ =&gt; 1)
    ///    .Match(x => x &lt; 0, =&gt; -1)
    ///    .WithDefault(_ =&gt; 0);
    ///    
    /// var sign = pat.Run(5); //1
    /// sign = pat.Run(-4); //-1
    /// sign = pat.Run(0); //0
    /// </code>
    /// We can also create recursive patterns. Below we have an implementation of the Collatz "algorithm" which does the following:
    /// <list type="number">
    ///     <item>If a number is even, divide it by two and repeat, or</item>
    ///     <item>if a number is odd, multiply by three and add 1, and repeat.</item>
    /// </list>
    /// The local function <c>go</c> is used to create the recursion. We first declare it to have the name available in the <c>collatz</c>-<see cref="Pattern{TIn, TOut}"/>, then we tie the know by running the pattern in <c>go</c>. Note that we insert the terminating pattern (<c>&lt;=1</c>) as the first, to avoid infinite recursion.
    /// <br />
    /// Per the Collatz-conjecture, every number eventually reaches one.
    /// <code>
    /// public int Collatz(int num)
    /// {
    ///    //Create the referancable name "go".
    ///    Func&lt;int, int&gt; go(int x) = null;
    ///    
    ///    //Create the pattern (only once).
    ///    var collatz = Pattern
    ///       .Match((int x) => x &lt; 1, x => 1)
    ///       .Match(x => x % 2 == 0, x => go(x/2))
    ///       .WithDefault(x => go(x*3 + 1));
    ///       
    ///    //Tie the know but instantiating "go" with running the pattern.
    ///    go = x => collatz.Run(x);
    ///    
    ///    //Run the pattern. We either return 1 or we don't terminate.
    ///    return go(num);
    /// }
    /// </code>
    /// One can also carry state in patterns through the input-object. For instance, we can calculate the factorial tail-recursively thus:
    /// <code>
    /// var n = 15;
    /// var fac = Pattern
    ///    .Match(((int n, long sum) x) =&gt; x.n &lt;= 1, x =&gt; x.sum)
    ///    .MatchTailRec(x =&gt; x.n &gt; 1, x =&gt; (x.n-1, (x.n* x.sum)))
    ///    .Run((n, 1));
    /// </code>
    /// Here, we pass a tuple consisting of the current number (n) and the sum so far (sum) into the pattern-match, and in the second case, we decrement n while multiplying the previous sum with n. We get the final number instead of the tuple because the terminating, first case discards the tuple and just returns the sum.
    /// </example>
    public static class Pattern
    {
        /// <summary>
        /// Creates a pattern-match on an input object <typeparamref name="TIn"/>, starting with a single case.
        /// </summary>
        /// <typeparam name="TIn">The type of the input object.</typeparam>
        /// <typeparam name="TOut">The type of the output object.</typeparam>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="action">The function that should be applied to the input object if the case applies.</param>
        public static Pattern<TIn, TOut> Match<TIn, TOut>(Func<TIn, bool> predicate, Func<TIn, TOut> action)
            => Pattern<TIn, TOut>.StartMatch(predicate, action);

        /// <summary>
        /// Creates a pattern-match on an input object <typeparamref name="TIn"/>, starting with a list of cases.
        /// </summary>
        /// <typeparam name="TIn">The type of the input object.</typeparam>
        /// <typeparam name="TOut">The type of the output object.</typeparam>
        /// <param name="cases">The list of cases, consisting of predicates and functions to be applied to the input if the case applies.</param>
        public static Pattern<TIn, TOut> MatchMany<TIn, TOut>(IEnumerable<(Func<TIn, bool> predicate, Func<TIn, TOut> action)> cases)
            => Pattern<TIn, TOut>.StartMatch(cases);

        /// <summary>
        /// Appends a new, tail-recursive case modulo cons to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// </summary>
        /// <typeparam name="TIn">The type of the input object.</typeparam>
        /// <typeparam name="TOut">The type of the output object.</typeparam>
        /// <param name="p">The pattern to which to append.</param>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="head">The function which is applied to each element created by this pattern.</param>
        /// <param name="tail">The function which iterates through the output.</param>
        public static Pattern<TIn, FuncList<TOut>> MatchTailRecModuloCons<TIn, TOut>(
            this Pattern<TIn, FuncList<TOut>> p,
            Func<TIn, bool> predicate,
            Func<TIn, TOut> head,
            Func<TIn, TIn> tail)
            => Pattern<TIn, TOut>.MatchTailRec(p, predicate, x => new FuncList<TOut> { x }, (x, xs) => { xs.Add(x); return xs; }, (xs, ys) => { xs.AddRange(ys); return xs; }, head, tail);

        /// <summary>
        /// Appends a new, tail-recursive case modulo cons to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// The returned structure <typeparamref name="TOutSemi"/> must form a semigroup under <paramref name="combineResults"/>.
        /// </summary>
        /// <typeparam name="TIn">The type of the input object.</typeparam>
        /// <typeparam name="TOut">The type of the output object.</typeparam>
        /// <typeparam name="TOutSemi">The type of the result of the <paramref name="head"/>-function.</typeparam>
        /// <param name="p">The pattern to which to append.</param>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="head">The function which is applied to each element created by this pattern.</param>
        /// <param name="tail">The function which iterates through the output.</param>
        /// <param name="addResult">The function to append a single result to the previously generated ones.</param>
        /// <param name="mkResult">The function to create a single result.</param>
        /// <param name="combineResults">The function to append new results to the previously generated ones.</param>
        public static Pattern<TIn, TOutSemi> MatchTailRecModuloCons<TIn, TOut, TOutSemi>(
            this Pattern<TIn, TOutSemi> p,
            Func<TIn, bool> predicate,
            Func<TIn, TOut> head,
            Func<TIn, TIn> tail,
            Func<TOut, TOutSemi> mkResult,
            Func<TOut, TOutSemi, TOutSemi> addResult,
            Func<TOutSemi, TOutSemi, TOutSemi> combineResults)
            => Pattern<TIn, TOut>.MatchTailRec(p, predicate, mkResult, addResult, combineResults, head, tail);
    }

    /// <summary>
    /// A list of cases which is checked top-down when the pattern is run.
    /// </summary>
    /// <typeparam name="TIn">The type of the input object.</typeparam>
    /// <typeparam name="TOut">The type of the output object.</typeparam>
    public class Pattern<TIn, TOut> : ICopyable<Pattern<TIn, TOut>>
    {
        private readonly List<Func<TIn, bool>> predicates;
        private readonly List<Func<TIn, TOut>> actions;
        private readonly List<Func<TIn, TIn>> tailRecActions;
        private Func<TIn, TOut> defaultPattern = _ => throw new PatternMatchException();

        /// <summary>
        /// Creates a new <see cref="Pattern{TIn, TOut}"/> out of a list of predicates an actions. The caller must ensure that both lists have the same number of elements.
        /// </summary>
        /// <param name="predicates">The list of predicates for the cases.</param>
        /// <param name="actions">The list of actions for the cases.</param>
        /// <param name="tailRecActions">The list of tail-recursive actions. This list must have the same number of entries as <paramref name="actions"/>, but with the values at the indexes of the non-tail-recursive actions being null.</param>
        /// <param name="defaultPattern">The default pattern. If null is passed, the default pattern which throws a <see cref="NullReferenceException"/> will be used.</param>
        /// <exception cref="ArgumentException">If the lengths of <paramref name="predicates"/>, <paramref name="actions"/>, and <paramref name="tailRecActions"/> do not agree.</exception>
        private Pattern(IEnumerable<Func<TIn, bool>> predicates, IEnumerable<Func<TIn, TOut>> actions, IEnumerable<Func<TIn, TIn>> tailRecActions, Func<TIn, TOut> defaultPattern)
        {
            this.predicates = new List<Func<TIn, bool>>(predicates);
            this.actions = new List<Func<TIn, TOut>>(actions);
            this.tailRecActions = new List<Func<TIn, TIn>>(tailRecActions);

            if (this.predicates.Count != this.actions.Count || this.predicates.Count != this.tailRecActions.Count)
                throw new ArgumentException("Inconsistent number of cases in Pattern. This likely indicates a bug in the implementation.");

            this.defaultPattern = defaultPattern ?? this.defaultPattern;
        }

        /// <summary>
        /// Creates a <see cref="Pattern{TIn, TOut}"/> out of a single case.
        /// </summary>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="action">The function that should be applied to the input object if the case applies.</param>
        public static Pattern<TIn, TOut> StartMatch(Func<TIn, bool> predicate, Func<TIn, TOut> action)
            => new Pattern<TIn, TOut>(new[] { predicate }, new[] { action }, new Func<TIn, TIn>[] { null }, null);

        /// <summary>
        /// Creates a <see cref="Pattern{TIn, TOut}"/> out of a sequence of cases.
        /// </summary>
        /// <param name="cases">The list of cases, consisting of predicates and functions to be applied to the input if the case applies.</param>
        public static Pattern<TIn, TOut> StartMatch(IEnumerable<(Func<TIn, bool> predicate, Func<TIn, TOut> action)> cases)
        {
            var (predicates, actions) = cases.Unzip();
            var tailRecActions = Enumerable.Repeat<Func<TIn, TIn>>(null, predicates.Count());
            return new Pattern<TIn, TOut>(predicates, actions, tailRecActions, null);
        }

        /// <summary>
        /// Appends a new case to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// </summary>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="action">The function that should be applied to the input object if the case applies.</param>
        public Pattern<TIn, TOut> Match(Func<TIn, bool> predicate, Func<TIn, TOut> action)
        {
            predicates.Add(predicate);
            actions.Add(action);
            tailRecActions.Add(null);
            return this;
        }

        /// <summary>
        /// Appends a list of new cases to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// </summary>
        /// <param name="cases">The list of cases, consisting of predicates and functions to be applied to the input if the case applies.</param>
        public Pattern<TIn, TOut> Match(IEnumerable<(Func<TIn, bool> predicate, Func<TIn, TOut> action)> cases)
        {
            var (predicates, actions) = cases.Unzip();

            this.predicates.AddRange(predicates);
            this.actions.AddRange(actions);
            tailRecActions.AddRange(Enumerable.Repeat<Func<TIn, TIn>>(null, cases.Count()));
            return this;
        }

        /// <summary>
        /// Appends a new, tail-recursive case to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// Note that a tail-recursive case <em>cannot</em> be a terminating case and if the input-object always matches a tail-recursive case, <see cref="Pattern{TIn, TOut}.Run(TIn)"/> will not terminate.
        /// <br />
        /// Instead of <paramref name="action"/> returning an output, it returns an <typeparamref name="TIn"/>-object with which the pattern-match will be called tail-recursively. This means that no new stack-frame will be allocated for the recursive call. Think of this like running a pattern-match in a loop. For general recursion (which runs the risk of stack overflows), see the examples in <see cref="Pattern{TIn, TOut}"/>.
        /// </summary>
        /// <remarks>
        /// If tail-recursion is all you need, using <see cref="MatchTailRec(Func{TIn, bool}, Func{TIn, TIn})"/> is actually superior to a native <c>switch</c>-expression with a recursive call in it (w.r.t. the consumption of stack-memory) because the C# runtime does not support tail-recursion. Of course, any tail-recursive <c>switch</c>-expression can be easily transformed into one which does not run into a stack overflow (becsause of the recursion alone) by floating out the recursion into a loop around the the <c>switch</c>.</remarks>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="action">The function that should be applied to the input object if the case applies.</param>
        public Pattern<TIn, TOut> MatchTailRec(Func<TIn, bool> predicate, Func<TIn, TIn> action)
        {
            TOut tailRecCall(TIn input)
            {
                input = action(input);
                var caseMatched = false;

                while (true)
                {
                    using (var predEnum = predicates.GetEnumerator())
                    using (var actionEnum = actions.GetEnumerator())
                    using (var tailRecEnum = tailRecActions.GetEnumerator())
                    {
                        caseMatched = false;

                        //Go through all cases with the current input.
                        while (!caseMatched && predEnum.MoveNext() && actionEnum.MoveNext() && tailRecEnum.MoveNext())
                            if (predEnum.Current(input))
                            {
                                //We matched a tail-recursive case -> call the tail recursive action and continue.
                                if (tailRecEnum.Current != null)
                                {
                                    input = tailRecEnum.Current(input);
                                    caseMatched = true;
                                }
                                //We matched a non-recursive case -> return immediately.
                                else
                                    return actionEnum.Current(input);
                            }
                    }

                    //If we haven't matched any case, we return with the default pattern.
                    if (!caseMatched)
                        return defaultPattern(input);
                }
            };

            predicates.Add(predicate);
            tailRecActions.Add(action);
            actions.Add(tailRecCall);
            return this;
        }


        /// <summary>
        /// Appends a new, tail-recursive case modulo cons to the end of a <see cref="Pattern{TIn, TOut}"/>. The calling object will be modified and <c>this</c> will be returned.
        /// The returned structure <typeparamref name="TOutSemi"/> must form a semigroup under <paramref name="combineResults"/>.
        /// </summary>
        /// <typeparam name="TOutSemi">The type of the result of the <paramref name="head"/>-function.</typeparam>
        /// <param name="pattern">The pattern to which to append.</param>
        /// <param name="predicate">The predicate which returns true if the case applies.</param>
        /// <param name="head">The function which is applied to each element created by this pattern.</param>
        /// <param name="tail">The function which iterates through the output.</param>
        /// <param name="addResult">The function to append a single result to the previously generated ones.</param>
        /// <param name="mkResult">The function to create a single result.</param>
        /// <param name="combineResults">The function to append new results to the previously generated ones.</param>
        public static Pattern<TIn, TOutSemi> MatchTailRec<TOutSemi>(
            Pattern<TIn, TOutSemi> pattern,
            Func<TIn, bool> predicate,
            Func<TOut, TOutSemi> mkResult,
            Func<TOut, TOutSemi, TOutSemi> addResult,
            Func<TOutSemi, TOutSemi, TOutSemi> combineResults,
            Func<TIn, TOut> head,
            Func<TIn, TIn> tail)
        {
            TOutSemi tailRecCall(TIn input)
            {
                var caseMatched = false;

                var h = head(input);
                input = tail(input);

                var results = mkResult(h);

                while (true)
                {

                    using (var predEnum = pattern.predicates.GetEnumerator())
                    using (var actionEnum = pattern.actions.GetEnumerator())
                    using (var tailRecEnum = pattern.tailRecActions.GetEnumerator())
                    {
                        caseMatched = false;

                        //Go through all cases with the current input.
                        while (!caseMatched && predEnum.MoveNext() && actionEnum.MoveNext() && tailRecEnum.MoveNext())
                            if (predEnum.Current(input))
                            {
                                //We matched a tail-recursive case -> call the tail recursive action and continue.
                                if (tailRecEnum.Current != null)
                                {
                                    h = head(input);
                                    results = addResult(h, results);
                                    input = tailRecEnum.Current(input);

                                    caseMatched = true;
                                }
                                //We matched a non-recursive case -> return immediately.
                                else
                                {
                                    results = combineResults(results, actionEnum.Current(input));
                                    return results;
                                }
                            }
                    }

                    //If we haven't matched any case, we return with the default pattern.
                    if (!caseMatched)
                    {
                        results = combineResults(results, pattern.defaultPattern(input));
                        return results;
                    }
                }
            };

            pattern.predicates.Add(predicate);
            pattern.tailRecActions.Add(tail);
            pattern.actions.Add(tailRecCall);
            return pattern;
        }

        /// <summary>
        /// Sets the default pattern which always applies after all other cases. This call overwrites any previously set default pattern, except if null is passed in <paramref name="defaultPattern"/>, in which case nothing is done.
        /// </summary>
        /// <param name="defaultPattern"></param>
        /// <returns></returns>
        public Pattern<TIn, TOut> WithDefault(Func<TIn, TOut> defaultPattern)
        {
            this.defaultPattern = defaultPattern ?? this.defaultPattern;
            return this;
        }

        /// <summary>
        /// Runs a pattern against an input object <paramref name="arg"/> and returns the result of the first case that applies, or the result of the default pattern is none applies.
        /// </summary>
        /// <param name="arg">The input object.</param>
        /// <returns>The result of the first case which applies to the input object, with that case's action run against the input object.</returns>
        /// <exception cref="PatternMatchException">If no case applies and no alternative default pattern was set via <see cref="WithDefault(Func{TIn, TOut})"/>.</exception>
        public TOut Run(TIn arg)
        {
            using (var predEnum = predicates.GetEnumerator())
            using (var actionEnum = actions.GetEnumerator())
            {
                while (predEnum.MoveNext() && actionEnum.MoveNext())
                    if (predEnum.Current(arg))
                        return actionEnum.Current(arg);
            }

            return defaultPattern(arg);
        }

        /// <inheritdoc />
        public Pattern<TIn, TOut> Copy()
            => new Pattern<TIn, TOut>(predicates, actions, tailRecActions, defaultPattern);
    }
}
