# Getting Started

Suppose you have the following method.

```csharp
public class Foo
{
    // ...
    
    public void Bar(int i, string s)
    {
        this.i = i;
        this.s = s;
    }
    
    // ...
}

```

Assume that we'd like to enforce certain constraints on the parameters `i` and
`s` to `Bar`. Let's say that `i` must not be negative, and that `s` must be
composed  by at least one non-`null` character. Normally the logic to verify
that these conditions are met would look similar to the following.

```csharp
public class Foo
{
    // ...
    
    public void Bar(int i, string s)
    {
        if (i < 0)
        {
            throw new ArgumentOutOfException()
        }
        if (string.IsNullOrWhitespace(s))
        {
            throw new ArgumentException();
        }
        this.i = i;
        this.s = s;
    }
}
```

We could modify our verification logic somewhat to adhere to what some consider
"good style". For example, we might instead check for the negation of each
condition, and perform the assignment in the main clause. We could even remove
the braces and perform the assignment on the same line as each antecedent.
However, these changes don't necessarily strike at the heart of the problem:
we have failed to abstract away a common operation, and are repeating ourselves
as a result. This not only adds additional boilerplate to the method, but it's likely that
this or similar logic is duplicated across other methods that require argument
validation.

Instead, why don't we collapse the entire validation operation into a single
method call.

```csharp
public class Foo
{
    // ...
    
    public void Bar(int i, string s)
    {
        this.i = Extra.Guard.Against.Negative(i, nameof(i));
        this.s = Extra.Guard.Against.NullOrWhitespae(s, nameof(s));
    }
    
    // ...
}
```

Wow! That's a lot more concise. We even managed to combine the assignment with
the verification since the methods shown return the argument being validated if
it respects the preconditions on it. All methods declared by the `Against` class
do so.

There are plenty of other methods for common validation scenarios. In addition,
You may test arguments against an arbitrary predicate using the `Violation`
method. Let's say that arguments to parameter `s` of `Bar` ought to match a
given regular expression. We can achieve this as follows.

```csharp
public class Foo
{
    // ...
    
    private Regex regex;
    
    public void Bar(int i, string s)
    {
        this.i = Extra.Guard.Against.Negative(i, nameof(i));
        this.s = Extra.Guard.Against.Violation(s, s => _regex.IsMatch(s));
    }
    
    // ...
}
```

## Enumerables

Validating arguments to parameters that belong to `IEnumerable` present a
challenge since it often involves ensuring that *every* element they emit
satisfies a precondition. Moreover, it is often the case that you'd like to
verify multiple, orthogonal conditions on an `IEnumerable`. For that reason,
the Extra.Guard library provides the `EnumerableValidator` class. You may call
one or more methods through the fluent interface exposed by this class to verify
any number of properties of the `IEnumerable` supplied to it. The following
methods are available.

- `Empty` &mdash; verifies that an `IEnumerable` argument emits at least one
element
- `NullElement` &mdash; verifies that no element emitted by the an `IEnumerable`
argument is `null`.
- `AnyViolation` &mdash; similar to `Against.Violation`, verifies that no
element of an `IEnumerable` argument fails to satisfy an arbitrary predicate.

After calling the necessary methods on an `EnumerableValdiator` instance, you
may call `Validated` to obtain the `IEnumerable` originally supplied.

```csharp
public class Foo
{
    // ...
    
    private Regex regex;
    
    public void Baz(IEnumerable<string> e)
    {
        this.e = Extra.Guard.Against
            .InvalidEnumerable(e)
            .Empty()
            .NullElements()
            .AnyViolation(element => regex.IsMatch(element))
            .Validated();
    }
}
```

Note that calling `InvalidEnumerable` on an `IEnumerable` forces its immediate
validation.

The `Against` class also declares a few convenience methods that address common
`IEnumerable` validation scenarios.

- `Against.Empty`
- `Against.NullElements`
- `Against.EmptyOrNullElements`

That's it! With the facilities provided by the Extra.Guard library, you can
reduce boilerplate, and better convey your intent in code. 