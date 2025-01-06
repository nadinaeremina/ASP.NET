using JsonAPI;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// WriteAsJson()/WriteAsJsonAsync() ������� HttpResponse. ���� ����� ��������� ������������� ���������� � ���� �������
// � ������ JSON � ������������� ��� ��������� "content-type" ������������� �������� "application/json; charset=utf-8":
// ��� ��������� �� ������� ������ � ������� JSON � ������ HttpRequest ��������� ����� ReadFromJsonAsync().
// �� ��������� ������������� ������ � ������ ������������� ����.

// 'JSON' - Java Script Object Notation - ������������ (������) �������� ������ ("����": <��������>)
// Json-������ ������ ���������� � ���� ������� (���� ����� ��� ����� 'post')

// 1 ///////////////////////////////////////// ������� ������: JSON ////////////////////////////////////////////
// 1.1 // ��� ������

// body-raw-json

// �������� ������ � ��������� ����
//{
//    "intField": 33,
//    "stringField": "Hello from Postman", 
//    "nullableBoolField": true
//}

// http://localhost:8080/json-input
// ������� ����� �����:
// [json-input] received: 33 - Hello from Postman - True

// json-������ ������ ���������� � ���� �������,� ���� ������� - ��� ����� 'put'
app.MapPost("/json-input", async (HttpContext context) =>
{
	try
	{
		// 1 // ������ � �������� ������� ������ // ������� � ���������������
		InputData? data=await context.Request.ReadFromJsonAsync<InputData>();
        // 'ReadFromJsonAsync' - ���� ����� ������� ���������� ��������������
        // ���������� � '<>' �������� ����� ����, � ������ �� 'nullable'? ������ ��� ����� ���� ������ 'null'
        // 'HasJsonContentType' - �� ���������� true, ���� ������ ������� json
        if (data == null)
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"data is null");
			return;
        }
		// 2 // ����� ������ � �������
		string replay = $"[json-input] received: {data}";
        // [json-input] received: 33 - Hello from Postman - True // �������� ����� ������
        // 3 // ��������� �����
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(replay);
    }
    catch (Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
	}
});

// 1.2 // � �������
// ����� �� ����� ������ 'from body' - ASP � ��� ������, ��� ��� ����� ���������������, ������ ������ ���
app.MapPost("/json-input-sugar", (InputData data) =>
{
    // ���� �������� ������ c����� - �� ��� �� ����� �������������� ��� ������ json
    // �������� ������ � ��������� �������, ��������, ��������
    // ���� ��� �������� 'InputData?' - �� ����� 'nullable',
    // � ��� ��������. ����� - �������� ����������, ������ ����� �������� 'null'
    
    // 2 // ����� ������ � �������
    string replay = $"[json-input-sugar] received: {data}";

	// 3 // ��������� �����
	return replay;
});

// 2 //////////////////////////////////////////// ���������: JSON ///////////////////////////////////////////////
// 2.1 // ��� ������

// body-none

// http://localhost:8080/json-output
// Content-Type: application / json; charset = utf - 8

// �������������, ����� ��� ��� post-������
app.MapGet("/json-output", async (HttpContext context) =>
{
    try
    {
        // 1 // ����������� �����
        OutputData? data = new OutputData()
        {
            Message = "hello from asp server json-output",
            NumberCode = 200
        };

        // 2 // ��������� �����
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(data); 
        // ������ ��������� � json-������� ('OutputData' � ������ 'json')
        // 'WriteAsJsonAsync' - ������ ����� ���������� ������ json-������
        // ��������� generic �� �����, ������ ��� .net ��������� ��������� ��-��� ����� ��������� �������
        // � ����������� ��� ������������� �� ������ ��������� ��-� �������,
        // ��������� ���� ��������������� �� �����, ������ ��-��, � ������� ���� getter
        // ������������� ��� ��������� "content-type" ������������� �������� "application/json; charset=utf-8":
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
    }
});

// 2.2 // � �������
// ����� �� ����� ������ 'from body' - ASP � ��� ������, ��� ��� ����� ���������������, ������ ������ ���
// body-none
// http://localhost:8080/json-output-sugar

app.MapGet("/json-output-sugar", () =>
// ������ �� ���������
{
    // 1 // ����������� �����
    OutputData? data = new OutputData()
    {
        Message = "hello from asp server json-output",
        NumberCode = 200
    };
    // 2 // ��������� �����
    // ����� ������ ���������� ������, � ���� ���������� ���������� �� ��������� ������
    // �� ASP �������� ������������ � 'json' �������������
    return data;
    // asp �������� ������������ json
});


// 3 //////////////////////////////////////// ������� ������: JSON; ���������: JSON /////////////////////////////////
// 3.1 // ��� ������

// http://localhost:8080/json-input-output
// body-row-json

//{
//    "intField": 33,
//    "stringField": "Hello from Postman", 
//    "nullableBoolField": true
//}

// �������� ����� ������
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
        // 1 // ������ � �������� ������� ������ // ������� � ���������������
        InputData? data = await context.Request.ReadFromJsonAsync<InputData>();
        // ReadFromJsonAsync - ���� ����� ������� ���������� ��������������
        // � ������ ������ �������������� ���������� � '<InputData>'
        // ����� ��������� 'null' ���������

        // 'HasJsonContentType' - �� ���������� true, ���� ������ ������� json

        // 2 // ����� ������ � �������
        OutputData result = new OutputData()
        {
            Message = "hello from asp server json-input-output",
            NumberCode = 200,
            OutputForInput = data
        };

        // 3 // ��������� �����
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(result);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync($"something is wrong: ${ex.Message}");
    }
});

// 3.2 // � �������
// ����� �� ����� ������ 'from body' - ASP � ��� ������, ��� ��� ����� ���������������, ������ ������ ���

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
