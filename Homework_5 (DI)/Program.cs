using Homework_5__DI_;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITimeInformator, CurrentTime>(); 

var app = builder.Build();

app.MapGet("/", () => new StringMessage() { Message = "Server started and ready to work" });

app.MapGet("/ping", () => new StringMessage() { Message = "pong" });

// без сахара
app.MapGet("/time-no-sugar", (HttpContext context) =>
{
    ITimeInformator service = context.RequestServices.GetRequiredService<ITimeInformator>();
    return service.GetTimeStringMessage();
});

// с сахаром
app.MapGet("/time", (ITimeInformator service) =>
{
    return service.GetTimeStringMessage();
});

app.Run();
