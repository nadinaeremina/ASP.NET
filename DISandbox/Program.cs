using DISandbox.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//добавляем зависимотси различных типов в IoC-контейнер 
// 'Service scopes' (lifetime) 
////////////////////////////////////////////// 1 ///////////////////////////////////////////////////////////////////
//builder.Services.AddTransient<ITimeService>(); // scope будет трансиентный 
// создается заново при каждом обращении к IoC-контейнеру
////////////////////////////////////////////// 2 //////////////////////////////////////////////////////////////////////
// builder.Services.AddScoped<ITimeService>(); // scope будет scoped 
// существует в рамках обработки одного http-запроса 
// сервис создается один раз и в рамках запроса
///////////////////////////////////////////////  3 ///////////////////////////////////////////////////////////////////
// builder.Services.AddSingleton<ITimeService>(); // scope будет singleton 
// сохраняется на всю работу программы
// создается один раз при первом обращении к нему через Ioc-контейнер, далее не пересодается
// при последующих обращениях будет работать


// область добавления зависимостей в Ioc - контейнер
// для того, чтобы привязать интерфейс  -мы можем указать два generic-параметра 
// тип интерфейса и тип имплементации (либо просто класс, если это не имплементация)
// 1 // builder.Services.AddTransient<ITimeService, UTCTimeService>();

// 2 // builder.Services.AddTransient<ITimeService, LocalTimeService>(); 

// 3 // если хотим передать параметр - здесь дополнительно передадим лямбду 
// если хотим посмотреть время по 'TimeZone'
// builder.Services.AddTransient<ITimeService>(opts => new LocalTimeService(3)); 
// имплементацию можно не передавать, так как все очевидно

// 4 // используем фабрику
builder.Services.AddTransient(opts => TimeServiceFactory.CreateTimeService()); // добавили фабрику
// 'generic' уже можно не писать, потому что метод возвращает интерфейс

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// содержимое 'IoC-контейнер'
// мы можем посмотреть на сервисы, которые присутствуют в нашем веб-приложении
app.MapGet("/services", () =>
{
    StringBuilder sb = new StringBuilder();
    // StringBuilder — это класс, который представляет динамическую строку и
    // предназначен для модификации строк без создания новых объектов
    // здесь увидим общее количество наших сервисов
    sb.AppendLine($"Total service count: {builder.Services.Count}");
    // достанем их из 'builder'
    // выводим все сервисы, которые болтаются в веб-приложении в IoC-контейнере
    foreach (ServiceDescriptor? service in builder.Services)
    // 'ServiceDescriptor' — это класс, представляющий службу, которая внедряется в 'ServiceCollection',
    // который является нашим контейнером внедрения зависимостей
    {
        if (service == null)
        {
            sb.Append("null");
            continue;
        }
        // добавляем в сервис различные параметры, если он не пустой
        sb.AppendLine($"{service.ServiceType.Name} - {service.Lifetime}");
    }
    return sb.ToString();
});

/////////////////////////////////////////////////// Доступ к сервисам: ///////////////////////////////////////////////////////////
// 1 // явно через 'HttpContext' // совмещение сахарного и несахарного метода
app.MapGet("/context/time", (HttpContext context) => 
{ 
    // получаем сервис из Ioc-контейнера
    ITimeService service = context.RequestServices.GetRequiredService<ITimeService>();
    // 'GetService' - возвращает nullable-обьект, если сервиса нет - будет null, а
    // 'GetRequiredService' то же самое, но выкинет exception, если 'null'

    // используем сервис
    return service.GetTime();
});

// 2 // явно через замыкание - бьект WebApplication 
// 'WebApplication' тоже предоставляет доступ к сервисам, через него тоже можно достать сервис
app.MapGet("/app-object/time", () =>
{
    ITimeService service = app.Services.GetRequiredService<ITimeService>();
    return service.GetTime();
});
// здесь мы обращаемся ко внешней переменной, поэтому, когда мы вынесем
// обработчики в отдельный класс контроллеров-то этот способ работать не будет

// 3 // неявно через передачу параметров конструкторов\методов (сахарный вариант)
app.MapGet("/sugar/time", (ITimeService service) =>
{
    // 'asp' проанализирует через механизм рефлексии сигнатуру этой лямбды, поймет,
    // что нужен 'TimeService', сам найдет этот тип в Ioc-контейнере, подставит
    return service.GetTime();
});

app.Run();
