var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ��� ��������
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));
// http://localhost:8080/greeting?name=Nadi
app.MapGet("/greeting", (string name) => new StringMessage(Message: $"Hello, {name}"));

// ���������� ������������ middleware-����������� - �� ���������� ������ ������� ASP
// �� ��������� ������� ���-�� �� ������� � �����, � ����� ����� ������ �� ������ � �����
// � ������ ����������� �������� 'HttpContext'
// � ������ ������ ����� ��� middleware ����� ��������� ��������� ��� �������
// 'RequestDelegate next' - ��������� ���������� �������
app.Use(async(HttpContext context, RequestDelegate next) =>
{
    // ����������� ����� ������� ������
    // 1 // �������� �� ������ ���������� ����������� (next)
    Console.WriteLine("Before Next");

    // 2 // ����� ���������� �����������
    // ���� �� ��� �� �����-�� �������� �� ������� - �� � ��� �� ����� �������������� ���������� ��������� (3 �����)
    await next(context);

    // 3 // �������� ����� ������ next (���������� �����������)
    Console.WriteLine("After Next");
});

// 'MiddleWare' ��������� ������� ����� �� ������� � �����, � ����� ����� ����� �� ������ � �����
// ����� ��������� ���: ������������ �������, ������ ������� �� ����, ��������������, �����������, ����������
// �����-�� ������� ��������, �������������� ������, ������ ������, ��������������� ����������� ������������ ������

app.Run();

record StringMessage(string Message);
