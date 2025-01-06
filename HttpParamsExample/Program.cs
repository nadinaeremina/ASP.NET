using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// ��� �������� �� ������ ������
// ��� ������ -  � ��������� 'HttpContext context'
// � ������� - ��� ����� �������� 'HttpContext context'

// 1 // get-params 
// 1.1 without sugar
// http://localhost:8080/get-params?a=15&b=hello
// ��������� ����� ��������� � ������, � ����� �� ������� 'params'

app.MapGet("/get-params", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��

        // �� 'Request' ����� ������� get-���-��
        //"QueryString" - ������ � get-���-��, �� ������� �� �������� - ������� ���������� "Query"
        // "Query" - ���-���, ��� ����� - ��� ����� ���-�� � � ������� �� ��� ���� ������ ��������
        // ������� ���-�� �� ����� � �� ������� ('!' - �������, ��� ��� �� nullable)
        // �������� 'nullable' � 'not null'
        int a = Convert.ToInt32(context.Request.Query["a"][0]!);
        // ����� ����� �������� "a" - ������������ ������ ����� ������ - ������� ������������ � "int"
        string b = context.Request.Query["b"][0]!;

        // 2 ��������� ������ � �����
        string reply = $"[get-params]: received a = {a}; b = {b}.";

        // 3 �������� � ��������� ���-�
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
    // 2 ��������� ������ � �����
    return $"[get-params]: received a = {a}; b = {b}.";
});
// ������������ ������ ��-������
// ���������� ������� ����� ����� ���������, � �������� �� ����� (������, ��� ����� ��������)

// 2 // post-params (�������� ���-�� � ���� �������)
// 2.1 without sugar
// http://localhost:8080/post-params
// ���-�� �������� � 'body' - 'form-data'

app.MapPost("/post-params", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��

        // �� 'Request' ����� ������� get-���-��
        // "Form" - ������ "Query", ����� �������� ������ �� ���-��������
        // ���-���, ��� ����� - ��� ����� ���-�� � � ������� �� ��� ���� ������ ��������
        // ������� ���-�� �� ����� � �� ������� ('!' - �������, ��� ��� �� nullable)
        int a = Convert.ToInt32(context.Request.Form["a"][0]!);
        // ����� ����� �������� "a" - ������������ ������ ����� ������ - ������� ������������ � "int"
        string b = context.Request.Form["b"][0]!;

        // 2 ��������� ������ � �����

        string reply = $"[post-params]: received a = {a}; b = {b}.";

        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync(reply);

    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 2.2 with sugar
// http://localhost:8080/post-params-sugar
// ���-�� �������� � 'body' - 'form-data'

app.MapPost("/post-params-sugar", ([FromForm] int a, [FromForm] string b) =>
{
    // 2 ��������� ������ � �����
    return $"[post-params]: received a = {a}; b = {b}.";
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

// "FromForm" - ����� ���-�� ����������� �� FormData, � �� �� ������ �������, ����� ��� ����� get-���-��
// �� �������� ������������� �������������� ������ - ��� ����� �� ��������� - ������ �� �������� ������� �� �� �����
// ������ ��� �� �� �� ����� ����������, � �� 'Postman'

// 3.1 // headers // without sugar
// http://localhost:8080/header

app.MapGet("/header", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��

        // ��������� ���� � 'Response' � � 'Request', � 'Response' �� �� �����, � � 'Request' �� �� ���������
        // ��������� ��������� ������������ � ���� ��-� (Accept, UserAgent)
        // "Headers" - ���-���, ��� ����� - ��� ����� ���������� � � ������� �� ��� ���� ������ ��������
        // ������� ���-�� �� ����� � �� ������� ('!' - �������, ��� ��� �� nullable)
        string data = context.Request.Headers["My-Header-Data"][0]!;

        // 2 ��������� ������ � �����
        string reply = $"[header]: received '{data}' header";

        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync(reply);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 3.2 with sugar
// ����� ���������� ��� ��������� (Name = "My-Header-Data"), � ����� �� ����������
// http://localhost:8080/header-sugar

app.MapGet("/header-sugar", ([FromHeader(Name = "My-Header-Data")] string data) =>
{
    // 2 ��������� ������ � �����
    return $"[header]: received '{data}' header.";
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

// 4.1 // url-vars (path-vars) // without sugar
// ����� ���-�� � 'get' � 'put' ���������
// 'number' - ����� ����������, ��������� ��������� - ��� �������� �������� ��������� �������� (rout parameter)
// 'int' - ���������� �����, 'alpha' - ����� � ��, regex()-���������

// http://localhost:8080/url-vars/15
// http://localhost:8080/url-vars/hello - �� ���������

// � �������� ������� ����� �������� ���������� � �� ���������
app.MapGet("/url-vars/{number:int}", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-�� // �������� �� �����
        int number = Convert.ToInt32(context.Request.RouteValues["number"]);

        // 2 ��������� ������ � �����
        string reply = $"[header]: received number = {number}";

        // 3 �������� � ��������� ���-�
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
    // 2 ��������� ������ � �����
    return $"[header]: received number = {number}";
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

// 5.1 // http-request body // without sugar
// ����� ���-�� � ������ ���������, ����� get, set, options
// ����� ����: put, patch, delete (���� ����� - ��� post)
// http://localhost:8080/body-data
// ����� ��� ���� ��������� � 'body-row-text'

app.MapPost("/body-data", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��
        using (StreamReader sr = new StreamReader(context.Request.Body))
        {
            string data = await sr.ReadToEndAsync();
            // 'ReadToEndAsync' - ���������� ��������� ��� �������, ������� � ������� ������� �� ����� ������
            // ���������� �� � ���� ����� ������

            // 2 ��������� ������ � �����
            string reply = $"[body-data]: received '{data}' body";

            // 3 �������� � ��������� ���-�
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
// ASP ��� �������������� ���������� ���������� ���� json, ���� xml-������
// ����� ��� ���� ��������� � 'body-row-json' � ��������
// � 'Hraders' ������ ���� 'Content-Type application/json'

app.MapPost("/body-data-sugar", ([FromBody] string data) =>
{
    // ������� ������ ������, ���� ��� � �������� (��� json)
    // 2 ��������� ������ � �����
    return $"[body-data-sugar]: received '{data}' body";
});

app.Run();
