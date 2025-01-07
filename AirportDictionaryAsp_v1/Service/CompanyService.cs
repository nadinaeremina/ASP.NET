using AirportDictionaryAsp_v1.Model;

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
    }
}
