using UserChallange.Model.Service;

namespace UserChallange.Model.Crypto
{
    // сервис // имплементация // отдельная логика, скрытая под интерфейсом
    public class BCryptEncoder : IEncoder
    {
        public string Encode(string data)
        {
            data = BCrypt.Net.BCrypt.HashPassword(data);
            return data;
        }
    }
}
