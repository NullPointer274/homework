namespace StudentApp.Data;

public class Kart
{
    public int Id { get; set; }
    public string Model { get; set; } = string.Empty;
    public int EngineVolume { get; set; }
    public int MaxSpeed { get; set; }
    public int PricePerRace { get; set; }

    public List<Race> Races { get; set; } = new();

    public override string ToString()
        => $"{Model}: {EngineVolume} см³, до {MaxSpeed} км/ч, {PricePerRace} ₽ за заезд";
}