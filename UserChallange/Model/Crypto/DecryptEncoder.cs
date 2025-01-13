using UserChallange.Model.Service;

namespace UserChallange.Model.Crypto
{
    // в этом методе кодирования - если кодировать уже закодированное этим методом слово
    // - оно возвращается в свой первоначальный вид
    public class DecryptEncoder : IEncoder
    {
        public string Encode(string data)
        {
            // Секретный ключ (длина - 16 bit)
            ushort secretKey = 0x0088;
            char[] mas_char;
            string result = "";
            mas_char = data.ToArray();
            foreach (var el in mas_char)
            {
                //производим шифрование каждого отдельного элемента и сохраняем его в строку
                result += TopSecret(el, secretKey);
            }
            return result;
        }
        public static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey); //Производим XOR операцию
            return character;
        }
    }
}
