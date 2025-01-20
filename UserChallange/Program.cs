using JwtToken.Api.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserChallange.Api.Middleware;
using UserChallange.Model.Crypto;
using UserChallange.Model.Service;
using UserChallange.Model.Users;
using UserChallange.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<UserScenarios>();
builder.Services.AddTransient<UserAdministrationScenarios>();
builder.Services.AddTransient<IUserRepository, UserStorage>();
builder.Services.AddDbContext<ApplicationDbContext>();

// добавили фабрику
builder.Services.AddTransient(opts => EncoderFactory.CreateEncoder());

// в ASP есть встроенный 'middleware' для авторизации и аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtService.ConfigureJWTOptions);
// выполнили конфигурацию (передаем данный метод - можно просто лямбду)
// 'AuthenticationScheme' - это просто схема
// 'AddJwtBearer' - конфигурация

// берем название схемы // необходимо еще сконфигурировать Jwt
builder.Services.AddAuthorization();
// эти 2 сервиса проверяют наличие атрибута 'Authorize' у обработчика
// проверяют переданные данные

// добавляем наш второй метод из 'JwtService' (нестатический)
builder.Services.AddTransient<JwtService>();

var app = builder.Build();

app.MapControllers();

// выполняем конфигурацию

// Для добавления компонента middleware, который представляет класс,
// в конвейер обработки запроса применяется метод UseMiddleware().
app.UseMiddleware<ErrorMiddleware>();
app.UseMiddleware<SecurityMiddleware>();

// добавить middleware в аутентификацию и авторизацию
app.UseAuthentication();
app.UseAuthorization();

app.Run();

// цель приложения:
// зарегать пользователя и получть его апи-ключ после успешной регистрации
// получить пользователя по апи-ключу

// 1 // 'Model' - модель проекта - описание сущностей и их поведения в предметной области
// уровень модели ни от чего не зависит, все зависит от модели.

// 1.1 // 'Users' - пакет работы с пользователем, класс, описывающий сущность пользователя
// 'User' - класс, описывающий сущность пользователя
// 'UserScenarios' - класс со сценариями и поведением пользователей

// 1.2 // 'Exceptions' - пакет с исключениями модели 

// 1.3 // 'Service' - интерфейсы (контракты) внешних сервисов, которые нужны модели для работы
// 'IEncoder' - интерфейс шифровальщика для генерации API-ключей
// 'IUserRepository' - интерфейс хранилища пользователей

// 2 // Stub - пакет с имплементациями-заглушками интерфейсов сервисов модели

// 3 // Api - интерфейс взаимодействия программы с внешним миром, реализация json в Api (JSON WEB API)
// 3.1 // 'Controllers' пакет с классами-контроллерами
// 3.2 // 'Messages' - пакет с сообщениями Api
// 3.2 // 'Middleware' - пакет с Middleware Api

// http://localhost:8080/api/resource - get, post

// тк в шарпе есть рефлексия - можно получить любую информацию о типах