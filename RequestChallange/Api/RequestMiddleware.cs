using Microsoft.Extensions.Primitives;
using RequestChallange.Model;
using System.Globalization;
using System.Resources;

namespace RequestChallange.Api
{
    // 'RequestMiddleware' - наш собственный middleware, который получает информацию о времени запроса и обрабатываеи ошибки в приложении
    public class RequestMiddleware : MiddlewareBase
    {
        public RequestMiddleware(RequestDelegate next) : base(next) { }

        // Middleware может быть создано с помощью класса с методом 'InvokeAsync' и параметром типа 'RequestDelegate' в конструкторе
        // 'RequestDelegate' требуется для выполнения следующего middleware в последовательности
        public override async Task InvokeAsync(HttpContext context)
        {
            RequestService request = context.RequestServices.GetRequiredService<RequestService>();
            // 'GetRequiredService' — метод, который позволяет получить
            // необходимую службу из контейнера внедрения зависимостей (DI)
            var time = DateTime.UtcNow;

            ////////////// 2 //////////////// вызов следующего обработчика /////////////////////////////////////////////
            await _next(context);

            ////////////// 3 /////////////// действия после вызова next (следующего обработчика) ////////////////////////
            // используем метод 'AddAsync' класса 'RequestService',
            // который добавляет данных об очередном запросе в систему
            await request.AddAsync(context.Request, context.Response, time); 
        }
    }
}