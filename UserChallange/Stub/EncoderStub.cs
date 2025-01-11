using System.Security.Cryptography;
using System.Text;
using UserController.Model.Service;

namespace UserController.Stub
{
    // EncoderStub - заглушка для IEncoder через md5
    public class EncoderStub : IEncoder
    {
        public string Encode(string data)
        {
            // Создаем экземпляр MD5
            using (MD5 md5Hash = MD5.Create())
            {
                // Преобразуем строку в массив байтов
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
                // Создаем строку из массива байтов
                StringBuilder sBuilder = new StringBuilder();
                // Преобразуем каждый байт в шестнадцатеричное представление
                for (int i = 0; i < bytes.Length; i++)
                {
                    sBuilder.Append(bytes[i].ToString("x2"));
                }
                // Возвращаем полученную строку
                return sBuilder.ToString();
            }
        }
    }
}
