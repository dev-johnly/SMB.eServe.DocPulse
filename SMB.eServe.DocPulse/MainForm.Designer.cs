namespace SMB.eServe.DocPulse
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = "DocPulse Configuration";
        }


        private void SetupUI()
        {
            var tabControl = new TabControl
            {
                Location = new Point(10, 10),
                Size = new Size(960, 540)
            };

            
            var sftpPanel = new Panel
            {
                Location = new Point(10, 200),
                Size = new Size(920, 140),
                BorderStyle = BorderStyle.None
            };

           

            var tabGeneral = new TabPage("General Configuration");

           
            var lblSource = new Label { Text = "Source Folder:", Location = new Point(10, 20) };
            txtSource = new TextBox { Width = 600, Location = new Point(130, 18) };
            var btnSource = new Button { Text = "Browse", Location = new Point(740, 16) };
            btnSource.Click += (s, e) => BrowseFolder(txtSource);

            var lblDest = new Label { Text = "Destination Folder:", Location = new Point(10, 60) };
            txtDest = new TextBox { Width = 600, Location = new Point(130, 58) };
            var btnDest = new Button { Text = "Browse", Location = new Point(740, 56) };
            btnDest.Click += (s, e) => BrowseFolder(txtDest);

            rdoShared = new RadioButton { Text = "Shared Folder", Location = new Point(130, 90), Checked = true };
            rdoSftp = new RadioButton { Text = "SFTP", Location = new Point(250, 90) };

            var lblFailed = new Label { Text = "Failed Files Folder:", Location = new Point(10, 130) };
            txtFailed = new TextBox { Width = 600, Location = new Point(130, 128) };
            var btnFailed = new Button { Text = "Browse", Location = new Point(740, 126) };
            btnFailed.Click += (s, e) => BrowseFolder(txtFailed);

            var lblAttempts = new Label { Text = "Max Failed Attempts:", Location = new Point(10, 170) };
            numAttempts = new NumericUpDown { Location = new Point(130, 168), Minimum = 1, Maximum = 10, Value = 3 };

            
            var lblSftpHost = new Label { Text = "SFTP Host:", Location = new Point(0, 10) };
            txtSftpHost = new TextBox { Width = 300, Location = new Point(120, 8) };


            var lblSftpPort = new Label { Text = "SFTP Port:", Location = new Point(440, 10) };
            txtSftpPort = new NumericUpDown
            {
                Location = new Point(550, 8),
                Minimum = 1,
                Maximum = 65535,
                Value = 22,
                Width = 80
            };

            var lblSftpUser = new Label { Text = "SFTP Username:", Location = new Point(0, 45) };
            txtSftpUser = new TextBox { Width = 300, Location = new Point(120, 43) };


            var lblSftpPass = new Label { Text = "SFTP Password:", Location = new Point(440, 45) };
            txtSftpPass = new TextBox { Width = 200, Location = new Point(550, 43), PasswordChar = '*' };


            var lblSftpRemote = new Label { Text = "Remote Path:", Location = new Point(0, 80) };
            txtSftpRemotePath = new TextBox { Width = 300, Location = new Point(120, 78) };


            sftpPanel.Controls.AddRange(new Control[]
            {
                lblSftpHost, txtSftpHost,
                lblSftpPort, txtSftpPort,
                lblSftpUser, txtSftpUser,
                lblSftpPass, txtSftpPass,
                lblSftpRemote, txtSftpRemotePath
            });

           

            var btnSave = new Button { Text = "Save Settings", Location = new Point(10, 340) };
            btnSave.Click += (s, e) => SaveSettings(
                txtSource.Text, txtDest.Text, txtFailed.Text, rdoSftp.Checked,
                (int)numAttempts.Value,
                txtSftpHost.Text, (int)txtSftpPort.Value, txtSftpUser.Text,
                txtSftpPass.Text, txtSftpRemotePath.Text
            );

            tabGeneral.Controls.AddRange(new Control[]
            {
                lblSource, txtSource, btnSource,
                lblDest, txtDest, btnDest,
                rdoShared, rdoSftp,
                lblFailed, txtFailed, btnFailed,
                lblAttempts, numAttempts,
                sftpPanel,  
                btnSave
            });

            tabControl.TabPages.Add(tabGeneral);
            this.Controls.Add(tabControl);

            rdoSftp.CheckedChanged += (s, e) => sftpPanel.Visible = rdoSftp.Checked;
            sftpPanel.Visible = rdoSftp.Checked; 
        }




        #endregion
    }
}
