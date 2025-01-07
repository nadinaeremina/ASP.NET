using AirportDictionaryAsp_v1.Model;
using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Service
{
    // класс для выполнения операций со странами, ему нужен 'dbcontext' - работает с EF
    public class CountryService
    {
        // 'dbcontext' - это зависимость и она передается через конструктор
        // '_db' - обьект для доступа в БД для выполнения операций
        private readonly ApplicationDbContext _db;
        public CountryService(ApplicationDbContext db)
        {
            _db = db;
        }

        // получить список стран - берем наши страны и в 'ToList()' их загоняем
        public async Task<List<Country>> ListAllAsync()
        {
            return await _db.Countries.ToListAsync();
            // ToListAsync() — это метод для получения списка объектов в асинхронном режиме в EF Core
            // аэропорты сюда не выгрузятся, хотя их список присутствует в модели ''country
            // потому что EF работает лениво
        }

        // импортировать список стран // добавить
        public async Task ImportAsync(List<Country> countries)
        {
            // те страны, которые дублируются - удалить

            // 1 // убрали повторения кода во входных данных 
            countries = countries
                .GroupBy(countries => countries.Code)
                .Select(countries => countries.First())
                .ToList();
            // 'FIRST' в контексте оператора SELECT возвращает первое значение выбранного столбца

            // 2 // уберем повторения которые уже есть в БД
            // список существующих стран
            List<Country> existingCountries = await _db.Countries.ToListAsync();

            // если входящий код ни с одним уже существующим не сопадает - тогда добавляем страну
            countries = countries
                .Where(c => existingCountries.All(ec => ec.Code != c.Code))
                .ToList();
            // 'All' - метод проверяет, удовлетворяют ли все элементы в коллекции определённому условию

            // 3 // сохранить оставшиеся страны
            await _db.Countries.AddRangeAsync(countries);
            await _db.SaveChangesAsync();
        }

        // найти страну по коду
        public async Task<int?> GetIdByCode(string code)
        {
            Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.Code == code);
            if (country == null)
            {
                return null;
            }
            return country.Id;
        }
    }
}
