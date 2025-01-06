using JsonAPI;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// WriteAsJson()/WriteAsJsonAsync() объекта HttpResponse. Этот метод позволяет сериализовать переданные в него объекты
// в формат JSON и автоматически для заголовка "content-type" устанавливает значение "application/json; charset=utf-8":
// Для получения из запроса объект в формате JSON в классе HttpRequest определен метод ReadFromJsonAsync().
// Он позволяет сериализовать данные в объект определенного типа.

// 'JSON' - Java Script Object Notation - спецификация (формат) описания данных ("ключ": <значение>)
// Json-данные всегда передаются в теле запроса (чаще всего это метод 'post')

// 1 ///////////////////////////////////////// входные данные: JSON ////////////////////////////////////////////
// 1.1 // без сахара

// body-raw-json

// передаем данные в следующем виде
//{
//    "intField": 33,
//    "stringField": "Hello from Postman", 
//    "nullableBoolField": true
//}

// http://localhost:8080/json-input
// получим ответ ввиде:
// [json-input] received: 33 - Hello from Postman - True

// json-данные всегда передаются в теле запроса,а тело запроса - это метод 'put'
app.MapPost("/json-input", async (HttpContext context) =>
{
	try
	{
		// 1 // чтение и проверка входных данных // считали и десериализовали
		InputData? data=await context.Request.ReadFromJsonAsync<InputData>();
        // 'ReadFromJsonAsync' - этот метод содержт встроенный десериализатор
        // передается в '<>' параметр опред типа, а вернет он 'nullable'? потому что может быть считан 'null'
        // 'HasJsonContentType' - он возвращает true, если клиент прислал json
        if (data == null)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"data is null");
			return;
        }
		// 2 // чтото делаем с данными
		string replay = $"[json-input] received: {data}";
        // [json-input] received: 33 - Hello from Postman - True // получаем такие данные
        // 3 // отправить ответ
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(replay);
    }
    catch (Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
	}
});

// 1.2 // с сахаром
// здесь не нужно писать 'from body' - ASP и так поймет, что его нужно десериализовать, чтения данных нет
app.MapPost("/json-input-sugar", (InputData data) =>
{
    // если передать просто cтроку - то она не будет распознаваться как обьект json
    // работает только с обьектами классов, структур, рекордов
    // если так передать 'InputData?' - то будет 'nullable',
    // а без вопросит. знака - параметр обязателен, нельзя будет передать 'null'
    
    // 2 // чтото делаем с данными
    string replay = $"[json-input-sugar] received: {data}";

	// 3 // отправить ответ
	return replay;
});

// 2 //////////////////////////////////////////// результат: JSON ///////////////////////////////////////////////
// 2.1 // без сахара

// body-none

// http://localhost:8080/json-output
// Content-Type: application / json; charset = utf - 8

// необязательно, чтобы это был post-запрос
app.MapGet("/json-output", async (HttpContext context) =>
{
    try
    {
        // 1 // подготовить ответ
        OutputData? data = new OutputData()
        {
            Message = "hello from asp server json-output",
            NumberCode = 200
        };

        // 2 // отправить ответ
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(data); 
        // данные запишутся в json-формате ('OutputData' в обьект 'json')
        // 'WriteAsJsonAsync' - засчет этого произойдет запись json-данных
        // указывать generic не нужно, потому что .net пройдется публичным св-вом через рефлексию обьекта
        // и сериализует его автоматически на сонове публичных св-в обьекта,
        // публичные поля сериализоваться не будут, только св-ва, в которых есть getter
        // автоматически для заголовка "content-type" устанавливает значение "application/json; charset=utf-8":
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
    }
});

// 2.2 // с сахаром
// здесь не нужно писать 'from body' - ASP и так поймет, что его нужно десериализовать, чтения данных нет
// body-none
// http://localhost:8080/json-output-sugar

app.MapGet("/json-output-sugar", () =>
// ничего не принимает
{
    // 1 // подготовить ответ
    OutputData? data = new OutputData()
    {
        Message = "hello from asp server json-output",
        NumberCode = 200
    };
    // 2 // отправить ответ
    // здесь просто возвращаем обьект, а если обработчик возвращает не строковый обьект
    // то ASP выполнит сериализацию в 'json' автоматически
    return data;
    // asp выполнит сериализацию json
});


// 3 //////////////////////////////////////// входные данные: JSON; результат: JSON /////////////////////////////////
// 3.1 // без сахара

// http://localhost:8080/json-input-output
// body-row-json

//{
//    "intField": 33,
//    "stringField": "Hello from Postman", 
//    "nullableBoolField": true
//}

// получаем такие данные
// {
//"message": "hello from asp server json-input-output",
//    "numberCode": 200,
//    "outputForInput": {
//        "intField": 33,
//        "stringField": "Hello from Postman",
//        "nullableBoolField": true
//    }
//}

app.MapPost("/json-input-output", async (HttpContext context) =>
{
    try
    {
        // 1 // чтение и проверка входных данных // считали и десериализовали
        InputData? data = await context.Request.ReadFromJsonAsync<InputData>();
        // ReadFromJsonAsync - этот метод содержт встроенный десериализатор
        // в данном случае десериализация происходит в '<InputData>'
        // здесь допустимо 'null' отправить

        // 'HasJsonContentType' - он возвращает true, если клиент прислал json

        // 2 // чтото делаем с данными
        OutputData result = new OutputData()
        {
            Message = "hello from asp server json-input-output",
            NumberCode = 200,
            OutputForInput = data
        };

        // 3 // отправить ответ
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(result);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
    }
});

// 3.2 // с сахаром
// здесь не нужно писать 'from body' - ASP и так поймет, что его нужно десериализовать, чтения данных нет

// http://localhost:8080/json-input-output-sugar

app.MapPost("/json-input-output-sugar", (InputData? data) =>
{
    return new OutputData()
    {
        Message = "hello from asp server json-output",
        NumberCode = 200, 
        OutputForInput = data
    };
});

app.Run();
