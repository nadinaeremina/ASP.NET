using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryAsp_v1.Api
{
    // CountryController - контроллер для работы со странами

    [Route("api/country")]
    [ApiController]
    // сделаем его наследником 'ControllerBase'
    public class CountryController : ControllerBase
    {
        // затаскиваем сервис для работы со странами как зависимость
        private readonly CountryService _countries;

        public CountryController(CountryService countries)
        {
            _countries = countries;
        }

        // обработчики

        // получаем список стран
        [HttpGet]
        public async Task<List<CountryMessage>> ListAllAsync()
        {
            // получаем список стран
            List<Country> countries = await _countries.ListAllAsunc();

            // селектим их // из класса рекорд получаем
            return countries
                .Select(c => new CountryMessage(Name: c.Name, Code: c.Code))
                .ToList();
        }

        // импорт стран
        [HttpPut]
        // 'IActionResult' - rezultApi в контроллерах
        public async Task<IActionResult> ImportAsync(List<CountryMessage> countries)
        {
            // берем наши страны, селектим их, в класс 'Country' устанавливая имя и код
            // из рекорда класс получаем
            List<Country> imported = countries
                .Select(c => new Country() { Name = c.Name, Code = c.Code })
                .ToList();

            await _countries.ImportAsync(imported);

            // 204
            return NoContent();
        }
        // если такой код уже существует - то страна не добавится

        // строка PUT в Postman
        // [ { "name": "Россия", "code": "rus" }, { "name": "Белоруссия", "code": "blr" },
        // { "name": "Китай", "code": "chn" }] 
        // все это ввиде массива
    }
}
