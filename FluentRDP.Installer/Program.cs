using System;
using System.IO;
using WixSharp;
using WixSharp.Controls;

namespace FluentRDP.Installer;

public static class Program
{
    public static void Main(string[] args)
    {
        // Handle MSBuild invocation from WixSharp.targets - just exit successfully
        if (args.Length > 0 && args[0].StartsWith("/MSBUILD:", StringComparison.OrdinalIgnoreCase))
            Environment.Exit(0);

        try
        {
            var options = CommandLineOptions.Parse(args);
            if (options == null)
                Environment.Exit(0);

            BuildMsi(options.Version, options.PublishFolder, options.OutputMsiPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex}");
            Environment.Exit(1);
        }
    }

    private static void BuildMsi(string version, string publishFolder, string outputMsiPath)
    {
        var publishFolderExists = Directory.Exists(publishFolder);
        if (!publishFolderExists)
            throw new DirectoryNotFoundException($"Publish folder not found: {publishFolder}");

        var productFolder = new Dir(@"%ProgramFiles%\FluentRDP", new Files($@"{publishFolder}\*.*"));
        var productIconFile = Path.Combine(publishFolder, "FluentRDP.ico");
        var productIcon = System.IO.File.Exists(productIconFile) ? productIconFile : null;
        var startMenuShortcut = new Dir(@"%ProgramMenu%\FluentRDP", new ExeFileShortcut("FluentRDP", "[INSTALLDIR]FluentRDP.exe", "") { WorkingDirectory = "[INSTALLDIR]" });
        var project = new Project("Fluent RDP", productFolder, startMenuShortcut)
        {
            // Upgrade code - keep this constant for updates
            GUID = new Guid("A8B5C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D"),
            Version = new Version(version),
            Description = "Modern RDP client with live window resizing. Drop-in replacement for mstsc.exe",
            UI = WUI.WixUI_InstallDir,
            Platform = Platform.x64,
            CustomUI = new DialogSequence()
                .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.InstallDirDlg))
                .On(NativeDialogs.InstallDirDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg)),
            ControlPanelInfo = new ProductInfo
            {
                Manufacturer = "Schick Software",
                ProductIcon = productIcon
            },
            OutFileName = Path.GetFileNameWithoutExtension(outputMsiPath),
            OutDir = Path.GetDirectoryName(outputMsiPath) ?? "."
        };

        project.BuildMsi();
    }
}