using Renci.SshNet;

namespace SMB.eServe.DocPulse.Services
{
    public class SftpService
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;

        public SftpService(string host, string username, string password, int port = 22)
        {
            _host = host;
            _username = username;
            _password = password;
            _port = port;
        }

        public void UploadFile(string localPath, string remotePath)
        {
            using var client = new SftpClient(_host, _port, _username, _password);
            client.Connect();
            using var stream = File.OpenRead(localPath);
            client.UploadFile(stream, remotePath, true);
            client.Disconnect();
        }
    }

}
