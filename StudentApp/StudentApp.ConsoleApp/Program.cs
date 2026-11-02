using Microsoft.EntityFrameworkCore;
using StudentApp.Data;

Console.WriteLine($"Путь к файлу базы: {AppDbContext.DbPath}");
Console.WriteLine();

using var db = new AppDbContext();

db.Database.EnsureCreated();
DbSeed.Seed(db);

Console.WriteLine("Клиенты");
foreach (var c in db.Clients)
    Console.WriteLine($"  [{c.Id}] {c}");

Console.WriteLine();
Console.WriteLine("Карты");
foreach (var k in db.Karts)
    Console.WriteLine($"  [{k.Id}] {k}");

Console.WriteLine();
Console.WriteLine("Заезды");
var races = db.Races
    .Include(r => r.Client)
    .Include(r => r.Kart)
    .OrderBy(r => r.StartTime)
    .ToList();

foreach (var r in races)
    Console.WriteLine($"  [{r.Id}] {r}");