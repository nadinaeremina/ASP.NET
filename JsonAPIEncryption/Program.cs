using JsonAPIEncryption.Encryption;
using JsonAPIEncryption.Messages;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// ���������� ������� 'IEncoder' � �������������� 'MD5Encoder' � ��� 'IoC-���������'
// builder.Services.AddTransient<IEncoder, MD5Encoder>();
// ���������� ������� 'IEncoder' � �������������� 'BCryptEncoder' � ��� 'IoC-���������'
//builder.Services.AddTransient<IEncoder, BCryptEncoder>();

// ���������� ������� ��� �������� ������������� ����������
builder.Services.AddTransient(opts => EncoderFactory.CreateEncoder()); // �������� �������
// 'generic' ��� ����� �� ������, ������ ��� ����� ���������� ���������

var app = builder.Build();

// 'Process.GetCurrentProcess' � ����������� ����� ������ System.Diagnostics,
// ������� ���������� ����� ������ Process, �������������� �������, ������� �������� �������� � ������� ������
// 'StartTime' - �������� ����� ������� �������� ��������

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

// ������ �������:
//{
//    "data": "gtdhfghjhjkkkkkk",
//    "count": 1
//}

// without sugar
app.MapPost("/encode", async (HttpContext context, IEncoder encoder) =>
{
    try
    {
        // 1 // ��������� ������� ������
        StringData? data = null;
        try
        {
            data = await context.Request.ReadFromJsonAsync<StringData>();
            // 'ReadFromJsonAsync' - ��������� ������ JSON �� ������� � ��������������� ��� � ������ ������������ ����
            // 'ReadToEndAsync ' - ���������� ��������� ��� ������� � ������� ������� �� ����� �������� ������ ������
            // � ���������� �� � ���� ����� ������.
        }
        // ���� 'raw' ���������� ������
        catch (InvalidOperationException ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "qwery is empty");
            await context.Response.WriteAsJsonAsync(error);
        }
        // ���� � ���� 'data' ����������� ������ // �� ������ �������� ????????????????????
        catch (InvalidEncodingDataException ex)
        {
            // 400
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            ErrorMessage error = new ErrorMessage(ex.GetType().Name, "in the 'data' field");
            await context.Response.WriteAsJsonAsync(error);
        }
        // ���� ������������ ������ json (������ � ����������)
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

        // 2 // ��������� ����������
        // IEncoder mEncoder = context.RequestServices.GetRequiredService<IEncoder>();
        string encodedData = encoder.Encode(data.Data, data.Count);

        // 4 // ��������� �����
        EncodingResult result = new EncodingResult(encodedData, encoder.AlgorithmName, data.Count);
        // 200
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(result);
    }
    // ����� �������� ���� ????????????????????????
    catch (ArgumentException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, "2a");
        await context.Response.WriteAsJsonAsync(error);
    }
    // ����� �������� ���� ????????????????????????
    catch (InvalidOperationException ex)
    {
        // 400
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, "2i�");
        await context.Response.WriteAsJsonAsync(error);
    }
    // �� ������ �������� ?????????????????????
    // ���� ������ �� �������� ��� ������������
    // �������� �� �����: 
    //{
    //    "id": 5,
    //    "name": "�������",
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
    // ��� ��������� ��� ������ � 500
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
// 'Results Api' - ��������, ������� ��������� ����������� ���������� ��������,
// ������������ ��������������� ���� ����� ������ ������ �������

// ����� ���� ��������� 'IResult', � ����� ������ �� ��������� 
app.MapPost("/encode-sugar", IResult (StringData data, IEncoder encoder) =>
{
    try
    {
        // 2 // ��������� ����������
        // IEncoder mEncoder = context.RequestServices.GetRequiredService<IEncoder>();
        string encodedData = encoder.Encode(data.Data, data.Count);

        // 4 // ��������� �����
        EncodingResult result = new EncodingResult(encodedData, encoder.AlgorithmName, data.Count);
        // 200
        // ��������� ����� // 200 // OK
        return Results.Ok(result); // 200
    }
    // ����� �������� ���� ????????????????????????
    catch (ArgumentException ex)
    {
        // 400
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        return Results.BadRequest(error);
    }
    // ����� �������� ���� ????????????????????????
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