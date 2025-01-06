namespace AirportDictionaryAsp_v1.Model
{
    // чтобы работать с классами из папки 'Model' - нужно поставить дополнительные библиотеки:
    // 'Microsoft.EntityFrameworkCore.SqlServer'
    // 'Microsoft.EntityFrameworkCore.Tools'

    // класс, описывающий сущность авиакомпании
    public class Company
    {
        // поля
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // связи
        // множество аэропортов
        // 'required' - значение должно быть установлено
        public HashSet<Airport>? Airports { get; set; }

        // конструктор
        public Company() {}
    }
}
