using UserController.Api.Middleware;
using UserController.Model.Service;
using UserController.Model.Users;
using UserController.Stub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<UserScenarios>();
builder.Services.AddTransient<IUserRepository, UserStorageStub>();
builder.Services.AddTransient<IEncoder, EncoderStub>();

var app = builder.Build();

app.MapControllers();

// выполняем конфигурацию

// Для добавления компонента middleware, который представляет класс,
// в конвейер обработки запроса применяется метод UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<SecurityMiddleware>();

app.Run();

// 1 // 'Model' - модель проекта - описание сущностей и их поведения в предметной области
// уровень модели ни от чего не зависит, все зависит от модели.

// 1.1 // 'Users' - пакет работы с пользователем, класс, описывающий сущность пользователя
// 'User' - класс, описывающий сущность пользователя
// 'UserScenarios' - класс со сценариями и поведением пользователей

// 1.2 // 'Exceptions' - пакет с исключениями модели 

// 1.3 // 'Service' - интерфейсы (контракты) внешних сервисов, которые нужны модели для работы
// 'IEncoder' - интерфейс шифровальщика для генерации API-ключей
// 'IUserRepository' - интерфейс хранилища пользователей

// 2 // Stub - пакет с имплементациями-заглушками  интерфейсов сервисов модели

// ping?????????????????????
// http://localhost:8080/api/resource - get, post