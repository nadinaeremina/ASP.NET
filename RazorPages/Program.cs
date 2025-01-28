var builder = WebApplication.CreateBuilder(args);

// подкючаем 'Razor Pages' - добавляем в проект сервисы 'Razor Pages'
builder.Services.AddRazorPages();

var app = builder.Build();

// выполним маппинг (маршрутизация) страниц 'Razor Pages'
app.MapRazorPages();
// маршрутизация позволяет сопоставить  строку запроса URL с определенной страницей Razor
// на основании ее расположения в проекте в папке Pages

app.Run();

// сервис и маппинг обеспечивают работу со страницами
// html+razor и C# page model - модель страницы
// выбираем ' Razor Page - Empty'