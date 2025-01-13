using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using UserChallange.Api.Atributes;
using UserChallange.Api.Messages;
using UserChallange.Model.Exceptions;
using UserChallange.Model.Users;

namespace UserChallange.Api.Middleware
{
    // SecurityMiddleware - middleware для защиты приложения
    public class SecurityMiddleware : MiddlewareBase
    {
        // контейнер с описанием защищенных ресурсов
        // это те пути, на которые мы настраиваем 'Middleware', которые будут защищены
        // ключ: resource path
        // значение: resource methods string

        ////////// 1 способ (без использования аннотаций)
        //private readonly Dictionary<string, string> resources = new Dictionary<string, string>()
        //{
        //    { "/api/resource", "GETPOST" },
        //    {"/api/ping", "GET" }
        //};

        public SecurityMiddleware(RequestDelegate next) : base(next) { }

        // Middleware может быть создано с помощью класса с методом 'InvokeAsync' и параметром типа 'RequestDelegate' в конструкторе
        // 'RequestDelegate' требуется для выполнения следующего middleware в последовательности
        public override async Task InvokeAsync(HttpContext context)
        {
            /////////////// если нет использования аннотаций /////////////////////
            //// получаем данные из запроса
            //string path = context.Request.Path;
            //string method = context.Request.Method;

            //// проверить, нужно ли защищать вызов (если нет использования аннотаций)
            //bool secured = resources.ContainsKey(path) && resources[path].Contains(method);

            /////////////// если есть использование аннотаций /////////////////////
            // 1 // получить целевой обработчик - endpoint (точка назначения) - запроса (он есть у каждого запроса)
            Endpoint? endpoint = context.Features.Get<IEndpointFeature>()!.Endpoint;
            if (endpoint == null)
            {
                await _next(context);
                return;
            }
            // 2 // получить из него метаданные атрибута 'protect'
            ProtectAttribute? attr = endpoint.Metadata.GetMetadata<ProtectAttribute>();
            // если у атрибута есть пар-ры - то их тоже можно достать
            // 3 // если он не пустой, то атрибут у метода есть и нужна защита
            bool secured = attr != null;

            // если не защищен, то обработать как обычно
            if (!secured)
            {
                await _next(context);
                return;
            }

            ////////////////// если защищен - то выполнить аутентификацию //////////////////////
            
            // 1) достать заголовок с api-ключом
            // получаем значение из заголовока
            StringValues apiKeyValues = context.Request.Headers["X-Api-Key"];
            if (apiKeyValues.Count != 1 || string.IsNullOrEmpty(apiKeyValues[0]))
            {
                // 400
                ErrorMessage error = new ErrorMessage(Type: "InvalidApiKeyHeader", Message: "X-Api-Key header is invalid");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(error);
                return;
            }
            // если все ок - достаем значение ключа и это будет не пустое значение
            string apiKey = apiKeyValues[0]!;

            // 2) попробовать найти пользователя по данному ключу
            // достанем сервис 'UserScenarios' из контекста
            UserScenarios users = context.RequestServices.GetRequiredService<UserScenarios>();
            try
            {
                // получаем юзера по апи - вернет ошибку, если такого нет
                User user = await users.GetUserAsync(apiKey);
                // проверить его права
                if (attr!.IsVIP && !user.IsVIP)
                {
                    // 403
                    ErrorMessage error = new ErrorMessage(Type: "VIPStatusRequired", Message: "The provided api-key is not VIP");
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(error);
                    return;
                }
                // если все ок - пропустить запрос дальше
                await _next(context); 
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
        // можно было авторизацию вынести в отдельный 'middleware
    }
}
