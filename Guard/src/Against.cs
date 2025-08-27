using Extra.Extensions;

namespace Guard;

/// <summary>
/// The <c>Against</c> class provides utility methods for preventing violations to
/// preconditions over function arguments.
/// </summary>
public static class Against
{
    /// <summary>
    /// Verifies that the specified <paramref name="type" /> is assignable to the
    /// specified <paramref name="target" /> <see cref="Type" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to validate.</param>
    /// <param name="target">
    /// The <see cref="Type" /> from which the one specified must derive.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified <paramref name="type" /> if it is assignable to the specified
    /// <paramref name="target" /> <see cref="Type" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="type" /> is not assignable to the specified
    /// <paramref name="target" /> <see cref="Type" />.
    /// </exception>
    public static Type InvalidType(Type type, Type target, string? name = null)
    {
        return type.IsAssignableTo(GetCandidate(type, target))
            ? type
            : throw new ArgumentException($"The provided type does not extend {target.Name}.", name);
    }

    private static Type GetCandidate(Type type, Type target)
    {
        if (target.IsGenericTypeDefinition)
        {
            foreach (var bt in type.GetEveryBaseType())
            {
                if (bt.IsGenericType && bt.GetGenericTypeDefinition() == target)
                {
                    return bt;
                }
            }
        }
        return target;
    }

    /// <summary>
    /// Verifies that the specified <paramref name="type" /> is assignable to
    /// <typeparamref name="TTarget" />.
    /// </summary>
    /// <typeparam name="TTarget">
    /// The <see cref="Type" /> from which the one specified must derive.
    /// </typeparam>
    /// <param name="type">The <see cref="Type" /> to validate.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified <paramref name="type" /> if it is assignable to
    /// <typeparamref name="TTarget" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="type" /> is not assignable to
    /// <typeparamref name="TTarget" />.
    /// </exception>
    public static Type InvalidType<TTarget>(Type type, string? name = null)
    {
        return InvalidType(type, typeof(TTarget), name);
    }

    /// <summary>
    /// Verifies that the specified <see cref="string" />, <paramref name="s" />, is
    /// not <c>null</c>, the empty string, or composed solely of whitespace characters.
    /// </summary>
    /// <param name="s">The <see cref="string" /> to validate</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="s" /> if it is neither <c>null</c>, empty, nor composed solely
    /// of whitespace characters.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="s" /> is either <c>null</c>, empty, or composed solely of
    /// whitespace characters.
    /// </exception>
    public static string NullOrWhitespace(string s, string? name = null)
    {
        return string.IsNullOrWhiteSpace(s)
            ? throw new ArgumentException("The provided string is null, empty, or whitespace.", name)
            : s;
    }

    /// <summary>
    /// Verifies that the specified <see cref="int" />, <paramref name="i" />, is
    /// greater than or equal to zero.
    /// </summary>
    /// <param name="i">The <see cref="int" /> to validate.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="i" /> if it is greater than or equal to zero.
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="i" /> is less than zero.</exception>
    public static int Negative(int i, string? name = null)
    {
        return int.IsNegative(i) ? throw new ArgumentException("The provided integer is negative.", name) : i;
    }

    /// <summary>
    /// Verifies that the specified <see cref="Nullable{T}" /> <see cref="int" />,
    /// <paramref name="i" />, is greater than or equal to zero or <c>null</c>.
    /// </summary>
    /// <param name="i">
    /// The <see cref="Nullable{T}" /> <see cref="int" /> to validate.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="i" /> if it is greater than or equal to zero or <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="i" /> is less than zero.</exception>
    public static int? Negative(int? i, string? name = null)
    {
        return i.HasValue ? Negative(i.Value, name) : i;
    }

