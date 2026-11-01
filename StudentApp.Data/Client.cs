namespace StudentApp.Data;

public class Client
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsMember { get; set; }

    public List<Race> Races { get; set; } = new();

    public override string ToString()
        => $"{FullName}, тел. {Phone}{(IsMember ? " (член клуба)" : "")}";
}