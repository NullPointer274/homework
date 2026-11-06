using System.Linq;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using StudentApp.Data;

namespace StudentApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db);

            ClientsList.ItemsSource = db.Clients.ToList();
            KartsList.ItemsSource = db.Karts.ToList();

            RacesList.ItemsSource = db.Races
                .Include(r => r.Client)
                .Include(r => r.Kart)
                .ToList();
        }
    }
}