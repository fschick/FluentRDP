using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WixSharp;
using WixSharp.Controls;
using WixToolset.Dtf.WindowsInstaller;

namespace FluentRDP.Installer;

public static class Program
{
    private const string PRODUCT_REGISTRY_KEY = @"Software\Schick Software\FluentRDP";
    private const string INSTALL_DIR_VALUE = "InstallationDirectory";

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

        var uniqueProductCode = new Guid("A8B5C3D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D");
        var productVersionId = ProductIdFromVersion(version);
        var productIconFile = Path.Combine(publishFolder, "FluentRDP.ico");
        var productIcon = System.IO.File.Exists(productIconFile) ? productIconFile : null;

        var project = new Project(
            "Fluent RDP",
            new Dir(@"%ProgramFiles%\FluentRDP", new Files($@"{publishFolder}\*.*")),
            new Dir(@"%ProgramMenu%\FluentRDP", new ExeFileShortcut("FluentRDP", "[INSTALLDIR]FluentRDP.exe", "") { WorkingDirectory = "[INSTALLDIR]" }),
            new ManagedAction(ReadInstallDir, Return.ignore, When.Before, new Step("AppSearch"), Condition.NOT_Installed, Sequence.InstallExecuteSequence | Sequence.InstallUISequence) { Execute = Execute.firstSequence },
            new RegValue(WixSharp.RegistryHive.LocalMachine, PRODUCT_REGISTRY_KEY, INSTALL_DIR_VALUE, "[INSTALLDIR]") { AttributesDefinition = "Component:Permanent=yes" }
        )
        {
            // MSI: UpgradeCode = same for all versions. ProductId = different per version.
            GUID = uniqueProductCode,
            UpgradeCode = uniqueProductCode,
            ProductId = productVersionId,
            Description = "Modern RDP client with live window resizing. Drop-in replacement for mstsc.exe",
            EmitConsistentPackageId = true,
            Version = new Version(version),
            Language = "en-US",
            UI = WUI.WixUI_InstallDir,
            Platform = Platform.x64,
            Scope = InstallScope.perMachine,
            MajorUpgrade = new MajorUpgrade
            {
                Schedule = UpgradeSchedule.afterInstallInitialize,
                DowngradeErrorMessage = "A newer version of Fluent RDP is already installed.",
                AllowSameVersionUpgrades = true,
            },
            ControlPanelInfo = new ProductInfo
            {
                Manufacturer = "Schick Software",
                ProductIcon = productIcon,
                UrlInfoAbout = "https://github.com/fschick/FluentRDP",
                InstallLocation = "[INSTALLDIR]",
            },
            CustomUI = new DialogSequence()
                .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.InstallDirDlg))
                .On(NativeDialogs.InstallDirDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg)),
            OutFileName = Path.GetFileNameWithoutExtension(outputMsiPath),
            OutDir = Path.GetDirectoryName(outputMsiPath) ?? ".",
        };

        project.EnableResilientPackage();

        project.BuildMsi();
    }

    /// <summary>
    /// Generates a deterministic ProductId (ProductCode) from the version.
    /// Must be different from UpgradeCode and unique per version.
    /// </summary>
    private static Guid ProductIdFromVersion(string version)
    {
        var bytes = Encoding.UTF8.GetBytes("FluentRDP.ProductId." + version);
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(bytes);
        hash[6] = (byte)((hash[6] & 0x0F) | 0x40);
        hash[8] = (byte)((hash[8] & 0x3F) | 0x80);
        return new Guid(hash);
    }

    [CustomAction]
    public static ActionResult ReadInstallDir(Session session)
    {
        var currentInstallLocation = Registry.LocalMachine
            .OpenSubKey(PRODUCT_REGISTRY_KEY)?
            .GetValue(INSTALL_DIR_VALUE)
            .ToString();

        if (Directory.Exists(currentInstallLocation))
            session["INSTALLDIR"] = currentInstallLocation;

        return ActionResult.Success;
    }
}