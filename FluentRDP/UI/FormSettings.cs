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

    internal ApplicationSettings UpdatedSettings { get; private set; }

    internal FormSettings(ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        InitializeComponent();
        InitializeEnumComboBoxes();

        UpdatedSettings = settings;
        LoadConnectionSettingsToUi(UpdatedSettings.Connection);
    }

    private void InitializeEnumComboBoxes()
    {
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
    }

    private void LoadConnectionSettingsToUi(ConnectionSettings settings)
    {
        Text = UpdatedSettings.RdpFilePath != null
            ? $"Settings ({Path.GetFileName(UpdatedSettings.RdpFilePath)}) - FluentRDP"
            : "Settings - FluentRDP";

        using (new TextChangedEventSuppressor(tbHostname, tbHostname_TextChanged))
            tbHostname.Text = settings.Hostname ?? string.Empty;
        using (new TextChangedEventSuppressor(tbUsername, tbUsername_TextChanged))
            tbUsername.Text = settings.Username ?? string.Empty;
        tbDomain.Text = settings.Domain ?? string.Empty;
        chkEnableCredSsp.CheckState = GetCheckState(settings.EnableCredSsp);
        UpdateSavedCredentialHints();

        SetComboBoxValue(cmbKeyboardMode, settings.KeyboardMode);
        SetComboBoxValue(cmbAudioMode, settings.AudioPlaybackMode);
        SetComboBoxValue(cmbRedirectAudioCapture, settings.RedirectAudioCapture);
        tbRedirectDrives.Text = settings.RedirectDrives ?? string.Empty;
        chkRedirectClipboard.CheckState = GetCheckState(settings.RedirectClipboard);
        chkRedirectPrinters.CheckState = GetCheckState(settings.RedirectPrinters);
        chkRedirectSmartCards.CheckState = GetCheckState(settings.RedirectSmartCards);

        SetScreenModeComboBoxToUi(settings.ScreenMode, settings.UseAllMonitors);
        cmbWidth.Text = settings.Width.ToString();
        cmbHeight.Text = settings.Height.ToString();
        chkAutoResize.CheckState = GetCheckState(settings.AutoResize);
        chkSmartSizing.CheckState = GetCheckState(settings.SmartSizing);
        SetComboBoxValue(cmbScaleFactor, settings.ScaleFactor);
        SetComboBoxValue(cmbColorDepth, settings.ColorDepth);
        chkEnableCompression.CheckState = GetCheckState(settings.EnableCompression);
        chkEnableBitmapPersistence.CheckState = GetCheckState(settings.EnableBitmapPersistence);

        chkDisplayConnectionBar.CheckState = GetCheckState(settings.DisplayConnectionBar);
        chkPinConnectionBar.CheckState = GetCheckState(settings.PinConnectionBar);

        SetComboBoxValue(cmbAuthenticationLevel, settings.AuthenticationLevel);
    }

    private ConnectionSettings GetConnectionSettingsFromUi()
    {
        var screenModeOption = GetComboBoxValue<ScreenModeOption>(cmbScreenMode);
        var (screenMode, useAllMonitors) = GetScreenModeValuesFromUi(screenModeOption);

        var updatedConnectionSettings = UpdatedSettings.Connection.Clone();
        updatedConnectionSettings.Hostname = string.IsNullOrWhiteSpace(tbHostname.Text) ? null : tbHostname.Text;
        updatedConnectionSettings.Username = string.IsNullOrWhiteSpace(tbUsername.Text) ? null : tbUsername.Text;
        updatedConnectionSettings.Domain = string.IsNullOrWhiteSpace(tbDomain.Text) ? null : tbDomain.Text;
        updatedConnectionSettings.Password = string.IsNullOrWhiteSpace(tbPassword.Text) ? updatedConnectionSettings.Password : tbPassword.Text;
        updatedConnectionSettings.EnableCredSsp = GetBoolValue(chkEnableCredSsp.CheckState);

        updatedConnectionSettings.KeyboardMode = GetComboBoxValue<KeyboardMode?>(cmbKeyboardMode);
        updatedConnectionSettings.AudioPlaybackMode = GetComboBoxValue<AudioPlaybackMode?>(cmbAudioMode);
        updatedConnectionSettings.RedirectAudioCapture = GetComboBoxValue<bool?>(cmbRedirectAudioCapture);
        updatedConnectionSettings.RedirectDrives = string.IsNullOrWhiteSpace(tbRedirectDrives.Text) ? null : tbRedirectDrives.Text;
        updatedConnectionSettings.RedirectClipboard = GetBoolValue(chkRedirectClipboard.CheckState);
        updatedConnectionSettings.RedirectPrinters = GetBoolValue(chkRedirectPrinters.CheckState);
        updatedConnectionSettings.RedirectSmartCards = GetBoolValue(chkRedirectSmartCards.CheckState);

        updatedConnectionSettings.ScreenMode = screenMode;
        updatedConnectionSettings.UseAllMonitors = useAllMonitors;
        updatedConnectionSettings.Width = string.IsNullOrWhiteSpace(cmbWidth.Text) ? null : int.Parse(cmbWidth.Text);
        updatedConnectionSettings.Height = string.IsNullOrWhiteSpace(cmbHeight.Text) ? null : int.Parse(cmbHeight.Text);
        updatedConnectionSettings.AutoResize = GetBoolValue(chkAutoResize.CheckState);
        updatedConnectionSettings.SmartSizing = GetBoolValue(chkSmartSizing.CheckState);
        updatedConnectionSettings.ScaleFactor = GetComboBoxValue<uint?>(cmbScaleFactor);
        updatedConnectionSettings.ColorDepth = GetComboBoxValue<int?>(cmbColorDepth);
        updatedConnectionSettings.EnableCompression = GetBoolValue(chkEnableCompression.CheckState);
        updatedConnectionSettings.EnableBitmapPersistence = GetBoolValue(chkEnableBitmapPersistence.CheckState);

        updatedConnectionSettings.DisplayConnectionBar = GetBoolValue(chkDisplayConnectionBar.CheckState);
        updatedConnectionSettings.PinConnectionBar = GetBoolValue(chkPinConnectionBar.CheckState);

        updatedConnectionSettings.AuthenticationLevel = GetComboBoxValue<AuthenticationLevel?>(cmbAuthenticationLevel);

        return updatedConnectionSettings;
    }

    private static (ScreenMode? screenMode, bool? useAllMonitors) GetScreenModeValuesFromUi(ScreenModeOption screenMode)
    {
        return screenMode switch
        {
            ScreenModeOption.Windowed => (ScreenMode.Windowed, false),
            ScreenModeOption.FullScreen => (ScreenMode.FullScreen, false),
            ScreenModeOption.UseAllMonitors => (ScreenMode.FullScreen, true),
            _ => throw new InvalidOperationException($"Invalid screen mode {screenMode}."),
        };
    }

    private void SetScreenModeComboBoxToUi(ScreenMode? screenMode, bool? useAllMonitors)
    {
        var screenModeOption = (screenMode, useAllMonitors) switch
        {
            (ScreenMode.FullScreen, true) => ScreenModeOption.UseAllMonitors,
            (ScreenMode.FullScreen, _) => ScreenModeOption.FullScreen,
            (ScreenMode.Windowed, _) => ScreenModeOption.Windowed,
            _ => throw new InvalidOperationException($"Invalid screen mode {screenMode}."),
        };

        SetComboBoxValue(cmbScreenMode, screenModeOption);
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

    private static T? GetComboBoxValue<T>(ComboBox comboBox)
    {
        if (comboBox.SelectedItem is ComboBoxItem<T> item)
            return item.Value;
        return default;

    }

    private static void SetComboBoxValue<T>(ComboBox comboBox, T value)
    {
        var indexToSelect = comboBox.Items
            .OfType<ComboBoxItem<T>>()
            .ToList()
            .FindIndex(item => item.Value?.Equals(value) == true);

        comboBox.SelectedIndex = Math.Max(0, indexToSelect);
    }

    private void UpdateSavedCredentialHints()
    {
        var usernameIsEmpty = string.IsNullOrWhiteSpace(tbUsername.Text);
        var savedUsername = RdpCredentialService.CredentialsExist(tbHostname.Text);
        var savedCredentialsExists = savedUsername != null;
        if (usernameIsEmpty && savedCredentialsExists)
            using (new TextChangedEventSuppressor(tbUsername, tbUsername_TextChanged))
                tbUsername.Text = savedUsername;

        var credentialsToSaveAvailable = !string.IsNullOrWhiteSpace(tbHostname.Text) && !string.IsNullOrWhiteSpace(tbUsername.Text);
        lbCredentialSave.Text = savedCredentialsExists ? "Update" : "Save";
        lbCredentialSave.Visible = credentialsToSaveAvailable;
        lbCredentialRemove.Visible = savedCredentialsExists;
        lbCredentialRemove.Location = lbCredentialRemove.Location with { X = credentialsToSaveAvailable ? 163 : 112 };
    }

    private void SaveSettingsToRdpFile(bool saveAs)
    {
        var connectionSettings = GetConnectionSettingsFromUi();
        var rdpFilePath = UpdatedSettings.RdpFilePath;
        var rdpFileNotExists = !File.Exists(UpdatedSettings.RdpFilePath);

        if (saveAs || rdpFileNotExists)
        {
            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "RDP Files (*.rdp)|*.rdp|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "rdp";
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(rdpFilePath);
            saveFileDialog.FileName = Path.GetFileName(rdpFilePath);
            saveFileDialog.Title = "Export RDP File";

            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            UpdatedSettings.RdpFilePath = saveFileDialog.FileName;
        }

        RdpFileService.SaveToFile(connectionSettings, UpdatedSettings.RdpFilePath!);
    }

    private void UpdateSettingsAndClose()
    {
        UpdatedSettings.Connection = GetConnectionSettingsFromUi();
        Close();
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

    private void btnCancel_Click(object sender, EventArgs e)
        => Close();

    private void cmbWidth_KeyPress(object sender, KeyPressEventArgs e)
        => e.Handled = AllowDigitsOnly(e.KeyChar);

    private void cmbWidth_TextChanged(object sender, EventArgs e)
        => AllowDigitsOnly(sender as ComboBox);

    private void cmbHeight_KeyPress(object sender, KeyPressEventArgs e)
        => e.Handled = AllowDigitsOnly(e.KeyChar);

    private void cmbHeight_TextChanged(object sender, EventArgs e)
        => AllowDigitsOnly(sender as ComboBox);

    private void btnSaveAs_Click(object sender, EventArgs e)
        => SaveSettingsToRdpFile(true);

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

    private void btnConnect_Click(object sender, EventArgs e)
        => UpdateSettingsAndClose();

    private void btnSave_Click(object sender, EventArgs e)
        => SaveSettingsToRdpFile(saveAs: false);

    private void tbHostname_TextChanged(object? sender, EventArgs e)
        => UpdateSavedCredentialHints();

    private void tbUsername_TextChanged(object? sender, EventArgs e)
        => UpdateSavedCredentialHints();

    private void lbCredentialSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        var password = string.IsNullOrWhiteSpace(tbPassword.Text) ? UpdatedSettings.Connection.Password : tbPassword.Text;
        RdpCredentialService.SaveCredentials(tbHostname.Text, tbUsername.Text, password);
        UpdateSavedCredentialHints();
    }

    private void lbCredentialRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        RdpCredentialService.RemoveCredentials(tbHostname.Text);
        UpdateSavedCredentialHints();
    }

    private enum ScreenModeOption
    {
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