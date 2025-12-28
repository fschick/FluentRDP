using System.Diagnostics;
using System.Reflection;

namespace FluentRDP.Models;

[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
internal class SettingToRdpFile
{
    public PropertyInfo Property { get; }
    public string RdpName { get; }
    public string? DefaultValue { get; }

    public SettingToRdpFile(PropertyInfo property, string rdpName, string? defaultValue)
    {
        Property = property;
        RdpName = rdpName;
        DefaultValue = defaultValue;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{Property.Name}={RdpName}, Default={DefaultValue}";
}