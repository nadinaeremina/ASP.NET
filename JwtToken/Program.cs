using JwtToken.Stub;
using UserChallange.Api.Middleware;
using UserChallange.Model.Service;
using UserChallange.Model.Users;
using UserChallange.Stub;
using Microsoft.AspNetCore.Authentication.JwtBearer; // дополнительно устанавливаем
using JwtToken.Api.JWT;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<UserScenarios>();
builder.Services.AddTransient<IUserRepository, UserStorageStub>();
builder.Services.AddTransient<IEncoder, EncoderStub>();
builder.Services.AddTransient<UserAdministrationScenarios>();

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

app.UseMiddleware<ErrorMiddleware>();

// добавить middleware в аутентификацию и авторизацию
app.UseAuthentication();
app.UseAuthorization();

app.Run();

// 'JWT-token' используется с готовой библиотекой, со встроенными инструментами ASP, с готовой схемой аутентификации
// снижает нагрузку на сервер и базу, ускоряет работу

///////////////////////////// существует 2 схемы работы с тз клиента приложения: /////////////////////////////////
// 1 // ввод учетных данных, сохранение сессии аутентификации некоторое время,
// по завершению которых требуется повторный ввод учетных данных (чаще всего это cookies)
// это нужно для удобства пользования приложением
// такая схема используется на фронтендовском сервере или mvc-приложении

// 2 // генерируется некоторый ключ, который затем необходимо добавлять в каждый запрос к серверу
// и по которому будет осуществляться аутентификация и авторизация 

// 'JWT-token' - json web token - это открытый стандарт для создания токенов доступа, основанный на формате JSON
// 'JWT-token' с помощью надежных алгоритмов шифрования позволяет
// закодировать различную информацию и выдать ее в виде токена
// кодируются:
// - данные опользователе ('claim') - логин, дата регистрации, пол, роль
// - время жизни токена (когда был создан и сколько должен прожить)
// - издатель/потребитель ('issuer'/'audience')
// на выходе мы получаем веб-токен, который позволяет с помощью алгоритмов 'Jwt'
// достать, декодировать различные параметры из токена и осуществлять с помощью этого
// авторизацию, аутентификацию
// 'Jwt' - это стандарт
// при его использовании - мы будем использовать готовое решение ASP для защиты ('Security Middleware')
// а сама проверка будет осуществляться через 'JWT'
// 'jwt.io' - сайт, где можно побаловаться с токенами
// токены, как правило, выписываются на 5 минут до получаса-часа (в основном 15 минут)

// 1 // сначала пользователь получает токен (аутентификация) по учетным данным
// учетные данные - креды - 'credentials' : логин, пароль / пин-код / api-ключ
// 2 // добавление jwt-токена в заголовки каждого запроса, требующего аутентификации
// 3 // по истечении жизни токена возвращаемся к п.1

// есть два подхода при защите 'endpoint':
// указать, что защищать (ASP-подход)
// указать, что не защищать (Java)
