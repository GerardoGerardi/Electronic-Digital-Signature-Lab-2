using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TextGeneration
{
    public interface ITextGenerator//интерфейс генератора случайных текстов
    {
        string GenerateText();
    }
}
