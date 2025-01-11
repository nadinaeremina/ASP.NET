using Microsoft.Extensions.Primitives;
using UserController.Api.Messages;
using UserController.Model.Exceptions;
using UserController.Model.Users;

namespace UserController.Api.Middleware
{
    // SecurityMiddleware - middleware для защиты приложения
    public class SecurityMiddleware : MiddlewareBase
    {
        // контейнер с описанием защищенных ресурсов
        // ключ: resource path
        // значение: resource methods string
        private readonly Dictionary<string, string> resources = new Dictionary<string, string>()
        {
            { "/api/resource", "GETPOST" },
            {"/api/ping", "GET" }
        };

        public SecurityMiddleware(RequestDelegate next) : base(next) { }

        // Middleware может быть создано с помощью класса с методом 'InvokeAsync' и параметром типа 'RequestDelegate' в конструкторе
        // 'RequestDelegate' требуется для выполнения следующего middleware в последовательности
        public override async Task InvokeAsync(HttpContext context)
        {
            // проверить, нужно ли защищать вызов
            string path = context.Request.Path;
            string method = context.Request.Method;
            bool secured = resources.ContainsKey(path) && resources[path].Contains(method);

            // если не защищен, то обработать как обычно
            if (!secured)
            {
                await _next(context);
                return;
            }

            // если защищен - то выполнить аутентификацию
            // 1) достать заголовок с api-ключом
            StringValues apiKeyValues = context.Request.Headers["X-Api-Key"];
            if (apiKeyValues.Count != 1 || string.IsNullOrEmpty(apiKeyValues[0]))
            {
                // 400
                ErrorMessage error = new ErrorMessage(Type: "InvalidApiKeyHeader", Message: "X-Api-Key header is invalid");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(error);
                return;
            }
            string apiKey = apiKeyValues[0]!;

            // 2) попробовать найти пользователя по данному ключу
            UserScenarios users = context.RequestServices.GetRequiredService<UserScenarios>();
            try
            {
                await users.GetUserAsync(apiKey);
                await _next(context); // пропустить запрос дальше
            }
            catch (UserNotFoundException)
            {
                // 401
                ErrorMessage error = new ErrorMessage(Type: "UnauthorizedApiKeyHeader", Message: "X-Api-Key header is unauthorized");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(error);
                return;
            }
        }
    }
}
