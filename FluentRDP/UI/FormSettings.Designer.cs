namespace FluentRDP.UI
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbKeyboardMode = new System.Windows.Forms.ComboBox();
            lblKeyboardMode = new System.Windows.Forms.Label();
            cmbRedirectAudioCapture = new System.Windows.Forms.ComboBox();
            lblRedirectAudioCapture = new System.Windows.Forms.Label();
            cmbAudioMode = new System.Windows.Forms.ComboBox();
            lblAudioPlaybackMode = new System.Windows.Forms.Label();
            tbRedirectDrives = new System.Windows.Forms.TextBox();
            lblRedirectDrives = new System.Windows.Forms.Label();
            grpServerAuthentication = new System.Windows.Forms.GroupBox();
            lblAuthenticationLevel = new System.Windows.Forms.Label();
            cmbAuthenticationLevel = new System.Windows.Forms.ComboBox();
            grpRedirection = new System.Windows.Forms.GroupBox();
            chkRedirectSmartCards = new System.Windows.Forms.CheckBox();
            chkRedirectPrinters = new System.Windows.Forms.CheckBox();
            chkRedirectClipboard = new System.Windows.Forms.CheckBox();
            grpDisplay = new System.Windows.Forms.GroupBox();
            cmbHeight = new System.Windows.Forms.ComboBox();
            lblSizing = new System.Windows.Forms.Label();
            cmbWidth = new System.Windows.Forms.ComboBox();
            chkEnableBitmapPersistence = new System.Windows.Forms.CheckBox();
            chkEnableCompression = new System.Windows.Forms.CheckBox();
            lblScreenMode = new System.Windows.Forms.Label();
            chkAutoResize = new System.Windows.Forms.CheckBox();
            cmbScreenMode = new System.Windows.Forms.ComboBox();
            chkSmartSizing = new System.Windows.Forms.CheckBox();
            cmbScaleFactor = new System.Windows.Forms.ComboBox();
            lblScaleFactor = new System.Windows.Forms.Label();
            cmbColorDepth = new System.Windows.Forms.ComboBox();
            lblColorDepth = new System.Windows.Forms.Label();
            lblHeight = new System.Windows.Forms.Label();
            lblWidthHeight = new System.Windows.Forms.Label();
            grpConnection = new System.Windows.Forms.GroupBox();
            cmbUsername = new System.Windows.Forms.ComboBox();
            lbCredentials = new System.Windows.Forms.Label();
            tbPassword = new System.Windows.Forms.TextBox();
            lblPassword = new System.Windows.Forms.Label();
            lbCredentialRemove = new System.Windows.Forms.LinkLabel();
            cmbHostname = new System.Windows.Forms.ComboBox();
            chkEnableCredSsp = new System.Windows.Forms.CheckBox();
            lbCredentialSave = new System.Windows.Forms.LinkLabel();
            lblUsername = new System.Windows.Forms.Label();
            lblHostname = new System.Windows.Forms.Label();
            btnSaveAs = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            grpExperience = new System.Windows.Forms.GroupBox();
            chkPinConnectionBar = new System.Windows.Forms.CheckBox();
            chkDisplayConnectionBar = new System.Windows.Forms.CheckBox();
            btnConnect = new System.Windows.Forms.Button();
            grpServerAuthentication.SuspendLayout();
            grpRedirection.SuspendLayout();
            grpDisplay.SuspendLayout();
            grpConnection.SuspendLayout();
            grpExperience.SuspendLayout();
            SuspendLayout();
            // 
            // cmbKeyboardMode
            // 
            cmbKeyboardMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbKeyboardMode.FormattingEnabled = true;
            cmbKeyboardMode.Location = new System.Drawing.Point(123, 22);
            cmbKeyboardMode.Name = "cmbKeyboardMode";
            cmbKeyboardMode.Size = new System.Drawing.Size(190, 23);
            cmbKeyboardMode.TabIndex = 1;
            // 
            // lblKeyboardMode
            // 
            lblKeyboardMode.AutoSize = true;
            lblKeyboardMode.Location = new System.Drawing.Point(6, 25);
            lblKeyboardMode.Name = "lblKeyboardMode";
            lblKeyboardMode.Size = new System.Drawing.Size(85, 15);
            lblKeyboardMode.TabIndex = 0;
            lblKeyboardMode.Text = "Windows keys:";
            // 
            // cmbRedirectAudioCapture
            // 
            cmbRedirectAudioCapture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRedirectAudioCapture.FormattingEnabled = true;
            cmbRedirectAudioCapture.Location = new System.Drawing.Point(123, 80);
            cmbRedirectAudioCapture.Name = "cmbRedirectAudioCapture";
            cmbRedirectAudioCapture.Size = new System.Drawing.Size(190, 23);
            cmbRedirectAudioCapture.TabIndex = 5;
            // 
            // lblRedirectAudioCapture
            // 
            lblRedirectAudioCapture.AutoSize = true;
            lblRedirectAudioCapture.Location = new System.Drawing.Point(6, 83);
            lblRedirectAudioCapture.Name = "lblRedirectAudioCapture";
            lblRedirectAudioCapture.Size = new System.Drawing.Size(73, 15);
            lblRedirectAudioCapture.TabIndex = 4;
            lblRedirectAudioCapture.Text = "Audio input:";
            // 
            // cmbAudioMode
            // 
            cmbAudioMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAudioMode.FormattingEnabled = true;
            cmbAudioMode.Location = new System.Drawing.Point(123, 51);
            cmbAudioMode.Name = "cmbAudioMode";
            cmbAudioMode.Size = new System.Drawing.Size(190, 23);
            cmbAudioMode.TabIndex = 3;
            // 
            // lblAudioPlaybackMode
            // 
            lblAudioPlaybackMode.AutoSize = true;
            lblAudioPlaybackMode.Location = new System.Drawing.Point(6, 54);
            lblAudioPlaybackMode.Name = "lblAudioPlaybackMode";
            lblAudioPlaybackMode.Size = new System.Drawing.Size(57, 15);
            lblAudioPlaybackMode.TabIndex = 2;
            lblAudioPlaybackMode.Text = "Playback:";
            // 
            // tbRedirectDrives
            // 
            tbRedirectDrives.Location = new System.Drawing.Point(123, 109);
            tbRedirectDrives.Name = "tbRedirectDrives";
            tbRedirectDrives.Size = new System.Drawing.Size(190, 23);
            tbRedirectDrives.TabIndex = 7;
            // 
            // lblRedirectDrives
            // 
            lblRedirectDrives.AutoSize = true;
            lblRedirectDrives.Location = new System.Drawing.Point(6, 112);
            lblRedirectDrives.Name = "lblRedirectDrives";
            lblRedirectDrives.Size = new System.Drawing.Size(72, 15);
            lblRedirectDrives.TabIndex = 6;
            lblRedirectDrives.Text = "Local drives:";
            // 
            // grpServerAuthentication
            // 
            grpServerAuthentication.Controls.Add(lblAuthenticationLevel);
            grpServerAuthentication.Controls.Add(cmbAuthenticationLevel);
            grpServerAuthentication.Location = new System.Drawing.Point(349, 310);
            grpServerAuthentication.Name = "grpServerAuthentication";
            grpServerAuthentication.Size = new System.Drawing.Size(326, 85);
            grpServerAuthentication.TabIndex = 4;
            grpServerAuthentication.TabStop = false;
            grpServerAuthentication.Text = "Server authentication";
            // 
            // lblAuthenticationLevel
            // 
            lblAuthenticationLevel.AutoSize = true;
            lblAuthenticationLevel.Location = new System.Drawing.Point(6, 25);
            lblAuthenticationLevel.Name = "lblAuthenticationLevel";
            lblAuthenticationLevel.Size = new System.Drawing.Size(100, 15);
            lblAuthenticationLevel.TabIndex = 0;
            lblAuthenticationLevel.Text = "Invalid certificate:";
            // 
            // cmbAuthenticationLevel
            // 
            cmbAuthenticationLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAuthenticationLevel.FormattingEnabled = true;
            cmbAuthenticationLevel.Location = new System.Drawing.Point(123, 22);
            cmbAuthenticationLevel.Name = "cmbAuthenticationLevel";
            cmbAuthenticationLevel.Size = new System.Drawing.Size(190, 23);
            cmbAuthenticationLevel.TabIndex = 1;
            // 
            // grpRedirection
            // 
            grpRedirection.Controls.Add(cmbRedirectAudioCapture);
            grpRedirection.Controls.Add(lblRedirectAudioCapture);
            grpRedirection.Controls.Add(tbRedirectDrives);
            grpRedirection.Controls.Add(cmbAudioMode);
            grpRedirection.Controls.Add(cmbKeyboardMode);
            grpRedirection.Controls.Add(lblRedirectDrives);
            grpRedirection.Controls.Add(lblAudioPlaybackMode);
            grpRedirection.Controls.Add(chkRedirectSmartCards);
            grpRedirection.Controls.Add(chkRedirectPrinters);
            grpRedirection.Controls.Add(chkRedirectClipboard);
            grpRedirection.Controls.Add(lblKeyboardMode);
            grpRedirection.Location = new System.Drawing.Point(10, 179);
            grpRedirection.Name = "grpRedirection";
            grpRedirection.Size = new System.Drawing.Size(326, 216);
            grpRedirection.TabIndex = 1;
            grpRedirection.TabStop = false;
            grpRedirection.Text = "Redirection";
            // 
            // chkRedirectSmartCards
            // 
            chkRedirectSmartCards.AutoSize = true;
            chkRedirectSmartCards.Location = new System.Drawing.Point(8, 188);
            chkRedirectSmartCards.Name = "chkRedirectSmartCards";
            chkRedirectSmartCards.Size = new System.Drawing.Size(133, 19);
            chkRedirectSmartCards.TabIndex = 10;
            chkRedirectSmartCards.Text = "Redirect smart cards";
            chkRedirectSmartCards.UseVisualStyleBackColor = true;
            // 
            // chkRedirectPrinters
            // 
            chkRedirectPrinters.AutoSize = true;
            chkRedirectPrinters.Location = new System.Drawing.Point(8, 163);
            chkRedirectPrinters.Name = "chkRedirectPrinters";
            chkRedirectPrinters.Size = new System.Drawing.Size(112, 19);
            chkRedirectPrinters.TabIndex = 9;
            chkRedirectPrinters.Text = "Redirect printers";
            chkRedirectPrinters.UseVisualStyleBackColor = true;
            // 
            // chkRedirectClipboard
            // 
            chkRedirectClipboard.AutoSize = true;
            chkRedirectClipboard.Location = new System.Drawing.Point(8, 138);
            chkRedirectClipboard.Name = "chkRedirectClipboard";
            chkRedirectClipboard.Size = new System.Drawing.Size(122, 19);
            chkRedirectClipboard.TabIndex = 8;
            chkRedirectClipboard.Text = "Redirect clipboard";
            chkRedirectClipboard.UseVisualStyleBackColor = true;
            // 
            // grpDisplay
            // 
            grpDisplay.Controls.Add(cmbHeight);
            grpDisplay.Controls.Add(lblSizing);
            grpDisplay.Controls.Add(cmbWidth);
            grpDisplay.Controls.Add(chkEnableBitmapPersistence);
            grpDisplay.Controls.Add(chkEnableCompression);
            grpDisplay.Controls.Add(lblScreenMode);
            grpDisplay.Controls.Add(chkAutoResize);
            grpDisplay.Controls.Add(cmbScreenMode);
            grpDisplay.Controls.Add(chkSmartSizing);
            grpDisplay.Controls.Add(cmbScaleFactor);
            grpDisplay.Controls.Add(lblScaleFactor);
            grpDisplay.Controls.Add(cmbColorDepth);
            grpDisplay.Controls.Add(lblColorDepth);
            grpDisplay.Controls.Add(lblHeight);
            grpDisplay.Controls.Add(lblWidthHeight);
            grpDisplay.Location = new System.Drawing.Point(349, 12);
            grpDisplay.Name = "grpDisplay";
            grpDisplay.Size = new System.Drawing.Size(326, 212);
            grpDisplay.TabIndex = 2;
            grpDisplay.TabStop = false;
            grpDisplay.Text = "Display";
            // 
            // cmbHeight
            // 
            cmbHeight.FormattingEnabled = true;
            cmbHeight.Location = new System.Drawing.Point(228, 49);
            cmbHeight.Name = "cmbHeight";
            cmbHeight.Size = new System.Drawing.Size(85, 23);
            cmbHeight.TabIndex = 5;
            cmbHeight.TextChanged += cmbHeight_TextChanged;
            cmbHeight.KeyPress += cmbHeight_KeyPress;
            // 
            // lblSizing
            // 
            lblSizing.AutoSize = true;
            lblSizing.Location = new System.Drawing.Point(6, 82);
            lblSizing.Name = "lblSizing";
            lblSizing.Size = new System.Drawing.Size(78, 15);
            lblSizing.TabIndex = 6;
            lblSizing.Text = "Screen sizing:";
            // 
            // cmbWidth
            // 
            cmbWidth.FormattingEnabled = true;
            cmbWidth.Location = new System.Drawing.Point(123, 49);
            cmbWidth.Name = "cmbWidth";
            cmbWidth.Size = new System.Drawing.Size(85, 23);
            cmbWidth.TabIndex = 3;
            cmbWidth.TextChanged += cmbWidth_TextChanged;
            cmbWidth.KeyPress += cmbWidth_KeyPress;
            // 
            // chkEnableBitmapPersistence
            // 
            chkEnableBitmapPersistence.AutoSize = true;
            chkEnableBitmapPersistence.Location = new System.Drawing.Point(8, 188);
            chkEnableBitmapPersistence.Name = "chkEnableBitmapPersistence";
            chkEnableBitmapPersistence.Size = new System.Drawing.Size(164, 19);
            chkEnableBitmapPersistence.TabIndex = 14;
            chkEnableBitmapPersistence.Text = "Enable bitmap persistence";
            chkEnableBitmapPersistence.UseVisualStyleBackColor = true;
            // 
            // chkEnableCompression
            // 
            chkEnableCompression.AutoSize = true;
            chkEnableCompression.Location = new System.Drawing.Point(8, 163);
            chkEnableCompression.Name = "chkEnableCompression";
            chkEnableCompression.Size = new System.Drawing.Size(132, 19);
            chkEnableCompression.TabIndex = 13;
            chkEnableCompression.Text = "Enable compression";
            chkEnableCompression.UseVisualStyleBackColor = true;
            // 
            // lblScreenMode
            // 
            lblScreenMode.AutoSize = true;
            lblScreenMode.Location = new System.Drawing.Point(6, 25);
            lblScreenMode.Name = "lblScreenMode";
            lblScreenMode.Size = new System.Drawing.Size(79, 15);
            lblScreenMode.TabIndex = 0;
            lblScreenMode.Text = "Screen Mode:";
            // 
            // chkAutoResize
            // 
            chkAutoResize.AutoSize = true;
            chkAutoResize.Location = new System.Drawing.Point(123, 80);
            chkAutoResize.Name = "chkAutoResize";
            chkAutoResize.Size = new System.Drawing.Size(84, 19);
            chkAutoResize.TabIndex = 7;
            chkAutoResize.Text = "Auto resize";
            chkAutoResize.UseVisualStyleBackColor = true;
            chkAutoResize.CheckedChanged += chkAutoResize_CheckedChanged;
            // 
            // cmbScreenMode
            // 
            cmbScreenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbScreenMode.FormattingEnabled = true;
            cmbScreenMode.Location = new System.Drawing.Point(123, 22);
            cmbScreenMode.Name = "cmbScreenMode";
            cmbScreenMode.Size = new System.Drawing.Size(190, 23);
            cmbScreenMode.TabIndex = 1;
            // 
            // chkSmartSizing
            // 
            chkSmartSizing.AutoSize = true;
            chkSmartSizing.Location = new System.Drawing.Point(212, 80);
            chkSmartSizing.Name = "chkSmartSizing";
            chkSmartSizing.Size = new System.Drawing.Size(90, 19);
            chkSmartSizing.TabIndex = 8;
            chkSmartSizing.Text = "Smart sizing";
            chkSmartSizing.UseVisualStyleBackColor = true;
            chkSmartSizing.CheckedChanged += chkSmartSizing_CheckedChanged;
            // 
            // cmbScaleFactor
            // 
            cmbScaleFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbScaleFactor.FormattingEnabled = true;
            cmbScaleFactor.Location = new System.Drawing.Point(123, 105);
            cmbScaleFactor.Name = "cmbScaleFactor";
            cmbScaleFactor.Size = new System.Drawing.Size(190, 23);
            cmbScaleFactor.TabIndex = 10;
            // 
            // lblScaleFactor
            // 
            lblScaleFactor.AutoSize = true;
            lblScaleFactor.Location = new System.Drawing.Point(6, 108);
            lblScaleFactor.Name = "lblScaleFactor";
            lblScaleFactor.Size = new System.Drawing.Size(94, 15);
            lblScaleFactor.TabIndex = 9;
            lblScaleFactor.Text = "DPI Scale Factor:";
            // 
            // cmbColorDepth
            // 
            cmbColorDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbColorDepth.FormattingEnabled = true;
            cmbColorDepth.Location = new System.Drawing.Point(123, 134);
            cmbColorDepth.Name = "cmbColorDepth";
            cmbColorDepth.Size = new System.Drawing.Size(190, 23);
            cmbColorDepth.TabIndex = 12;
            // 
            // lblColorDepth
            // 
            lblColorDepth.AutoSize = true;
            lblColorDepth.Location = new System.Drawing.Point(6, 137);
            lblColorDepth.Name = "lblColorDepth";
            lblColorDepth.Size = new System.Drawing.Size(74, 15);
            lblColorDepth.TabIndex = 11;
            lblColorDepth.Text = "Color Depth:";
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Location = new System.Drawing.Point(212, 52);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new System.Drawing.Size(12, 15);
            lblHeight.TabIndex = 4;
            lblHeight.Text = "/";
            // 
            // lblWidthHeight
            // 
            lblWidthHeight.AutoSize = true;
            lblWidthHeight.Location = new System.Drawing.Point(6, 53);
            lblWidthHeight.Name = "lblWidthHeight";
            lblWidthHeight.Size = new System.Drawing.Size(89, 15);
            lblWidthHeight.TabIndex = 2;
            lblWidthHeight.Text = "Width / Height:";
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(cmbUsername);
            grpConnection.Controls.Add(lbCredentials);
            grpConnection.Controls.Add(tbPassword);
            grpConnection.Controls.Add(lblPassword);
            grpConnection.Controls.Add(lbCredentialRemove);
            grpConnection.Controls.Add(cmbHostname);
            grpConnection.Controls.Add(chkEnableCredSsp);
            grpConnection.Controls.Add(lbCredentialSave);
            grpConnection.Controls.Add(lblUsername);
            grpConnection.Controls.Add(lblHostname);
            grpConnection.Location = new System.Drawing.Point(10, 12);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new System.Drawing.Size(326, 161);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "Connection";
            // 
            // cmbUsername
            // 
            cmbUsername.FormattingEnabled = true;
            cmbUsername.Location = new System.Drawing.Point(123, 51);
            cmbUsername.Name = "cmbUsername";
            cmbUsername.Size = new System.Drawing.Size(190, 23);
            cmbUsername.TabIndex = 3;
            cmbUsername.TextChanged += cmbUsername_TextChanged;
            // 
            // lbCredentials
            // 
            lbCredentials.AutoSize = true;
            lbCredentials.Location = new System.Drawing.Point(5, 108);
            lbCredentials.Name = "lbCredentials";
            lbCredentials.Size = new System.Drawing.Size(101, 15);
            lbCredentials.TabIndex = 6;
            lbCredentials.Text = "Saved credentials:";
            // 
            // tbPassword
            // 
            tbPassword.Location = new System.Drawing.Point(123, 80);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new System.Drawing.Size(190, 23);
            tbPassword.TabIndex = 5;
            tbPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(6, 83);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(60, 15);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password:";
            // 
            // lbCredentialRemove
            // 
            lbCredentialRemove.AutoSize = true;
            lbCredentialRemove.Location = new System.Drawing.Point(179, 108);
            lbCredentialRemove.Name = "lbCredentialRemove";
            lbCredentialRemove.Size = new System.Drawing.Size(50, 15);
            lbCredentialRemove.TabIndex = 8;
            lbCredentialRemove.TabStop = true;
            lbCredentialRemove.Text = "Remove";
            lbCredentialRemove.LinkClicked += lbCredentialRemove_LinkClicked;
            // 
            // cmbHostname
            // 
            cmbHostname.FormattingEnabled = true;
            cmbHostname.Location = new System.Drawing.Point(123, 22);
            cmbHostname.Name = "cmbHostname";
            cmbHostname.Size = new System.Drawing.Size(190, 23);
            cmbHostname.TabIndex = 1;
            cmbHostname.SelectionChangeCommitted += cmbHostname_SelectionChangeCommitted;
            cmbHostname.TextChanged += cmbHostname_TextChanged;
            cmbHostname.KeyDown += cmbHostname_KeyDown;
            // 
            // chkEnableCredSsp
            // 
            chkEnableCredSsp.AutoSize = true;
            chkEnableCredSsp.Location = new System.Drawing.Point(6, 136);
            chkEnableCredSsp.Name = "chkEnableCredSsp";
            chkEnableCredSsp.Size = new System.Drawing.Size(238, 19);
            chkEnableCredSsp.TabIndex = 9;
            chkEnableCredSsp.Text = "CredSSP / Network Level Authentication";
            chkEnableCredSsp.UseVisualStyleBackColor = true;
            // 
            // lbCredentialSave
            // 
            lbCredentialSave.AutoSize = true;
            lbCredentialSave.Location = new System.Drawing.Point(123, 108);
            lbCredentialSave.Name = "lbCredentialSave";
            lbCredentialSave.Size = new System.Drawing.Size(45, 15);
            lbCredentialSave.TabIndex = 7;
            lbCredentialSave.TabStop = true;
            lbCredentialSave.Text = "Update";
            lbCredentialSave.LinkClicked += lbCredentialSave_LinkClicked;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(6, 54);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(63, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username:";
            // 
            // lblHostname
            // 
            lblHostname.AutoSize = true;
            lblHostname.Location = new System.Drawing.Point(6, 25);
            lblHostname.Name = "lblHostname";
            lblHostname.Size = new System.Drawing.Size(65, 15);
            lblHostname.TabIndex = 0;
            lblHostname.Text = "Hostname:";
            // 
            // btnSaveAs
            // 
            btnSaveAs.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSaveAs.Location = new System.Drawing.Point(12, 405);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new System.Drawing.Size(91, 23);
            btnSaveAs.TabIndex = 7;
            btnSaveAs.Text = "Save as...";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSave.Location = new System.Drawing.Point(111, 405);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 23);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(600, 405);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // grpExperience
            // 
            grpExperience.Controls.Add(chkPinConnectionBar);
            grpExperience.Controls.Add(chkDisplayConnectionBar);
            grpExperience.Location = new System.Drawing.Point(349, 230);
            grpExperience.Name = "grpExperience";
            grpExperience.Size = new System.Drawing.Size(326, 74);
            grpExperience.TabIndex = 3;
            grpExperience.TabStop = false;
            grpExperience.Text = "Experience";
            // 
            // chkPinConnectionBar
            // 
            chkPinConnectionBar.AutoSize = true;
            chkPinConnectionBar.Location = new System.Drawing.Point(8, 47);
            chkPinConnectionBar.Name = "chkPinConnectionBar";
            chkPinConnectionBar.Size = new System.Drawing.Size(126, 19);
            chkPinConnectionBar.TabIndex = 1;
            chkPinConnectionBar.Text = "Pin connection bar";
            chkPinConnectionBar.UseVisualStyleBackColor = true;
            // 
            // chkDisplayConnectionBar
            // 
            chkDisplayConnectionBar.AutoSize = true;
            chkDisplayConnectionBar.Location = new System.Drawing.Point(8, 22);
            chkDisplayConnectionBar.Name = "chkDisplayConnectionBar";
            chkDisplayConnectionBar.Size = new System.Drawing.Size(147, 19);
            chkDisplayConnectionBar.TabIndex = 0;
            chkDisplayConnectionBar.Text = "Display connection bar";
            chkDisplayConnectionBar.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            btnConnect.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnConnect.Location = new System.Drawing.Point(519, 405);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new System.Drawing.Size(75, 23);
            btnConnect.TabIndex = 5;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // FormSettings
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(689, 440);
            Controls.Add(btnConnect);
            Controls.Add(grpExperience);
            Controls.Add(btnSaveAs);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpServerAuthentication);
            Controls.Add(grpRedirection);
            Controls.Add(grpDisplay);
            Controls.Add(grpConnection);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings - FluentRDP";
            grpServerAuthentication.ResumeLayout(false);
            grpServerAuthentication.PerformLayout();
            grpRedirection.ResumeLayout(false);
            grpRedirection.PerformLayout();
            grpDisplay.ResumeLayout(false);
            grpDisplay.PerformLayout();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            grpExperience.ResumeLayout(false);
            grpExperience.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRedirectAudioCapture;
        private System.Windows.Forms.Label lblRedirectAudioCapture;
        private System.Windows.Forms.ComboBox cmbAudioMode;
        private System.Windows.Forms.Label lblAudioPlaybackMode;
        private System.Windows.Forms.ComboBox cmbKeyboardMode;
        private System.Windows.Forms.Label lblKeyboardMode;
        private System.Windows.Forms.TextBox tbRedirectDrives;
        private System.Windows.Forms.Label lblRedirectDrives;
        private System.Windows.Forms.GroupBox grpServerAuthentication;
        private System.Windows.Forms.ComboBox cmbAuthenticationLevel;
        private System.Windows.Forms.Label lblAuthenticationLevel;
        private System.Windows.Forms.GroupBox grpRedirection;
        private System.Windows.Forms.CheckBox chkRedirectSmartCards;
        private System.Windows.Forms.CheckBox chkRedirectPrinters;
        private System.Windows.Forms.CheckBox chkRedirectClipboard;
        private System.Windows.Forms.GroupBox grpDisplay;
        private System.Windows.Forms.CheckBox chkEnableBitmapPersistence;
        private System.Windows.Forms.CheckBox chkEnableCompression;
        private System.Windows.Forms.Label lblScreenMode;
        private System.Windows.Forms.CheckBox chkAutoResize;
        private System.Windows.Forms.ComboBox cmbScreenMode;
        private System.Windows.Forms.CheckBox chkSmartSizing;
        private System.Windows.Forms.ComboBox cmbScaleFactor;
        private System.Windows.Forms.Label lblScaleFactor;
        private System.Windows.Forms.ComboBox cmbColorDepth;
        private System.Windows.Forms.Label lblColorDepth;
        private System.Windows.Forms.Label lblWidthHeight;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.CheckBox chkEnableCredSsp;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblSizing;
        private System.Windows.Forms.ComboBox cmbWidth;
        private System.Windows.Forms.ComboBox cmbHeight;
        private System.Windows.Forms.GroupBox grpExperience;
        private System.Windows.Forms.CheckBox chkPinConnectionBar;
        private System.Windows.Forms.CheckBox chkDisplayConnectionBar;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.LinkLabel lbCredentialSave;
        private System.Windows.Forms.LinkLabel lbCredentialRemove;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lbCredentials;
        private System.Windows.Forms.ComboBox cmbHostname;
        private System.Windows.Forms.ComboBox cmbUsername;
    }
}
