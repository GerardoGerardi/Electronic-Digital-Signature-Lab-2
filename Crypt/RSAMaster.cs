using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Crypt
{
    public class RSAMaster:ICryptMaster
    { 
        public string SignContent(string content,RSAParameters privateKey)//подпись сообщения
        {
           
            var byteContent = new UTF8Encoding().GetBytes(content);
            byte[] signedBytes;
            using(var provider = new RSACryptoServiceProvider())
            try
            {
                provider.ImportParameters(privateKey);
                signedBytes = provider.SignData(byteContent, CryptoConfig.MapNameToOID("SHA256"));
            }
            catch(Exception ex)
            {
                    Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                provider.PersistKeyInCsp = false;
            }
            return Convert.ToBase64String(signedBytes);
        }
        public bool VerifyData(string content,string signedContent, RSAParameters publicKey)//сверка сообщения
        {
           
            var encoder = new UTF8Encoding();
            var byteContent = encoder.GetBytes(content);
            var byteSignedContent = Convert.FromBase64String(signedContent);
            bool success = false;
            using (var provider = new RSACryptoServiceProvider())
            {
                try
                {
                    provider.ImportParameters(publicKey);
                    success = provider.VerifyData(byteContent, CryptoConfig.MapNameToOID("SHA256"), byteSignedContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    provider.PersistKeyInCsp = false;
                }
            }
            return success;
        }
        public KeyValuePair<RSAParameters, RSAParameters> GetKeys()//генерация ключа
        {
            var keyProvider = RSA.Create();
            return new KeyValuePair<RSAParameters, RSAParameters>(keyProvider.ExportParameters(false), keyProvider.ExportParameters(true));
            
        }



    }
}