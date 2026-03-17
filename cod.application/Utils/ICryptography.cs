using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Utils
{
    public interface ICryptography
    {
        string EncryptString(string key, string plainText);
        string DecryptString(string key, string cipherText);
    }
}
