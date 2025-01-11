using Microsoft.AspNetCore.Http;
using RequestChallange.Api;
using RequestChallange.Model;
using RequestChallange.Storage;

var builder = WebApplication.CreateBuilder(args);

// �������� ����� ��� ����������� (��
// � ��� �� ����)
builder.Services.AddControllers();
// 'AddTransient' � ������� ��������� ������������ (DI) � ASP.NET Core
// ������ ����� ��������� ������� ������ ���, ����� ��� �����������.

// ���������� �������� � IoC-���������
builder.Services.AddTransient<RequestService>();
// 'IRequestRepository' � �������������� 'RequestStorage'
builder.Services.AddTransient<IRequestRepository, RequestStorage>();
// 'RequestStorage' ����� 'dbContext'
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// MapControllers() � ��� ����� � ASP.NET Core, ������� ������������ �������� �����������
// � ���������, ��������� ������������� �� ������ ���������
app.MapControllers();

// �������� � ��������� ����� 'ApplicationController' 
// handlers
//app.MapGet("/", () => new StringMessage(Message: "server is running"));
//app.MapGet("/ping", () => new StringMessage(Message: "pong"));

// �������� � ��������� ����� 'RequestController' 
// request handlers - �����������
//app.MapGet("/request", async (RequestService request) => await request.ListAllAsync());
//app.MapDelete("/request", async (RequestService request) =>
//{
//    await request.DeleteAllDataAsync();
//    return Results.NoContent();
//});

// �������� � ��������� ����� 'RequestMiddleware'
// 'RequestDelegate next' - ��������� ���������� �������
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    // ������������� ������� � ���������� ����������

//    // ������ ����� �������� ������
//    // �� ����� �������� ����������� � ���������� ������� ���������� ���������;
//    // 1 // ����� �������� Services ������� WebApplication(service locator)
//    // 2 // ����� �������� RequestServices ��������� ������� HttpContext � ����������� middleware(service locator)
//    // 3 // ����� ����������� ������
//    // 4 // ����� �������� ������ Invoke ���������� middleware
//    // 5 // ����� �������� Services ������� WebApplicationBuilder

//    ////////////// 1 /////////////// �������� �� ������ ���������� ����������� (next) /////////////////////////
//    // �������� ������ // ������� ��������� ������ 'RequestService'
//    RequestService request = context.RequestServices.GetRequiredService<RequestService>();
//    // 'GetRequiredService' � �����, ������� ��������� ��������
//    // ����������� ������ �� ���������� ��������� ������������ (DI)
//    var time = DateTime.UtcNow;

//    ////////////// 2 //////////////// ����� ���������� ����������� /////////////////////////////////////////////
//    await next(context);

//    ////////////// 3 /////////////// �������� ����� ������ next (���������� �����������) ////////////////////////
//    // ���������� ����� 'AddAsync' ������ 'RequestService',
//    // ������� ��������� ������ �� ��������� ������� � �������
//    await request.AddAsync(context.Request, context.Response, time);
//});

/////////////////////////////////// ��������� ������������ //////////////////////////////////////////////////////

// ��� ���������� ���������� middleware, ������� ������������ �����,
// � �������� ��������� ������� ����������� ����� UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<RequestMiddleware>();

app.Run();

// ������� � ��������� ���� 'ApiMessages'
// record StringMessage(string Message);

// ����� 'Model' - ���������� ������ ����������