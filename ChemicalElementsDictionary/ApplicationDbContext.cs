﻿using Microsoft.EntityFrameworkCore;

namespace ChemicalElementsDictionary
{
    // класс-менеджер для работы с БД через EF
    public class ApplicationDbContext: DbContext
    {
        public required DbSet<Element> Elements { get; set; }
        // 'required' - новый модификатор, который требуется для ссылочных nullable-полей,
        // здесь точно должно быть заинициализировано значение

        // переопределение метода
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"
                Data Source=NADI\SQLEXPRESS;
                Initial Catalog=elements_db_pv324;
                Integrated Security=SSPI;
                Timeout=10;
                TrustServerCertificate=True;
            ";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

// Миграции
// 'Tools' - 'Nuget Package Manager' - 'Package Manager Console'
// 'Add-Migration Init' - папочка 'Mogration'  в корне создается
// 'Remove-Migration' - для удаления миграции
// 'Update-Database' - применение миграции
