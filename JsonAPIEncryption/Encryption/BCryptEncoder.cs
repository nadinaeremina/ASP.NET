namespace JsonAPIEncryption.Encryption
{
    // сервис // имплементация // отдельная логика, скрытая под интерфейсом
    public class BCryptEncoder : IEncoder
    {
        public string AlgorithmName => "BCrypt";

        public string Encode(string data, int count)
        {
            //throw new NotImplementedException();
            do
            {
                data = BCrypt.Net.BCrypt.HashPassword(data);
                if (count == 1)
                {
                    break;
                }
                count--;
            } while (true);
            return data;
        }
    }
}
