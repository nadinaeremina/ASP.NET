using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// два варианта на каждый случай
// без сахара -  с передачей 'HttpContext context'
// с сахаром - без явной передачи 'HttpContext context'

// 1 // get-params 
// 1.1 without sugar
// http://localhost:8080/get-params?a=15&b=hello
// параметры можно вписывать в строку, а можно во вкладке 'params'

app.MapGet("/get-params", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры

        // из 'Request' можно достать get-пар-ры
        //"QueryString" - строка с get-пар-ми, но парсить ее неудобно - поэтому используем "Query"
        // "Query" - кол-ция, где ключи - это имена пар-ов и у каждого из них есть массив значений
        // достаем пар-ры по ключу и по индексу ('!' - уверены, что там не nullable)
        // приводит 'nullable' к 'not null'
        int a = Convert.ToInt32(context.Request.Query["a"][0]!);
        // здесь берем значение "a" - возвращается всегда ввиде строки - поэтому конвертируем в "int"
        string b = context.Request.Query["b"][0]!;

        // 2 выполнить работу с нимим
        string reply = $"[get-params]: received a = {a}; b = {b}.";

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(reply);

    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 1.2 with sugar
// http://localhost:8080/get-params-sugar?a=15&b=hello

app.MapGet("/get-params-sugar", (int a, string b) =>
{
    // 2 выполнить работу с нимим
    return $"[get-params]: received a = {a}; b = {b}.";
});
// обрабатывает ошибки по-своему
// несахарный вариант можно везде применить, а сахарный не везде (бывает, что нужен контекст)

// 2 // post-params (передача пар-ов в теле запроса)
// 2.1 without sugar
// http://localhost:8080/post-params
// пар-ры передаем в 'body' - 'form-data'

app.MapPost("/post-params", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры

        // из 'Request' можно достать get-пар-ры
        // "Form" - аналог "Query", здесь хранятся данные из тел-запросов
        // кол-ция, где ключи - это имена пар-ов и у каждого из них есть массив значений
        // достаем пар-ры по ключу и по индексу ('!' - уверены, что там не nullable)
        int a = Convert.ToInt32(context.Request.Form["a"][0]!);
        // здесь берем значение "a" - возвращается всегда ввиде строки - поэтому конвертируем в "int"
        string b = context.Request.Form["b"][0]!;

        // 2 выполнить работу с нимим

        string reply = $"[post-params]: received a = {a}; b = {b}.";

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(reply);

    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 2.2 with sugar
// http://localhost:8080/post-params-sugar
// пар-ры передаем в 'body' - 'form-data'

app.MapPost("/post-params-sugar", ([FromForm] int a, [FromForm] string b) =>
{
    // 2 выполнить работу с нимим
    return $"[post-params]: received a = {a}; b = {b}.";
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

// "FromForm" - чтобы пар-ры извлекались из FormData, а не из строки запроса, иначе это будут get-пар-ры
// тк включена ненастроенная автоматическая защита - нам нужно ее отключить - защита от отправки запроса не из формы
// потому что мы не из формы отправляем, а из 'Postman'

// 3.1 // headers // without sugar
// http://localhost:8080/header

app.MapGet("/header", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры

        // заголовки есть в 'Response' и в 'Request', в 'Response' мы их пишем, а в 'Request' мы их считываем
        // некоторые заголовки присутствуют в виде св-в (Accept, UserAgent)
        // "Headers" - кол-ция, где ключи - это имена заголовков и у каждого из них есть массив значений
        // достаем пар-ры по ключу и по индексу ('!' - уверены, что там не nullable)
        string data = context.Request.Headers["My-Header-Data"][0]!;

        // 2 выполнить работу с нимим
        string reply = $"[header]: received '{data}' header";

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(reply);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 3.2 with sugar
// можно передавать имя заголовка (Name = "My-Header-Data"), а можно не передавать
// http://localhost:8080/header-sugar

app.MapGet("/header-sugar", ([FromHeader(Name = "My-Header-Data")] string data) =>
{
    // 2 выполнить работу с нимим
    return $"[header]: received '{data}' header.";
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

// 4.1 // url-vars (path-vars) // without sugar
// можно исп-ть с 'get' и 'put' запросами
// 'number' - некая переменная, указываем типизацию - она позволит огранить возможные значения (rout parameter)
// 'int' - десятичные цифры, 'alpha' - буквы и др, regex()-регулярка

// http://localhost:8080/url-vars/15
// http://localhost:8080/url-vars/hello - не сработает

// в фигурных скобках пишем название переменной и ее типизацию
app.MapGet("/url-vars/{number:int}", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры // получаем по ключу
        int number = Convert.ToInt32(context.Request.RouteValues["number"]);

        // 2 выполнить работу с нимим
        string reply = $"[header]: received number = {number}";

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(reply);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 4.2 with sugar
// http://localhost:8080/url-vars-sugar/15

app.MapGet("/url-vars-sugar/{number:int}", (int number) =>
{
    // 2 выполнить работу с нимим
    return $"[header]: received number = {number}";
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

// 5.1 // http-request body // without sugar
// можно исп-ть с любыми запросами, кроме get, set, options
// могут быть: put, patch, delete (чаще всего - это post)
// http://localhost:8080/body-data
// текст при этом находится в 'body-row-text'

app.MapPost("/body-data", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры
        using (StreamReader sr = new StreamReader(context.Request.Body))
        {
            string data = await sr.ReadToEndAsync();
            // 'ReadToEndAsync' - асинхронно считывает все символы, начиная с текущей позиции до конца потока
            // возвращает их в виде одной строки

            // 2 выполнить работу с нимим
            string reply = $"[body-data]: received '{data}' body";

            // 3 записать и отправить рез-т
            await context.Response.WriteAsync(reply);
        }
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 5.2 with sugar
// http://localhost:8080/body-data-sugar
// ASP для автоматической распаковки использует либо json, либо xml-формат
// текст при этом находится в 'body-row-json' в кавычках
// в 'Hraders' должен быть 'Content-Type application/json'

app.MapPost("/body-data-sugar", ([FromBody] string data) =>
{
    // считает строку только, если она в кавычках (как json)
    // 2 выполнить работу с нимим
    return $"[body-data-sugar]: received '{data}' body";
});

app.Run();
