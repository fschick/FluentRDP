using System.Drawing;
using System.Windows.Forms;

namespace FluentRDP.UI
{
    partial class FormRdpClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelRdp = new Panel();
            lbRdp = new Label();
            panelStartup = new Panel();
            lblStatusMessage = new TextBox();
            btnConnect = new Button();
            btnSettings = new Button();
            panelConnect = new Panel();
            btnCancel = new Button();
            lbConnecting = new Label();
            panelRdp.SuspendLayout();
            panelStartup.SuspendLayout();
            panelConnect.SuspendLayout();
            SuspendLayout();
            // 
            // panelRdp
            // 
            panelRdp.Controls.Add(lbRdp);
            panelRdp.Dock = DockStyle.Fill;
            panelRdp.Location = new Point(0, 0);
            panelRdp.Name = "panelRdp";
            panelRdp.Size = new Size(964, 511);
            panelRdp.TabIndex = 0;
            // 
            // lbRdp
            // 
            lbRdp.Anchor = AnchorStyles.Bottom;
            lbRdp.Font = new Font("Segoe UI", 10F);
            lbRdp.Location = new Point(350, 227);
            lbRdp.Name = "lbRdp";
            lbRdp.Size = new Size(264, 80);
            lbRdp.TabIndex = 4;
            lbRdp.Text = "RDP client";
            lbRdp.TextAlign = ContentAlignment.MiddleCenter;
            lbRdp.Visible = false;
            // 
            // panelStartup
            // 
            panelStartup.Controls.Add(lblStatusMessage);
            panelStartup.Controls.Add(btnConnect);
            panelStartup.Controls.Add(btnSettings);
            panelStartup.Dock = DockStyle.Fill;
            panelStartup.Location = new Point(0, 0);
            panelStartup.Name = "panelStartup";
            panelStartup.Size = new Size(964, 511);
            panelStartup.TabIndex = 1;
            // 
            // lblStatusMessage
            // 
            lblStatusMessage.Anchor = AnchorStyles.None;
            lblStatusMessage.BackColor = SystemColors.Control;
            lblStatusMessage.BorderStyle = BorderStyle.None;
            lblStatusMessage.Cursor = Cursors.IBeam;
            lblStatusMessage.Font = new Font("Segoe UI", 10F);
            lblStatusMessage.ForeColor = Color.DarkRed;
            lblStatusMessage.Location = new Point(202, 367);
            lblStatusMessage.Multiline = true;
            lblStatusMessage.Name = "lblStatusMessage";
            lblStatusMessage.ReadOnly = true;
            lblStatusMessage.Size = new Size(560, 106);
            lblStatusMessage.TabIndex = 2;
            lblStatusMessage.Text = "Error code / extended error code, Line 1\r\n\r\nError Message, Line 3\r\nError Message, Line 4\r\nError Message, Line 5\r\nError Message, Line 6";
            lblStatusMessage.TextAlign = HorizontalAlignment.Center;
            lblStatusMessage.Visible = false;
            // 
            // btnConnect
            // 
            btnConnect.Anchor = AnchorStyles.None;
            btnConnect.Font = new Font("Segoe UI", 14F);
            btnConnect.Location = new Point(382, 194);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(200, 50);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.None;
            btnSettings.Font = new Font("Segoe UI", 14F);
            btnSettings.Location = new Point(382, 264);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(200, 50);
            btnSettings.TabIndex = 1;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // panelConnect
            // 
            panelConnect.Controls.Add(btnCancel);
            panelConnect.Controls.Add(lbConnecting);
            panelConnect.Dock = DockStyle.Fill;
            panelConnect.Location = new Point(0, 0);
            panelConnect.Name = "panelConnect";
            panelConnect.Size = new Size(964, 511);
            panelConnect.TabIndex = 1;
            panelConnect.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.Font = new Font("Segoe UI", 14F);
            btnCancel.Location = new Point(382, 275);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(200, 50);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // lbConnecting
            // 
            lbConnecting.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lbConnecting.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbConnecting.Location = new Point(12, 208);
            lbConnecting.Name = "lbConnecting";
            lbConnecting.Size = new Size(940, 53);
            lbConnecting.TabIndex = 3;
            lbConnecting.Text = "Connecting...";
            lbConnecting.TextAlign = ContentAlignment.MiddleCenter;
            lbConnecting.Visible = false;
            // 
            // FormRdpClient
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 511);
            Controls.Add(panelStartup);
            Controls.Add(panelConnect);
            Controls.Add(panelRdp);
            Name = "FormRdpClient";
            Text = "FluentRDP";
            FormClosing += FormRdpClient_FormClosing;
            Shown += FormRdpClient_Shown;
            panelRdp.ResumeLayout(false);
            panelStartup.ResumeLayout(false);
            panelStartup.PerformLayout();
            panelConnect.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelRdp;
        private Panel panelStartup;
        private TextBox lblStatusMessage;
        private Button btnConnect;
        private Button btnSettings;
        private Panel panelConnect;
        private Label lbConnecting;
        private Label lbRdp;
        private Button btnCancel;
    }
}