    /// <summary>
    /// Verifies that the specified <see cref="int" />, <paramref name="i" />, is on
    /// the discrete, inclusive interval bounded by the specified
    /// <paramref name="min" /> and <paramref name="max" /> values.
    /// </summary>
    /// <param name="i">The <see cref="int" /> to validate.</param>
    /// <param name="min">The lowest value that <paramref name="i" /> may hold.</param>
    /// <param name="max">The highest value that <paramref name="i" /> may hold.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns><paramref name="i" /> if it is on the defined interval.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="i" /> is less than <paramref name="min" /> or greater than
    /// <paramref name="max" />.
    /// </exception>
    public static int OutOfRange(int i, int min, int max, string? name = null)
    {
        return i < min || i > max
            ? throw new ArgumentOutOfRangeException(
                $"{i} is not between {min} (inclusive) and {max} (exclusive).",
                name)
            : i;
    }

    /// <summary>
    /// Verifies that the specified <see cref="Nullable{T}" /> <see cref="int" />,
    /// <paramref name="i" />, is on the discrete, inclusive interval bounded by the
    /// specified <paramref name="min" /> and <paramref name="max" /> values.
    /// </summary>
    /// <param name="i">
    /// The <see cref="Nullable{T}" /> <see cref="int" /> to validate.
    /// </param>
    /// <param name="min">The lowest value that <paramref name="i" /> may hold.</param>
    /// <param name="max">The highest value that <paramref name="i" /> may hold.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="i" /> if it is on the defined interval, or if it is <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="i" /> is less than <paramref name="min" /> or greater than
    /// <paramref name="max" />.
    /// </exception>
    public static int? OutOfRange(int? i, int min, int max, string? name = null)
    {
        return i.HasValue ? OutOfRange(i.Value, min, max, name) : i;
    }

    /// <summary>
    /// Verifies that the specified <see cref="double" />, <paramref name="d" />, is
    /// greater than or equal to zero.
    /// </summary>
    /// <param name="d">The <see cref="double" /> to validate.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="d" /> if it is greater than or equal to zero.
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="d" /> is less than zero.</exception>
    public static double Negative(double d, string? name = null)
    {
        return double.IsNegative(d) ? throw new ArgumentException("The provided double is negative.", name) : d;
    }

    /// <summary>
    /// Verifies that the specified <see cref="Nullable{T}" /> <see cref="double" />,
    /// <paramref name="d" />, is greater than or equal to zero or <c>null</c>.
    /// </summary>
    /// <param name="d">The <see cref="Nullable{Double}" /> to validate.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="d" /> if it is greater than or equal to zero or <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">If <paramref name="d" /> is less than zero.</exception>
    public static double? Negative(double? d, string? name = null)
    {
        return d.HasValue ? Negative(d.Value, name) : d;
    }

    /// <summary>
    /// Verifies that the specified <see cref="double" />, <paramref name="d" />, is on
    /// the continuous, inclusive interval bounded by the specified
    /// <paramref name="min" /> and <paramref name="max" /> values.
    /// </summary>
    /// <param name="d">The <see cref="double" /> to validate.</param>
    /// <param name="min">The lowest value that <paramref name="d" /> may hold.</param>
    /// <param name="max">The highest value that <paramref name="d" /> may hold.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns><paramref name="d" /> if it is on the defined interval.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="d" /> is less than <paramref name="min" /> or greater than
    /// <paramref name="max" />.
    /// </exception>
    public static double OutOfRange(double d, double min, double max, string? name = null)
    {
        return d < min || d > max
            ? throw new ArgumentOutOfRangeException(
                $"{d} is not between {min} (inclusive) and {max} (exclusive).",
                name)
            : d;
    }

    /// <summary>
    /// Verifies that the specified <see cref="Nullable{T}" /> <see cref="double" />,
    /// <paramref name="d" />, is on the continuous, inclusive interval bounded by the
    /// specified <paramref name="min" /> and <paramref name="max" /> values.
    /// </summary>
    /// <param name="d">
    /// The <see cref="Nullable{T}" /> <see cref="double" /> to validate.
    /// </param>
    /// <param name="min">The lowest value that <paramref name="d" /> may hold.</param>
    /// <param name="max">The highest value that <paramref name="d" /> may hold.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="d" /> if it is on the defined interval, or if it is <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="d" /> is less than <paramref name="min" /> or greater than
    /// <paramref name="max" />.
    /// </exception>
    public static double? OutOfRange(double? d, double min, double max, string? name = null)
    {
        return d.HasValue ? OutOfRange(d.Value, min, max, name) : d;
    }

