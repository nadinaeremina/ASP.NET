using Microsoft.AspNetCore.Http;
using RequestChallange.Api;
using RequestChallange.Model;
using RequestChallange.Storage;

var builder = WebApplication.CreateBuilder(args);

// добавили разом все контроллеры (но
// у нас он один)
builder.Services.AddControllers();
// 'AddTransient' в системе внедрения зависимостей (DI) в ASP.NET Core
// создаёт новый экземпляр сервиса каждый раз, когда его запрашивают.

// добавление сервисов в IoC-контейнер
builder.Services.AddTransient<RequestService>();
// 'IRequestRepository' с имплементацией 'RequestStorage'
builder.Services.AddTransient<IRequestRepository, RequestStorage>();
// 'RequestStorage' нужен 'dbContext'
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// MapControllers() — это метод в ASP.NET Core, который сопоставляет действия контроллера
// с запросами, используя маршрутизацию на основе атрибутов
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
//    // мы можем получить добавленные в приложения сервисы различными способами;
//    // 1 // Через свойство Services объекта WebApplication(service locator)
//    // 2 // Через свойство RequestServices контекста запроса HttpContext в компонентах middleware(service locator)
//    // 3 // Через конструктор класса
//    // 4 // Через параметр метода Invoke компонента middleware
//    // 5 // Через свойство Services объекта WebApplicationBuilder

//    ////////////// 1 /////////////// действия до вызова следующего обработчика (next) /////////////////////////
//    // получили сервис // создали экземпляр класса 'RequestService'
//    RequestService request = context.RequestServices.GetRequiredService<RequestService>();
//    // 'GetRequiredService' — метод, который позволяет получить
//    // необходимую службу из контейнера внедрения зависимостей (DI)
//    var time = DateTime.UtcNow;

//    ////////////// 2 //////////////// вызов следующего обработчика /////////////////////////////////////////////
//    await next(context);

//    ////////////// 3 /////////////// действия после вызова next (следующего обработчика) ////////////////////////
//    // используем метод 'AddAsync' класса 'RequestService',
//    // который добавляет данных об очередном запросе в систему
//    await request.AddAsync(context.Request, context.Response, time);
//});

/////////////////////////////////// выполняем конфигурацию //////////////////////////////////////////////////////

// Для добавления компонента middleware, который представляет класс,
// в конвейер обработки запроса применяется метод UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<RequestMiddleware>();

app.Run();

// вынесен в отдельный файл 'ApiMessages'
// record StringMessage(string Message);

// папка 'Model' - определяет модель приложения