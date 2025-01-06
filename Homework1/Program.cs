var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    string current_time = $"{DateTime.Now:HH:mm:ss}";
    await context.Response.WriteAsync($"server is running {current_time}");
});

app.MapGet("/hi", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync("Hello world");
});

app.MapGet("/ping", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync("pong");
});

app.MapGet("/info", async (HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status200OK;
    int category=32;
    if (Environment.Is64BitOperatingSystem)
    {
        category = 64;
    }
    // ����� ���������� ��� 'else' - ��� ����� �������� ���, ��� ����� �� ��������
    await context.Response.WriteAsync($"��� ����������: {Environment.MachineName}\n" +
                                      $"������������ ������� ��������: {category}-���������\n" +
                                      $"��������� � ����� ������: {Environment.OSVersion}\n" +
                                      $"�����, �������� � ������� �������� ������: {Convert.ToInt32(Environment.TickCount / 1000 / 60)} �����\n" + 
                                      $"����� ������������ ���������� ������: {Environment.WorkingSet/1024/1024} ��������");
});

app.Run();