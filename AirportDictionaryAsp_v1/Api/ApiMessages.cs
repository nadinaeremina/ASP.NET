using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportDictionaryAsp_v1.Api
{
    // record-ы сообщений API
    // вместо этого громоздкого класса можно использовать 'record'

    //public class StringMessages
    //{
    //    public string Message { get; set; }
    //    public StringMessages() {}
    //}

    // 'record' - это еще один способ описания данных в C#, по сути это набор полей
    // это тип, который является набором полей с данными, похож на классы,
    // но в нем не нужно описывать методы, конструкторы,
    // он соответствует классу с набором публичных автосвойств
    // рекорды всегда ссылочные (даже есть наследование)
    // имутабельные (нельзя после создания менять поля)
    // такие классы очень хорошо подходят для передачи данных

    // строковое сообщение
    public record StringMessages(string Message);

    // на уровне API делаем свои сообщения
    // сообщение с данными по стране
    public record CountryMessage(string Name, string Code);

    // AirportListItemMessage - сообщение с данными об аэропорте в списке аэропортов
    public record AirportListItemMessage(int Id, string Name, string Code, string Location, string CountryCode);
    // CountryCode - код лучше, чем имя, потому что по коду мы всегда можем страну идентифицировать однозначно
    // код универсален во всех системах, а не только в нашей (id - это код системы нашей, а код универсален)

    // AirportMessage - сообщение с полными данными об аэропорте
    public record AirportMessage(
        int Id,
        string Name,
        string Code,
        int OpeningYear,
        int RunwayCount,
        long AnnualPassengerTraffic,
        string Location,
        int CountryId,
        CountryMessage Country
    );

    // ErrorMessage - сообщение с ошибкой
    public record ErrorMessage(string Type, string Message);

    // добавление аэропорта
    public record AddAirportMessage(
        string Name,
        string Code,
        int OpeningYear,
        int RunwayCount,
        long AnnualPassengerTraffic,
        string Location,
        string CountryCode
    );
}
