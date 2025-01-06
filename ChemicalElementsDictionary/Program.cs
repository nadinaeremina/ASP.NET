using ChemicalElementsDictionary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// добавление сервисов приложения
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// базовые обработчики 
app.MapGet("/", () => new { Message = "server is running" });
app.MapGet("/ping", () => new { Message = "pong" });

// CRUD-операции работы с элементами

// GET /elements - получение всех элементов
app.MapGet("/elements", async (ApplicationDbContext db) =>
{
    return await db.Elements.ToListAsync();
});

// GET /elements/{id} - получение элемента по id
app.MapGet("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.Elements.FirstOrDefaultAsync(element => element.Id == id);
});

// POST /elements - добавление элемента
app.MapPost("/elements", async (Element element, ApplicationDbContext db) =>
{
    element.Id = 0;
    await db.Elements.AddAsync(element);
    await db.SaveChangesAsync();
    return element;
});

// DELETE /elements/{id} - удаление элемента id
app.MapDelete("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    Element? deleted = await db.Elements.FirstOrDefaultAsync(element => element.Id == id);
    if (deleted != null)
    {
        db.Elements.Remove(deleted);
        await db.SaveChangesAsync();
    }
    return deleted;
});

// PATCH /elements - редактирование элемента
app.MapPatch("/elements", async (Element element, ApplicationDbContext db) =>
{
    Element? updated = await db.Elements.FirstOrDefaultAsync(e => e.Id == element.Id);
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

// 1 // Add-Migration Init - создается один раз (в консоли), а выполнять их можно многократно
// при внесении и зменений в класс можно новые миграции создать
// 2 // Update-Database - применяем миграцию, подключаемся к БД и создаст таблицу
