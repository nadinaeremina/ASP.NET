var builder = WebApplication.CreateBuilder(args);

// подкючаем 'Razor Pages' - добавляем в проект сервисы 'Razor Pages'
builder.Services.AddRazorPages();

var app = builder.Build();

// выполним маппинг страниц 'Razor Pages'
app.MapRazorPages();

app.Run();
