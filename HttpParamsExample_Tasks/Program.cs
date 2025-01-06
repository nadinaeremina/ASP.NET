using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ����������� ����������
app.MapGet("/", () => "server is runing");

// 1 // get-params
// ��������� ������ ���������� �� ���� �� ��� - ������������ ������� ���������� �,��,��,��,��
// 1.1 // without sugar
// http://localhost:8080/convert/no-sugar?from=kb&to=mb&value=15550

app.MapGet("/convert/no-sugar", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������
        // �� ����� ������� ��������������
        string from = context.Request.Query["from"][0]!;
        // � ����� ������� ��������������
        string to = context.Request.Query["to"][0]!;
        // ��������
        double value = Convert.ToDouble(context.Request.Query["value"][0]);

        // 2 ��������� ����������� (����� � ��� - ��������� ������� � �����, ����� � ������� ��������)
        double bytes = 0;
        if (from == "b")
        {
            bytes = value;
        }
        else if (from == "kb")
        {
            // ��������� ��������� � �����
            bytes = value * 1024;
        }
        else if (from == "mb")
        {
            // ��������� ��������� � �����
            bytes = value * 1024 * 1024;
        }
        else if (from == "gb")
        {
            // ��������� ��������� � �����
            bytes = value * 1024 * 1024 * 1024;
        }
        else if (from == "tb")
        {
            // ��������� ��������� � �����
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
            // ��������� ����� � ���������
            result = bytes / 1024;
        }
        else if (to == "mb")
        {
            // ��������� ����� � ���������
            result = bytes / (1024 * 1024);
        }
        else if (to == "gb")
        {
            // ��������� ����� � ���������
            result = bytes / (1024 * 1024 * 1024);
        }
        else if (to == "tb")
        {
            // ��������� ����� � ���������
            result = bytes / (1024L * 1024 * 1024 * 1024);
            // '1024L' - ������� 'long' (���� �� �� ���� �������� - �� ������� � ���-�� ���� 'long')
        }
        else
        {
            throw new ArgumentException($"unknown unit: {from}; allowed units: b, kb, mb, gb, tb;");
        }

        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync($"{value} {from}. = {result} {to}.");

    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 1.2 // with sugar
// http://localhost:8080/convert/sugar?from=tb&to=kb&value=15500
// ���� ��� ����� �������������, �����������

app.MapGet("/convert/sugar", (string from, string to, double value) =>
{
    try
    {
        Dictionary<string, long> coefficients = new Dictionary<string, long>()
        {
            { "b", 1L},
            { "kb", 1L << 10}, // 2 � 10 �������
            { "mb", 1L << 20}, // 2 � 20 �������
            { "gb", 1L << 30}, // 2 � 30 �������
            { "tb", 1L << 40}  // 2 � 40 �������
            // �������� �������� ����� �� 10, 20, 30 ��� 40 ��������
            // ���� ����� ��������� ����� ���, ��� ����� �������� ���������
        };
        // ���� ��� ������� �� �������� ����� ����, ������� ������ � 'to' ��� 'from'
        if (!coefficients.ContainsKey(from) || !coefficients.ContainsKey(to))
        {
            return $"there are inknown units: {from} or {to}; allowed units: {string.Join(",", coefficients.Keys)}";
            // 'string.Join()' - ����� ��������� ��������������� ��������� ��������� � ���� ������ � ��������������
            // ���������� �����������. ���� ��������� �� ����� ���������, ����� ���������� ������ ������ (String.Empty)
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
// ������� �������� - � ������� �������� �������� ���� �������, ������� �������� ���������� � ������
// http://localhost:8080/body-data
// ���-�� ���������� � 'body-raw-text' ����� ������� 

app.MapPost("/body-data", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��
        using (StreamReader sr = new StreamReader(context.Request.Body))
        {
            string data = await sr.ReadToEndAsync();
            string[] tokens = data.Split(',');
            double a = Convert.ToDouble(tokens[0]);
            double b = Convert.ToDouble(tokens[1]);

            // 2 ��������� ������ � �����
            double result = Math.Sqrt(a*a + b*b);

            // 3 �������� � ��������� ���-�
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
// ASP ��� �������������� ���������� ���������� ���� json, ���� xml-������
// ����� ��� ���� ��������� � 'body-row-json' � ��������
// � 'Hraders' ������ ���� 'Content-Type application/json'

app.MapPost("/body-data-sugar", ([FromBody] string data) =>
{
    // ������� ������ ������, ���� ��� � �������� (��� 'json')
    // 1 ������� ������� ���-��
    try
    {
        // 1 ������� ������� ���-��
        string[] tokens = data.Split(',');
        double a = Convert.ToDouble(tokens[0]);
        double b = Convert.ToDouble(tokens[1]);

        // 2 ��������� ������ � �����
        return $"���������� �����: {Math.Sqrt(a * a + b * b)}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
});

// 3 // header
// 3.1 // without sugar
// �� ���� �������� ������, ���������� ������ �� ���������� ��������� ���� (����� ������)
// ���������� ������ ��������� ������ ������ ������ ��������� �������:
// "aaaaaabbbbbccdcaaa"-> "6a5b2cdc3a"
// ���� �����-�� ����� ����������� 1 ���, �� 1 ������ �� ����, ����� ������
// http://localhost:8080/header

app.MapGet("/header", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��

        // ��������� ���� � 'Response' � � 'Request', � 'Response' �� �� �����, � � 'Request' �� �� ���������
        // ��������� ��������� ������������ � ���� ��-�
        // "Headers" - ���-���, ��� ����� - ��� ����� ���������� � � ������� �� ��� ���� ������ ��������
        // ������� ���-�� �� ����� � �� ������� ('!' - �������, ��� ��� �� nullable)
        char[] data = (context.Request.Headers["My-Header-Data"][0]!).ToCharArray();
        string result = "";

        // 2 ��������� ������ � �����
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

        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync(result);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 3.2 with sugar
// ����� ���������� ��� ��������� (Name = "My-Header-Data"), � ����� �� ����������
// http://localhost:8080/header-sugar

app.MapGet("/header-sugar", ([FromHeader(Name = "My-Header-Data")] string data_string) =>
{
    try
    {
        // 1 ������� ������� ��� - ��
        char[] data = data_string.ToCharArray();
        // 2 ��������� ������ � �����
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

        // 3 �������� � ��������� ���-�
        return $"result: {result}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

// 4 // post-param
// 4.1 // without sugar
// ���������� ���������� - ��������� n!, n - �����, 0 <= n <= 20
// http://localhost:8080/post-params

app.MapPost("/post-params", async (HttpContext context) =>
{
    try
    {
        // ���-�� �������� � 'form-data'
        // 1 ������� ������� ���-��

        // �� 'Request' ����� ������� get-���-��
        // "Form" - ������ "Query", ����� �������� ������ �� ���-��������
        // ���-���, ��� ����� - ��� ����� ���-�� � � ������� �� ��� ���� ������ ��������
        // ������� ���-�� �� ����� � �� ������� ('!' - �������, ��� ��� �� nullable)
        int number = Convert.ToInt32(context.Request.Form["number"][0]!);

        // 2 ��������� ������ � �����
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
        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync(factorial.ToString());
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"some error: {ex.Message}");
    }
});

// 4.2 with sugar
// http://localhost:8080/post-params-sugar
// ���-�� �������� � 'form-data'

app.MapPost("/post-params-sugar", ([FromForm] int number) =>
{
    try
    {
        // 2 ��������� ������ � �����
        if (number > 20 || number < 0)
        {
            return "Enter other number! The range should be from 0 to 20";
        }
        int factorial = 1;
        for (int i = 1; i <= number; i++)
        {
            factorial *= i;
        }
        // 3 �������� � ��������� ���-�
        return factorial.ToString();
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

// 5 // url
// 5.1 // without sugar
// �� ������� ��������� � ������� ������� � ����� ����������
// http://localhost:8080/url-vars/5,5

app.MapGet("/url-vars/{radius:double}", async (HttpContext context) =>
{
    try
    {
        // 1 ������� ������� ���-��
        double radius = Convert.ToDouble(context.Request.RouteValues["radius"]);

        // 2 ��������� ������ � �����
        double square = Math.PI * (radius * radius);
        double length = 2 * Math.PI * radius;

        // 3 �������� � ��������� ���-�
        await context.Response.WriteAsync($"������� ����������: {square}, ����� ����������: {length}");
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
        // 2 ��������� ������ � �����
        return $"������� ����������: {Math.PI * (radius * radius)}, ����� ����������: {2 * Math.PI * radius}";
    }
    catch (Exception ex)
    {
        return $"some error: {ex.Message}";
    }
}).DisableAntiforgery(); // ������ ��������� (���������, �����������-��� ������������)

app.Run();
