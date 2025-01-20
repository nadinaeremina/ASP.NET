using JwtToken.Stub;
using UserChallange.Api.Middleware;
using UserChallange.Model.Service;
using UserChallange.Model.Users;
using UserChallange.Stub;
using Microsoft.AspNetCore.Authentication.JwtBearer; // ������������� �������������
using JwtToken.Api.JWT;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<UserScenarios>();
builder.Services.AddTransient<IUserRepository, UserStorageStub>();
builder.Services.AddTransient<IEncoder, EncoderStub>();
builder.Services.AddTransient<UserAdministrationScenarios>();

// � ASP ���� ���������� 'middleware' ��� ����������� � ��������������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtService.ConfigureJWTOptions);
// ��������� ������������ (�������� ������ ����� - ����� ������ ������)
// 'AuthenticationScheme' - ��� ������ �����
// 'AddJwtBearer' - ������������

// ����� �������� ����� // ���������� ��� ���������������� Jwt
builder.Services.AddAuthorization();
// ��� 2 ������� ��������� ������� �������� 'Authorize' � �����������
// ��������� ���������� ������

// ��������� ��� ������ ����� �� 'JwtService' (�������������)
builder.Services.AddTransient<JwtService>();

var app = builder.Build();

app.MapControllers();

app.UseMiddleware<ErrorMiddleware>();

// �������� middleware � �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

app.Run();

// 'JWT-token' ������������ � ������� �����������, �� ����������� ������������� ASP, � ������� ������ ��������������
// ������� �������� �� ������ � ����, �������� ������

///////////////////////////// ���������� 2 ����� ������ � �� ������� ����������: /////////////////////////////////
// 1 // ���� ������� ������, ���������� ������ �������������� ��������� �����,
// �� ���������� ������� ��������� ��������� ���� ������� ������ (���� ����� ��� cookies)
// ��� ����� ��� �������� ����������� �����������
// ����� ����� ������������ �� �������������� ������� ��� mvc-����������

// 2 // ������������ ��������� ����, ������� ����� ���������� ��������� � ������ ������ � �������
// � �� �������� ����� �������������� �������������� � ����������� 

// 'JWT-token' - json web token - ��� �������� �������� ��� �������� ������� �������, ���������� �� ������� JSON
// 'JWT-token' � ������� �������� ���������� ���������� ���������
// ������������ ��������� ���������� � ������ �� � ���� ������
// ����������:
// - ������ ������������� ('claim') - �����, ���� �����������, ���, ����
// - ����� ����� ������ (����� ��� ������ � ������� ������ �������)
// - ��������/����������� ('issuer'/'audience')
// �� ������ �� �������� ���-�����, ������� ��������� � ������� ���������� 'Jwt'
// �������, ������������ ��������� ��������� �� ������ � ������������ � ������� �����
// �����������, ��������������
// 'Jwt' - ��� ��������
// ��� ��� ������������� - �� ����� ������������ ������� ������� ASP ��� ������ ('Security Middleware')
// � ���� �������� ����� �������������� ����� 'JWT'
// 'jwt.io' - ����, ��� ����� ������������ � ��������
// ������, ��� �������, ������������ �� 5 ����� �� ��������-���� (� �������� 15 �����)

// 1 // ������� ������������ �������� ����� (��������������) �� ������� ������
// ������� ������ - ����� - 'credentials' : �����, ������ / ���-��� / api-����
// 2 // ���������� jwt-������ � ��������� ������� �������, ���������� ��������������
// 3 // �� ��������� ����� ������ ������������ � �.1

// ���� ��� ������� ��� ������ 'endpoint':
// �������, ��� �������� (ASP-������)
// �������, ��� �� �������� (Java)
