using JsonAPIEncryption.Encryption;
using JsonAPIEncryption.Messages;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// добавление сервиса 'IEncoder' с имплементацией 'MD5Encoder' в наш 'IoC-контейнер'
// builder.Services.AddTransient<IEncoder, MD5Encoder>();
// добавление сервиса 'IEncoder' с имплементацией 'BCryptEncoder' в наш 'IoC-контейнер'
//builder.Services.AddTransient<IEncoder, BCryptEncoder>();

// используем фабрику дл€ удобства использовани€ приложени€
builder.Services.AddTransient(opts => EncoderFactory.CreateEncoder()); // добавили фабрику
// 'generic' уже можно не писать, потому что метод возвращает интерфейс

var app = builder.Build();

// 'Process.GetCurrentProcess' Ч статический метод класса System.Diagnostics,
// который возвращает новый объект Process, представл€ющий процесс, который €вл€етс€ активным в текущий момент
// 'StartTime' - получаем врем€ запуска текущего процесса

// without sugar
app.MapGet("/", async(HttpContext context) =>
{
    var result = new StringTimeMessage()
    {
        StartTime = Process.GetCurrentProcess().StartTime.ToUniversalTime().ToString("HH:mm:ss"),
        NowTime = DateTime.Now.ToString("HH:mm:ss"),
        Message = "server is running"
    };
    await context.Response.WriteAsJsonAsync(result);
});

// with sugar
app.MapGet("/sugar", () => new StringTimeMessage() {
    StartTime = Process.GetCurrentProcess().StartTime.ToUniversalTime().ToString("HH:mm:ss"),
    NowTime = DateTime.Now.ToString("HH:mm:ss"),
    Message = "server is running" 
});

// without sugar
app.MapGet("/ping", async (HttpContext context) =>
{
    var result = new StringMessage()
    {
        Message = "pong"
    };
    await context.Response.WriteAsJsonAsync(result);
});

// with sugar
app.MapGet("/ping-sugar", () => new StringMessage() { Message = "pong" });

// пример запроса:
//{
//    "data": "gtdhfghjhjkkkkkk",
//    "count": 1
//}

// without sugar
app.MapPost("/encode", async (HttpContext context, IEncoder encoder) =>
{
    try
    {
        // 1 // считываем входные данные
        StringData? data = null;
        try
        {
            data = await context.Request.ReadFromJsonAsync<StringData>();
            // 'ReadFromJsonAsync' - позвол€ет читать JSON из запроса и десериализовать его в объект определЄнного типа
            // 'ReadToEndAsync ' - асинхронно считывает все символы с текущей позиции до конца средства чтени€ текста
            // и возвращает их в виде одной строки.
        }
        // если 'raw' совершенно пуста€
        catch (InvalidOperationException ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "qwery is empty");
            await context.Response.WriteAsJsonAsync(error);
        }
        // если в поле 'data' отсутствуют данные // не всегда работает ????????????????????
        catch (InvalidEncodingDataException ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "in the 'data' field");
            await context.Response.WriteAsJsonAsync(error);
        }
        // если неправильный формат json (ошибки в синтаксисе)
        catch (JsonException ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "incorrect data format");
            await context.Response.WriteAsJsonAsync(error);
        }
        catch (Exception ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "1");
            await context.Response.WriteAsJsonAsync(error);
            return;
        }

        // 2 // выполнить шифрование
        // IEncoder mEncoder = context.RequestServices.GetRequiredService<IEncoder>();
        string encodedData = encoder.Encode(data.Data, data.Count);

        // 4 // отправить ответ
        EncodingResult result = new EncodingResult(encodedData, encoder.AlgorithmName, data.Count);
        // 200
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(result);
    }
    // когда попадаем сюда ????????????????????????
    catch (ArgumentException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, "2a");
        await context.Response.WriteAsJsonAsync(error);
    }
    // когда попадаем сюда ????????????????????????
    catch (InvalidOperationException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, "2iщ");
        await context.Response.WriteAsJsonAsync(error);
    }
    // не всегда работает ?????????????????????
    // если обьект не подходит дл€ сериализации
    // например он такой: 
    //{
    //    "id": 5,
    //    "name": " альций",
    //    "code": "Ca",
    //    "group": 2,
    //    "period": 4,
    //    "protonsNumber": 20
    //}
    catch (InvalidEncodingDataException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, "it is impossible to serialize");
        await context.Response.WriteAsJsonAsync(error);
    }
    // все остальные уже улет€т в 500
    catch (Exception ex)
    {
        // 500
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        await context.Response.WriteAsJsonAsync(error);
    }
});

// with sugar
// 'Results Api'
// 'Results Api' - оболочка, котора€ позвол€ет формировать результаты запросов,
// устанавлива€ соответствующие коды через вызовы нужных методов

// можно €вно указывать 'IResult', а можно вообще не указывать 
app.MapPost("/encode-sugar", IResult (StringData data, IEncoder encoder) =>
{
    try
    {
        // 2 // выполнить шифрование
        // IEncoder mEncoder = context.RequestServices.GetRequiredService<IEncoder>();
        string encodedData = encoder.Encode(data.Data, data.Count);

        // 4 // отправить ответ
        EncodingResult result = new EncodingResult(encodedData, encoder.AlgorithmName, data.Count);
        // 200
        // отправить ответ // 200 // OK
        return Results.Ok(result); // 200
    }
    // когда попадаем сюда ????????????????????????
    catch (ArgumentException ex)
    {
        // 400
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        return Results.BadRequest(error);
    }
    // когда попадаем сюда ????????????????????????
    catch (InvalidOperationException ex)
    {
        // 400
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        return Results.BadRequest(error);
    }
    catch (InvalidEncodingDataException ex)
    {
        // 400 // BadRequest
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        return Results.BadRequest(error);
    }
    catch (Exception ex)
    {
        // 500 // Internal Server Error
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        return Results.InternalServerError(error);
    }
});

app.Run();