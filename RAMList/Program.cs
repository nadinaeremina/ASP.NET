using Microsoft.EntityFrameworkCore;
using RAMList.Model;

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

// GET /rams - получение всех оперативок
app.MapGet("/rams", async (ApplicationDbContext db) =>
{
    return await db.RAMS.ToListAsync();
    // 'ToListAsync' - метод для получения списка объектов в асинхронном режиме в Entity Framework Core
});

// GET /rams/{id} - получение оперативки по id
app.MapGet("/rams/{id:int}", async (int id, ApplicationDbContext db) =>
{
    return await db.RAMS.FirstOrDefaultAsync(ram => ram.Id == id);
    // 'FirstOrDefaultAsync' - асинхронный метод, который возвращает первый элемент последовательности
    // или значение по умолчанию, если последовательность пуста
});

// POST /rams - добавление элемента
app.MapPost("/rams", async (RAM ram, ApplicationDbContext db) =>
{
    // лучше обнулить, тк если такой айди уже есть - будет переприсваивание полей
    ram.Id = 0;
    await db.RAMS.AddAsync(ram);
    // 'AddAsync' - асинхронный метод для добавления нового объекта в контекст в Entity Framework Core
    await db.SaveChangesAsync();
    // после сохранения элемента - у него появится свой айди
    return ram;
});

// DELETE /rams/{id} - удаление элемента id
app.MapDelete("/rams/{id:int}", async (int id, ApplicationDbContext db) =>
{
    // находим элемент - сохраняем найденный
    RAM? deleted = await db.RAMS.FirstOrDefaultAsync(ram => ram.Id == id);
    if (deleted != null)
    {
        db.RAMS.Remove(deleted);
        await db.SaveChangesAsync();
    }
    return deleted;
});

// PATCH /elements - редактирование элемента
app.MapPatch("/rams", async (RAM ram, ApplicationDbContext db) =>
{
    // сначала ищем
    RAM? updated = await db.RAMS.FirstOrDefaultAsync(r => r.Id == ram.Id);
    // если есть - обновляем поля, айди не меняется
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

// 1 // Add-Migration Init - создается один раз (в консоли), а выполнять их можно многократно
// при внесении и зменений в класс можно новые миграции создать
// 2 // Update-Database - применяем миграцию, подключаемся к БД и создаст таблицу

app.Run();
