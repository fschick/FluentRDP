using System;
using System.Windows.Forms;

namespace FluentRDP.Services;

internal sealed class TextChangedEventSuppressor : IDisposable
{
    private readonly Control _control;
    private readonly EventHandler _eventHandler;

    public TextChangedEventSuppressor(Control control, EventHandler eventHandler)
    {
        _control = control;
        _eventHandler = eventHandler;
        _control.TextChanged -= _eventHandler;
    }

    public void Dispose()
        => _control.TextChanged += _eventHandler;
}