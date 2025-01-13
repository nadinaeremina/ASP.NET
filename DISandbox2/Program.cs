using DISandbox2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TimeService>();

// 1 // �������� 'GreetingService' �� ��������� ����� �������, ������ ��� ���� ������
// ��������� � ������������ ������ ������ ('TimeService')
// builder.Services.AddTransient<GreetingService>();

// 2 // ����� �������� ������, ������� ������� ���
//builder.Services.AddTransient<GreetingService>(_ =>
//{
//    TimeService time = new TimeService();
//    return new GreetingService(time, "ru");
//});

// 'options' - ��� ��� ����� ������ � ������, ����� ������� ����� ������� ������� - 'IserviceProvider' 
// 'options' ��� ����� � ��� ������, ��� ����� ��� �������� ������ ������� ������� ������
// 3 // ����� 'IserviceProvider'
//builder.Services.AddTransient((IServiceProvider services) =>
//{
//    TimeService time = services.GetRequiredService<TimeService>();
//    return new GreetingService(time, "ru");
//});

// 3 // ����� � ���:
//builder.Services.AddTransient(serviceProvider =>
//{
//    TimeService time = serviceProvider.GetRequiredService<TimeService>();
//    return new GreetingService(time, "ru");
//});

// 4 // ��� ����� 'opts'
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
