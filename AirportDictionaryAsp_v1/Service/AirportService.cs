using AirportDictionaryAsp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Service
{
    // класс для выполнения операций с аэропортами - работает с EF
    public class AirportService
    {
        // 'dbcontext' - это зависимость и она передается через конструктор
        // '_db' - обьект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;
        public AirportService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 1 // получить список всех аэропортов - берем наши аэропорты и в 'ToList()' их загоняем
        public async Task<List<Airports>> ListAllAsync()
        {
            return await _db.Airports.ToListAsync();
            // ToListAsync() — это метод для получения списка объектов в асинхронном режиме в EFCore
        }

        // 2 // получить аэропорт по Id с выгрузкой информации о стране
        public async Task<Airports?> GetWithCountryAsync(int id)
        {
            return await _db.Airports
                // здесь выгружаются две таблицы, которые циклически ссылаютя друг на друга (аэропорт и страна)
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Id == id);
        }

        // 3 // получить аэропорт по коду
        // может быть 'nullable' - если не найден - сервис вернет 'null'
        public async Task<Airports?> GetWithCountryAsync(string code)
        {
            return await _db.Airports
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Code == code);
        }

        // 4 // добавление аэропорта
        // должны быть заполнены поля аэропорта, кроме id, а также countryId, навигационные св-ва не нужно писать
        public async Task AddAirportAsync(Airports airport)
        {
            await _db.Airports.AddAsync(airport);
            await _db.SaveChangesAsync();
        }

        // нужен для добавления аэропорта
        // узнать, существует ли такой код аэропортп
        public async Task<bool> IsExists(string code)
        {
            // если нашлось, то 'count' будет > 1
            return await _db.Airports.Where(a => a.Code == code).CountAsync() > 0;
        }

        // 5 // удаление аэропорта
        public async Task DeleteAsync(string code)
        {
            Airports? airport = await _db.Airports.FirstOrDefaultAsync(a => a.Code == code);
            if (airport != null)
            {
                _db.Airports.Remove(airport);
                await _db.SaveChangesAsync();
            }
        }

        // 6 // обновление среднегодового пассажиропотока аэропорта
        public async Task UpdateAsync(Airports airport, long Traffic)
        {
            airport.AnnualPassengerTraffic = Traffic;
            await _db.SaveChangesAsync();
        }

        // 7 // получить список авиакомпаний, присутствующих в аэропорте
        public async Task<List<Company>>? GetCompanies(int id)
        {
            Airports? airport = await _db.Airports.Include(a => a.Companies).FirstOrDefaultAsync(a => a.Id == id);
            return airport.Companies.ToList();
        }

        // 8 // добавление авиакомпании по айди в обслуживание аэропортом по айди
        public async Task AddCompanyByIdAsync(int idComp, int idAir)
        {
            Company? company = await _db.Companies.Include(c => c.Airports).FirstOrDefaultAsync(company => company.Id == idComp);
            Airports? airport = await _db.Airports.Include(a => a.Companies).FirstOrDefaultAsync(airport => airport.Id == idAir);
            if (company != null && airport != null)
            {
                airport.Companies.Add(company);
            }
            await _db.SaveChangesAsync();
        }

        // 9 // удаление авиакомпании по айди в обслуживание аэропортом по айди
        public async Task RemoveCompanyByIdAsync(int idComp, int idAir)
        {
            Company? company = await _db.Companies.Include(c => c.Airports).FirstOrDefaultAsync(company => company.Id == idComp);
            Airports? airport = await _db.Airports.Include(a => a.Companies).FirstOrDefaultAsync(airport => airport.Id == idAir);
            if (company != null && airport != null)
            {
                airport.Companies.Remove(company);
            }
            await _db.SaveChangesAsync();
        }

        // дополнительный метод
        public async Task<Airports?> GetWithCompanyAsync(int id)
        {
            return await _db.Airports
                // здесь выгружаются две таблицы, которые циклически ссылаютя друг на друга (аэропорт и страна)
                .Include(airport => airport.Companies)
                .FirstOrDefaultAsync(airport => airport.Id == id);
        }
    }
}
