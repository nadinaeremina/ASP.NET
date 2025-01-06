using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JsonAPIEncryption.Encryption
{
    // сервис // имплементация // отдельная логика, скрытая под интерфейсом
    public class MD5Encoder : IEncoder
    {
        public string AlgorithmName => "MD5";

        public string Encode(string data, int count)
        {
            //throw new NotImplementedException();
            using (MD5 md5Hash = MD5.Create())
            {
                StringBuilder sBuilder = new StringBuilder();
                do
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[]  hashBytes = md5Hash.ComputeHash(dataBytes);
                    foreach (byte b in hashBytes)
                    {
                        sBuilder.Append(b.ToString("x2"));
                    }
                    data = sBuilder.ToString();
                    if (count==1)
                    {
                        break;
                    }
                    count--;
                    sBuilder.Clear();
                } while (true);
                return data;
            }
        }
    }
}
