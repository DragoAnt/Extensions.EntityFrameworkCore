using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityFrameworkCore;

/// <summary>
/// Use this class for return empty or fixed items' collection as <see cref="IQueryable{T}"/> for use with
/// <see cref="EntityFrameworkQueryableExtensions.ToListAsync{TSource}"/> or <see cref="EntityFrameworkQueryableExtensions.ToArrayAsync{TSource}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class FixedQuery<T> : IAsyncEnumerable<T>, IQueryProvider, IOrderedQueryable<T>
{
    private static readonly IReadOnlyList<T> EmptyList = new List<T>().AsReadOnly();

    /// <summary>
    /// Empty <see cref="IQueryable{T}"/>
    /// </summary>
    public static readonly IQueryable<T> Empty = Create(EmptyList);

    /// <summary>
    /// Create  <see cref="IQueryable{T}"/> with fixed items collection
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IQueryable<T> Create(params T[]? items)
    {
        return Create((IEnumerable<T>?)items);
    }

    /// <summary>
    /// Create  <see cref="IQueryable{T}"/> with fixed items collection
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IQueryable<T> Create(IEnumerable<T>? items)
    {
        return new FixedQuery<T>(items ?? EmptyList).AsQueryable();
    }

    private readonly IQueryable<T> _items;

    private FixedQuery(IEnumerable<T> items)
        : this((items ?? throw new ArgumentNullException(nameof(items))).AsQueryable())
    {
    }

    private FixedQuery(IQueryable<T> items)
    {
        _items = items ?? throw new ArgumentNullException(nameof(items));
    }

    /// <inheritdoc />
#pragma warning disable CS1998
    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
#pragma warning restore CS1998
    {
        foreach (var item in _items)
        {
            yield return item;
        }
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public Type ElementType => _items.ElementType;

    /// <inheritdoc />
    public Expression Expression => _items.Expression;

    /// <inheritdoc />
    public IQueryProvider Provider => this;

    /// <inheritdoc />
    public IQueryable CreateQuery(Expression expression)
    {
        return new FixedQuery<T>(_items.Provider.CreateQuery<T>(expression));
    }

    /// <inheritdoc />
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new FixedQuery<TElement>(_items.Provider.CreateQuery<TElement>(expression));
    }

    /// <inheritdoc />
    public object? Execute(Expression expression)
    {
        return _items.Provider.Execute(expression);
    }

    /// <inheritdoc />
    public TResult Execute<TResult>(Expression expression)
    {
        return _items.Provider.Execute<TResult>(expression);
    }
}