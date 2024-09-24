namespace Guard;

/// <summary>
/// The <c>EnumerableValidator</c> class provides a fluent interface for asserting preconditions over <see
/// cref="IEnumerable{T}"/> arguments.
/// </summary>
/// <typeparam name="T">The type of element held by the <see cref="IEnumerable{T}"/> validated by this
/// <c>EnumerableValidator</c>.</typeparam>
public sealed class EnumerableValidator<T>
{
    private readonly IList<T> _elements;
    private readonly string? _name;

    internal EnumerableValidator(IList<T> elements, string? name)
    {
        _elements = elements;
        _name = name;
    }

    /// <summary>
    /// Verifies that the <see cref="IEnumerable{T}"/> supplied to this <c>EnumerableValidator</c> contains at least
    /// one element.
    /// </summary>
    /// <returns>This <c>EnumerableValidator</c>.</returns>
    /// <exception cref="ArgumentException">If the <see cref="IEnumerable{T}"/> supplied to this
    /// <c>EnumerableValidator</c> is empty.</exception>
    public EnumerableValidator<T> Empty()
    {
        if (_elements.Count == 0)
        {
            throw new ArgumentException("The provided enumerable is empty.", _name);
        }
        return this;
    }

    /// <summary>
    /// Verifies that the <see cref="IEnumerable{T}"/> supplied to this <c>EnumerableValidator</c> does not contain any
    /// <c>null</c> elements.
    /// </summary>
    /// <returns>This <c>EnumerableValidator</c>.</returns>
    /// <exception cref="ArgumentException">If the <see cref="IEnumerable{T}"/> supplied to this
    /// <c>EnumerableValidator</c> contains <c>null</c> elements.</exception>
    public EnumerableValidator<T> NullElements()
    {
        if (_elements.Any(element => element is null))
        {
            throw new ArgumentException("The provided enumerable contains null elements.", _name);
        }
        return this;
    }

    /// <summary>
    /// Provides the <see cref="IEnumerable{T}"/> supplied to this <c>EnumerableValidator</c>.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{T}"/> validated by this <c>EnumerableValidator</c>.</returns>
    public IEnumerable<T> Validated()
    {
        return _elements;
    }
}