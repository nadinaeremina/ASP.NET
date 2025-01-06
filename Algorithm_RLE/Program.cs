using Algorithm_RLE.Compression;
using Algorithm_RLE.Messages;
using JsonAPI.Compression;
using JsonAPI.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������ ���-����������
// ����� ����� ����������� ������������ ������������ (��������)
// ���������� ������� 'ICompressor' � �������������� 'SimplestRLECompressor' � ��� 'IoC-���������'
builder.Services.AddTransient<ICompressor, SimplestRLECompressor>();

// ������ ���-����������, �� ������ ����� ��� ������� 'IoC-���������' ���������� ������ ���� ����������������
WebApplication app = builder.Build();

// ���������������� ��� ���������� ���-���������� // �������� ������������
// ���� ����� ���������� ���������, �������� ���������� ������ - ������� ����� � ���� ������� 'json'
app.MapGet("/", () => new StringMessage() { Message = "Server is running" });
// {
//"message": "Server is running"
//}

app.MapGet("/ping", () => new StringMessage() { Message = "pong" });
// {
//"message": "pong"
//}

// 'JSON API' � ��� �������� ��� �������� ����������� ���������������� ����������,
// ������� ���������� JSON (JavaScript Object Notation) � �������� ������� ������ �������
/*
 �� ������������ ��� ��������� �������� �������� � ������������� API, ������������ ����� ��������� � ������ �������, 
 ���������, ��� ������������ ������ �����������, ����������� ��� �������� ������, � ��� ������ ������ �������� �� ��� �������
 �� ��������� �� ��������� HTTP-�������� � ���������� ������ ������, ������������ ����� �������������� � ��������,
 ��������� ����������� ������������ ������� � ����� � ������ �������������� ������� JSON
 �� ����� � ������ ���-�������� � ������������ ��� ���������, ���������� � ���������� ������� ����� �������� � ��������
 */

// 'JSON' - Java Script Object Notation - ������������ (������) �������� ������ ("����": <��������>)
// ���� �������� �����: string, number, boolean, null, object, array
// 'json' �������� ������� �������� ��� ����, ����� ������������ ������� ����� ����� � �������
// �� ���� ���������� �� �������� ������, ���� ���������� ��� ����, ����� ����������� 'json' � ������������� 'json'

// �������� RLE - �������� ������ ������ ('run-length encoding')
// 1 // without sugar

// body-row-json

// ������
// post / compress
//{
//    "data": "gtdhfghjhjkkkkkk"
//}
// ���������� ������ ������������� � 'StringData'

// ������ �� ����� ������� ��� ������, ������� ��������
app.MapPost("/compress", async (HttpContext context, ICompressor compressor) =>
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

        // 2 // ��������� ������ 
        // ICompressor compressor = new SimplestRLECompressor();
        // ��� ������ �� ������������� ������ �������� web-���������� - ����� �� ������� ��������� ������, � ������ ��� � �����  
        // ������ �� ����� ��������� ������� - ASP ���������� ��� � �������� �� ����������
        string compressedData = compressor.Compress(data.Data);

        // 4 // ��������� �����
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
    // ��� ��������� ��� ������ � 500
    catch (Exception ex)
    {
        // 500
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ErrorMessage error = new ErrorMessage(ex.GetType().Name, ex.Message);
        await context.Response.WriteAsJsonAsync(error);
    }
});

// ���������:
//{
//    "algorithmName": "Simplest RLE",
//    "compressedData": "gtdhfghjhj5k",
//    "dataLength": 16,
//    "compressedDataLength": 12,
//    "compressionFactor": 75
//}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////// ������ ����� ���������: ////////////////////////////////////////////
// 1 // ���������� ������������ - ��� ���������� ������ � ���������� (���� ������� ������ - ���������, ������������ ����������
// ��� ������ ����������� �����������, � �� ������ �������������� ������, �������� �� ������� �������� ����-�� �������� (������)

// 2 // ������ ������� ������ ��� ����������� �����, �� �� ��� ���������� - ����� ������ 'json' ��� ������ ����������� (������)

// 3 // ���������� ��������� ������� ������ � ���������� ��������� ������ �������� ���� �������, ������ ������� 5�� (������)

// 4 // ������������� ���������� ������� ���������� ��� 4�� ������ �������� � ����, ��� ����� ���������� ������ �������
// � ������� �� �� ������ ������� (��, ��� ������ ���� 5�� ���������� 4��) (������)

// 5 // ���������� ������ ����� ������������ ��������� �������� ������� ������, ��-�� ���� �������� ����� ������������� ����������
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// 2 // with sugar
// 2.1 // ���� ��������� ��� ���������� ������ - 'object', ������ ��� 3 'return' ������ ���������� ���� � ��� �� ���
app.MapPost("/compress-sugar", object (StringData data, ICompressor compressor) =>
{
    try
    {
        // ������ 
        string compressedData = compressor.Compress(data.Data);

        // ��������� �����
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

// 2.2 // ������������ ��������� - "IMessage" - ������������� ������ ����� ���������� ������ ���������� 'IMessage'
// ���� 'IMessage' ��� ������ ��������� ��� ������� - �� ������� � ������ ����
app.MapPost("/compress-sugar-2", IMessage (StringData data, ICompressor compressor) =>
{
    try
    {
        // ������ 
        string compressedData = compressor.Compress(data.Data);

        // ��������� �����
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
// 'Results Api' - ��������, ������� ��������� ����������� ���������� ��������,
// ������������ ��������������� ���� ����� ������ ������ �������

// ����� ���� ��������� 'IResult', � ����� ������ �� ��������� 
app.MapPost("/compress-sugar-api", IResult (StringData data, ICompressor compressor) =>
{
    try
    {
        // ������ 
        string compressedData = compressor.Compress(data.Data);
        CompressionResult result = new CompressionResult
        (
            compressor.AlgorithmName,
            compressedData,
            data.Data.Length
        );
        // ��������� ����� // 200 // OK
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

// ������ ���-����������
app.Run();