using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JsonAPIEncryption.Messages
{
    // обьект со строковыми данными
    public class StringData
    {
        // строка данных для шифрования
        public string Data { get; set; } = string.Empty;
        // кол-во прогонов данных через алгоритм шифрования, по умолчанию 1 (если не передается)
        private int count = 1;
        public int Count
        {
            get { return count; }
            set
            {
                // Добавляем валидацию: кол-во прогонов должно быть не меньше 1
                if (value < 1)
                {
                    throw new ArgumentException("Count must be more than 0");
                }
                count = value;
            }
        }
    }
}
