using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryAsp_v1.Api
{
    // контроллер для работы с аэропортами
    // сервисы необязательно внедряются в соответствующие контроллеры
    // аэропорт-контроллер может в себя внедрить как сервис аэропортов, так и сервис стран
    // 'Rout' - нужен для тогго, чтобы маршрутизироваться к методам контроллера
    // все методы данного контроллера, являющиеся обработчиками, будут в префиксе маршрута иметь это значение
    // 'ApiController' - данная аннотация используется для добавления контроллера в IoC-контейнер
    // и для их привязки к обработчикам
    [Route("api/airport")]
    [ApiController]
    public class AirportController: ControllerBase
    {
        // затаскиваем сервис для работы с аэропортами как зависимость
        private readonly AirportService _airports;

        // затаскиваем сервис для работы со странами как зависимость
        // потому что нам нужен код страны
        private readonly CountryService _countries;
        public AirportController(AirportService airports, CountryService countries)
        {
            _airports = airports;
            _countries = countries;
        }

        // обработчики

        // Get /api/airport
        // получаем список аэропортов // к данному обработчику будет вести метод 'get'
        [HttpGet]
        public async Task<List<AirportListItemMessage>> GetAllAsync()
        {
            // получаем список стран
            List<Airport> airports = await _airports.ListAllAsync();
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

            // собрать список сообщений со странами
            return airports.Select(airport => new AirportListItemMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                Location: airport.Location,
                CountryCode: countryCodeById[airport.CountryId]
            )).ToList();
        }

        // эти два метода с одинаковым путем, но у них разный тип передачи значения
        // в данном случае без указания типа эти два метода бы не сработали правильно
        // передача параметров через url - здесь какойто путь после аэропорта
        [HttpGet("{id:int}")]
        // 'IActionResult' нужен для того, чтобы вернуть 'OK', 'NotFound' и тд
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Airport? airport = await _airports.GetAsync(id);
            if (airport == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with id '{id}' not found"));
            }
            // 200
            AirportMessage result = new AirportMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                OpeningYear: airport.OpeningYear,
                RunwayCount: airport.RunwayCount,
                AnnualPassengerTraffic: airport.AnnualPassengerTraffic,
                Location: airport.Location,
                CountryId: airport.CountryId,
                Country: new CountryMessage(Name: airport.Country!.Name, Code: airport.Country!.Code)
            );
            return Ok(result);
        }

        // 'alpha' - строковый тип
        [HttpGet("{code:alpha}")]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            Airport? airport = await _airports.GetAsync(code);
            if (airport == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with code '{code}' not found"));
            }
            // 200
            AirportMessage result = new AirportMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                OpeningYear: airport.OpeningYear,
                RunwayCount: airport.RunwayCount,
                AnnualPassengerTraffic: airport.AnnualPassengerTraffic,
                Location: airport.Location,
                CountryId: airport.CountryId,
                Country: new CountryMessage(Name: airport.Country!.Name, Code: airport.Country!.Code)
            // '!' - разыменование ссылки, которая может быть пустой, но в данном случае не пустая
            // означает, что в 'Country' не пустое значение
            );
            return Ok(result);
        }

        // добавление аэропорта
        [HttpPost]
        public async Task<IActionResult> PostAsync(AddAirportMessage airportMessage)
        {
            int? countryId = await _countries.GetIdByCode(airportMessage.CountryCode);
            if (countryId == null)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "CountryNotFound", Message: $"country with code '{airportMessage.CountryCode}' not found"));
            }
            // там, где написано 'await' - код будет дожидаться операции
            if (await _airports.IsExists(airportMessage.Code))
            {
                // 409 - статус- конфликт, сервер конфликтует с тем состоянием, к которому мхотим его привести
                return Conflict(new ErrorMessage(Type: "DuplicatedAirportCode", Message: $"airport with code '{airportMessage.Code}' already exists"));
            }
            Airport airport = new Airport { 
                Name = airportMessage.Name,
                Code = airportMessage.Code,
                OpeningYear = airportMessage.OpeningYear,
                RunwayCount = airportMessage.RunwayCount,
                AnnualPassengerTraffic = airportMessage.AnnualPassengerTraffic,
                Location = airportMessage.Location,
                CountryId = countryId.Value,
            };      
            await _airports.AddAirportAsync(airport);
            return Created();
        }

        // удаление аэропорта по коду
        [HttpDelete("{code:alpha}")]
        public async Task<IActionResult> DeleteByCodeAsync(string code)
        {
            if (await _airports.IsExists(code) == false)
            {
                // 404
                return NotFound(new ErrorMessage(Type: "AirportNotFound", Message: $"airport with code '{code}' not found")); 
            }
            await _airports.DeleteAsync(code);
            // 204 результат 
            return NoContent();
        }
    }
}
