using Org.BouncyCastle.Asn1.Cms;

namespace JsonAPIEncryption.Messages
{
    // сообщение результата сжатия
    public class EncodingResult
    {
        // строка зашифрованных данных
        public string Encoded { get; set; } = string.Empty;
        // название алгоритма которым были зашифрованы данные
        public string Strategy { get; set; } = string.Empty;
        // кол-во прогонов через алгоритм шифрования
        public int Count { get; set; }
        public EncodingResult(string encoded, string strategy, int count) 
        {
            Encoded = encoded;
            Strategy = strategy;
            Count = count;
        }
    }
}