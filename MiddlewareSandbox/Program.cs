using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// это хэндлеры
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));
// http://localhost:8080/greeting?name=Nadi
app.MapGet("/greeting", (string name) => new StringMessage(Message: $"Hello, {name}"));

// добавление собственного middleware-обработчика через метод "Use" - он становится частью цепочки ASP
// он позволяет сделать что-то до запроса и после, а также может влиять на запрос и ответ
// middleware в методе Use выполняет действия до следующего в конвейере компонента и после него
// в лямбду обязательно передаем 'HttpContext'
// в данном случае через наш middleware будут проходить абсолютно все запросы
// 'RequestDelegate next' - следующий обработчик цепочки
app.Use(async(HttpContext context, RequestDelegate next) =>
{
    // 1 // действия до вызова следующего обработчика (next)
    Console.WriteLine("Before Next");

    // 2 // вызов следующего обработчика
    // обязательно нужно вызвать внутри
    // если мы его по каким-то причинам не вызовет - то у нас не будет осуществляться дальнейшая обработка (3 пункт)
    await next(context);
    // await next.Invoke();

    // 3 // действия после вызова next (следующего обработчика)
    Console.WriteLine("After Next");
});

// 'MiddleWare' позволяет сделать чтото до запроса и после, а также может влять на запрос и ответ
// можно применять для: логгирование сервера, защита сервера от атак, аутентификация, авторизация, выполнение
// каких-то учетных действий, преобразование данных, запись ошибок, переопределение стандартных обработчиков ошибок

app.Run();

record StringMessage(string Message);

// Middleware может быть создано с помощью класса с методом InvokeAsync() и параметром типа RequestDelegate в конструкторе
// Тип RequestDelegate требуется для выполнения следующего middleware в последовательности.

//public class LogURLMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<LogURLMiddleware> _logger;
//    public LogURLMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
//    {
//        _next = next;
//        _logger = loggerFactory?.CreateLogger<LogURLMiddleware>() ??
//        throw new ArgumentNullException(nameof(loggerFactory));
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        _logger.LogInformation($"Request URL: {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request)}");
//        await this._next(context);
//    }
//}

// Каждый middleware-компонент в ASP.NET Core:
// Имеет доступ как к входящим запросам, так и к отправляемым обратно ответам
// Может просто передать запрос следующему middleware в конвейере
// Может выполнять некоторую логику обработки и затем передавать этот запрос следующему middleware для дальнейшей обработки
// При необходимости может завершить (замкнуть) конвейер запросов
// Выполняется в том порядке, в котором он был добавлены в конвейер

// app.Run()
// добавляет middleware-компонент в виде Run[Middleware], который выполнится в конце конвейера
// Как правило, он действует как замыкающее middleware и добавляется в конце конвейера запросов,
// поскольку не может вызывать следующий middleware-компонент (next)

// app.Use()
// метод используется для конфигурирования нескольких middleware
// В отличие от app.Run(), мы можем включить в него параметр next,
// который вызывает следующий делегат запроса в конвейере
// Мы также можем замкнуть (завершить) конвейер, не вызывая параметр next