    /// <summary>
    /// Verifies that the specified element from <typeparamref name="T" />,
    /// <paramref name="t" />, is greater than or equal to the specified
    /// <paramref name="min" /> element.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of argument being validated.</typeparam>
    /// <param name="t">The element from <typeparamref name="T" /> to validate.</param>
    /// <param name="min">
    /// The element from <typeparamref name="T" /> to validate <paramref name="t" />
    /// against.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="t" /> if it is greater than or equal to the specified
    /// <paramref name="min" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="t" /> is less than the specified <paramref name="min" />.
    /// </exception>
    public static T LessThan<T>(T t, T min, string? name = null) where T : IComparable<T>
    {
        return t.IsLessThan(min)
            ? throw new ArgumentException($"The provided object compares less than {min}.", name)
            : t;
    }

    /// <summary>
    /// Verifies that the specified element from <typeparamref name="T" />,
    /// <paramref name="t" />, is less than or equal to the specified
    /// <paramref name="max" /> element.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of argument being validated.</typeparam>
    /// <param name="t">The element from <typeparamref name="T" /> to validate.</param>
    /// <param name="max">
    /// The element from <typeparamref name="T" /> to validate <paramref name="t" />
    /// against.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// <paramref name="t" /> if it is less than or equal to the specified
    /// <paramref name="max" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="t" /> is greater than the specified <paramref name="max" />.
    /// </exception>
    public static T GreaterThan<T>(T t, T max, string? name = null) where T : IComparable<T>
    {
        return t.IsGreaterThan(max)
            ? throw new ArgumentException($"The provided object compares less than {max}.", name)
            : t;
    }

    /// <summary>
    /// Verifies that the specified element from <typeparamref name="T" />,
    /// <paramref name="t" />, is not <c>null</c>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of argument being validated.</typeparam>
    /// <param name="t">The element from <typeparamref name="T" /> to validate.</param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns><paramref name="t" />, if it is not <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="t" /> is <c>null</c>.
    /// </exception>
    public static T Null<T>(T t, string? name = null)
    {
        return t ?? throw new ArgumentNullException(name, "The provided object is null.");
    }

    /// <summary>
    /// Verifies that the specified element from <typeparamref name="T" />,
    /// <paramref name="t" />, satisfies the specified <paramref name="precondition" />.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of argument being validated.</typeparam>
    /// <param name="t">The element from <typeparamref name="T" /> to validate.</param>
    /// <param name="precondition">
    /// A function from <typeparamref name="T" /> to <see cref="bool" />.
    /// </param>
    /// <param name="message">
    /// A natural language characterization of the way in which the specified
    /// <paramref name="precondition" /> fails.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified element from <typeparamref name="T" />, <paramref name="t" />, if
    /// it satisfies the specified <paramref name="precondition" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified element from <typeparamref name="T" />, <paramref name="t" />,
    /// does not satisfy the specified <paramref name="precondition" />.
    /// </exception>
    public static T Violation<T>(T t, Func<T, bool> precondition, string? message = null, string? name = null)
    {
        return precondition(t) ? t : throw new ArgumentException(message, name);
    }

    /// <summary>
    /// Verifies that the specified <paramref name="enumerable" /> emits at least one
    /// element.
    /// </summary>
    /// <remarks>
    /// The specified <paramref name="enumerable" /> is evaluated before it is
    /// validated.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of element held by the specified <paramref name="enumerable" />.
    /// </typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to validate.</param>
    /// <param name="suppressExceptions">
    /// If <c>true</c>, any <see cref="Exception" />s thrown while evaluating the
    /// specified <paramref name="enumerable" /> are caught and ignored.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified <paramref name="enumerable" />, if it emits at least one element.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="enumerable" /> emits no elements.
    /// </exception>
    public static IEnumerable<T> Empty<T>(
        IEnumerable<T> enumerable,
        bool suppressExceptions = false,
        string? name = null)
    {
        return InvalidEnumerable(enumerable, suppressExceptions, name)
              .Empty()
              .Validated();
    }

