using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace AirportDictionaryAsp_v1.Model
{
    // чтобы работать с классами из папки 'Model' - нужно поставить дополнительные библиотеки:
    // 'Microsoft.EntityFrameworkCore.SqlServer'
    // 'Microsoft.EntityFrameworkCore.Tools'

    // класс, описывающий сущность аэропорт

    // уникальность кода
    [Index(nameof(Code), IsUnique=true)]
    public class Airport
    {
        // поля
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int OpeningYear { get; set; }
        public int RunwayCount { get; set; }
        public long AnnualPassengerTraffic { get; set; }
        public string Location { get; set; } = string.Empty;

        // связи

        // аэропорт будет ссылаться на свою страну и иметь 'countryId'
        // внешний ключ
        public int CountryId { get; set; }

        // тк у 'EntityFramework' навигационные св-ва работают лениво - добавляем
        [ForeignKey(nameof(CountryId))]
        // будет агрегировать 'country'
        public Country? Country { get; set; }
        // 'Country' - это навигационное св-во
        // множество компаний
        // 'required' - значение должно быть установлено
        public HashSet<Company>? Companies { get; set; }
        // 'nullable' - может 'null' иметь значение по умолчанию

        // конструктор
        public Airport() {}
    }
}
