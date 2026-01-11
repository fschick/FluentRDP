using FluentRDP.UI;
using System;
using System.Windows.Forms;

namespace FluentRDP;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        using var rdpClient = new FormRdpClient(args);
        Application.Run(rdpClient);
    }
}