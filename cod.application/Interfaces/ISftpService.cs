using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface ISftpService
    {
        void CreateNewInstance(string host, string username, string password, int port = 22);
        void UploadFileFromBytes(byte[] fileBytes, string remoteFilePath);
    }
}
