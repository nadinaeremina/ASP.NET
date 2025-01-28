using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages2.Pages
{
    public class IndexModel : PageModel
    {
        // Задача: вывести список дней недели на русском языке
        // выходные дни (сб-вс) выделить красным цветом
        // сегодняшний день выделить полужирным начертанием
        // в C#: 0 - вс, 1 - пн, 2 - вт, 3 - ср, 4 - чт, 5 - пт, 6 - сб 

        // можем обьявить публичное св-во - его можно получать снаружи
        // но устанавливать только внутри класса
        // к публичным полям и св-вам можем получить доступ со страницы 'index.cshtml'
        public string Title { get; private set; } = "Razor Pages Sandbox!";
        public Dictionary<int, string> DayByNumber { get; private set; } = new Dictionary<int, string>()
        {
            {1, "понедельник"},
            {2, "вторник"},
            {3, "среда"},
            {4, "четверг"},
            {5, "пятница"},
            {6, "суббота"},
            {0, "воскресенье"},
        };
        public int TodayNumber { get; private set; } = (int)DateTime.Now.DayOfWeek;
        public void OnGet()
        {
            // выполним вычисления для обработки запроса
            // при каждом запуске будет пересчитываться
            TodayNumber = (int)DateTime.Now.DayOfWeek;
        }
    }
}
