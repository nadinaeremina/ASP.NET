using Microsoft.EntityFrameworkCore;
using RAMList.Model;

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

// GET /rams - ��������� ���� ����������
app.MapGet("/rams", async (ApplicationDbContext db) =>
{
    return await db.RAMS.ToListAsync();
    // 'ToListAsync' - ����� ��� ��������� ������ �������� � ����������� ������ � Entity Framework Core
});

// GET /rams/{id} - ��������� ���������� �� id
app.MapGet("/rams/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.RAMS.FirstOrDefaultAsync(ram => ram.Id == id);
    // 'FirstOrDefaultAsync' - ����������� �����, ������� ���������� ������ ������� ������������������
    // ��� �������� �� ���������, ���� ������������������ �����
});

// POST /rams - ���������� ��������
app.MapPost("/rams", async (RAM ram, ApplicationDbContext db) =>
{
    // ����� ��������, �� ���� ����� ���� ��� ���� - ����� ���������������� �����
    ram.Id = 0;
    await db.RAMS.AddAsync(ram);
    // 'AddAsync' - ����������� ����� ��� ���������� ������ ������� � �������� � Entity Framework Core
    await db.SaveChangesAsync();
    // ����� ���������� �������� - � ���� �������� ���� ����
    return ram;
});

// DELETE /rams/{id} - �������� �������� id
app.MapDelete("/rams/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // ������� ������� - ��������� ���������
    RAM? deleted = await db.RAMS.FirstOrDefaultAsync(ram => ram.Id == id);
    if (deleted != null)
    {
        db.RAMS.Remove(deleted);
        await db.SaveChangesAsync();
    }
    return deleted;
});

// PATCH /elements - �������������� ��������
app.MapPatch("/rams", async (RAM ram, ApplicationDbContext db) =>
{
    // ������� ����
    RAM? updated = await db.RAMS.FirstOrDefaultAsync(r => r.Id == ram.Id);
    // ���� ���� - ��������� ����, ���� �� ��������
    if (updated != null)
    {
        updated.Country = ram.Country;
        updated.Model = ram.Model;
        updated.MemoryType = ram.MemoryType;
        updated.FormFactor = ram.FormFactor;
        updated.Volume = ram.Volume;
        updated.ClockFrequency = ram.ClockFrequency;
        await db.SaveChangesAsync();
    }
    return updated;
});

app.Run();

// 1 // Add-Migration Init - ��������� ���� ��� (� �������), � ��������� �� ����� �����������
// ��� �������� � �������� � ����� ����� ����� �������� �������
// 2 // Update-Database - ��������� ��������, ������������ � �� � ������� �������

app.Run();