    /// <summary>
    /// Verifies that the specified <paramref name="enumerable" /> emits no <c>null</c>
    /// elements.
    /// </summary>
    /// <remarks>
    /// The specified <paramref name="enumerable" /> is evaluated before it is
    /// validated.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of element held by the specified <paramref name="enumerable" />.
    /// </typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to validate.</param>
    /// <param name="suppressExceptions">
    /// If <c>true</c>, any <see cref="Exception" />s thrown while evaluating the
    /// specified <paramref name="enumerable" /> are caught and ignored.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified <paramref name="enumerable" />, if it emits no <c>null</c>
    /// elements.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="enumerable" /> emits at least one <c>null</c>
    /// element.
    /// </exception>
    public static IEnumerable<T> NullElements<T>(
        IEnumerable<T> enumerable,
        bool suppressExceptions = false,
        string? name = null)
    {
        return InvalidEnumerable(enumerable, suppressExceptions, name)
              .NullElements()
              .Validated();
    }

    /// <summary>
    /// Verifies that the specified <paramref name="enumerable" /> emits at least one
    /// element and no <c>null</c> elements.
    /// </summary>
    /// <remarks>
    /// The specified <paramref name="enumerable" /> is evaluated before it is
    /// validated.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of element held by the specified <paramref name="enumerable" />.
    /// </typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to validate.</param>
    /// <param name="suppressExceptions">
    /// If <c>true</c>, any <see cref="Exception" />s thrown while evaluating the
    /// specified <paramref name="enumerable" /> are caught and ignored.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>
    /// The specified <paramref name="enumerable" />, if it emits at least one element
    /// and no <c>null</c> elements.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the specified <paramref name="enumerable" /> emits no elements or at least
    /// one <c>null</c> element.
    /// </exception>
    public static IEnumerable<T> EmptyOrNullElements<T>(
        IEnumerable<T> enumerable,
        bool suppressExceptions = false,
        string? name = null)
    {
        return InvalidEnumerable(enumerable, suppressExceptions, name)
              .Empty()
              .NullElements()
              .Validated();
    }

    /// <summary>
    /// Provides an <see cref="EnumerableValidator{T}" /> for asserting preconditions
    /// over the specified <paramref name="enumerable" /> argument.
    /// </summary>
    /// <remarks>
    /// The specified <paramref name="enumerable" /> is evaluated before it is
    /// validated.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of element held by the specified <paramref name="enumerable" />.
    /// </typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to validate.</param>
    /// <param name="suppressExceptions">
    /// If <c>true</c>, any <see cref="Exception" />s thrown while evaluating the
    /// specified <paramref name="enumerable" /> are caught and ignored.
    /// </param>
    /// <param name="name">The identifier for the parameter being validated.</param>
    /// <returns>A new <see cref="EnumerableValidator{T}" />.</returns>
    public static EnumerableValidator<T> InvalidEnumerable<T>(
        IEnumerable<T> enumerable,
        bool suppressExceptions = false,
        string? name = null)
    {
        return new EnumerableValidator<T>(ToList(enumerable, suppressExceptions), name);
    }

    private static List<T> ToList<T>(IEnumerable<T> collection, bool suppressExceptions)
    {
        using var enumerator = collection.GetEnumerator();
        var list = new List<T>();
        var next = true;
        while (next)
        {
            try
            {
                next = enumerator.MoveNext();
            }
            catch
            {
                if (suppressExceptions)
                {
                    next = true;
                    continue;
                }
                throw;
            }
            if (next)
            {
                list.Add(enumerator.Current);
            }
        }
        return list;
    }
}
