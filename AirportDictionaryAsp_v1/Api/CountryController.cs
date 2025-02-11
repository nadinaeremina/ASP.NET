﻿using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryAsp_v1.Api
{
    // CountryController - контроллер для работы со странами

    // ресурс этого контроллера - это страны
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

        // 1 // получаем список всех стран
        [HttpGet]
        public async Task<List<CountryMessage>> ListAllAsync()
        {
            // получаем список стран
            List<Country> countries = await _countries.ListAllAsync();

            // селектим их // из класса рекорд получаем
            return countries
                .Select(c => new CountryMessage(Name: c.Name, Code: c.Code))
                .ToList();
        }

        // 2 // получить список аэропортов страны по коду страны
        [HttpGet("{code:alpha}/airports")]
        public async Task<List<AirportListItemMessage>> GetAllAirportsByCodeAsync(string code)
        {
            // получаем список аэропортов
            List<Airport> airports = await _countries.GetAllAirportsAsync(code);
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

            // собрать список сообщений со странами
            return airports.Select(airport => new AirportListItemMessage(
                Id: airport.Id,
                Name: airport.Name,
                Code: airport.Code,
                Location: airport.Location,
                CountryCode: countryCodeById[airport.CountryId]
            )).ToList();
        }

        // 3 // очистить данные всех стран с аэропортами 
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            await _countries.DeleteAllAsync();
            return Ok();
        }

        // 4 // импортировать данные о странах 
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
