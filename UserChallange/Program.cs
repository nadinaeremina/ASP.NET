using UserController.Api.Middleware;
using UserController.Model.Service;
using UserController.Model.Users;
using UserController.Stub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<UserScenarios>();
builder.Services.AddTransient<IUserRepository, UserStorageStub>();
builder.Services.AddTransient<IEncoder, EncoderStub>();

var app = builder.Build();

app.MapControllers();

// ��������� ������������

// ��� ���������� ���������� middleware, ������� ������������ �����,
// � �������� ��������� ������� ����������� ����� UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<SecurityMiddleware>();

app.Run();

// 1 // 'Model' - ������ ������� - �������� ��������� � �� ��������� � ���������� �������
// ������� ������ �� �� ���� �� �������, ��� ������� �� ������.

// 1.1 // 'Users' - ����� ������ � �������������, �����, ����������� �������� ������������
// 'User' - �����, ����������� �������� ������������
// 'UserScenarios' - ����� �� ���������� � ���������� �������������

// 1.2 // 'Exceptions' - ����� � ������������ ������ 

// 1.3 // 'Service' - ���������� (���������) ������� ��������, ������� ����� ������ ��� ������
// 'IEncoder' - ��������� ������������� ��� ��������� API-������
// 'IUserRepository' - ��������� ��������� �������������

// 2 // Stub - ����� � ���������������-����������  ����������� �������� ������

// ping?????????????????????
// http://localhost:8080/api/resource - get, post