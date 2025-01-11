using Microsoft.AspNetCore.WebUtilities;

namespace RequestChallange.Api
{
    // 'ErrorMiddleware' - middleware обработки ошибок
    public class ErrorMiddleware : MiddlewareBase
    {
        public ErrorMiddleware(RequestDelegate next) : base(next) { }

        // Middleware может быть создано с помощью класса с методом 'InvokeAsync' и параметром типа 'RequestDelegate' в конструкторе
        // 'RequestDelegate' требуется для выполнения следующего middleware в последовательности
        public override async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            // process 4xx (excepting errors by our methods)
            int statusCode = context.Response.StatusCode;
            if (statusCode / 100 == 4)
            {
                ErrorMessage error = new ErrorMessage(Type: statusCode.ToString(), Message: "ошибка запроса");
                await context.Response.WriteAsJsonAsync(error);
            }
            else
            {
                ErrorMessage error = new ErrorMessage(Type: statusCode.ToString(), Message: "ошибка на сервере");
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
