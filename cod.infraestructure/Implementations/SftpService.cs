using cod.application.Interfaces;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace cod.infraestructure.Implementations
{
    public class SftpService : ISftpService
    {
        private SftpClient _client;

        public SftpService()
        {
        }

        public void CreateNewInstance(string host, string username, string password, int port = 22)
        {
            _client?.Dispose();

            _client = new SftpClient("06269ae.wcomhost.com", port, username, password);

            _client.Connect();
        }

        public void UploadFileFromBytes(byte[] fileBytes, string remoteFilePath)
        {
            try
            {
                var client = _client;

                string remoteDirectory = Path.GetDirectoryName(remoteFilePath)?.Replace('\\', '/');

                // Verificar y crear el directorio si no existe
                if (!string.IsNullOrEmpty(remoteDirectory))
                {
                    string currentPath = "";
                    foreach (var dir in remoteDirectory.Split('/'))
                    {
                        if (string.IsNullOrEmpty(dir)) continue;

                        currentPath += "/" + dir;
                        try
                        {
                            if (!client.Exists(currentPath))
                            {
                                client.CreateDirectory(currentPath);
                            }
                        }
                        catch { } // Ignora errores si el directorio ya existe
                    }
                }

                using var stream = new MemoryStream(fileBytes);
                client.UploadFile(stream, remoteFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading bytes to {remoteFilePath}: {ex.Message}", ex);
            }
        }

    }
}
