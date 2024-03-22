using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Common.TextGeneration
{
    public class TextGenerator:ITextGenerator
    {
        string _ApiKey;
        public TextGenerator(string key)
        {
            _ApiKey = key;
        }

        public string GenerateText()//реализация генерации текстов через внешнее апи с анекдотами
        {
            var result = "error with JokeApi";
            var client = new HttpClient();
            try
            {
                //client.BaseAddress = new Uri();
                var content = client.GetAsync("http://rzhunemogu.ru/Rand.aspx?CType=1").Result.Content.ReadAsStream();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var sr = new StreamReader(content, Encoding.GetEncoding(1251)))//приколы с неподдерживаемыми кодировками
                {
                    var text = sr.ReadToEnd();
                    result = XElement.Parse(text).Descendants("content").First().Value;//апи отдает данные в xml
                }
            }
            finally
            {
                client.Dispose();
                
            }
            return result;
        }
    }
}
