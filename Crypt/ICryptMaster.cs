using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypt
{
    public interface ICryptMaster//интерфейс для работы с RSA шифрованием
    {
        string SignContent(string content, RSAParameters privateKey);
        bool VerifyData(string content, string signedContent, RSAParameters publicKey);
        public KeyValuePair<RSAParameters, RSAParameters> GetKeys();
    }
}
