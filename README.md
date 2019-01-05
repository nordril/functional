# Functional

This library brings functional programming to C#. It contains the following:

* Algebra,
* Utility classes,
* Category theory,
* Enumerable-extensions,
* Pattern-matching.

## Algebra
  Algebraic structures like Monoids, Semigroups, Groups, etc.
  
## Utility classes

### Maybe

Maybe, also known as Option, represents the concept of a value that might be missing.

Reference types have null to indicate that a value is missing and value-types have Nullable, but there's no type that works for both.
Maybe alleviates this problem by supporting both, and more - it has a large number of utility functions that make working with
possibly missing values easier, safer, and explicit.

The easiest to create a Maybe is via static constructors:

```c#
var nothing = Maybe.Nothing<int>();
var something = Maybe.Just(5);
```

The values can be accessed safely via the `ValueOr` family of functions:

```c#
int result = nothing.ValueOr(3); //result=3
result = something.ValueOr(4); //result=5
```

We can determine whether a Maybe contains a value simply enough:

```c#
nothing.HasValue; //false
something.HasValue; //true
```

We can also unsafely extract the value from a Maybe via the `Value`-method:

```c#
int result = nothing.Value(); //throw a PatternMatchException
int result = something.Value(); //result=5
```

More usefully, we can apply functions to Maybes in a safe way:

```c#
int result = something.Map(x => x*2).ToMaybe(); //result = Maybe.Just(10);
```

We might also have a situation wherein we want to chain multiple functions, each of which takes an `int` and returns a `Maybe<int>`,
meaning that it can fail. Instead of checking before each function whether we have a value and then manually extracting it, we can use
the `Bind`-function:

```c#
Func<int, Maybe<int>> f = x => Maybe.JustIf(x >= 0, (int)Math.Sqrt(x));
Func<int, Maybe<int>> g = x => Maybe.JustIf(x % 2 == 0, x * 2);
Func<int, Maybe<int>> h = x => Maybe.JustIf(x % 5 == 0 && x % 10 != 0, x);

int result = something
  .Bind(f)
  .Bind(g)
  .Bind(h)
  .ToMaybe();
```

This is morally equivalent to writing `h(g(f(something)))`, but that wouldn't work because each function takes an `int`, not `Maybe<int>`.
If the initial value or the result of any intermediate function is `Maybe.Nothing`, the whole chain returns `Maybe.Nothing`.

### Either

Either is a bit like Maybe, supporting optional values, but it realizes the concept of "Either one value or another exists".
We can again use the static constructors to create Eithers:

```c#
var left = Either.FromLeft<string, int>("some error");
var right = Either.FromRight<string, int>(10);
```

We can do the the same things with Either as we can with Maybe:
* Checking which value is present (`IsLeft` and `IsRight`),
* Extracting the left- or right-value unsafely (`Left()` and `Right()`).
* Mapping over the left- or right-value (`MapLeft` and `MapRight`, but also `Bimap`, which takes two functions and maps over both
  values simultaneously.
* Extracting a value unsafely (`Coalesce`).

Canonically, the left-value represents some kind of error or failure - this could be an error-string, a subclass of `Exception`,
an error-code, or anything else. Of course, you don't need to interpret a left-value as an error at all.

### Unit

A unit-type is a type with only one value and no members, equivalent to C#'s native `void`. The problem with `void`, however, is that
you cannot use it as a generic type parameter. Unit just has one way of creating it:

```c#
new Unit();
```

The only thing it represents that something was done. For example, we might have a function which either returns an error-string or nothing
in case of success. We can model this as:

```c#
Maybe<string> FunctionThatMightFail();
```

Or as:

```c#
Either<string, Unit> FunctionThatMightFail();
```

More practically, we might want to apply a function that performs IO onto each element of a list:

```c#
Func<int, Unit> writeToConsole = x => Console.WriteLine(x);

var numbers = new [] {1, 2, 3, 4};
numbers.Select(writeToConsole).ToList(); //ToList is necessary because Enumerable.Select is lazy in its evaluation.
```

### (I)FuncList

IFuncList is like a regular `List`, with the addition of functionalit related to functional programming:
* Structural (element-wise) equality comparisons,
* Support for `Map`, `Bind`, etc.

It is thus "a functional list". To see why support for, e.g., `Bind`, might be useful, let's chain some functions:

```c#
var numbers = new FuncList{1, 2, 3};

Func<int, IFuncList<int>> positiveAndNegative = x => new FuncList{x, x * (-1)};
Func<int, IFuncList<int>> andAddOne = x => new FuncList{x, x+1};

var result = numbers.
  .Bind(positiveAndNegative)
  .Bind(andAddOne)
  .ToFuncList();
  
// result = [1, 2, -1, 0,  2, 3, -2, -1, 3, 4, -3, -2]
```

We can also take a regular function that works on two numbers like `+` and add each element of a list to each element of a second one:

```c#
var numbers = new FuncList{1, 2, 3};
var numbers2 = new FuncList{5, 6, 7};

var result = numbers.Bind(x => numbers2.Bind(y => new FuncList{x + y})).ToFuncList();

// result = [6, 7, 8, 7, 8, 9, 8, 9, 10]
```

### State

Sometimes we want to return a result, but we also want to keep track of state which we can read and modify at each step.
Of course, we can do this via global variables, but there we have two problems:
* The dependency on the global variable is invisible, and
* We only have one of it; multiple computations in parallel with modify the global variable in unpredictable ways.

As a solution, we have State.

//todo: more to come.