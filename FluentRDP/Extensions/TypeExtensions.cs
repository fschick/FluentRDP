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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FluentRDP.Extensions;

/// <summary>Extensions for the <see cref="T:System.Type"/> class.</summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class TypeExtensions
{
    /// <summary>
    /// Returns the underlying type if the type is nullable; otherwise returns the type itself.
    /// </summary>
    [return: NotNullIfNotNull(nameof(type))]
    public static Type? GetUnderlyingType(this Type? type)
    {
        if (type == null)
            return null;

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}