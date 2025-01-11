using AirportDictionaryAsp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Service
{
    // класс для выполнения операций с авиакомпаниями - работает с EF
    public class CompanyService
    {
        // 'dbcontext' - это зависимость и она передается через конструктор
        // '_db' - обьект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;
        public CompanyService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 1 // получить список всех авиакомпаний - берем наши авиакомпании и в 'ToList()' их загоняем
        public async Task<List<Company>> ListAllAsync()
        {
            return await _db.Companies.ToListAsync();
            // ToListAsync() — это метод для получения списка объектов в асинхронном режиме в EFCore
        }

        // 2 // получить авиакомпанию по Id 
        public async Task<Company?> GetAsync(int id)
        {
            return await _db.Companies
                .FirstOrDefaultAsync(company => company.Id == id);
        }

        // 3 // получить список аэропортов, в которых присутствует заданная авиакомпания по id
        public async Task<List<Airport>?> GetAirports(int id)
        {
            Company? company = await _db.Companies.Include(c => c.Airports).FirstOrDefaultAsync(c => c.Id == id);
            if (company==null)
            {
                return null;
            }
            return company!.Airports!.ToList();
        }

        // 4 // добавление авиакомпании 
        // должны быть заполнены поля авикомпании, кроме id
        public async Task AddCompanyAsync(Company company)
        {
            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();
        }

        // 5 // удаление авиакомпании
        public async Task DeleteAsync(int id)
        {
            Company? company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company != null)
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
            }
        }

        // дополнительные методы

        // узнать, существует ли такая компания по имени
        public async Task<bool> IsExists(string name)
        {
            // если нашлось, то 'count' будет > 1
            return await _db.Companies.Where(c => c.Name == name).CountAsync() > 0;
        }

        // узнать, существует ли такая компания по айди
        public async Task<bool> IsExists(int id)
        {
            // если нашлось, то 'count' будет > 1
            return await _db.Companies.Where(c => c.Id == id).CountAsync() > 0;
        }
    }
}
