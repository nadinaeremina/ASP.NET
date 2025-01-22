var builder = WebApplication.CreateBuilder(args);

// ��������� 'Razor Pages' - ��������� � ������ ������� 'Razor Pages'
builder.Services.AddRazorPages();

var app = builder.Build();

// �������� ������� (�������������) ������� 'Razor Pages'
app.MapRazorPages();
// ������������� ��������� �����������  ������ ������� URL � ������������ ��������� Razor
// �� ��������� �� ������������ � ������� � ����� Pages

app.Run();

// ������ � ������� ������������ ������ �� ����������
// html+razor � C# page model - ������ ��������
// �������� ' Razor Page - Empty'