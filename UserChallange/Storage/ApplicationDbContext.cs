using Microsoft.EntityFrameworkCore;
using UserChallange.Model.Users;

namespace UserChallange.Storage
{
    public class ApplicationDbContext: DbContext
    {
        public required DbSet<User> Users { get; set; }

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
}