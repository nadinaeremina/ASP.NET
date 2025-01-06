using Microsoft.AspNetCore.Http;
using RequestChallange.Model;
using RequestChallange.Storage;

var builder = WebApplication.CreateBuilder(args);
// добавление сервисов в IoC-контейнер
builder.Services.AddTransient<RequestService>();
// 'IRequestRepository' с имплементацией 'RequestStorage'
builder.Services.AddTransient<IRequestRepository, RequestStorage>();
// 'RequestStorage' нужен 'dbContext'
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// handlers
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));

// request handlers - обработчики
app.MapGet("/request", async (RequestService requests) => await requests.ListAllAsync());
app.MapDelete("/request", async (RequestService requests) =>
{
    await requests.DeleteAllDataAsync();
    return Results.NoContent();
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // мы можем получить добавленные в приложени€ сервисы различными способами;
    // 1 // „ерез свойство Services объекта WebApplication(service locator)
    // 2 // „ерез свойство RequestServices контекста запроса HttpContext в компонентах middleware(service locator)
    // 3 // „ерез конструктор класса
    // 4 // „ерез параметр метода Invoke компонента middleware
    // 5 // „ерез свойство Services объекта WebApplicationBuilder

    ////////////// 1 /////////////// действи€ до вызова следующего обработчика (next) /////////////////////////
    // получили сервис // создали экземпл€р класса 'RequestService'
    RequestService request = context.RequestServices.GetRequiredService<RequestService>();
    // 'GetRequiredService' Ч метод, который позвол€ет получить
    // необходимую службу из контейнера внедрени€ зависимостей (DI)
    var time = DateTime.UtcNow;

    ///////////// 2 //////////////// вызов следующего обработчика /////////////////////////////////////////////
    await next(context);

    //////////// 3 /////////////// действи€ после вызова next (следующего обработчика) ////////////////////////
    // используем метод 'AddAsync' класса 'RequestService',
    // который добавл€ет данных об очередном запросе в систему
    await request.AddAsync(context.Request, context.Response, time);
}); 

app.Run();

record StringMessage(string Message);

// папка 'Model' - определ€ет модель приложени€