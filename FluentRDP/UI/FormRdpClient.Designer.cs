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
            panelStatus = new Panel();
            lblStatusMessage = new Label();
            btnConnect = new Button();
            btnSettings = new Button();
            panelStatus.SuspendLayout();
            SuspendLayout();
            // 
            // panelRdp
            // 
            panelRdp.Dock = DockStyle.Fill;
            panelRdp.Location = new Point(0, 0);
            panelRdp.Name = "panelRdp";
            panelRdp.Size = new Size(964, 511);
            panelRdp.TabIndex = 0;
            // 
            // panelStatus
            // 
            panelStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelStatus.Controls.Add(lblStatusMessage);
            panelStatus.Controls.Add(btnConnect);
            panelStatus.Controls.Add(btnSettings);
            panelStatus.Location = new Point(50, 50);
            panelStatus.Name = "panelStatus";
            panelStatus.Size = new Size(864, 411);
            panelStatus.TabIndex = 1;
            // 
            // lblStatusMessage
            // 
            lblStatusMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatusMessage.Font = new Font("Segoe UI", 10F);
            lblStatusMessage.ForeColor = Color.DarkRed;
            lblStatusMessage.Location = new Point(152, 325);
            lblStatusMessage.Name = "lblStatusMessage";
            lblStatusMessage.Size = new Size(560, 80);
            lblStatusMessage.TabIndex = 2;
            lblStatusMessage.TextAlign = ContentAlignment.BottomCenter;
            lblStatusMessage.Visible = false;
            // 
            // btnConnect
            // 
            btnConnect.Anchor = AnchorStyles.None;
            btnConnect.Font = new Font("Segoe UI", 14F);
            btnConnect.Location = new Point(332, 144);
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
            btnSettings.Location = new Point(332, 214);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(200, 50);
            btnSettings.TabIndex = 1;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // FormRdpClient
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 511);
            Controls.Add(panelStatus);
            Controls.Add(panelRdp);
            Name = "FormRdpClient";
            Text = "FluentRDP";
            FormClosing += FormRdpClient_FormClosing;
            Shown += FormRdpClient_Shown;
            panelStatus.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelRdp;
        private Panel panelStatus;
        private Label lblStatusMessage;
        private Button btnConnect;
        private Button btnSettings;
    }
}
