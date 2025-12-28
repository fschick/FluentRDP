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
            grpKeyboard = new System.Windows.Forms.GroupBox();
            cmbKeyboardMode = new System.Windows.Forms.ComboBox();
            lblKeyboardMode = new System.Windows.Forms.Label();
            grpAudio = new System.Windows.Forms.GroupBox();
            cmbRedirectAudioCapture = new System.Windows.Forms.ComboBox();
            lblRedirectAudioCapture = new System.Windows.Forms.Label();
            cmbAudioMode = new System.Windows.Forms.ComboBox();
            lblAudioPlaybackMode = new System.Windows.Forms.Label();
            grpDrives = new System.Windows.Forms.GroupBox();
            txtRedirectDrives = new System.Windows.Forms.TextBox();
            lblRedirectDrives = new System.Windows.Forms.Label();
            grpServerAuthentication = new System.Windows.Forms.GroupBox();
            cmbAuthenticationLevel = new System.Windows.Forms.ComboBox();
            lblAuthenticationLevel = new System.Windows.Forms.Label();
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
            chkEnableCredSsp = new System.Windows.Forms.CheckBox();
            txtUsername = new System.Windows.Forms.TextBox();
            lblUsername = new System.Windows.Forms.Label();
            txtDomain = new System.Windows.Forms.TextBox();
            lblDomain = new System.Windows.Forms.Label();
            txtHostname = new System.Windows.Forms.TextBox();
            lblHostname = new System.Windows.Forms.Label();
            btnExportRdp = new System.Windows.Forms.Button();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            grpKeyboard.SuspendLayout();
            grpAudio.SuspendLayout();
            grpDrives.SuspendLayout();
            grpServerAuthentication.SuspendLayout();
            grpRedirection.SuspendLayout();
            grpDisplay.SuspendLayout();
            grpConnection.SuspendLayout();
            SuspendLayout();
            // 
            // grpKeyboard
            // 
            grpKeyboard.Controls.Add(cmbKeyboardMode);
            grpKeyboard.Controls.Add(lblKeyboardMode);
            grpKeyboard.Location = new System.Drawing.Point(332, 76);
            grpKeyboard.Name = "grpKeyboard";
            grpKeyboard.Size = new System.Drawing.Size(315, 56);
            grpKeyboard.TabIndex = 3;
            grpKeyboard.TabStop = false;
            grpKeyboard.Text = "Keyboard";
            // 
            // cmbKeyboardMode
            // 
            cmbKeyboardMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbKeyboardMode.FormattingEnabled = true;
            cmbKeyboardMode.Location = new System.Drawing.Point(113, 22);
            cmbKeyboardMode.Name = "cmbKeyboardMode";
            cmbKeyboardMode.Size = new System.Drawing.Size(190, 23);
            cmbKeyboardMode.TabIndex = 1;
            // 
            // lblKeyboardMode
            // 
            lblKeyboardMode.AutoSize = true;
            lblKeyboardMode.Location = new System.Drawing.Point(13, 25);
            lblKeyboardMode.Name = "lblKeyboardMode";
            lblKeyboardMode.Size = new System.Drawing.Size(85, 15);
            lblKeyboardMode.TabIndex = 0;
            lblKeyboardMode.Text = "Windows keys:";
            // 
            // grpAudio
            // 
            grpAudio.Controls.Add(cmbRedirectAudioCapture);
            grpAudio.Controls.Add(lblRedirectAudioCapture);
            grpAudio.Controls.Add(cmbAudioMode);
            grpAudio.Controls.Add(lblAudioPlaybackMode);
            grpAudio.Location = new System.Drawing.Point(332, 138);
            grpAudio.Name = "grpAudio";
            grpAudio.Size = new System.Drawing.Size(315, 86);
            grpAudio.TabIndex = 4;
            grpAudio.TabStop = false;
            grpAudio.Text = "Remote audio";
            // 
            // cmbRedirectAudioCapture
            // 
            cmbRedirectAudioCapture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRedirectAudioCapture.FormattingEnabled = true;
            cmbRedirectAudioCapture.Location = new System.Drawing.Point(113, 51);
            cmbRedirectAudioCapture.Name = "cmbRedirectAudioCapture";
            cmbRedirectAudioCapture.Size = new System.Drawing.Size(190, 23);
            cmbRedirectAudioCapture.TabIndex = 3;
            // 
            // lblRedirectAudioCapture
            // 
            lblRedirectAudioCapture.AutoSize = true;
            lblRedirectAudioCapture.Location = new System.Drawing.Point(13, 54);
            lblRedirectAudioCapture.Name = "lblRedirectAudioCapture";
            lblRedirectAudioCapture.Size = new System.Drawing.Size(73, 15);
            lblRedirectAudioCapture.TabIndex = 2;
            lblRedirectAudioCapture.Text = "Audio input:";
            // 
            // cmbAudioMode
            // 
            cmbAudioMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAudioMode.FormattingEnabled = true;
            cmbAudioMode.Location = new System.Drawing.Point(113, 22);
            cmbAudioMode.Name = "cmbAudioMode";
            cmbAudioMode.Size = new System.Drawing.Size(190, 23);
            cmbAudioMode.TabIndex = 1;
            // 
            // lblAudioPlaybackMode
            // 
            lblAudioPlaybackMode.AutoSize = true;
            lblAudioPlaybackMode.Location = new System.Drawing.Point(13, 25);
            lblAudioPlaybackMode.Name = "lblAudioPlaybackMode";
            lblAudioPlaybackMode.Size = new System.Drawing.Size(57, 15);
            lblAudioPlaybackMode.TabIndex = 0;
            lblAudioPlaybackMode.Text = "Playback:";
            // 
            // grpDrives
            // 
            grpDrives.Controls.Add(txtRedirectDrives);
            grpDrives.Controls.Add(lblRedirectDrives);
            grpDrives.Location = new System.Drawing.Point(332, 230);
            grpDrives.Name = "grpDrives";
            grpDrives.Size = new System.Drawing.Size(315, 56);
            grpDrives.TabIndex = 5;
            grpDrives.TabStop = false;
            grpDrives.Text = "Drives";
            // 
            // txtRedirectDrives
            // 
            txtRedirectDrives.Location = new System.Drawing.Point(113, 22);
            txtRedirectDrives.Name = "txtRedirectDrives";
            txtRedirectDrives.Size = new System.Drawing.Size(190, 23);
            txtRedirectDrives.TabIndex = 1;
            // 
            // lblRedirectDrives
            // 
            lblRedirectDrives.AutoSize = true;
            lblRedirectDrives.Location = new System.Drawing.Point(13, 25);
            lblRedirectDrives.Name = "lblRedirectDrives";
            lblRedirectDrives.Size = new System.Drawing.Size(72, 15);
            lblRedirectDrives.TabIndex = 0;
            lblRedirectDrives.Text = "Local drives:";
            // 
            // grpServerAuthentication
            // 
            grpServerAuthentication.Controls.Add(cmbAuthenticationLevel);
            grpServerAuthentication.Controls.Add(lblAuthenticationLevel);
            grpServerAuthentication.Location = new System.Drawing.Point(332, 12);
            grpServerAuthentication.Name = "grpServerAuthentication";
            grpServerAuthentication.Size = new System.Drawing.Size(315, 58);
            grpServerAuthentication.TabIndex = 2;
            grpServerAuthentication.TabStop = false;
            grpServerAuthentication.Text = "Server authentication";
            // 
            // cmbAuthenticationLevel
            // 
            cmbAuthenticationLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAuthenticationLevel.FormattingEnabled = true;
            cmbAuthenticationLevel.Location = new System.Drawing.Point(113, 22);
            cmbAuthenticationLevel.Name = "cmbAuthenticationLevel";
            cmbAuthenticationLevel.Size = new System.Drawing.Size(190, 23);
            cmbAuthenticationLevel.TabIndex = 1;
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
            // grpRedirection
            // 
            grpRedirection.Controls.Add(chkRedirectSmartCards);
            grpRedirection.Controls.Add(chkRedirectPrinters);
            grpRedirection.Controls.Add(chkRedirectClipboard);
            grpRedirection.Location = new System.Drawing.Point(332, 293);
            grpRedirection.Name = "grpRedirection";
            grpRedirection.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            grpRedirection.Size = new System.Drawing.Size(315, 105);
            grpRedirection.TabIndex = 6;
            grpRedirection.TabStop = false;
            grpRedirection.Text = "Redirection";
            // 
            // chkRedirectSmartCards
            // 
            chkRedirectSmartCards.AutoSize = true;
            chkRedirectSmartCards.Location = new System.Drawing.Point(13, 79);
            chkRedirectSmartCards.Name = "chkRedirectSmartCards";
            chkRedirectSmartCards.Size = new System.Drawing.Size(133, 19);
            chkRedirectSmartCards.TabIndex = 3;
            chkRedirectSmartCards.Text = "Redirect smart cards";
            chkRedirectSmartCards.UseVisualStyleBackColor = true;
            // 
            // chkRedirectPrinters
            // 
            chkRedirectPrinters.AutoSize = true;
            chkRedirectPrinters.Location = new System.Drawing.Point(13, 54);
            chkRedirectPrinters.Name = "chkRedirectPrinters";
            chkRedirectPrinters.Size = new System.Drawing.Size(112, 19);
            chkRedirectPrinters.TabIndex = 1;
            chkRedirectPrinters.Text = "Redirect printers";
            chkRedirectPrinters.UseVisualStyleBackColor = true;
            // 
            // chkRedirectClipboard
            // 
            chkRedirectClipboard.AutoSize = true;
            chkRedirectClipboard.Location = new System.Drawing.Point(13, 29);
            chkRedirectClipboard.Name = "chkRedirectClipboard";
            chkRedirectClipboard.Size = new System.Drawing.Size(122, 19);
            chkRedirectClipboard.TabIndex = 0;
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
            grpDisplay.Location = new System.Drawing.Point(12, 161);
            grpDisplay.Name = "grpDisplay";
            grpDisplay.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            grpDisplay.Size = new System.Drawing.Size(314, 237);
            grpDisplay.TabIndex = 1;
            grpDisplay.TabStop = false;
            grpDisplay.Text = "Display";
            // 
            // cmbHeight
            // 
            cmbHeight.FormattingEnabled = true;
            cmbHeight.Location = new System.Drawing.Point(218, 58);
            cmbHeight.Name = "cmbHeight";
            cmbHeight.Size = new System.Drawing.Size(85, 23);
            cmbHeight.TabIndex = 11;
            cmbHeight.TextChanged += cmbHeight_TextChanged;
            cmbHeight.KeyPress += cmbHeight_KeyPress;
            // 
            // lblSizing
            // 
            lblSizing.AutoSize = true;
            lblSizing.Location = new System.Drawing.Point(13, 89);
            lblSizing.Name = "lblSizing";
            lblSizing.Size = new System.Drawing.Size(78, 15);
            lblSizing.TabIndex = 16;
            lblSizing.Text = "Screen sizing:";
            // 
            // cmbWidth
            // 
            cmbWidth.FormattingEnabled = true;
            cmbWidth.Location = new System.Drawing.Point(113, 58);
            cmbWidth.Name = "cmbWidth";
            cmbWidth.Size = new System.Drawing.Size(85, 23);
            cmbWidth.TabIndex = 10;
            cmbWidth.TextChanged += cmbWidth_TextChanged;
            cmbWidth.KeyPress += cmbWidth_KeyPress;
            // 
            // chkEnableBitmapPersistence
            // 
            chkEnableBitmapPersistence.AutoSize = true;
            chkEnableBitmapPersistence.Location = new System.Drawing.Point(113, 195);
            chkEnableBitmapPersistence.Name = "chkEnableBitmapPersistence";
            chkEnableBitmapPersistence.Size = new System.Drawing.Size(164, 19);
            chkEnableBitmapPersistence.TabIndex = 13;
            chkEnableBitmapPersistence.Text = "Enable bitmap persistence";
            chkEnableBitmapPersistence.UseVisualStyleBackColor = true;
            // 
            // chkEnableCompression
            // 
            chkEnableCompression.AutoSize = true;
            chkEnableCompression.Location = new System.Drawing.Point(113, 170);
            chkEnableCompression.Name = "chkEnableCompression";
            chkEnableCompression.Size = new System.Drawing.Size(132, 19);
            chkEnableCompression.TabIndex = 12;
            chkEnableCompression.Text = "Enable compression";
            chkEnableCompression.UseVisualStyleBackColor = true;
            // 
            // lblScreenMode
            // 
            lblScreenMode.AutoSize = true;
            lblScreenMode.Location = new System.Drawing.Point(13, 32);
            lblScreenMode.Name = "lblScreenMode";
            lblScreenMode.Size = new System.Drawing.Size(79, 15);
            lblScreenMode.TabIndex = 0;
            lblScreenMode.Text = "Screen Mode:";
            // 
            // chkAutoResize
            // 
            chkAutoResize.AutoSize = true;
            chkAutoResize.Location = new System.Drawing.Point(113, 87);
            chkAutoResize.Name = "chkAutoResize";
            chkAutoResize.Size = new System.Drawing.Size(84, 19);
            chkAutoResize.TabIndex = 6;
            chkAutoResize.Text = "Auto resize";
            chkAutoResize.UseVisualStyleBackColor = true;
            chkAutoResize.CheckedChanged += chkAutoResize_CheckedChanged;
            // 
            // cmbScreenMode
            // 
            cmbScreenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbScreenMode.FormattingEnabled = true;
            cmbScreenMode.Location = new System.Drawing.Point(113, 29);
            cmbScreenMode.Name = "cmbScreenMode";
            cmbScreenMode.Size = new System.Drawing.Size(190, 23);
            cmbScreenMode.TabIndex = 1;
            // 
            // chkSmartSizing
            // 
            chkSmartSizing.AutoSize = true;
            chkSmartSizing.Location = new System.Drawing.Point(203, 87);
            chkSmartSizing.Name = "chkSmartSizing";
            chkSmartSizing.Size = new System.Drawing.Size(90, 19);
            chkSmartSizing.TabIndex = 7;
            chkSmartSizing.Text = "Smart sizing";
            chkSmartSizing.UseVisualStyleBackColor = true;
            chkSmartSizing.CheckedChanged += chkSmartSizing_CheckedChanged;
            // 
            // cmbScaleFactor
            // 
            cmbScaleFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbScaleFactor.FormattingEnabled = true;
            cmbScaleFactor.Location = new System.Drawing.Point(113, 112);
            cmbScaleFactor.Name = "cmbScaleFactor";
            cmbScaleFactor.Size = new System.Drawing.Size(190, 23);
            cmbScaleFactor.TabIndex = 9;
            // 
            // lblScaleFactor
            // 
            lblScaleFactor.AutoSize = true;
            lblScaleFactor.Location = new System.Drawing.Point(13, 115);
            lblScaleFactor.Name = "lblScaleFactor";
            lblScaleFactor.Size = new System.Drawing.Size(94, 15);
            lblScaleFactor.TabIndex = 8;
            lblScaleFactor.Text = "DPI Scale Factor:";
            // 
            // cmbColorDepth
            // 
            cmbColorDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbColorDepth.FormattingEnabled = true;
            cmbColorDepth.Location = new System.Drawing.Point(113, 141);
            cmbColorDepth.Name = "cmbColorDepth";
            cmbColorDepth.Size = new System.Drawing.Size(190, 23);
            cmbColorDepth.TabIndex = 11;
            // 
            // lblColorDepth
            // 
            lblColorDepth.AutoSize = true;
            lblColorDepth.Location = new System.Drawing.Point(13, 144);
            lblColorDepth.Name = "lblColorDepth";
            lblColorDepth.Size = new System.Drawing.Size(74, 15);
            lblColorDepth.TabIndex = 10;
            lblColorDepth.Text = "Color Depth:";
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Location = new System.Drawing.Point(202, 61);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new System.Drawing.Size(12, 15);
            lblHeight.TabIndex = 4;
            lblHeight.Text = "/";
            // 
            // lblWidthHeight
            // 
            lblWidthHeight.AutoSize = true;
            lblWidthHeight.Location = new System.Drawing.Point(13, 60);
            lblWidthHeight.Name = "lblWidthHeight";
            lblWidthHeight.Size = new System.Drawing.Size(89, 15);
            lblWidthHeight.TabIndex = 2;
            lblWidthHeight.Text = "Width / Height:";
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(chkEnableCredSsp);
            grpConnection.Controls.Add(txtUsername);
            grpConnection.Controls.Add(lblUsername);
            grpConnection.Controls.Add(txtDomain);
            grpConnection.Controls.Add(lblDomain);
            grpConnection.Controls.Add(txtHostname);
            grpConnection.Controls.Add(lblHostname);
            grpConnection.Location = new System.Drawing.Point(12, 12);
            grpConnection.Name = "grpConnection";
            grpConnection.Padding = new System.Windows.Forms.Padding(10, 10, 10, 8);
            grpConnection.Size = new System.Drawing.Size(314, 143);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "Connection";
            // 
            // chkEnableCredSsp
            // 
            chkEnableCredSsp.AutoSize = true;
            chkEnableCredSsp.Location = new System.Drawing.Point(13, 116);
            chkEnableCredSsp.Name = "chkEnableCredSsp";
            chkEnableCredSsp.Size = new System.Drawing.Size(108, 19);
            chkEnableCredSsp.TabIndex = 6;
            chkEnableCredSsp.Text = "Enable CredSSP";
            chkEnableCredSsp.UseVisualStyleBackColor = true;
            // 
            // txtUsername
            // 
            txtUsername.Location = new System.Drawing.Point(113, 58);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(190, 23);
            txtUsername.TabIndex = 3;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(13, 61);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(63, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username:";
            // 
            // txtDomain
            // 
            txtDomain.Location = new System.Drawing.Point(113, 87);
            txtDomain.Name = "txtDomain";
            txtDomain.Size = new System.Drawing.Size(190, 23);
            txtDomain.TabIndex = 5;
            // 
            // lblDomain
            // 
            lblDomain.AutoSize = true;
            lblDomain.Location = new System.Drawing.Point(13, 90);
            lblDomain.Name = "lblDomain";
            lblDomain.Size = new System.Drawing.Size(52, 15);
            lblDomain.TabIndex = 4;
            lblDomain.Text = "Domain:";
            // 
            // txtHostname
            // 
            txtHostname.Location = new System.Drawing.Point(113, 29);
            txtHostname.Name = "txtHostname";
            txtHostname.Size = new System.Drawing.Size(190, 23);
            txtHostname.TabIndex = 1;
            // 
            // lblHostname
            // 
            lblHostname.AutoSize = true;
            lblHostname.Location = new System.Drawing.Point(13, 32);
            lblHostname.Name = "lblHostname";
            lblHostname.Size = new System.Drawing.Size(65, 15);
            lblHostname.TabIndex = 0;
            lblHostname.Text = "Hostname:";
            // 
            // btnExportRdp
            // 
            btnExportRdp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnExportRdp.Location = new System.Drawing.Point(12, 411);
            btnExportRdp.Name = "btnExportRdp";
            btnExportRdp.Size = new System.Drawing.Size(116, 23);
            btnExportRdp.TabIndex = 9;
            btnExportRdp.Text = "Export to RDP...";
            btnExportRdp.UseVisualStyleBackColor = true;
            btnExportRdp.Click += btnExportRdp_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(491, 411);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(75, 23);
            btnOk.TabIndex = 7;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(572, 411);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // FormSettings
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(657, 446);
            Controls.Add(btnExportRdp);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(grpKeyboard);
            Controls.Add(grpAudio);
            Controls.Add(grpDrives);
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
            grpKeyboard.ResumeLayout(false);
            grpKeyboard.PerformLayout();
            grpAudio.ResumeLayout(false);
            grpAudio.PerformLayout();
            grpDrives.ResumeLayout(false);
            grpDrives.PerformLayout();
            grpServerAuthentication.ResumeLayout(false);
            grpServerAuthentication.PerformLayout();
            grpRedirection.ResumeLayout(false);
            grpRedirection.PerformLayout();
            grpDisplay.ResumeLayout(false);
            grpDisplay.PerformLayout();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRedirectAudioCapture;
        private System.Windows.Forms.Label lblRedirectAudioCapture;
        private System.Windows.Forms.GroupBox grpAudio;
        private System.Windows.Forms.ComboBox cmbAudioMode;
        private System.Windows.Forms.Label lblAudioPlaybackMode;
        private System.Windows.Forms.GroupBox grpKeyboard;
        private System.Windows.Forms.ComboBox cmbKeyboardMode;
        private System.Windows.Forms.Label lblKeyboardMode;
        private System.Windows.Forms.GroupBox grpDrives;
        private System.Windows.Forms.TextBox txtRedirectDrives;
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
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.TextBox txtHostname;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.Button btnExportRdp;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblSizing;
        private System.Windows.Forms.ComboBox cmbWidth;
        private System.Windows.Forms.ComboBox cmbHeight;
    }
}
