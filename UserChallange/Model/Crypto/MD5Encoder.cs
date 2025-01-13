using System.Security.Cryptography;
using System.Text;
using UserChallange.Model.Service;

namespace UserChallange.Model.Crypto
{
    // сервис // имплементация // отдельная логика, скрытая под интерфейсом
    public class MD5Encoder : IEncoder
    {
        public string Encode(string data)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                StringBuilder sBuilder = new StringBuilder();

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = md5Hash.ComputeHash(dataBytes);
                foreach (byte b in hashBytes)
                {
                    sBuilder.Append(b.ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
