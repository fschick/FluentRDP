// The MIT License (MIT)
// Copyright (c) 2025 © Florian Schick, 2025 all rights reserved
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
using System.Linq;
using System.Reflection;

namespace FluentRDP.Extensions;

/// <summary>
/// Extension methods for object merging
/// </summary>
internal static class ObjectExtensions
{
    /// <summary>
    /// Merges properties from source object to target object.
    /// Only copies properties that are null or empty in the target.
    /// </summary>
    /// <typeparam name="T">The type of objects to merge</typeparam>
    /// <param name="target">Target object (command line options)</param>
    /// <param name="source">Source object (from RDP file)</param>
    /// <param name="excludeProperties">Optional array of property names to exclude from merging</param>
    public static void MergeFrom<T>(this T? target, T? source, params string[] excludeProperties) where T : class
    {
        if (source == null || target == null)
            return;

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propertyIsMergeable = property is { CanRead: true, CanWrite: true };
            if (!propertyIsMergeable)
                continue;

            var propertyIsExcluded = excludeProperties.Contains(property.Name, StringComparer.OrdinalIgnoreCase);
            if (propertyIsExcluded)
                continue;

            var targetValue = property.GetValue(target, null);
            var sourceValue = property.GetValue(source, null);
            var targetIsEmpty = IsValueEmpty(targetValue, property.PropertyType);
            var sourceHasValue = !IsValueEmpty(sourceValue, property.PropertyType);

            if (targetIsEmpty && sourceHasValue)
                property.SetValue(target, sourceValue, null);
        }
    }


    public static bool EqualsExceptBy<TMember>(this TMember? updatedMember, TMember? originalMember, params string[] excludedProperties)
    {
        if (ReferenceEquals(updatedMember, originalMember))
            return true;

        if (updatedMember == null || originalMember == null)
            return false;

        var objectsAreEqual = typeof(TMember)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => !excludedProperties.Contains(property.Name, StringComparer.Ordinal))
            .All(property =>
            {
                var updatedValue = property.GetValue(updatedMember, null);
                var originalValue = property.GetValue(originalMember, null);
                return Equals(updatedValue, originalValue);
            });

        return objectsAreEqual;
    }

    /// <summary>
    /// Determines if a value is considered "empty" (not set) for the purpose of merging.
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="propertyType">The type of the property</param>
    /// <returns>True if the value is considered empty</returns>
    private static bool IsValueEmpty(object? value, Type propertyType)
    {
        // Null values are always empty
        if (value == null)
            return true;

        // For strings, check if empty
        if (propertyType == typeof(string))
            return string.IsNullOrWhiteSpace((string)value);

        // For nullable types, the value is not empty since we already checked for null above
        // For non-nullable value types, they're never empty since they always have a value
        return false;
    }
}