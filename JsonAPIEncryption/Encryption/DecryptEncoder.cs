﻿namespace JsonAPIEncryption.Encryption
{
    // в этом методе кодирования - если кодировать уже закодированное этим методом слово
    // - оно возвращается в свой первоначальный вид
    public class DecryptEncoder : IEncoder
    {
        public string AlgorithmName => "Decrypt";

        public string Encode(string data, int count)
        {
            //throw new NotImplementedException();
            // Секретный ключ (длина - 16 bit)
            ushort secretKey = 0x0088;
            char[] mas_char;
            string result = "";      
            do
            {
                mas_char = data.ToArray();
                foreach (var el in mas_char)  
                {
                    //производим шифрование каждого отдельного элемента и сохраняем его в строку
                    result += TopSecret(el, secretKey);  
                }
                data = result;
                if (count == 1)
                {
                    break;
                }
                count--;
                result = "";
            } while (true);
            return data;
        }
        public static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey); //Производим XOR операцию
            return character;
        }
    }
}