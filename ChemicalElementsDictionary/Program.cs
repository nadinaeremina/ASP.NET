using ChemicalElementsDictionary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// добавление сервисов приложения 
builder.Services.AddDbContext<ApplicationDbContext>();
// тк здесь 'AddDbContext' - любой класс мы сюда не запишем - это должен быть наследник 'DbContext'
// после этого он будет доступен в IoC-контейнере, его можно будет доставать сахарным способом

var app = builder.Build();

// базовые обработчики // анонимные обьекты (у нас нет такого класса)
app.MapGet("/", () => new { Message = "server is running" });
app.MapGet("/ping", () => new { Message = "pong" });

// CRUD-операции работы с элементами

// GET /elements - получение всех элементов
app.MapGet("/elements", async (ApplicationDbContext db) =>
{
    return await db.Elements.ToListAsync();
    // 'ToListAsync' - метод для получения списка объектов в асинхронном режиме в Entity Framework Core
});

// GET /elements/{id} - получение элемента по id
app.MapGet("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.Elements.FirstOrDefaultAsync(element => element.Id == id);
    // 'FirstOrDefaultAsync' - асинхронный метод, который возвращает первый элемент последовательности
    // или значение по умолчанию, если последовательность пуста
});

// POST /elements - добавление элемента
app.MapPost("/elements", async (Element element, ApplicationDbContext db) =>
{
    // лучше обнулить, тк если такой айди уже есть - будет переприсваивание полей
    element.Id = 0;
    await db.Elements.AddAsync(element);
    // 'AddAsync' - асинхронный метод для добавления нового объекта в контекст в Entity Framework Core
    await db.SaveChangesAsync();
    // после сохранения элемента - у него появится свой айди
    return element;
});

// DELETE /elements/{id} - удаление элемента id
app.MapDelete("/elements/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // находим элемент - сохраняем найденный
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
    // сначала ищем
    Element? updated = await db.Elements.FirstOrDefaultAsync(e => e.Id == element.Id);
    // если есть - обновляем поля, айди не меняется
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
