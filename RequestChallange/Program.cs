using Microsoft.AspNetCore.Http;
using RequestChallange.Model;
using RequestChallange.Storage;

var builder = WebApplication.CreateBuilder(args);
// ���������� �������� � IoC-���������
builder.Services.AddTransient<RequestService>();
// 'IRequestRepository' � �������������� 'RequestStorage'
builder.Services.AddTransient<IRequestRepository, RequestStorage>();
// 'RequestStorage' ����� 'dbContext'
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// handlers
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));

// request handlers - �����������
app.MapGet("/request", async (RequestService requests) => await requests.ListAllAsync());
app.MapDelete("/request", async (RequestService requests) =>
{
    await requests.DeleteAllDataAsync();
    return Results.NoContent();
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // �� ����� �������� ����������� � ���������� ������� ���������� ���������;
    // 1 // ����� �������� Services ������� WebApplication(service locator)
    // 2 // ����� �������� RequestServices ��������� ������� HttpContext � ����������� middleware(service locator)
    // 3 // ����� ����������� ������
    // 4 // ����� �������� ������ Invoke ���������� middleware
    // 5 // ����� �������� Services ������� WebApplicationBuilder

    ////////////// 1 /////////////// �������� �� ������ ���������� ����������� (next) /////////////////////////
    // �������� ������ // ������� ��������� ������ 'RequestService'
    RequestService request = context.RequestServices.GetRequiredService<RequestService>();
    // 'GetRequiredService' � �����, ������� ��������� ��������
    // ����������� ������ �� ���������� ��������� ������������ (DI)
    var time = DateTime.UtcNow;

    ///////////// 2 //////////////// ����� ���������� ����������� /////////////////////////////////////////////
    await next(context);

    //////////// 3 /////////////// �������� ����� ������ next (���������� �����������) ////////////////////////
    // ���������� ����� 'AddAsync' ������ 'RequestService',
    // ������� ��������� ������ �� ��������� ������� � �������
    await request.AddAsync(context.Request, context.Response, time);
}); 

app.Run();

record StringMessage(string Message);

// ����� 'Model' - ���������� ������ ����������