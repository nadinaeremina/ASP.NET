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
    // лучше обходиться без 'else' - чем более линейный код, тем более он читаемый
    await context.Response.WriteAsync($"Имя компьютера: {Environment.MachineName}\n" +
                                      $"Операционная система является: {category}-разрядной\n" +
                                      $"Платформа и номер версии: {Environment.OSVersion}\n" +
                                      $"Время, истекшее с момента загрузки ситемы: {Convert.ToInt32(Environment.TickCount / 1000 / 60)} минут\n" + 
                                      $"Обьем используемой физической памяти: {Environment.WorkingSet/1024/1024} мегабайт");
});

app.Run();