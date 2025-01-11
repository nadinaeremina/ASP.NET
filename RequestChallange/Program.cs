using Microsoft.AspNetCore.Http;
using RequestChallange.Api;
using RequestChallange.Model;
using RequestChallange.Storage;

var builder = WebApplication.CreateBuilder(args);

// добавили разом все контроллеры (но
// у нас он один)
builder.Services.AddControllers();
// 'AddTransient' в системе внедрени€ зависимостей (DI) в ASP.NET Core
// создаЄт новый экземпл€р сервиса каждый раз, когда его запрашивают.

// добавление сервисов в IoC-контейнер
builder.Services.AddTransient<RequestService>();
// 'IRequestRepository' с имплементацией 'RequestStorage'
builder.Services.AddTransient<IRequestRepository, RequestStorage>();
// 'RequestStorage' нужен 'dbContext'
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// MapControllers() Ч это метод в ASP.NET Core, который сопоставл€ет действи€ контроллера
// с запросами, использу€ маршрутизацию на основе атрибутов
app.MapControllers();

// вынесены в отдельный класс 'ApplicationController' 
// handlers
//app.MapGet("/", () => new StringMessage(Message: "server is running"));
//app.MapGet("/ping", () => new StringMessage(Message: "pong"));

// вынесены в отдельный класс 'RequestController' 
// request handlers - обработчики
//app.MapGet("/request", async (RequestService request) => await request.ListAllAsync());
//app.MapDelete("/request", async (RequestService request) =>
//{
//    await request.DeleteAllDataAsync();
//    return Results.NoContent();
//});

// вынесено в отдельный класс 'RequestMiddleware'
// 'RequestDelegate next' - следующий обработчик цепочки
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    // перехватываем запросы и записываем результаты

//    // сперва нужно получить сервис
//    // мы можем получить добавленные в приложени€ сервисы различными способами;
//    // 1 // „ерез свойство Services объекта WebApplication(service locator)
//    // 2 // „ерез свойство RequestServices контекста запроса HttpContext в компонентах middleware(service locator)
//    // 3 // „ерез конструктор класса
//    // 4 // „ерез параметр метода Invoke компонента middleware
//    // 5 // „ерез свойство Services объекта WebApplicationBuilder

//    ////////////// 1 /////////////// действи€ до вызова следующего обработчика (next) /////////////////////////
//    // получили сервис // создали экземпл€р класса 'RequestService'
//    RequestService request = context.RequestServices.GetRequiredService<RequestService>();
//    // 'GetRequiredService' Ч метод, который позвол€ет получить
//    // необходимую службу из контейнера внедрени€ зависимостей (DI)
//    var time = DateTime.UtcNow;

//    ////////////// 2 //////////////// вызов следующего обработчика /////////////////////////////////////////////
//    await next(context);

//    ////////////// 3 /////////////// действи€ после вызова next (следующего обработчика) ////////////////////////
//    // используем метод 'AddAsync' класса 'RequestService',
//    // который добавл€ет данных об очередном запросе в систему
//    await request.AddAsync(context.Request, context.Response, time);
//});

/////////////////////////////////// выполн€ем конфигурацию //////////////////////////////////////////////////////

// ƒл€ добавлени€ компонента middleware, который представл€ет класс,
// в конвейер обработки запроса примен€етс€ метод UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<RequestMiddleware>();

app.Run();

// вынесен в отдельный файл 'ApiMessages'
// record StringMessage(string Message);

// папка 'Model' - определ€ет модель приложени€