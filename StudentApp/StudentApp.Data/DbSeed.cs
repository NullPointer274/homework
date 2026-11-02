namespace StudentApp.Data;

public static class DbSeed
{
    public static void Seed(AppDbContext db)
    {
        if (db.Clients.Any() || db.Karts.Any() || db.Races.Any())
            return;

        var clients = new List<Client>
        {
            new() { FullName = "Иван Петров",   Phone = "+7 900 111-22-33", IsMember = true  },
            new() { FullName = "Анна Смирнова", Phone = "+7 900 222-33-44", IsMember = false },
            new() { FullName = "Олег Кузнецов", Phone = "+7 900 333-44-55", IsMember = true  },
            new() { FullName = "Мария Орлова",  Phone = "+7 900 444-55-66", IsMember = false },
        };

        var karts = new List<Kart>
        {
            new() { Model = "Sodi GT5",   EngineVolume = 270, MaxSpeed = 60, PricePerRace = 1500 },
            new() { Model = "CRG Rental", EngineVolume = 390, MaxSpeed = 75, PricePerRace = 2000 },
            new() { Model = "Rotax Evo",  EngineVolume = 125, MaxSpeed = 95, PricePerRace = 3000 },
        };

        db.Clients.AddRange(clients);
        db.Karts.AddRange(karts);
        db.SaveChanges();

        var now = DateTime.Now;
        var races = new List<Race>
        {
            new() { ClientId = clients[0].Id, KartId = karts[0].Id, StartTime = now.AddHours(-3),   Laps = 15, BestLapMs = 32_500, TotalPrice = 1500, IsFinished = true  },
            new() { ClientId = clients[1].Id, KartId = karts[1].Id, StartTime = now.AddHours(-2),   Laps = 20, BestLapMs = 29_100, TotalPrice = 2000, IsFinished = true  },
            new() { ClientId = clients[2].Id, KartId = karts[2].Id, StartTime = now.AddMinutes(-40), Laps = 12, BestLapMs = 27_800, TotalPrice = 3000, IsFinished = false },
            new() { ClientId = clients[3].Id, KartId = karts[0].Id, StartTime = now.AddMinutes(-20), Laps = 10, BestLapMs = 33_200, TotalPrice = 1500, IsFinished = false },
        };

        db.Races.AddRange(races);
        db.SaveChanges();
    }
}