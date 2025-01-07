using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RAMList.Model
{
    // класс-менеджер для работы с БД через EF
    public class ApplicationDbContext : DbContext
    {
        public required DbSet<RAM> RAMS { get; set; }
        // 'required' - новый модификатор, который требуется для ссылочных nullable-полей,
        // здесь точно должно быть заинициализировано значение

        // переопределение метода
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"
                Data Source=NADI\SQLEXPRESS;
                Initial Catalog=RAMS_db_pv324;
                Integrated Security=SSPI;
                Timeout=10;
                TrustServerCertificate=True;
            ";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
