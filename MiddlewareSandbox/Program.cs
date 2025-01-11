using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ��� ��������
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));
// http://localhost:8080/greeting?name=Nadi
app.MapGet("/greeting", (string name) => new StringMessage(Message: $"Hello, {name}"));

// ���������� ������������ middleware-����������� ����� ����� "Use" - �� ���������� ������ ������� ASP
// �� ��������� ������� ���-�� �� ������� � �����, � ����� ����� ������ �� ������ � �����
// middleware � ������ Use ��������� �������� �� ���������� � ��������� ���������� � ����� ����
// � ������ ����������� �������� 'HttpContext'
// � ������ ������ ����� ��� middleware ����� ��������� ��������� ��� �������
// 'RequestDelegate next' - ��������� ���������� �������
app.Use(async(HttpContext context, RequestDelegate next) =>
{
    // 1 // �������� �� ������ ���������� ����������� (next)
    Console.WriteLine("Before Next");

    // 2 // ����� ���������� �����������
    // ����������� ����� ������� ������
    // ���� �� ��� �� �����-�� �������� �� ������� - �� � ��� �� ����� �������������� ���������� ��������� (3 �����)
    await next(context);
    // await next.Invoke();

    // 3 // �������� ����� ������ next (���������� �����������)
    Console.WriteLine("After Next");
});

// 'MiddleWare' ��������� ������� ����� �� ������� � �����, � ����� ����� ����� �� ������ � �����
// ����� ��������� ���: ������������ �������, ������ ������� �� ����, ��������������, �����������, ����������
// �����-�� ������� ��������, �������������� ������, ������ ������, ��������������� ����������� ������������ ������

app.Run();

record StringMessage(string Message);

// Middleware ����� ���� ������� � ������� ������ � ������� InvokeAsync() � ���������� ���� RequestDelegate � ������������
// ��� RequestDelegate ��������� ��� ���������� ���������� middleware � ������������������.

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

// ������ middleware-��������� � ASP.NET Core:
// ����� ������ ��� � �������� ��������, ��� � � ������������ ������� �������
// ����� ������ �������� ������ ���������� middleware � ���������
// ����� ��������� ��������� ������ ��������� � ����� ���������� ���� ������ ���������� middleware ��� ���������� ���������
// ��� ������������� ����� ��������� (��������) �������� ��������
// ����������� � ��� �������, � ������� �� ��� ��������� � ��������

// app.Run()
// ��������� middleware-��������� � ���� Run[Middleware], ������� ���������� � ����� ���������
// ��� �������, �� ��������� ��� ���������� middleware � ����������� � ����� ��������� ��������,
// ��������� �� ����� �������� ��������� middleware-��������� (next)

// app.Use()
// ����� ������������ ��� ���������������� ���������� middleware
// � ������� �� app.Run(), �� ����� �������� � ���� �������� next,
// ������� �������� ��������� ������� ������� � ���������
// �� ����� ����� �������� (���������) ��������, �� ������� �������� next
