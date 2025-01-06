using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;

var builder = WebApplication.CreateBuilder(args);

// ���������� ���� ������������
builder.Services.AddControllers();

// ���������� �������� ���������� 
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<CountryService>();
builder.Services.AddTransient<AirportService>();
builder.Services.AddTransient<CompanyService>();

var app = builder.Build();

// ���������������� ���������� // ����������� ����������� � ������������
app.MapControllers();

app.Run();

// ����������� - ������, ������� �������� ������-����������� http-�������� - ��� ������ �����������
// �������� ����� Add - Razor Component - Api Controller Empty

// ������ 'Model' - ��� ������������ ������ 'EntityFramework' � ��� � ���� ��������
// ������ 'Service' - ������ ������ � ��, ��� ��, ����� ��� ����� ��������, ������ � dbContext 
// ��� - ��, ��� �������� � EF (��������), ��������� API � ������ ������
// �� ����� ������ �������� �������� ��� ������, ��� ���������� ������ � EF
// ��� ������ ����� ���� ������ ��������, ������� ������ ����������, ������� ������ API

// ������ 'API' - �������������� ��� ��, ��� �� �������� - ������ ������� - ������� ������, ������� ������
// ����� �� �������� ������ �������� � ������� ���������

// ������� ������� ����������, ����� ������, ������� ������������-������� � ��������
