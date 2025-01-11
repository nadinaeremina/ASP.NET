using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryAsp_v1.Model
{
    // чтобы работать с классами из папки 'Model' - нужно поставить дополнительные библиотеки:
    // 'Microsoft.EntityFrameworkCore.SqlServer'
    // 'Microsoft.EntityFrameworkCore.Tools'

    // класс, описывающий сущность страны

    // уникальность кода
    [Index(nameof(Code), IsUnique = true)]
    public class Country
    {
        // поля
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty; // alpha3

        // связи
        // 'required' - значение должно быть установлено
        public HashSet<Airports>? Airports { get; set; }

        // конструктор
        public Country() {}
    }
}
