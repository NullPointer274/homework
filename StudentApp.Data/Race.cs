namespace StudentApp.Data;

public class Race
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int KartId { get; set; }
    public Kart Kart { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public int Laps { get; set; }
    public int BestLapMs { get; set; }
    public int TotalPrice { get; set; }
    public bool IsFinished { get; set; }

    public override string ToString()
        => $"{StartTime:dd.MM.yyyy HH:mm} | клиент: {Client?.FullName} | карт: {Kart?.Model} | " +
           $"{Laps} кр., лучший круг {BestLapMs} мс, {TotalPrice} ₽, " +
           $"{(IsFinished ? "завершён" : "идёт")}";
}