using Mono.Options;
using System;
using System.IO;
using System.Linq;

namespace FluentRDP.Installer;

/// <summary>
/// Parses and stores command line options for FluentRDP.Installer
/// </summary>
internal class CommandLineOptions
{
    /// <summary>
    /// Gets or sets whether to show help
    /// </summary>
    public bool ShowHelp { get; set; }

    /// <summary>
    /// Gets or sets the product version
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the path to the published application folder
    /// </summary>
    public string PublishFolder { get; set; }

    /// <summary>
    /// Gets or sets the output path for the MSI file
    /// </summary>
    public string OutputMsiPath { get; set; }

    /// <summary>
    /// Parses command line arguments using Mono.Options
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Parsed options</returns>
    public static CommandLineOptions Parse(string[] args)
    {
        var options = new CommandLineOptions();
        var optionSet = CreateOptionSet(options);

        try
        {
            var extraArgs = optionSet.Parse(args);

            // Handle positional arguments for backward compatibility
            if (extraArgs.Count > 0 && string.IsNullOrWhiteSpace(options.Version))
                options.Version = extraArgs[0];
            if (extraArgs.Count > 1 && string.IsNullOrWhiteSpace(options.PublishFolder))
                options.PublishFolder = extraArgs[1];
            if (extraArgs.Count > 2 && string.IsNullOrWhiteSpace(options.OutputMsiPath))
                options.OutputMsiPath = extraArgs[2];

            if (extraArgs.Count > 3)
                throw new OptionException($"Unrecognized arguments: {string.Join(", ", extraArgs.Skip(3))}", string.Join(", ", extraArgs.Skip(3)));

            if (options.ShowHelp)
            {
                DisplayHelp(optionSet);
                return null; // Signal to exit
            }

            // Validate required parameters
            if (string.IsNullOrWhiteSpace(options.Version))
                throw new OptionException("Version is required. Use --version or provide as first positional argument.", "version");
            if (string.IsNullOrWhiteSpace(options.PublishFolder))
                throw new OptionException("Publish folder is required. Use --publish-folder or provide as second positional argument.", "publish-folder");
            if (string.IsNullOrWhiteSpace(options.OutputMsiPath))
                throw new OptionException("Output MSI path is required. Use --output or provide as third positional argument.", "output");

            // Resolve relative paths to absolute paths
            if (!Path.IsPathRooted(options.PublishFolder))
                options.PublishFolder = Path.GetFullPath(options.PublishFolder);
            if (!Path.IsPathRooted(options.OutputMsiPath))
                options.OutputMsiPath = Path.GetFullPath(options.OutputMsiPath);

            return options;
        }
        catch (OptionException ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine("Use --help for usage information.");
            throw;
        }
    }

    /// <summary>
    /// Creates the option set for command line parsing
    /// </summary>
    private static OptionSet CreateOptionSet(CommandLineOptions options)
    {
        return new OptionSet
        {
            { "v|version=", "Product version (e.g., 1.0.0)", v => options.Version = v },
            { "p|publish-folder=", "Path to the published application folder", v => options.PublishFolder = v },
            { "o|output=", "Output path for the MSI file", v => options.OutputMsiPath = v },
            { "h|help|?", "Show this help message", v => options.ShowHelp = v != null }
        };
    }

    /// <summary>
    /// Displays help information
    /// </summary>
    private static void DisplayHelp(OptionSet optionSet)
    {
        Console.WriteLine("FluentRDP.Installer - MSI Installer Builder");
        Console.WriteLine();
        Console.WriteLine("Usage: FluentRDP.Installer.exe [options] [version] [publishFolder] [outputMsiPath]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        optionSet.WriteOptionDescriptions(Console.Out);
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  FluentRDP.Installer.exe --version 1.0.0 --publish-folder ./publish --output FluentRDP.msi");
        Console.WriteLine("  FluentRDP.Installer.exe 1.0.0 ./publish FluentRDP.msi");
    }
}
