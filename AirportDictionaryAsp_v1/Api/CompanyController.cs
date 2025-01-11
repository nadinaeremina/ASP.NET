using AirportDictionaryAsp_v1.Api;
using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace AirportDictionaryAsp_v1.Api
{
    // контроллер для работы с авиакомпаниями
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // затаскиваем сервис для работы с авиакомпаниями как зависимость
        private readonly CompanyService _companies;

        // затаскиваем сервис для работы со странами как зависимость
        // потому что нам нужен код страны
        private readonly CountryService _countries;

        public CompanyController(CompanyService companies, CountryService countries)
        {
            _companies = companies;
            _countries = countries;
        }

        // обработчики

        // Get /api/airport
        // 1 // получаем список авиакомпаний // к данному обработчику будет вести метод 'get'
        [HttpGet]
        public async Task<List<CompanyListItemMessage>> GetAllAsync()
        {
            // получаем список авиакомпаний
            List<Company> companies = await _companies.ListAllAsync();

            // собрать список сообщений со странами
            return companies.Select(company => new CompanyListItemMessage(
                Id: company.Id,
                Name: company.Name
            )).ToList();
        }

        // 2 // получить авиакомпанию по id
        [HttpGet("{id:int}")]
        // 'IActionResult' нужен для того, чтобы вернуть 'OK', 'NotFound' и тд
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Company? company = await _companies.GetAsync(id);
            if (company == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"company with id '{id}' not found"));
            }
            // 200
            CompanyListItemMessage result = new CompanyListItemMessage(
                Id: company.Id,
                Name: company.Name
            );
            return Ok(result);
        }

        // ?????????????????????????????????????????????????????????????????????????????
        // 3 // получить список аэропортов, в которых присутствует заданная авиакомпания 
        [HttpGet("{id:int}/airports")]
        public async Task<IActionResult> GetAirportsAsync(int id)
        {
            List<Airports>? airports = await _companies.GetAirports(id);
            if (airports == null)
            {
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"company with id '{id}' not found"));
            }
            // получаем список стран
            List<Country> countries = await _countries.ListAllAsync();

            // преобразовать список стран в словарь с ключами - id и значениями - кодами 

            // 1 способ
            Dictionary<int, string> countryCodeById =
              countries.ToDictionary(
                  country => country.Id,
                  country => country.Code
              );

            // 2 способ
            //Dictionary<int, string> countryCodeById = new Dictionary<int, string>();
            //foreach (Country country in countries)
            //{
            //    countryCodeById[country.Id] = country.Code;
            //}

            return Ok(airports.Select(airport => new AirportListItemMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                Location: airport.Location,
                CountryCode: countryCodeById[airport.CountryId]
            )).ToList());
        }

        // 4 // добавление авиакомпании
        [HttpPost]
        public async Task<IActionResult> PostAsync(AddCompanyMessage addCompany)
        {
            if (await _companies.IsExists(addCompany.Name))
            {
                // 409 - статус- конфликт, сервер конфликтует с тем состоянием, к которому мхотим его привести
                return Conflict(new ErrorMessage(Type: "DuplicatedCompanyCode", Message: $"company with name '{addCompany.Name}' already exists"));
            }
            Company company = new Company
            {
                Name = addCompany.Name
            };
            await _companies.AddCompanyAsync(company);
            return Ok();
        }

        // 5 // удаление авиакомпании по айди
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteByCodeAsync(int id)
        {
            if (await _companies.IsExists(id) == false)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "CompanyNotFound", Message: $"company with id '{id}' not found"));
            }
            await _companies.DeleteAsync(id);
            // 204 результат 
            return NoContent();
        }
    }
}
