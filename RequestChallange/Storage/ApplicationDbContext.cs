using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RequestChallange.Storage
{
    public class ApplicationDbContext: DbContext
    {
        public required DbSet<RequestData> Requests { get; set; }

        // переопределение метода конфигурации
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string? connectionString = conf.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    // есть 'dbContext' - можно сделать миграции
    // Add-Migration Init -OutputDir "./Storage/Migtations" - чтобы миграции были в папочке 'Storage'
    // 'Remove-Migration' - удалить миграции
}
