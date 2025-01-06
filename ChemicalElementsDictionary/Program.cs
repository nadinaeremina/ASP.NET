using ChemicalElementsDictionary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� ���������� 
builder.Services.AddDbContext<ApplicationDbContext>();
// �� ����� 'AddDbContext' - ����� ����� �� ���� �� ������� - ��� ������ ���� ��������� 'DbContext'
// ����� ����� �� ����� �������� � IoC-����������, ��� ����� ����� ��������� �������� ��������

var app = builder.Build();

// ������� ����������� // ��������� ������� (� ��� ��� ������ ������)
app.MapGet("/", () => new { Message = "server is running" });
app.MapGet("/ping", () => new { Message = "pong" });

// CRUD-�������� ������ � ����������

// GET /elements - ��������� ���� ���������
app.MapGet("/elements", async (ApplicationDbContext db) =>
{
    return await db.Elements.ToListAsync();
    // 'ToListAsync' - ����� ��� ��������� ������ �������� � ����������� ������ � Entity Framework Core
});

// GET /elements/{id} - ��������� �������� �� id
app.MapGet("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.Elements.FirstOrDefaultAsync(element => element.Id == id);
    // 'FirstOrDefaultAsync' - ����������� �����, ������� ���������� ������ ������� ������������������
    // ��� �������� �� ���������, ���� ������������������ �����
});

// POST /elements - ���������� ��������
app.MapPost("/elements", async (Element element, ApplicationDbContext db) =>
{
    // ����� ��������, �� ���� ����� ���� ��� ���� - ����� ���������������� �����
    element.Id = 0;
    await db.Elements.AddAsync(element);
    // 'AddAsync' - ����������� ����� ��� ���������� ������ ������� � �������� � Entity Framework Core
    await db.SaveChangesAsync();
    // ����� ���������� �������� - � ���� �������� ���� ����
    return element;
});

// DELETE /elements/{id} - �������� �������� id
app.MapDelete("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // ������� ������� - ��������� ���������
    Element? deleted = await db.Elements.FirstOrDefaultAsync(element => element.Id == id);
    if (deleted != null)
    {
        db.Elements.Remove(deleted);
        await db.SaveChangesAsync();
    }
    return deleted;
});

// PATCH /elements - �������������� ��������
app.MapPatch("/elements", async (Element element, ApplicationDbContext db) =>
{
    // ������� ����
    Element? updated = await db.Elements.FirstOrDefaultAsync(e => e.Id == element.Id);
    // ���� ���� - ��������� ����, ���� �� ��������
    if (updated != null)
    {
        updated.Name = element.Name;
        updated.Code = element.Code;
        updated.Group = element.Group;
        updated.Period = element.Period;
        updated.ProtonsNumber = element.ProtonsNumber;
        await db.SaveChangesAsync();
    }
    return updated;
});

app.Run();

// 1 // Add-Migration Init - ��������� ���� ��� (� �������), � ��������� �� ����� �����������
// ��� �������� � �������� � ����� ����� ����� �������� �������
// 2 // Update-Database - ��������� ��������, ������������ � �� � ������� �������
