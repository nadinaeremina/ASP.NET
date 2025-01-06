// ����� ����������������, ��������� � ����������� ���-����������

// ���������� web-����������
// 1 // ��������� 'builder' ���������� (�������-���������-builder)

// ����� ������� ��� 4-�� ������� ���������
// 1.1
// var builder = WebApplication.CreateBuilder();

// 1.2
// var builder = WebApplication.CreateBuilder(args);

// 1.3
//WebApplicationBuilder builder = WebApplication.CreateBuilder();

// 1.4.1
// ����� ��������� ��� ��� ���
// WebApplicationOptions options = new() { Args = args };
// ���� ������� - ��:
// WebApplicationBuilder builder = WebApplication.CreateBuilder(options);
// ���� ��� - ��
// 1.4.2
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 2 // ���������������� ������������ ���������� (���� �� ����������)

// �������� web-����������
// 3 // ������ web-����������
var app = builder.Build();

// 4 // ������������ web-���������� - �������� ������������ � ��������
// 'MapGet' - 'map' - ��������, 'get' - get-������ // ������� 
// ����� 'MapGet' �� ����������� ����������, ������� ���������� ������ ���������� � ���� ������
// ������� ������ ������������ � ��������� �� ���� �������� - 'HttpContext context'
// � �������� ���� ������ ����������

// 'HttpContext' - ������, ������� ������������� ���������� � �������,
// ��������� ��������� ������� � ����� ��� ����� ������ ����������������
// ��� ������ ��������� ���������� ������������� ��������� ContentType, � ��� ����� ���������,
// ������� ����������� � ������������ ���������� 
// response.Headers.ContentType = "text/plain; charset=utf-8";
// ���� ���������� ��������� html-���, �� ��� ����� ���������� ���������� ��� ��������� Content-Type �������� text/htm
// response.ContentType = "text/html; charset=utf-8";

// 'path' - ���� (/ users)
// 'QueryString' - ������ ������� (?name=Tom&age=37)

// 4.1.1
app.MapGet("/", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    // ���� ���������� ������-���, ���� �� �������� �������������

    await context.Response.WriteAsync("server is running");
    // 'WriteAsync' - ��������� �������� � ���� ������ �����
});

// 4.1.2
app.MapGet("/sugar1", () =>
{
    return "server is runnung(sugar1)";
    // � ������ ������ ������ ������ ������� �� �� ��������, ��� � � 4.1.1
});

// 4.1.3
app.MapGet("/sugar2", () => "server is runnung(sugar2)");

// 'HTTP-context - ������, ������� �������� � ���� ��� ��������� ��������, 
// ��������� � ���������� http-������� (�������� ������� � �������� ������)
// 'context.Response' � ' context.Request'
// ���� �� �� ����� ������ ���, ������� �������� � ������ � �������� �����������
// �� ������������� ��� ����� ���-� HTTP-���������

// ���� ����� �� ������
// 4.2.1
//app.MapGet("/", () => "Hello from ASP Server!"); // �������� ����
//app.MapGet("/ping", () => "pong");
// 1 ���-� - ����, 2 ���-� - ������

// 4.2.2
//app.MapGet("", () => "Hello from ASP Server!"); 
//app.MapGet("ping", () => "pong");

// 4.3 // ������ ������� �����
app.MapGet("/time", async (HttpContext context) =>
{
    string nowTime = $"{DateTime.Now:HH.mm.ss}";
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync(nowTime);
});

// 4.4 // ������� ���� �������� �� ��
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
    await context.Response.WriteAsync($"�� ������ ���� ��������: {(new_year - today).Days + count} ����.");
});

// 4.5 // ������� ���� ������ � �������� ��
app.MapGet("/days-from-last-new-year", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    DateTime today = DateTime.Today;
    DateTime last_new_year = new DateTime(today.Year-1, 12, 31);
    await context.Response.WriteAsync($"� ���������� ������ ���� ������: {(today - last_new_year).Days} ����.");
});

// 5 // ������ web-���������� (��� � ������ ����� ����� ������� �������� �������)
app.Run();

// ��� ����� ������, ������� ��������� ��� �������,
// ������� ��������������� �� ����������� �� ����� 8080
// ��� ������ ���������, ���������� �� ����� ����� ctrl+C
// ����� Postman ��� ����� ������� ����� ��������� ������� ������ ����������
// �� ���� ����� ������� http-�������, ���������� http-������

// Postman
// ������� � ������: 
// http://localhost:8080 - Hello from ASP Server!
// http://localhost:8080/ping - pong

// ����� � ����� �������

// ������ ������, ������������ �������, ����� ��������� �� ���������� �������� (Hander)
// ���������� ������ ����� �������
// ������������� (routing) - ������� ������������� http-������� � ������������
// ����� ������ ������ ���� ���������, ���� ���� �� ����� ������ ��� �����������
// ����������(middleware) ������������ ������ � ���������� �����
// ���������� - �-���, ��� ������: ������� ������, ���������� ���, ������� ������, ������� �����

///////////////////////////////////// ��-�� ������ 'WebApplicationBuilder': ///////////////////////////////////////////

// Configuration: ������������ ������ ConfigurationManager, ������� ����������� ��� ���������� ������������ � ����������
// ������������ ������������ ���������� � ���� ������� IConfiguration

// Environment: ������������� ���������� �� ���������, � ������� �������� ����������
// ������������ ��������� ���������� � ���� IWebHostEnvironment

// Host: ������ IHostBuilder, ������� ����������� ��� ��������� �����

// Logging: ��������� ���������� ��������� ������������ � ����������

// Services: ������������ ��������� �������� � ��������� ��������� ������� � ����������

// WebHost: ������ IWebHostBuilder, ������� ��������� ��������� ��������� ��������� �������

// Lifetime: ��������� �������� ����������� � �������� ���������� ����� ����������

// Logger: ������������ ������ ���������� �� ���������

// Urls: ������������ ����� �������, ������� ���������� ������

//////////////////////////////// ���� ����� ��������� ��� ����������: //////////////////////////////////////////////////
// IHost: ����������� ��� ������� � ��������� �����, ������� ������������ �������� �������
// IApplicationBuilder: ����������� ��� ��������� �����������, ������� ��������� � ��������� �������
// IEndpointRouteBuilder: ����������� ��� ��������� ���������, ������� �������������� � ���������

/////////////////////////////// ��� ���������� ������ ����� WebApplication ���������� ��������� ������: ////////////////

// Run(): ��������� ����������
// RunAsync(): ���������� ��������� ����������
// Start(): ��������� ����������
// StartAsync(): ��������� ����������
//StopAsync(): ������������� ����������

// ������ - ��� ���������, ������� ����� �������� � ����� ���� �������� �������, ��������, ������� ����� ����������