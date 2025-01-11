using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Model
{
    public class ApplicationDbContext: DbContext
    {
        // таблицы
        public required DbSet<Airports> Airports { get; set; }
        public required DbSet<Company> Companies { get; set; }
        public required DbSet<Country> Countries { get; set; }

        // конфигурирование
        // переопределение 'OnConfiguring'
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // достаем нашу строку подключения
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string useConnection = config.GetSection("UseConnection").Value ?? "DefaultConnection";
            string? connectionString = config.GetConnectionString(useConnection);
            optionsBuilder.UseSqlServer(connectionString);
        }

        // строка подключения находится в конфиге

        // Миграции
        // 'Tools' - 'Nuget Package Manager' - 'Package Manager Console'
        // 'Add-Migration Init' - папочка 'Mogration'  в корне создается
        // 'Remove-Migration' - для удаления миграции
        // 'Update-Database' - применение миграции
    }
}
