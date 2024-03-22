using Common.Dto;
using Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InstanceManagement
{
    public interface IInstanceManager//необходимость менеджера обусловлена возможностью сценария взаимодействия несмколльких пользователей с апи
    {
        public string Add();
        public int Count { get; }
        public MessageWithCertificate this[string id] { get; }
    }
}
