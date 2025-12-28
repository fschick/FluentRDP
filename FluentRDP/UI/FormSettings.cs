using FluentRDP.Configuration;
using FluentRDP.Configuration.Enums;
using FluentRDP.Platform;
using FluentRDP.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FluentRDP.UI;

public partial class FormSettings : Form
{
    private static readonly List<Size> _displayResolutions = Interop.GetAvailableDisplayResolutions();

    internal ApplicationSettings? UpdatedSettings { get; private set; }

    private FormSettings()
    {
        InitializeComponent();
        InitializeEnumComboBoxes();
    }

    internal FormSettings(ApplicationSettings? settings) : this()
    {
        UpdatedSettings = settings;
        if (settings != null)
            LoadConnectionSettingsToUi(settings.Connection);
    }

    private void InitializeEnumComboBoxes()
    {
        cmbScreenMode.Items.Add(new ComboBoxItem<ScreenModeOption>(ScreenModeOption.Windowed, "Windowed"));
        cmbScreenMode.Items.Add(new ComboBoxItem<ScreenModeOption>(ScreenModeOption.FullScreen, "Full Screen"));
        cmbScreenMode.Items.Add(new ComboBoxItem<ScreenModeOption>(ScreenModeOption.UseAllMonitors, "Use All Monitors"));
        cmbScreenMode.SelectedIndex = 0;

        var resolutionsWidths = _displayResolutions.Select(res => res.Width).Distinct().ToList();
        var resolutionsHeights = _displayResolutions.Select(res => res.Height).Distinct().ToList();
        foreach (var width in resolutionsWidths)
            cmbWidth.Items.Add(new ComboBoxItem<int>(width, width.ToString()));
        foreach (var height in resolutionsHeights)
            cmbHeight.Items.Add(new ComboBoxItem<int>(height, height.ToString()));

        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(null, "Same as host"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(100, "100%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(125, "125%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(150, "150%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(175, "175%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(200, "200%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(250, "250%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(300, "300%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(400, "400%"));
        cmbScaleFactor.Items.Add(new ComboBoxItem<uint?>(500, "500%"));
        cmbScaleFactor.SelectedIndex = 0;

        cmbColorDepth.Items.Add(new ComboBoxItem<int?>(15, "High Color (15 bit)"));
        cmbColorDepth.Items.Add(new ComboBoxItem<int?>(16, "High Color (16 bit)"));
        cmbColorDepth.Items.Add(new ComboBoxItem<int?>(24, "True Color (24 bit)"));
        cmbColorDepth.Items.Add(new ComboBoxItem<int?>(32, "Highest Quality (32 bit)"));
        cmbColorDepth.SelectedIndex = 0;

        cmbAuthenticationLevel.Items.Add(new ComboBoxItem<AuthenticationLevel?>(AuthenticationLevel.ConnectWithoutWarning, "Connect Without Warning"));
        cmbAuthenticationLevel.Items.Add(new ComboBoxItem<AuthenticationLevel?>(AuthenticationLevel.DoNotConnect, "Do Not Connect"));
        cmbAuthenticationLevel.Items.Add(new ComboBoxItem<AuthenticationLevel?>(AuthenticationLevel.WarnUser, "Warn User"));
        cmbAuthenticationLevel.Items.Add(new ComboBoxItem<AuthenticationLevel?>(AuthenticationLevel.NoRequirement, "No Requirement"));
        cmbAuthenticationLevel.SelectedIndex = 0;

        cmbKeyboardMode.Items.Add(new ComboBoxItem<KeyboardMode?>(KeyboardMode.OnLocalComputer, "On Local Computer"));
        cmbKeyboardMode.Items.Add(new ComboBoxItem<KeyboardMode?>(KeyboardMode.OnRemoteComputer, "On Remote Computer"));
        cmbKeyboardMode.Items.Add(new ComboBoxItem<KeyboardMode?>(KeyboardMode.InFullScreenOnly, "In Full Screen Only"));
        cmbKeyboardMode.SelectedIndex = 0;

        cmbAudioMode.Items.Add(new ComboBoxItem<AudioPlaybackMode?>(AudioPlaybackMode.PlayOnLocal, "Play on Local"));
        cmbAudioMode.Items.Add(new ComboBoxItem<AudioPlaybackMode?>(AudioPlaybackMode.PlayOnRemote, "Play on Remote"));
        cmbAudioMode.Items.Add(new ComboBoxItem<AudioPlaybackMode?>(AudioPlaybackMode.DoNotPlay, "Do Not Play"));
        cmbAudioMode.SelectedIndex = 0;

        cmbRedirectAudioCapture.Items.Add(new ComboBoxItem<bool?>(false, "Do not record"));
        cmbRedirectAudioCapture.Items.Add(new ComboBoxItem<bool?>(true, "Record from this computer"));
        cmbRedirectAudioCapture.SelectedIndex = 0;
    }

    private void LoadConnectionSettingsToUi(ConnectionSettings settings)
    {
        txtHostname.Text = settings.Hostname ?? string.Empty;
        txtDomain.Text = settings.Domain ?? string.Empty;
        txtUsername.Text = settings.Username ?? string.Empty;
        chkEnableCredSsp.CheckState = GetCheckState(settings.EnableCredSsp);
        SetComboBoxValue(cmbAuthenticationLevel, settings.AuthenticationLevel);

        cmbWidth.Text = settings.Width.ToString();
        cmbHeight.Text = settings.Height.ToString();
        SetComboBoxValue(cmbColorDepth, settings.ColorDepth);
        SetComboBoxValue(cmbScaleFactor, settings.ScaleFactor);
        chkSmartSizing.CheckState = GetCheckState(settings.SmartSizing);
        chkAutoResize.CheckState = GetCheckState(settings.AutoResize);
        LoadScreenModeComboBoxToUi(settings.ScreenMode, settings.UseAllMonitors);
        chkEnableCompression.CheckState = GetCheckState(settings.EnableCompression);
        chkEnableBitmapPersistence.CheckState = GetCheckState(settings.EnableBitmapPersistence);

        SetComboBoxValue(cmbAudioMode, settings.AudioPlaybackMode);
        SetComboBoxValue(cmbRedirectAudioCapture, settings.RedirectAudioCapture);
        txtRedirectDrives.Text = settings.RedirectDrives ?? string.Empty;
        chkRedirectClipboard.CheckState = GetCheckState(settings.RedirectClipboard);
        chkRedirectPrinters.CheckState = GetCheckState(settings.RedirectPrinters);
        chkRedirectSmartCards.CheckState = GetCheckState(settings.RedirectSmartCards);
        SetComboBoxValue(cmbKeyboardMode, settings.KeyboardMode);
    }

    private ConnectionSettings GetConnectionSettingsFromUi()
    {
        var screenModeOption = GetComboBoxValue<ScreenModeOption>(cmbScreenMode);
        var (screenMode, useAllMonitors) = GetScreenModeValuesFromUi(screenModeOption);

        return new ConnectionSettings
        {
            Hostname = string.IsNullOrWhiteSpace(txtHostname.Text) ? null : txtHostname.Text,
            Domain = string.IsNullOrWhiteSpace(txtDomain.Text) ? null : txtDomain.Text,
            Username = string.IsNullOrWhiteSpace(txtUsername.Text) ? null : txtUsername.Text,
            EnableCredSsp = GetBoolValue(chkEnableCredSsp.CheckState),
            AuthenticationLevel = GetComboBoxValue<AuthenticationLevel?>(cmbAuthenticationLevel),

            Width = string.IsNullOrWhiteSpace(cmbWidth.Text) ? null : int.Parse(cmbWidth.Text),
            Height = string.IsNullOrWhiteSpace(cmbHeight.Text) ? null : int.Parse(cmbHeight.Text),
            ColorDepth = GetComboBoxValue<int?>(cmbColorDepth),
            ScaleFactor = GetComboBoxValue<uint?>(cmbScaleFactor),
            SmartSizing = GetBoolValue(chkSmartSizing.CheckState),
            AutoResize = GetBoolValue(chkAutoResize.CheckState),
            ScreenMode = screenMode,
            UseAllMonitors = useAllMonitors,
            EnableCompression = GetBoolValue(chkEnableCompression.CheckState),
            EnableBitmapPersistence = GetBoolValue(chkEnableBitmapPersistence.CheckState),

            AudioPlaybackMode = GetComboBoxValue<AudioPlaybackMode?>(cmbAudioMode),
            RedirectAudioCapture = GetComboBoxValue<bool?>(cmbRedirectAudioCapture),

            RedirectDrives = string.IsNullOrWhiteSpace(txtRedirectDrives.Text) ? null : txtRedirectDrives.Text,
            RedirectClipboard = GetBoolValue(chkRedirectClipboard.CheckState),
            RedirectPrinters = GetBoolValue(chkRedirectPrinters.CheckState),
            RedirectSmartCards = GetBoolValue(chkRedirectSmartCards.CheckState),
            KeyboardMode = GetComboBoxValue<KeyboardMode?>(cmbKeyboardMode)
        };
    }

    private static CheckState GetCheckState(bool? value)
    {
        if (!value.HasValue)
            return CheckState.Indeterminate;
        return value.Value ? CheckState.Checked : CheckState.Unchecked;
    }

    private static bool? GetBoolValue(CheckState checkState)
    {
        return checkState switch
        {
            CheckState.Checked => true,
            CheckState.Unchecked => false,
            _ => null
        };
    }

    private static void SetComboBoxValue<T>(ComboBox comboBox, T value)
    {
        var indexToSelect = comboBox.Items
            .OfType<ComboBoxItem<T>>()
            .ToList()
            .FindIndex(item => item.Value?.Equals(value) == true);

        comboBox.SelectedIndex = Math.Max(0, indexToSelect);
    }

    private static T? GetComboBoxValue<T>(ComboBox comboBox)
    {
        if (comboBox.SelectedItem is ComboBoxItem<T> item)
            return item.Value;
        return default;
    }

    private void LoadScreenModeComboBoxToUi(ScreenMode? screenMode, bool? useAllMonitors)
    {
        var screenModeOption = (screenMode, useAllMonitors) switch
        {
            (ScreenMode.FullScreen, true) => ScreenModeOption.UseAllMonitors,
            (ScreenMode.FullScreen, _) => ScreenModeOption.FullScreen,
            (ScreenMode.Windowed, _) => ScreenModeOption.Windowed,
            _ => ScreenModeOption.NotSet
        };

        SetComboBoxValue(cmbScreenMode, screenModeOption);
    }

    private static (ScreenMode? screenMode, bool? useAllMonitors) GetScreenModeValuesFromUi(ScreenModeOption option)
    {
        return option switch
        {
            ScreenModeOption.Windowed => (ScreenMode.Windowed, null),
            ScreenModeOption.FullScreen => (ScreenMode.FullScreen, false),
            ScreenModeOption.UseAllMonitors => (ScreenMode.FullScreen, true),
            _ => (null, null)
        };
    }

    private void ExportSettingsToRdpFile()
    {
        var connectionSettings = GetConnectionSettingsFromUi();
        var filename = GetRdpFilename(UpdatedSettings);
        using var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "RDP Files (*.rdp)|*.rdp|All Files (*.*)|*.*";
        saveFileDialog.DefaultExt = "rdp";
        saveFileDialog.InitialDirectory = Path.GetDirectoryName(filename);
        saveFileDialog.FileName = Path.GetFileName(filename);
        saveFileDialog.Title = "Export RDP File";

        if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
            return;

        RdpFileService.SaveToFile(connectionSettings, saveFileDialog.FileName);
        UpdatedSettings?.RdpFilePath = saveFileDialog.FileName;
    }

    private static string GetRdpFilename(ApplicationSettings? settings)
    {
        if (!string.IsNullOrWhiteSpace(settings?.RdpFilePath))
            return settings.RdpFilePath;

        var filename = settings?.Connection.Hostname ?? "connection";
        return $"{filename}.rdp";
    }

    private static bool AllowDigitsOnly(char keyChar)
    {
        // backspace, delete, arrows via KeyDown, etc.
        var isControlKey = char.IsControl(keyChar);
        var isDigit = char.IsDigit(keyChar);
        return !isControlKey && !isDigit;
    }

    private void AllowDigitsOnly(ComboBox? combobox)
    {
        if (combobox == null)
            return;

        var original = combobox.Text;
        var filtered = new string(original.Where(char.IsDigit).ToArray());
        if (original == filtered)
            return;

        // preserve caret position as well as possible
        var selStart = Math.Max(0, combobox.SelectionStart);
        combobox.Text = filtered;
        combobox.SelectionStart = Math.Min(filtered.Length, selStart);

        AdjustWidthAndHeightValues();
    }

    private void AdjustWidthAndHeightValues()
    {
        if (chkAutoResize.Checked)
            return;

        var lowerScreenSize = GetNextLowerScreenResolution();
        if (string.IsNullOrWhiteSpace(cmbWidth.Text))
            cmbWidth.Text = lowerScreenSize.Width.ToString();
        if (string.IsNullOrWhiteSpace(cmbHeight.Text))
            cmbHeight.Text = lowerScreenSize.Height.ToString();
    }

    private static Size GetNextLowerScreenResolution()
    {
        var currentScreenSize = Screen.PrimaryScreen?.Bounds.Size ?? Size.Empty;
        var lowerScreenSize = _displayResolutions.FirstOrDefault(res => res.Width < currentScreenSize.Width - 100 && res.Height < currentScreenSize.Height - 100);
        if (lowerScreenSize.IsEmpty)
            lowerScreenSize = _displayResolutions.Min();
        return lowerScreenSize;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        UpdatedSettings?.Connection = GetConnectionSettingsFromUi();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void cmbWidth_KeyPress(object sender, KeyPressEventArgs e)
        => e.Handled = AllowDigitsOnly(e.KeyChar);

    private void cmbWidth_TextChanged(object sender, EventArgs e)
        => AllowDigitsOnly(sender as ComboBox);

    private void cmbHeight_KeyPress(object sender, KeyPressEventArgs e)
        => e.Handled = AllowDigitsOnly(e.KeyChar);

    private void cmbHeight_TextChanged(object sender, EventArgs e)
        => AllowDigitsOnly(sender as ComboBox);

    private void btnExportRdp_Click(object sender, EventArgs e)
        => ExportSettingsToRdpFile();

    private void chkAutoResize_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAutoResize.Checked)
            chkSmartSizing.Checked = false;

        AdjustWidthAndHeightValues();
    }

    private void chkSmartSizing_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSmartSizing.Checked)
            chkAutoResize.Checked = false;
    }

    private enum ScreenModeOption
    {
        NotSet,
        Windowed,
        FullScreen,
        UseAllMonitors
    }

    private class ComboBoxItem<T>
    {
        public string Display { get; }

        public T Value { get; }

        public ComboBoxItem(T value, string display)
        {
            Value = value;
            Display = display;
        }

        public override string ToString() => Display;
    }
}