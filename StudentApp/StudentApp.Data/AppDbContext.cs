using Microsoft.EntityFrameworkCore;

namespace StudentApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Kart> Karts => Set<Kart>();
    public DbSet<Race> Races => Set<Race>();

    public static string DbPath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "studentapp.db");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}