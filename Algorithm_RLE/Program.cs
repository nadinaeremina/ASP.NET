using Algorithm_RLE.Compression;
using Algorithm_RLE.Messages;
using JsonAPI.Compression;
using JsonAPI.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// настройка сборки веб-приложения
// здесь может выполняться конфигурация зависимостей (сервисов)
// добавление сервиса 'ICompressor' с имплементацией 'SimplestRLECompressor' в наш 'IoC-контейнер'
builder.Services.AddTransient<ICompressor, SimplestRLECompressor>();

// сборка веб-приложения, на данном этапе все сервисы 'IoC-контейнер' контейнера должны быть сконфигурированы
WebApplication app = builder.Build();

// конфигурирование уже созданного веб-приложения // привязка обработчиков
// если будем передавать соббщения, создавая экземпляры класса - получим ответ в виде обьекта 'json'
app.MapGet("/", () => new StringMessage() { Message = "Server is running" });
// {
//"message": "Server is running"
//}

app.MapGet("/ping", () => new StringMessage() { Message = "pong" });
// {
//"message": "pong"
//}

// 'JSON API' — это стандарт для создания интерфейсов программирования приложений,
// который использует JSON (JavaScript Object Notation) в качестве формата обмена данными
/*
 Он предназначен для упрощения процесса создания и использования API, устанавливая набор конвенций и лучших практик, 
 описывает, как пользователи должны запрашивать, захватывать или изменять данные, и как сервер должен отвечать на эти запросы
 Он направлен на ускорение HTTP-запросов и уменьшение объёма данных, передаваемых между пользователями и сервером,
 позволяет приложениям обмениваться данными в лёгком и широко поддерживаемом формате JSON
 Он лежит в основе веб-сервисов и используется для получения, обновления и управления данными между клиентом и сервером
 */

// 'JSON' - Java Script Object Notation - спецификация (формат) описания данных ("ключ": <значение>)
// типы значений ключа: string, number, boolean, null, object, array
// 'json' явдяется хорошим решением для того, чтобы организовать общение между бэком и фронтом
// тк есть унификация по описанию данных, есть библиотеки для того, чтобы упаковывать 'json' и распаковывать 'json'

// алгоритм RLE - алгоритм сжатия данных ('run-length encoding')
// 1 // without sugar

// body-row-json

// запрос
// post / compress
//{
//    "data": "gtdhfghjhjkkkkkk"
//}
// полученные данные сериализуются в 'StringData'

// теперь мы можем достать наш сервис, который добавили
app.MapPost("/compress", async (HttpContext context, ICompressor compressor) =>
{
    try
    {
        // 1 // считываем входные данные
        StringData? data = null;
        try
        {
            data = await context.Request.ReadFromJsonAsync<StringData>();
            // 'ReadFromJsonAsync' - позволяет читать JSON из запроса и десериализовать его в объект определённого типа
            // 'ReadToEndAsync ' - асинхронно считывает все символы с текущей позиции до конца средства чтения текста
            // и возвращает их в виде одной строки.
        }
        catch (Exception ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
            await context.Response.WriteAsJsonAsync(error);
            return;
        }
        if(data == null)
        {
            throw new InvalidCompressionDataException("input data is null");
        }

        // 2 // выполнить сжатие 
        // ICompressor compressor = new SimplestRLECompressor();
        // эта строка не соответствует логике создания web-интерфейса - здесь мы создаем экземпляр класса, а такого нет в плане  
        // теперь мы умеем доставать сервисы - ASP распознает его и достанет из контейнера
        string compressedData = compressor.Compress(data.Data);

        // 4 // отправить ответ
        CompressionResult result = new CompressionResult(compressor.AlgorithmName, compressedData, data.Data.Length);
        // 200
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(result);
    }
    catch(InvalidCompressionDataException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        await context.Response.WriteAsJsonAsync(error);
    }
    // все остальные уже улетят в 500
    catch (Exception ex)
    {
        // 500
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        await context.Response.WriteAsJsonAsync(error);
    }
});

// результат:
//{
//    "algorithmName": "Simplest RLE",
//    "compressedData": "gtdhfghjhj5k",
//    "dataLength": 16,
//    "compressedDataLength": 12,
//    "compressionFactor": 75
//}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////// минусы этого алгоритма: ////////////////////////////////////////////
// 1 // отсутствие декомпозиции - нет разделения логики и интерфейса (если вылетит ошибка - непонятно, неправильные вычисления
// или запрос неправильно считывается, а от дельно протестировать нельзя, хотелось бы вынести алгоритм куда-то отдельно (решено)

// 2 // формат общения удобен для клиентского глаза, но не для приложения - нужен формат 'json' для обмена сообщениями (решено)

// 3 // отстутсвие валидации входных данных и корректной обработки ошибок струтуры тела запроса, ошибки валятся 5хх (решено)

// 4 // использование встроенных классов исключений для 4хх ошибок приводит к тому, что можно пропустить ошибки сервера
// и принять их за ошибки клиента (то, что должно быть 5хх становится 4хх) (решено)

// 5 // обработчик помимо своих обязанностей выполняет создание сервиса сжатия, из-за чего теряется смысл использования интерфейса
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// 2 // with sugar
// 2.1 // явно указываем тип результата лямбды - 'object', потому что 3 'return' должны возвращать один и тот же тип
app.MapPost("/compress-sugar", object (StringData data, ICompressor compressor) =>
{
    try
    {
        // сжатие 
        string compressedData = compressor.Compress(data.Data);

        // отправить ответ
        return new CompressionResult(compressor.AlgorithmName, compressedData, data.Data.Length);
    }
    catch(InvalidCompressionDataException ex)
    {
        return new ErrorMessage(ex.GetType().Name, ex.Message);
    }
    catch (Exception ex)
    {
        return new ErrorMessage(ex.GetType().Name, ex.Message);
    }
});

// 2.2 // возвращаемый результат - "IMessage" - следовательно лямбда может возвращать любого наследника 'IMessage'
// хотя 'IMessage' это вообще интерфейс без методов - те привели к общему виду
app.MapPost("/compress-sugar-2", IMessage (StringData data, ICompressor compressor) =>
{
    try
    {
        // сжатие 
        string compressedData = compressor.Compress(data.Data);

        // отправить ответ
        return new CompressionResult(compressor.AlgorithmName, compressedData, data.Data.Length);
    }
    catch (InvalidCompressionDataException ex)
    {
        return new ErrorMessage(ex.GetType().Name, ex.Message);
    }
    catch (Exception ex)
    {
        return new ErrorMessage(ex.GetType().Name, ex.Message);
    }
});

// 2.3 // 'Results Api'
// 'Results Api' - оболочка, которая позволяет формировать результаты запросов,
// устанавливая соответствующие коды через вызовы нужных методов

// можно явно указывать 'IResult', а можно вообще не указывать 
app.MapPost("/compress-sugar-api", IResult (StringData data, ICompressor compressor) =>
{
    try
    {
        // сжатие 
        string compressedData = compressor.Compress(data.Data);
        CompressionResult result = new CompressionResult
        (
            compressor.AlgorithmName,
            compressedData,
            data.Data.Length
        );
        // отправить ответ // 200 // OK
        return Results.Ok(result); // 200
    }
    catch (InvalidCompressionDataException ex)
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

// запуск веб-приложения
app.Run();