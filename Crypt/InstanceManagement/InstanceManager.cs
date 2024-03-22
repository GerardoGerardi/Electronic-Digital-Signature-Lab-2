using Common.Dto;
using Common.TextGeneration;
using Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InstanceManagement
{
    public class InstanceManager:IInstanceManager
    {
        //при обьращении пользователя создается сеанс с парой ключей и подписанным текстом
        Dictionary<string, Instance> _instances;//словарь с инстансами пользователей    
        ICryptMaster _cryptMaster;//библиотека для подписи и сверки сообщений RSA+SHA256
        ITextGenerator _textGenerator;//случайный генератор текста

        public InstanceManager(ICryptMaster cryptMaster, ITextGenerator textGenerator)
        {
            _cryptMaster = cryptMaster;
            _textGenerator = textGenerator;
            _instances = new Dictionary<string, Instance>();
        }

        public string Add()//добавление инстанса при обращении пользователя 
        {
            var inst = new Instance(_textGenerator.GenerateText(), _cryptMaster);
            _instances.Add(inst.Content.PublicKey, inst);
            return inst.Content.PublicKey;
        }
        public int Count { get => _instances.Count; }
        public MessageWithCertificate this[string id]//получение конкретного инстанса полльзователя по ключу
        {
            get
            {

                if (_instances.TryGetValue(id, out var content))
                    return content.Content;
                else
                    return null;
            }
        }


    }
}
