namespace JsonAPIEncryption.Encryption
{
    // класс, который будет считывать конфиги
    // для того, чтобы переключать приложение, не меняя код
    public static class EncoderFactory
    {
        public static IEncoder CreateEncoder()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            // чтото считываем из конфига 'appsettings.json'
            string encodingType = config.GetSection("EncodingType").Value ?? "default";
            switch (encodingType)
            {
                case "MD5":
                    return new MD5Encoder();
                // если в конфиге поставить "EncoderType": "MD5"
                case "BCrypt":
                    return new BCryptEncoder();
                case "Decrypt":
                    return new DecryptEncoder();
                default:
                    throw new InvalidOperationException($"unknown encoding type");
            }
        }
    }
}