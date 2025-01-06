using DISandbox.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//��������� ����������� ��������� ����� � IoC-��������� 
// 'Service scopes' (lifetime) 
////////////////////////////////////////////// 1 ///////////////////////////////////////////////////////////////////
//builder.Services.AddTransient<ITimeService>(); // scope ����� ������������ 
// ��������� ������ ��� ������ ��������� � IoC-����������
////////////////////////////////////////////// 2 //////////////////////////////////////////////////////////////////////
// builder.Services.AddScoped<ITimeService>(); // scope ����� scoped 
// ���������� � ������ ��������� ������ http-������� 
// ������ ��������� ���� ��� � � ������ �������
///////////////////////////////////////////////  3 ///////////////////////////////////////////////////////////////////
// builder.Services.AddSingleton<ITimeService>(); // scope ����� singleton 
// ����������� �� ��� ������ ���������
// ��������� ���� ��� ��� ������ ��������� � ���� ����� Ioc-���������, ����� �� ������������
// ��� ����������� ���������� ����� ��������


// ������� ���������� ������������ � Ioc - ���������
// ��� ����, ����� ��������� ���������  -�� ����� ������� ��� generic-��������� 
// ��� ���������� � ��� ������������� (���� ������ �����, ���� ��� �� �������������)
// 1 // builder.Services.AddTransient<ITimeService, UTCTimeService>();

// 2 // builder.Services.AddTransient<ITimeService, LocalTimeService>(); 

// 3 // ���� ����� �������� �������� - ����� ������������� ��������� ������ 
// ���� ����� ���������� ����� �� 'TimeZone'
// builder.Services.AddTransient<ITimeService>(opts => new LocalTimeService(3)); 
// ������������� ����� �� ����������, ��� ��� ��� ��������

// 4 // ���������� �������
builder.Services.AddTransient(opts => TimeServiceFactory.CreateTimeService()); // �������� �������
// 'generic' ��� ����� �� ������, ������ ��� ����� ���������� ���������

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// ���������� 'IoC-���������'
// �� ����� ���������� �� �������, ������� ������������ � ����� ���-����������
app.MapGet("/services", () =>
{
    StringBuilder sb = new StringBuilder();
    // StringBuilder � ��� �����, ������� ������������ ������������ ������ �
    // ������������ ��� ����������� ����� ��� �������� ����� ��������
    // ����� ������ ����� ���������� ����� ��������
    sb.AppendLine($"Total service count: {builder.Services.Count}");
    // �������� �� �� 'builder'
    // ������� ��� �������, ������� ��������� � ���-���������� � IoC-����������
    foreach (ServiceDescriptor? service in builder.Services)
    // 'ServiceDescriptor' � ��� �����, �������������� ������, ������� ���������� � 'ServiceCollection',
    // ������� �������� ����� ����������� ��������� ������������
    {
        if (service == null)
        {
            sb.Append("null");
            continue;
        }
        // ��������� � ������ ��������� ���������, ���� �� �� ������
        sb.AppendLine($"{service.ServiceType.Name} - {service.Lifetime}");
    }
    return sb.ToString();
});

/////////////////////////////////////////////////// ������ � ��������: ///////////////////////////////////////////////////////////
// 1 // ���� ����� 'HttpContext' // ���������� ��������� � ����������� ������
app.MapGet("/context/time", (HttpContext context) => 
{ 
    // �������� ������ �� Ioc-����������
    ITimeService service = context.RequestServices.GetRequiredService<ITimeService>();
    // 'GetService' - ���������� nullable-������, ���� ������� ��� - ����� null, �
    // 'GetRequiredService' �� �� �����, �� ������� exception, ���� 'null'

    // ���������� ������
    return service.GetTime();
});

// 2 // ���� ����� ��������� - ����� WebApplication 
// 'WebApplication' ���� ������������� ������ � ��������, ����� ���� ���� ����� ������� ������
app.MapGet("/app-object/time", () =>
{
    ITimeService service = app.Services.GetRequiredService<ITimeService>();
    return service.GetTime();
});
// ����� �� ���������� �� ������� ����������, �������, ����� �� �������
// ����������� � ��������� ����� ������������-�� ���� ������ �������� �� �����

// 3 // ������ ����� �������� ���������� �������������\������� (�������� �������)
app.MapGet("/sugar/time", (ITimeService service) =>
{
    // 'asp' �������������� ����� �������� ��������� ��������� ���� ������, ������,
    // ��� ����� 'TimeService', ��� ������ ���� ��� � Ioc-����������, ���������
    return service.GetTime();
});

app.Run();
