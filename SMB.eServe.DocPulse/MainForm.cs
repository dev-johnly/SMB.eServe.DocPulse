using SMB.eServe.DocPulse.Models;
using SMB.eServe.DocPulse.Services;
using SMB.eServe.DocPulse.ViewModels;
using System;
using System.IO;
using System.Windows.Forms;

namespace SMB.eServe.DocPulse
{
    public partial class MainForm : Form
    {
        private ConversionSettings _settings;
        private readonly PurchaseOrderViewModel _viewModel = new();


        private TextBox txtSource;
        private TextBox txtDest;
        private TextBox txtFailed;
        private RadioButton rdoSftp;
        private RadioButton rdoShared;
        private NumericUpDown numAttempts;
        private TextBox txtSftpHost;
        private NumericUpDown txtSftpPort;
        private TextBox txtSftpUser;
        private TextBox txtSftpPass;
        private TextBox txtSftpRemotePath;


        public MainForm()
        {
            InitializeComponent();
            SetupUI(
                );
            Load += MainForm_Load;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            _settings = SettingsManager.Load();

            txtSource.Text = _settings.SourceFolder;
            txtDest.Text = _settings.DestinationFolder;
            txtFailed.Text = _settings.FailedFolder;
            rdoSftp.Checked = _settings.UseSftp;
            rdoShared.Checked = !_settings.UseSftp;
            numAttempts.Value = _settings.MaxFailureAttempts;

            txtSftpHost.Text = _settings.SftpHost;
            txtSftpPort.Value = _settings.SftpPort;
            txtSftpUser.Text = _settings.SftpUsername;
            txtSftpPass.Text = _settings.SftpPassword;
            txtSftpRemotePath.Text = _settings.SftpRemotePath;

            _viewModel.ApplySettings(_settings);
        }

        private void BrowseFolder(TextBox target)
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                target.Text = dialog.SelectedPath;
            }
        }

        private void SaveSettings(
            string source, string destination, string failed,
            bool useSftp, int maxAttempts,
            string sftpHost, int sftpPort, string sftpUser,
            string sftpPass, string sftpRemotePath)
        {
            _settings = new ConversionSettings
            {
                SourceFolder = source,
                DestinationFolder = destination,
                FailedFolder = failed,
                UseSftp = useSftp,
                MaxFailureAttempts = maxAttempts,
                SftpHost = sftpHost,
                SftpPort = sftpPort,
                SftpUsername = sftpUser,
                SftpPassword = sftpPass,
                SftpRemotePath = sftpRemotePath
            };

            SettingsManager.Save(_settings);
            _viewModel.ApplySettings(_settings);

            MessageBox.Show("Settings saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
