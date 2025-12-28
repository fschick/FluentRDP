using System;

namespace FluentRDP.Attributes;

/// <summary>
/// Specifies the RDP file property mapping for a ConnectionSettings property
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal sealed class RdpFileAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the RDP file property
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the default value for the RDP property when not explicitly set
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Initializes a new instance of the RdpFileAttribute class
    /// </summary>
    /// <param name="name">The name of the RDP file property</param>
    public RdpFileAttribute(string name)
        => Name = name;
}
