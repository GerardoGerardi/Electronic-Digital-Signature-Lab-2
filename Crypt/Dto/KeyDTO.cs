using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class KeyDTO//как оказалось передача ключа в url плохая идея, поэтому исполльзуем DTO
    {
        public string Key { get; set; }
    }
}
