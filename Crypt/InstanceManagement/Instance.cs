using Common.Dto;
using Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.InstanceManagement
{
    public class Instance//инстанс для сохранения данных при использовании второго сценария
    {
        KeyValuePair<RSAParameters,RSAParameters> keyProvider;//контейнер с ключами
        string _message;//оригинальный текст сообщения
        string _signedMessage;//подпись

        public Instance(string message,ICryptMaster cryptMaster)
        {
            keyProvider = cryptMaster.GetKeys();
            _message=message;
            _signedMessage = cryptMaster.SignContent(message, keyProvider.Value);//подпись сообщения
        }

        public MessageWithCertificate Content //преобразование в DTO для клиент-сервис взаимодействия
        { 
            get 
            {
                return new MessageWithCertificate()
                {
                    OriginalMessage = _message,
                    SignedMessage = _signedMessage,
                    PublicKey = keyProvider.Key.ToEncodedString()
                };
            } 
        }
    }
}
