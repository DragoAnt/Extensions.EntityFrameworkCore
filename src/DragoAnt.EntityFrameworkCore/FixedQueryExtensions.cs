using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore;

public static class FixedQueryExtensions
{
    /// <summary>
    ///  AsQueryable that supports <see cref="EntityFrameworkQueryableExtensions.ToListAsync{TSource}"/>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> AsQueryableFixed<T>(this IEnumerable<T>? enumerable)
    {
        return FixedQuery<T>.Create(enumerable);
    }
        
    /// <summary>
    ///  AsQueryable that supports <see cref="EntityFrameworkQueryableExtensions.ToListAsync{TSource}"/>
    /// </summary>
    /// <param name="array"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> AsQueryableFixed<T>(this T[]? array)
    {
        return FixedQuery<T>.Create(array);
    }
}