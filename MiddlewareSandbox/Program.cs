var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// это хэндлеры
app.MapGet("/", () => new StringMessage(Message: "server is running"));
app.MapGet("/ping", () => new StringMessage(Message: "pong"));
// http://localhost:8080/greeting?name=Nadi
app.MapGet("/greeting", (string name) => new StringMessage(Message: $"Hello, {name}"));

// добавление собственного middleware-обработчика - он становится частью цепочки ASP
// он позволяет сделать что-то до запроса и после, а также может влиять на запрос и ответ
// в лямбду обязательно передаем 'HttpContext'
// в данном случае через наш middleware будут проходить абсолютно все запросы
// 'RequestDelegate next' - следующий обработчик цепочки
app.Use(async(HttpContext context, RequestDelegate next) =>
{
    // обязательно нужно вызвать внутри
    // 1 // действия до вызова следующего обработчика (next)
    Console.WriteLine("Before Next");

    // 2 // вызов следующего обработчика
    // если мы его по каким-то причинам не вызовет - то у нас не будет осуществляться дальнейшая обработка (3 пункт)
    await next(context);

    // 3 // действия после вызова next (следующего обработчика)
    Console.WriteLine("After Next");
});

// 'MiddleWare' позволяет сделать чтото до запроса и после, а также может влять на запрос и ответ
// можно применять для: логгирование сервера, защита сервера от атак, аутентификация, авторизация, выполнение
// каких-то учетных действий, преобразование данных, запись ошибок, переопределение стандартных обработчиков ошибок

app.Run();

record StringMessage(string Message);
