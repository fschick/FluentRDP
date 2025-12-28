// The MIT License (MIT)
// Copyright (c) 2023 © Florian Schick, 2023 all rights reserved
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FluentRDP.Extensions;

/// <summary>
/// Extensions methods for type <see cref="IEnumerable{T}"></see>
/// </summary>
[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class EnumerableExtensions
{
    /// <summary>
    /// Asynchronously projects each element of an <see cref="IEnumerable{T}"/> into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="concurrency">Max tasks that will be executed in parallel.</param>
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, int concurrency = int.MaxValue)
    {
        using var semaphore = new SemaphoreSlim(concurrency);
        return await Task.WhenAll(source.Select(async s =>
        {
            try
            {
                await semaphore.WaitAsync();
                return await selector(s);
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }

    /// <summary>
    /// Asynchronously projects each element of an <see cref="IEnumerable{T}"/> into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
        => (await source).Select(selector);

    /// <inheritdoc cref="SelectAsync{TSource, TResult}(Task{IEnumerable{TSource}}, Func{TSource, TResult})" />
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this Task<List<TSource>> source, Func<TSource, TResult> selector)
        => (await source).Select(selector);

    /// <summary>
    /// Projects each element of a sequence to an <see cref="IEnumerable{T}"/> and flattens the resulting sequences into one sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/></typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <param name="concurrency">Max tasks that will be executed in parallel.</param>
    public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<IEnumerable<TResult>>> selector, int concurrency = int.MaxValue)
        => (await SelectAsync(source, selector, concurrency)).SelectMany(x => x);

    /// <summary>
    /// Returns the input typed as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="source">The source.</param>
    public static async Task<IEnumerable<T>> AsEnumerableAsync<T>(this Task<List<T>> source)
        => (await source).AsEnumerable();

    /// <summary>
    /// Creates a <see cref="List{T}"/> from an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="source">The source.</param>
    public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> source)
        => (await source).ToList();

    /// <inheritdoc cref="ToListAsync{T}(Task{IEnumerable{T}})" />
    public static async Task<List<T>> ToListAsync<T>(this Task<IOrderedEnumerable<T>> source)
        => (await source).ToList();

    /// <summary>
    /// Concatenates two sequences.
    /// </summary>
    /// <param name="first">The first sequence to concatenate.</param>
    /// <param name="second">The sequence to concat to the first sequence.</param>
    public static async Task<IEnumerable<T>> ConcatAsync<T>(this Task<IEnumerable<T>> first, IEnumerable<T> second)
        => (await first).Concat(second);

    /// <inheritdoc cref="ConcatAsync{T}(Task{IEnumerable{T}}, IEnumerable{T})" />
    public static async Task<List<T>> ConcatAsync<T>(this Task<List<T>> first, IEnumerable<T> second)
        => (await first).Concat(second).ToList();

    /// <summary>
    /// Creates an array of <typeparamref name="T"/> from an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="source">The source.</param>
    public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> source)
        => (await source).ToArray();

    /// <summary>
    /// Filters a sequence of values based on a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static async Task<IEnumerable<T>> WhereAsync<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
        => (await source).Where(predicate);

    /// <inheritdoc cref="WhereAsync{T}(Task{IEnumerable{T}}, Func{T, bool})" />
    public static async Task<IEnumerable<T>> WhereAsync<T>(this Task<List<T>> source, Func<T, bool> predicate)
        => (await source).Where(predicate);

    /// <summary>
    /// Returns the first element of a sequence.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static async Task<T> FirstAsync<T>(this Task<IEnumerable<T>> source, Func<T, bool>? predicate = null)
        => predicate != null ? (await source).First(predicate) : (await source).First();

    /// <inheritdoc cref="FirstAsync{T}(Task{IEnumerable{T}}, Func{T, bool})" />
    public static async Task<T> FirstAsync<T>(this Task<List<T>> source, Func<T, bool>? predicate = null)
        => predicate != null ? (await source).First(predicate) : (await source).First();

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> source, T? defaultValue = default)
        => (await source).FirstOrDefault(defaultValue);

    /// <inheritdoc cref="FirstOrDefaultAsync{T}(Task{IEnumerable{T}}, T)" />
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<List<T>> source, T? defaultValue = default)
        => (await source).FirstOrDefault(defaultValue);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
        => (await source).FirstOrDefault(predicate);

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate, T defaultValue)
        => (await source).FirstOrDefault(predicate, defaultValue);

    /// <inheritdoc cref="FirstOrDefaultAsync{T}(Task{IEnumerable{T}}, Func{T, bool})" />
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<List<T>> source, Func<T, bool> predicate)
        => (await source).FirstOrDefault(predicate);

    /// <inheritdoc cref="FirstOrDefaultAsync{T}(Task{IEnumerable{T}}, Func{T, bool}, T)" />
    public static async Task<T?> FirstOrDefaultAsync<T>(this Task<List<T>> source, Func<T, bool> predicate, T defaultValue)
        => (await source).FirstOrDefault(predicate, defaultValue);

    /// <summary>
    /// Sorts the elements of a sequence in ascending order.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by the function that is represented by <paramref name="keySelector"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    public static async Task<IOrderedEnumerable<TSource>> OrderByAsync<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        => (await source).OrderBy(keySelector);

    /// <inheritdoc cref="OrderByAsync{TSource, TKey}(Task{IEnumerable{TSource}}, Func{TSource, TKey})" />
    public static async Task<IOrderedEnumerable<TSource>> OrderByAsync<TSource, TKey>(this Task<List<TSource>> source, Func<TSource, TKey> keySelector)
        => (await source).OrderBy(keySelector);

    /// <summary>
    /// Produces the set difference of two sequences according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{TSource}" /> whose keys that are not also in <paramref name="exclude"/> will be returned.</param>
    /// <param name="exclude">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> exclude, Func<TSource, TKey> keySelector)
        => ExceptBy(source, exclude, keySelector, keySelector);

    /// <inheritdoc cref="ExceptBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TSource}, Func{TSource, TKey})" />
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this List<TSource> source, List<TSource> exclude, Func<TSource, TKey> keySelector)
        => ExceptBy(source.AsEnumerable(), exclude.AsEnumerable(), keySelector);

    /// <summary>
    /// Produces the set difference of two sequences according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TExclude">The type of the elements of the exclude sequence.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{TSource}" /> whose keys that are not also in <paramref name="exclude"/> will be returned.</param>
    /// <param name="exclude">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <param name="sourceKeySelector">A function to extract the key for each source element.</param>
    /// <param name="excludeKeySelector">A function to extract the key for each exclude element.</param>
    /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
    public static IEnumerable<TSource> ExceptBy<TSource, TExclude, TKey>(this IEnumerable<TSource> source, IEnumerable<TExclude> exclude, Func<TSource, TKey> sourceKeySelector, Func<TExclude, TKey> excludeKeySelector)
    {
        var excludeKeys = new HashSet<TKey>(exclude.Select(excludeKeySelector));

        foreach (var element in source)
            if (excludeKeys.Add(sourceKeySelector(element)))
                yield return element;
    }

    /// <inheritdoc cref="ExceptBy{TSource, TExclude, TKey}(IEnumerable{TSource}, IEnumerable{TExclude}, Func{TSource, TKey}, Func{TExclude, TKey})" />
    public static IEnumerable<TSource> ExceptBy<TSource, TExclude, TKey>(this List<TSource> source, List<TExclude> exclude, Func<TSource, TKey> sourceKeySelector, Func<TExclude, TKey> excludeKeySelector)
        => ExceptBy(source.AsEnumerable(), exclude.AsEnumerable(), sourceKeySelector, excludeKeySelector);

    /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
    /// <para>The intersection of two sets A and B is defined as the set that contains all the elements of A that also appear in B, but no other elements.</para>
    /// <para>When the object returned by this method is enumerated, `Intersect` yields distinct elements occurring in both sequences in the order in which they appear in <paramref name="first" />.</para>
    /// <para>If <paramref name="comparer" /> is <see langword="null" />, the default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
    /// </remarks>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
        => IntersectBy(first, second, keySelector, keySelector, comparer);

    /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TSecond">The type of the elements of the intersection sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="first">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <param name="firstKeySelector">A function to extract the key for each element.</param>
    /// <param name="secondKeySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
    /// <para>The intersection of two sets A and B is defined as the set that contains all the elements of A that also appear in B, but no other elements.</para>
    /// <para>When the object returned by this method is enumerated, `Intersect` yields distinct elements occurring in both sequences in the order in which they appear in <paramref name="first" />.</para>
    /// <para>If <paramref name="comparer" /> is <see langword="null" />, the default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
    /// </remarks>
    public static IEnumerable<TSource> IntersectBy<TSource, TSecond, TKey>(this IEnumerable<TSource> first, IEnumerable<TSecond> second, Func<TSource, TKey> firstKeySelector, Func<TSecond, TKey> secondKeySelector, IEqualityComparer<TKey>? comparer = null)
    {
        var secondKeys = new HashSet<TKey>(second.Select(secondKeySelector));
        return first.IntersectBy(secondKeys, firstKeySelector, comparer);
    }

    /// <summary>
    /// Computes the sum of a sequence of <see cref="TimeSpan"/> values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of <see cref="TimeSpan"/> values to calculate the sum of.</param>
    /// <param name="selector">The sum of the values in the sequence.</param>
    /// <returns>The sum of the projected values.</returns>
    public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        => TimeSpan.FromTicks(source.Sum(x => selector(x).Ticks));

    /// <summary>
    /// Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys.
    /// </summary>
    /// <typeparam name="TInner">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TOuter">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <param name="inner">The first sequence to join.</param>
    /// <param name="outer">The sequence to join to the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
    /// <param name="comparer">An optional equality comparer to compare join keys.</param>
    public static IEnumerable<TResult> OuterJoin<TInner, TOuter, TKey, TResult>(this IEnumerable<TInner> inner, IEnumerable<TOuter> outer, Func<TInner, TKey> innerKeySelector, Func<TOuter, TKey> outerKeySelector, Func<TInner, TOuter?, TResult> resultSelector, IEqualityComparer<TKey>? comparer = null)
        => inner
            .GroupJoin(outer, innerKeySelector, outerKeySelector, (o, i) => new { Outer = o, Inner = i }, comparer)
            .SelectMany(join => join.Inner.DefaultIfEmpty(), (join, i) => resultSelector(join.Outer, i));

    /// <summary>
    /// Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys.
    /// </summary>
    /// <typeparam name="TInner">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TOuter">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <param name="inner">The first sequence to join.</param>
    /// <param name="outer">The sequence to join to the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
    public static IEnumerable<TResult> CrossJoin<TInner, TOuter, TKey, TResult>(this IEnumerable<TInner> inner, IEnumerable<TOuter> outer, Func<TInner, TKey> innerKeySelector, Func<TOuter, TKey> outerKeySelector, Func<TInner?, TOuter?, TResult> resultSelector)
    {
        var innerList = new Lazy<IEnumerable<TInner>>(() => inner as List<TInner> ?? inner.ToList());
        var outerList = new Lazy<IEnumerable<TOuter>>(() => outer as List<TOuter> ?? outer.ToList());

        return inner
           .Select(innerKeySelector)
           .Union(outerList.Value.Select(outerKeySelector))
           .Select(key =>
           {
               var i = innerList.Value.FirstOrDefault(x => innerKeySelector(x)?.Equals(key) == true);
               var o = outerList.Value.FirstOrDefault(x => outerKeySelector(x)?.Equals(key) == true);
               return resultSelector(i, o);
           });
    }

    /// <summary>
    /// Merges two sequences based on a <paramref name="keySelector"/>. Elements from the sequence <paramref name="merge"/> have precedence over <paramref name="origin"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <param name="origin">The origin sequence.</param>
    /// <param name="merge">The sequence to merge.</param>
    /// <param name="keySelector">The key selector.</param>
    /// <returns>
    /// The merged sequence.
    /// </returns>
    public static IEnumerable<TEntity?> Merge<TEntity, TKey>(this IEnumerable<TEntity> origin, IEnumerable<TEntity> merge, Func<TEntity, TKey> keySelector)
    {
        var originList = new Lazy<IEnumerable<TEntity>>(() => origin as List<TEntity> ?? origin.ToList());
        var mergeList = new Lazy<IEnumerable<TEntity>>(() => merge as List<TEntity> ?? merge.ToList());

        return origin
            .Select(keySelector)
            .Union(mergeList.Value.Select(keySelector))
            .Select(key =>
            {
                if (mergeList.Value.Any(x => keySelector(x)?.Equals(key) == true))
                    return mergeList.Value.First(x => keySelector(x)?.Equals(key) == true);
                return originList.Value.FirstOrDefault(x => keySelector(x)?.Equals(key) == true);
            });
    }

    /// <summary>
    /// Filters a sequence of values, excluding null elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
    /// <param name="enumerable">The sequence to filter.</param>
    /// <returns>A sequence that contains only the non-null elements from the input sequence.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class
        => enumerable.OfType<T>();

    /// <summary>
    /// Create a <see cref="DataTable"/> from a list of enumerable items.
    /// </summary>
    /// <typeparam name="TEntity">The type of the enumerable items</typeparam>
    /// <param name="source">The items to convert to a <see cref="DataTable"/></param>
    ///  <param name="columnNameSelector">A function to extract the column name for a property.</param>
    ///  <param name="columnIndexSelector">A function to extract the column position for a property.</param>
    /// <param name="tableName">The name of the table. If null, the name of the type <typeparamref name="TEntity"/> is taken.</param>
    /// <returns>The created <see cref="DataTable"/>.</returns>
    public static async Task<DataTable> ToDataTable<TEntity>(this IEnumerable<TEntity> source, Func<PropertyInfo, Task<string>>? columnNameSelector = null, Func<PropertyInfo, Task<int>>? columnIndexSelector = null, string? tableName = null) where TEntity : class
    {
        columnNameSelector ??= property => Task.FromResult(property.Name);
        columnIndexSelector ??= _ => Task.FromResult(0);

        var properties = typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var propertyToOrder = await properties
            .SelectAsync(async p => KeyValuePair.Create(p.Name, await columnIndexSelector(p)));
        var propertyToOrderMap = new Dictionary<string, int>(propertyToOrder);

        properties = properties
            .OrderBy(property => propertyToOrderMap[property.Name])
            .ToArray();

        var propertyWithName = await properties
            .SelectAsync(async property =>
                new
                {
                    Property = property,
                    Name = await columnNameSelector.Invoke(property),
                    Order = propertyToOrderMap[property.Name]
                }
            )
            .ToListAsync();

        var dataTable = new DataTable(tableName);
        foreach (var column in propertyWithName)
#pragma warning disable IDISP004 // Columns are disposed by DataTable.
            dataTable.Columns.Add(column.Name, column.Property.PropertyType.GetUnderlyingType());
#pragma warning restore IDISP004

        foreach (var item in source)
        {
            var values = new object?[propertyWithName.Count];
            for (var i = 0; i < propertyWithName.Count; i++)
                values[i] = propertyWithName[i].Property.GetValue(item, null);

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    /// <summary>
    /// Flattens a tree of items.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    /// <param name="treeItem">The tree item to flatten.</param>
    /// <param name="childSelector">A function to extract the children.</param>
    /// <param name="maxDepth">The max depth to extract elements for.</param>
    [return: NotNullIfNotNull(nameof(treeItem))]
    public static IEnumerable<TItem>? Flatten<TItem>(this TItem treeItem, Func<TItem, IEnumerable<TItem>> childSelector, int maxDepth = int.MaxValue)
    {
        if (treeItem == null)
            return null;

        var collection = new[] { treeItem };
        return maxDepth > 0
            ? collection.SelectMany(item => childSelector(item).Flatten(childSelector, maxDepth - 1)).Concat(collection)
            : collection;
    }

    /// <summary>
    /// Flattens a tree of items.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    /// <param name="tree">The tree to flatten.</param>
    /// <param name="childSelector">A function to extract the children.</param>
    /// <param name="maxDepth">The max depth to extract elements for.</param>
    public static IEnumerable<TItem> Flatten<TItem>(this IEnumerable<TItem> tree, Func<TItem, IEnumerable<TItem>> childSelector, int maxDepth = int.MaxValue)
        => Flatten(tree, childSelector, maxDepth, 0);

    private static IEnumerable<TItem> Flatten<TItem>(this IEnumerable<TItem> tree, Func<TItem, IEnumerable<TItem>> childSelector, int maxDepth, int currentDepth)
    {
        var collection = tree.ToList();
        return currentDepth < maxDepth
            ? collection.SelectMany(item => childSelector(item).Flatten(childSelector, maxDepth, currentDepth + 1)).Concat(collection)
            : collection;
    }
}