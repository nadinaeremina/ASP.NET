using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// стандартный обработчик
app.MapGet("/", () => "server is runing");

// 1 // get-params
// Конвертер единиц информации из всех во все - поддерживать единицы информации б,кб,мб,гб,тб
// 1.1 // without sugar
// http://localhost:8080/convert/no-sugar?from=kb&to=mb&value=15550

app.MapGet("/convert/no-sugar", async (HttpContext context) =>
{
    try
    {
        // 1 считать данные
        // из какой единицы конвертировать
        string from = context.Request.Query["from"][0]!;
        // в какую единицу конвертировать
        string to = context.Request.Query["to"][0]!;
        // значение
        double value = Convert.ToDouble(context.Request.Query["value"][0]);

        // 2 выполнить конвертацию (метод в лоб - перевести сначала в байты, затем в целевую величину)
        double bytes = 0;
        if (from == "b")
        {
            bytes = value;
        }
        else if (from == "kb")
        {
            // переводим килобайты в байты
            bytes = value * 1024;
        }
        else if (from == "mb")
        {
            // переводим мегабайты в байты
            bytes = value * 1024 * 1024;
        }
        else if (from == "gb")
        {
            // переводим гигабайты в байты
            bytes = value * 1024 * 1024 * 1024;
        }
        else if (from == "tb")
        {
            // переводим терабайты в байты
            bytes = value * 1024 * 1024 * 1024 * 1024;
        }
        else
        {
            throw new ArgumentException($"unknown unit: {from}; allowed units: b, kb, mb, gb, tb;");
        }
        double result = 0;
        if (to == "b")
        {      
            result = bytes;
        }
        else if (to == "kb")
        {
            // переводим байты в килобайты
            result = bytes / 1024;
        }
        else if (to == "mb")
        {
            // переводим байты в мегабайты
            result = bytes / (1024 * 1024);
        }
        else if (to == "gb")
        {
            // переводим байты в гигабайты
            result = bytes / (1024 * 1024 * 1024);
        }
        else if (to == "tb")
        {
            // переводим байты в терабайты
            result = bytes / (1024L * 1024 * 1024 * 1024);
            // '1024L' - литерал 'long' (если мы на него разделим - то получим в рез-те тоже 'long')
        }
        else
        {
            throw new ArgumentException($"unknown unit: {from}; allowed units: b, kb, mb, gb, tb;");
        }

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync($"{value} {from}. = {result} {to}.");

    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 1.2 // with sugar
// http://localhost:8080/convert/sugar?from=tb&to=kb&value=15500
// этот код более универсальный, расширенный

app.MapGet("/convert/sugar", (string from, string to, double value) =>
{
    try
    {
        Dictionary<string, long> coefficients = new Dictionary<string, long>()
        {
            { "b", 1L},
            { "kb", 1L << 10}, // 2 в 10 степени
            { "mb", 1L << 20}, // 2 в 20 степени
            { "gb", 1L << 30}, // 2 в 30 степени
            { "tb", 1L << 40}  // 2 в 40 степени
            // единичку сдвинуть влево на 10, 20, 30 или 40 побитово
            // сюда можно добавлять чтото еще, код будет работать корректно
        };
        // если наш словарь не содержит такой ключ, который введен в 'to' или 'from'
        if (!coefficients.ContainsKey(from) || !coefficients.ContainsKey(to))
        {
            return $"there are inknown units: {from} or {to}; allowed units: {string.Join(",", coefficients.Keys)}";
            // 'string.Join()' - метод позволяет конкатенировать коллекцию элементов в одну строку с использованием
            // указанного разделителя. Если коллекция не имеет элементов, метод возвращает пустую строку (String.Empty)
        }
        double result = value * coefficients[from] / coefficients[to];
        return $"{value} {from}. = {result} {to}.";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
});

// 2 // body
// 2.1 // without sugar
// Теорема Пифагора - в запросе передать значение двух катетов, вернуть значение гипотенузы в ответе
// http://localhost:8080/body-data
// пар-ры передаются в 'body-raw-text' через запятую 

app.MapPost("/body-data", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры
        using (StreamReader sr = new StreamReader(context.Request.Body))
        {
            string data = await sr.ReadToEndAsync();
            string[] tokens = data.Split(',');
            double a = Convert.ToDouble(tokens[0]);
            double b = Convert.ToDouble(tokens[1]);

            // 2 выполнить работу с нимим
            double result = Math.Sqrt(a*a + b*b);

            // 3 записать и отправить рез-т
            await context.Response.WriteAsync(result.ToString());
        }
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 2.2 with sugar

// http://localhost:8080/body-data-sugar
// ASP для автоматической распаковки использует либо json, либо xml-формат
// текст при этом находится в 'body-row-json' в кавычках
// в 'Hraders' должен быть 'Content-Type application/json'

app.MapPost("/body-data-sugar", ([FromBody] string data) =>
{
    // считает строку только, если она в кавычках (как 'json')
    // 1 считать входные пар-ры
    try
    {
        // 1 считать входные пар-ры
        string[] tokens = data.Split(',');
        double a = Convert.ToDouble(tokens[0]);
        double b = Convert.ToDouble(tokens[1]);

        // 2 выполнить работу с нимим
        return $"Гипотенуза равна: {Math.Sqrt(a * a + b * b)}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
});

// 3 // header
// 3.1 // without sugar
// на вход подается строка, состояющая только из английский маленьких букв (иначе ошибка)
// обработчик должен выполнить сжатие данной строки следующим образом:
// "aaaaaabbbbbccdcaaa"-> "6a5b2cdc3a"
// Если какая-то буква повторяется 1 раз, то 1 писать не надо, сразу символ
// http://localhost:8080/header

app.MapGet("/header", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры

        // заголовки есть в 'Response' и в 'Request', в 'Response' мы их пишем, а в 'Request' мы их считываем
        // некоторые заголовки присутствуют в виде св-в
        // "Headers" - кол-ция, где ключи - это имена заголовков и у каждого из них есть массив значений
        // достаем пар-ры по ключу и по индексу ('!' - уверены, что там не nullable)
        char[] data = (context.Request.Headers["My-Header-Data"][0]!).ToCharArray();
        string result = "";

        // 2 выполнить работу с нимим
        int count = 1;
        for (int i = 0; i < data.Length; i++)
        {
            if (i == data.Length - 1 || data[i] != data[i + 1])
            {
                if (count != 1)
                {
                    result += count;
                }
                result += data[i];
                count = 1;
                if (i == data.Length - 1)
                {
                    break;
                }
            }
            if (data[i] == data[i+1])
            {
                count++;
            }
        }

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(result);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 3.2 with sugar
// можно передавать имя заголовка (Name = "My-Header-Data"), а можно не передавать
// http://localhost:8080/header-sugar

app.MapGet("/header-sugar", ([FromHeader(Name = "My-Header-Data")] string data_string) =>
{
    try
    {
        // 1 считать входные пар - ры
        char[] data = data_string.ToCharArray();
        // 2 выполнить работу с нимим
        string result = "";
        int count = 1;
        for (int i = 0; i < data.Length; i++)
        {
            if (i == data.Length - 1 || data[i] != data[i + 1])
            {
                if (count != 1)
                {
                    result += count;
                }
                result += data[i];
                count = 1;
                if (i == data.Length - 1)
                {
                    break;
                }
            }
            if (data[i] == data[i + 1])
            {
                count++;
            }
        }

        // 3 записать и отправить рез-т
        return $"result: {result}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

// 4 // post-param
// 4.1 // without sugar
// вычисление факториала - вычислить n!, n - целое, 0 <= n <= 20
// http://localhost:8080/post-params

app.MapPost("/post-params", async (HttpContext context) =>
{
    try
    {
        // пар-ры передаем в 'form-data'
        // 1 считать входные пар-ры

        // из 'Request' можно достать get-пар-ры
        // "Form" - аналог "Query", здесь хранятся данные из тел-запросов
        // кол-ция, где ключи - это имена пар-ов и у каждого из них есть массив значений
        // достаем пар-ры по ключу и по индексу ('!' - уверены, что там не nullable)
        int number = Convert.ToInt32(context.Request.Form["number"][0]!);

        // 2 выполнить работу с нимим
        if (number > 20 || number < 0)
        {
            await context.Response.WriteAsync($"Enter other number! The range should be from 0 to 20");
            return;
        }
        int factorial = 1;
        for (int i = 1; i <= number; i++)
        {
            factorial *= i;
        }
        // 3 записать и отправить рез-т
        await context.Response.WriteAsync(factorial.ToString());
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 4.2 with sugar
// http://localhost:8080/post-params-sugar
// пар-ры передаем в 'form-data'

app.MapPost("/post-params-sugar", ([FromForm] int number) =>
{
    try
    {
        // 2 выполнить работу с нимим
        if (number > 20 || number < 0)
        {
            return "Enter other number! The range should be from 0 to 20";
        }
        int factorial = 1;
        for (int i = 1; i <= number; i++)
        {
            factorial *= i;
        }
        // 3 записать и отправить рез-т
        return factorial.ToString();
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

// 5 // url
// 5.1 // without sugar
// По радиусу посчитать и вернуть площадь и длину окружности
// http://localhost:8080/url-vars/5,5

app.MapGet("/url-vars/{radius:double}", async (HttpContext context) =>
{
    try
    {
        // 1 считать входные пар-ры
        double radius = Convert.ToDouble(context.Request.RouteValues["radius"]);

        // 2 выполнить работу с нимим
        double square = Math.PI * (radius * radius);
        double length = 2 * Math.PI * radius;

        // 3 записать и отправить рез-т
        await context.Response.WriteAsync($"Площадь окружности: {square}, длина окружности: {length}");
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 5.2 with sugar
// http://localhost:8080/url-vars-sugar/5

app.MapGet("/url-vars-sugar/{radius:double}", (double radius) =>
{
    try
    {
        // 2 выполнить работу с нимим
        return $"Площадь окружности: {Math.PI * (radius * radius)}, длина окружности: {2 * Math.PI * radius}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // защита отключена (антикража, антиподмена-для безопсаности)

app.Run();
