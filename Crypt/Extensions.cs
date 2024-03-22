using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypt
{
    public static class Extensions
    {
        
        public static string ToEncodedString(this RSAParameters source)//передавать внешний ключ как несколько массивов байт отстой,поэтому переводим его в строку
        {
            var exp = source.Exponent;
            var modules = source.Modulus;
            var superArr = new byte[exp.Length + modules.Length + 2];
            superArr[0] = (byte)(exp.Length - 1);
            int i = 1;
            for (; i < exp.Length + 1; i++)
                superArr[i] = exp[i - 1];
            superArr[i] = (byte)(modules.Length - 1);
            i++;
            for (int j = 0; j < modules.Length; j++)
                superArr[i + j] = modules[j];
            return Convert.ToBase64String(superArr);
        }

        public static RSAParameters ToPublicKey(this string source)//перевод внешнего ключа из строки в читаемый бибилиотекой параметр
        {
            var sourceArray = Convert.FromBase64String(source);
            var result = new RSAParameters();
            result.Exponent = new byte[sourceArray[0]+1];
            int i = 1;
            for (; i < sourceArray[0] + 2; i++)
                result.Exponent[i - 1] = sourceArray[i];
            result.Modulus = new byte[sourceArray[i] + 1];
            i++;
            Console.WriteLine($"Счетчик исходного массива {i}");
            for (; i < sourceArray.Length; i++)
                result.Modulus[i - result.Exponent.Length - 2] = sourceArray[i];
            return result;
        }
    }
}
