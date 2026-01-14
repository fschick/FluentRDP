using FluentRDP.Attributes;
using FluentRDP.Configuration;
using FluentRDP.Extensions;
using FluentRDP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace FluentRDP.Services;

internal static class SettingsToRdpFileService
{
    public static List<SettingToRdpFile> ConnectionMapping()
    {
        return typeof(ConnectionSettings)
            .GetProperties()
            .Select(prop => new
            {
                Property = prop,
                Attribute = prop.GetCustomAttribute<RdpFileAttribute>()
            })
            .Where(x => x.Attribute != null)
            .Select(x => new SettingToRdpFile(
                x.Property,
                x.Attribute!.Name,
                x.Attribute.DefaultValue
            ))
            .ToList();
    }

    public static object? ToSettingValue(this string? rdpValue, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(rdpValue))
            return null;

        var underlyingTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingTargetType == typeof(string))
            return rdpValue;
        if (underlyingTargetType == typeof(bool))
            return int.Parse(rdpValue) != 0;
        if (underlyingTargetType == typeof(int))
            return int.Parse(rdpValue);
        if (underlyingTargetType == typeof(uint))
            return uint.Parse(rdpValue);
        if (underlyingTargetType == typeof(Color))
            return rdpValue.ToColor();
        if (underlyingTargetType.IsEnum)
            return Enum.ToObject(underlyingTargetType, int.Parse(rdpValue));

        throw new NotSupportedException($"Conversion to type of '{underlyingTargetType.FullName}' is not supported.");
    }

    public static string? ToRdpValue(this object? value)
    {
        if (value == null)
            return null;

        var valueType = value.GetType();
        var underlyingValueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (underlyingValueType == typeof(string))
            return value.ToString();
        if (underlyingValueType == typeof(bool))
            return (bool)value ? "1" : "0";
        if (underlyingValueType == typeof(int) || underlyingValueType == typeof(uint))
            return value.ToString();
        if (underlyingValueType == typeof(Color))
            return ((Color)value).ToColorString();
        if (underlyingValueType.IsEnum)
            return ((int)value).ToString();

        throw new NotSupportedException($"Conversion from type of '{underlyingValueType.FullName}' is not supported.");
    }
}