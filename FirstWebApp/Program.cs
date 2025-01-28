// Здесь подготавливается, создается и запускается веб-приложение

// подготовка web-приложения
// 1 // создается 'builder' приложения (паттерн-строитель-builder)

// можно создать его 4-мя разными способами
// 1.1
// var builder = WebApplication.CreateBuilder();

// 1.2
// var builder = WebApplication.CreateBuilder(args);

// 1.3
//WebApplicationBuilder builder = WebApplication.CreateBuilder();

// 1.4.1
// Можно создавать или без нет
// WebApplicationOptions options = new() { Args = args };
// если создали - то:
// WebApplicationBuilder builder = WebApplication.CreateBuilder(options);
// если нет - то
// 1.4.2
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 2 // Конфигурирование зависимостей приложения (пока не интересует)

// создание web-приложения
// 3 // сборка web-приложения
var app = builder.Build();

// 4 // Конфигурация web-приложения - привязка обработчиков к запросам
// 'MapGet' - 'map' - привязка, 'get' - get-запрос // конфиги 
// через 'MapGet' мы привязываем обработчик, который передается вторым параметром в виде лямбды
// которая всегда ассинхронная и принимает на вход параметр - 'HttpContext context'
// и передаем путь первым параметром

// 'HttpContext' - объект, который инкапсулирует информацию о запросе,
// позволяет управлять ответом и имеет еще много другой функциональности
// для вывода кириллицы желательно устанавливать заголовок ContentType, в том числе кодировку,
// которая применяется в отправляемом содержимом 
// response.Headers.ContentType = "text/plain; charset=utf-8";
// Если необходимо отправить html-код, то для этого необходимо установить для заголовка Content-Type значение text/htm
// response.ContentType = "text/html; charset=utf-8";

// 'path' - путь (/ users)
// 'QueryString' - строка запроса (?name=Tom&age=37)

// 4.1.1
app.MapGet("/", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    // явно установили статус-код, хотя он ставится автоматически

    await context.Response.WriteAsync("server is running");
    // 'WriteAsync' - позволяет записать в тело ответа текст
});

// 4.1.2
app.MapGet("/sugar1", () =>
{
    return "server is runnung(sugar1)";
    // в данном случае сервер неявно сделает те же действия, что и в 4.1.1
});

// 4.1.3
app.MapGet("/sugar2", () => "server is runnung(sugar2)");

// 'HTTP-context - обьект, который включает в себя две важнейшие сущности, 
// связанные с обработкой http-запроса (сущность запроса и сущность ответа)
// 'context.Response' и ' context.Request'
// если мы не будем писать тип, который передаем в лямбде в качестве обработчика
// то автоматически это будет пар-р HTTP-контекста

// слэш можно не писать
// 4.2.1
//app.MapGet("/", () => "Hello from ASP Server!"); // корневой путь
//app.MapGet("/ping", () => "pong");
// 1 пар-р - путь, 2 пар-р - лямбда

// 4.2.2
//app.MapGet("", () => "Hello from ASP Server!"); 
//app.MapGet("ping", () => "pong");

// 4.3 // узнать текущее время
app.MapGet("/time", async (HttpContext context) =>
{
    string nowTime = $"{DateTime.Now:HH.mm.ss}";
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync(nowTime);
});

// 4.4 // сколько дней осталось до НГ
app.MapGet("/days-to-new-year", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    DateTime today = DateTime.Today;
    DateTime new_year = new DateTime(today.Year, 12, 31);
    int count = 0;
    if (today.Year % 4 == 0)
    {
        count++;
    }
    await context.Response.WriteAsync($"До нового года осталось: {(new_year - today).Days + count} дней.");
});

// 4.5 // сколько дней прошло с крайнего НГ
app.MapGet("/days-from-last-new-year", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    DateTime today = DateTime.Today;
    DateTime last_new_year = new DateTime(today.Year-1, 12, 31);
    await context.Response.WriteAsync($"С прошедшего нового года прошло: {(today - last_new_year).Days} дней.");
});

// 5 // запуск web-приложения (оно в вечном цикле будет слушать входящие запросы)
app.Run();

// это будет сервер, который принимает два запроса,
// который разворачивается на локалхолсте на порту 8080
// это вечная программа, остановить ее можно чреез ctrl+C
// через Postman или через браузер можно отправить запросы нашему приложению
// на вход будет ожидать http-запросы, отправлять http-ответы

// Postman
// Запросы и ответы: 
// http://localhost:8080 - Hello from ASP Server!
// http://localhost:8080/ping - pong

// Можно и через браузер

// каждый запрос, направленный серверу, будет направлен на обработчик запросов (Hander)
// обработчик именно этого запроса
// маршрутизация (routing) - процесс сопоставления http-запроса с обработчиком
// любой запрос должен быть обработан, даже если на такой запрос нет обработчика
// обработчик(middleware) обрабатывает запрос и возвращает ответ
// обработчик - ф-ция, его задача: принять запрос, обработать его, вызвать логику, вернуть ответ

///////////////////////////////////// Св-ва класса 'WebApplicationBuilder': ///////////////////////////////////////////

// Configuration: представляет объект ConfigurationManager, который применяется для добавления конфигурации к приложению
// представляет конфигурацию приложения в виде объекта IConfiguration

// Environment: предоставляет информацию об окружении, в котором запущено приложение
// представляет окружение приложения в виде IWebHostEnvironment

// Host: объект IHostBuilder, который применяется для настройки хоста

// Logging: позволяет определить настройки логгирования в приложении

// Services: представляет коллекцию сервисов и позволяет добавлять сервисы в приложение

// WebHost: объект IWebHostBuilder, который позволяет настроить отдельные настройки сервера

// Lifetime: позволяет получать уведомления о событиях жизненного цикла приложения

// Logger: представляет логгер приложения по умолчанию

// Urls: представляет набор адресов, которые использует сервер

//////////////////////////////// Этот класс применяет три интерфейса: //////////////////////////////////////////////////
// IHost: применяется для запуска и остановки хоста, который прослушивает входящие запросы
// IApplicationBuilder: применяется для установки компонентов, которые участвуют в обработке запроса
// IEndpointRouteBuilder: применяется для установки маршрутов, которые сопоставляются с запросами

/////////////////////////////// Для управления хостом класс WebApplication определяет следующие методы: ////////////////

// Run(): запускает приложение
// RunAsync(): асинхронно запускает приложение
// Start(): запускает приложение
// StartAsync(): запускает приложение
//StopAsync(): останавливает приложение

// Сервер - это программа, которая вечно работает и вечно ждет входящие запросы, клиентов, которых можно обработать