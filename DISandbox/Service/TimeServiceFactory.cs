namespace DISandbox.Service
{
    // класс, который будет считывать конфиги
    // для того, чтобы переключать приложение, не меняя код
    public static class TimeServiceFactory
    {
        public static ITimeService CreateTimeService()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            // чтото считываем из конфига 'appsettings.json'
            string serviceType = config.GetSection("ServiceType").Value ?? "default";
            switch (serviceType)
            {
                case "UTC":
                    return new UTCTimeService();
                // если в конфиге поставить UTC
                // "ServiceType": "Local", -   "ServiceType": "UTC",
                case "Local":
                    string? timezone = config.GetSection("Timezone").Value;
                    if (timezone == null)
                    {
                        return new LocalTimeService();
                        // если из конфига убрать "Timezone": 4, то покажет время по дефолту
                    }
                    return new LocalTimeService(Convert.ToInt32(timezone));
                default:
                    throw new InvalidOperationException($"unknown service type");
            }
        }
    }
}
