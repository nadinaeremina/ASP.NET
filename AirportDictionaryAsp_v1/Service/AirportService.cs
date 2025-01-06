using AirportDictionaryAsp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Service
{
    // класс для выполнения операций с аэропортами
    public class AirportService
    {
        // 'dbcontext' - это зависимость и она передается через конструктор
        // '_db' - обьект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;
        public AirportService(ApplicationDbContext db)
        {
            _db = db;
        }

        // получить список всех аэропортов - берем наши аэропорты и в 'ToList()' их загоняем
        public async Task<List<Airport>> ListAllAsync()
        {
            return await _db.Airports.ToListAsync();
            // ToListAsync() — это метод для получения списка объектов в асинхронном режиме в EFCore
        }

        // получить аэропорт по Id с выгрузкой информации о стране
        public async Task<Airport?> GetAsync(int id)
        {
            return await _db.Airports
                // здесь выгружаются две таблицы, которые циклически ссылаютя друг на друга (аэропорт и страна)
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Id == id);
        }

        // получить аэропорт по коду
        // может быть 'nullable' - если не найден - сервис вернет 'null'
        public async Task<Airport?> GetAsync(string code)
        {
            return await _db.Airports
                .Include(airport => airport.Country)
                .FirstOrDefaultAsync(airport => airport.Code == code);
        }

        // добавление аэропорта
        // должны быть заполнены поля аэропорта, кроме id, а также countryId, навигационные св-ва не нужно писать
        public async Task AddAirportAsync(Airport airport)
        {
            await _db.Airports.AddAsync(airport);
            await _db.SaveChangesAsync();
        }

        // узнать, существует ли такой код аэропортп
        public async Task<bool> IsExists(string code)
        {
            // если нашлось, то 'count' будет > 1
            return await _db.Airports.Where(a => a.Code == code).CountAsync() > 0;
        }

        public async Task DeleteAsync(string code)
        {
            Airport? airport = await _db.Airports.FirstOrDefaultAsync(a => a.Code == code);
            if(airport != null)
            {
                _db.Airports.Remove(airport);
                await _db.SaveChangesAsync();
            }
        }
    }
}
