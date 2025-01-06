using AirportDictionaryAsp_v1.Model;
using AirportDictionaryAsp_v1.Service;

var builder = WebApplication.CreateBuilder(args);

// добавление всех контроллеров
builder.Services.AddControllers();

// добавление сервисов приложения 
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<CountryService>();
builder.Services.AddTransient<AirportService>();
builder.Services.AddTransient<CompanyService>();

var app = builder.Build();

// конфигурирование приложения // привязываем контроллеры к обработчикам
app.MapControllers();

app.Run();

// контроллеры - классы, которые содержат методы-обработчики http-запросов - это логика отображения
// создание через Add - Razor Component - Api Controller Empty

// папака 'Model' - как организованы данные 'EntityFramework' и как с ними работать
// папака 'Service' - логика работы с БД, это то, какие нам нужны операции, работа с dbContext 
// это - то, что работает с EF (моделями), соединяет API и модель данных
// мы можем методы сервисов вызывать где угодно, они производят работу с EF
// эти классы можно куда угодно вытащить, достать нужную информацию, которую просит API

// папака 'API' - взаимодействие это то, как мы общаемся - задача простая - принять запрос, дернуть сервис
// чтобы он произвел нужную операцию и вернуть результат

// сначала создаем контроллер, потом сервис, сколько контроллеров-столько и сервисов
