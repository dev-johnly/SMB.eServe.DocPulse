using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMB.eServe.DocPulse.Models
{
    public class ConversionSettings
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public string FailedFolder { get; set; }
        public bool UseSftp { get; set; }
        public int MaxFailureAttempts { get; set; } = 3;


        
        public string SftpHost { get; set; }
        public int SftpPort { get; set; } = 22;
        public string SftpUsername { get; set; }
        public string SftpPassword { get; set; } 
        public string SftpRemotePath { get; set; } = "/";
    }
}
