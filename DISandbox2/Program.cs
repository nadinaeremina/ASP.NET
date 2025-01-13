using DISandbox2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TimeService>();

// 1 // внедрить 'GreetingService' не получится таким образом, потому что этот сервис
// принимает в конструкторе другой сервис ('TimeService')
// builder.Services.AddTransient<GreetingService>();

// 2 // можем написать лямбду, которая создает его
//builder.Services.AddTransient<GreetingService>(_ =>
//{
//    TimeService time = new TimeService();
//    return new GreetingService(time, "ru");
//});

// 'options' - это тот самый обьект в лямбде, через который можно достать сервисы - 'IserviceProvider' 
// 'options' нам нужен в тех местах, где нужно для создания одного сервиса достать другой
// 3 // через 'IserviceProvider'
//builder.Services.AddTransient((IServiceProvider services) =>
//{
//    TimeService time = services.GetRequiredService<TimeService>();
//    return new GreetingService(time, "ru");
//});

// 3 // можно и так:
//builder.Services.AddTransient(serviceProvider =>
//{
//    TimeService time = serviceProvider.GetRequiredService<TimeService>();
//    return new GreetingService(time, "ru");
//});

// 4 // или через 'opts'
builder.Services.AddTransient(opts =>
{
    TimeService time = opts.GetRequiredService<TimeService>();
    return new GreetingService(time, "ru");
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/time", (TimeService time) => time.GetTime());
app.MapGet("/greeting", (GreetingService greeting) => greeting.GetGreeting());

app.Run();
