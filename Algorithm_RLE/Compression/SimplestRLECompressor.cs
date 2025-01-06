using Algorithm_RLE.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonAPI.Compression
{
    // примитивнейший сжиматель RLE - это по сути сервис // имплементация
    // теперь у нас есть отдельная логика, причем она скрыта под интерфейсом
    public class SimplestRLECompressor : ICompressor
    {
        // непустая строка только из маленьких латинских букв // статическое поле, чтобы каждый раз его не создавать
        private static readonly Regex validDataMatcher = new Regex("^[a-z]+$");

        // св-ва интерфейсов необходимо переопределять
        public string AlgorithmName => "Simplest RLE";

        public string Compress(string data)
        {
            if (!validDataMatcher.IsMatch(data))
            {
                // выбросить исключение в случае невалидных данных
                throw new InvalidCompressionDataException("only english lowercase letters are allowed");
            }
            // сжатие
            char symb = data[0];
            int count = 1;
            StringBuilder sb = new StringBuilder();
            // 'StringBuilder' - предназначен для модификации строк и использует более эффективные методы для их объединения
            // используем его, чтобы в цикле не было никаких конкатенаций
            for (int i = 1; i < data.Length; i++)
            {
                if (symb == data[i])
                {
                    count++;
                    continue;
                }
                if (count > 1)
                {
                    sb.Append(count);
                    // 'append' - преобразовывает переданный объект в строку и добавляет к текущей строке
                }
                sb.Append(symb);
                symb = data[i];
                count = 1;
            }
            if (count > 1)
            {
                sb.Append(count);
            }
            sb.Append(symb);
            return sb.ToString();
        }
    }
}
