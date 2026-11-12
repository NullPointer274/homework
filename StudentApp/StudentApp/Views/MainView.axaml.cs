using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
            RacesList.ItemsSource = db.Races.Include(r => r.Client).Include(r => r.Kart).ToList();

            RaceClientCombo.ItemsSource = db.Clients.ToList();
            RaceKartCombo.ItemsSource = db.Karts.ToList();
        }
    }

    private void RefreshLists()
    {
        using (var db = new AppDbContext())
        {
            ClientsList.ItemsSource = db.Clients.ToList();
            KartsList.ItemsSource = db.Karts.ToList();
            RacesList.ItemsSource = db.Races.Include(r => r.Client).Include(r => r.Kart).ToList();
            RaceClientCombo.ItemsSource = db.Clients.ToList();
            RaceKartCombo.ItemsSource = db.Karts.ToList();
        }
    }

    private void ClientsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ClientsList.SelectedItem is Client client)
        {
            ClientFullName.Text = client.FullName;
            ClientPhone.Text = client.Phone;
            ClientIsMember.IsChecked = client.IsMember;
        }
    }

    private void AddClient_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ClientFullName.Text) || string.IsNullOrWhiteSpace(ClientPhone.Text))
        {
            ClientMessage.Text = "Ошибка: Заполните ФИО и Телефон!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Clients.Add(new Client { FullName = ClientFullName.Text, Phone = ClientPhone.Text, IsMember = ClientIsMember.IsChecked ?? false });
            db.SaveChanges();

            ClientMessage.Text = "Клиент добавлен!";
            ClientFullName.Text = ""; ClientPhone.Text = ""; ClientIsMember.IsChecked = false;
            RefreshLists();
        }
        catch (Exception ex) { ClientMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SaveClient_Click(object? sender, RoutedEventArgs e)
    {
        if (!(ClientsList.SelectedItem is Client selected))
        {
            ClientMessage.Text = "Ошибка: Выберите клиента в списке!";
            return;
        }
        if (string.IsNullOrWhiteSpace(ClientFullName.Text) || string.IsNullOrWhiteSpace(ClientPhone.Text))
        {
            ClientMessage.Text = "Ошибка: Заполните поля!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var clientToUpdate = db.Clients.Find(selected.Id);
            if (clientToUpdate != null)
            {
                clientToUpdate.FullName = ClientFullName.Text;
                clientToUpdate.Phone = ClientPhone.Text;
                clientToUpdate.IsMember = ClientIsMember.IsChecked ?? false;
                db.SaveChanges();
                ClientMessage.Text = "Данные обновлены!";
                RefreshLists();
            }
        }
        catch (Exception ex) { ClientMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeleteClient_Click(object? sender, RoutedEventArgs e)
    {
        if (!(ClientsList.SelectedItem is Client selected))
        {
            ClientMessage.Text = "Ошибка: Выберите клиента для удаления!";
            return;
        }

        using var db = new AppDbContext();

        bool hasRaces = db.Races.Any(r => r.ClientId == selected.Id);
        if (hasRaces)
        {
            ClientMessage.Text = "Ошибка: Нельзя удалить клиента, у него есть заезды!";
            return;
        }

        try
        {
            var clientToDelete = db.Clients.Find(selected.Id);
            if (clientToDelete != null)
            {
                db.Clients.Remove(clientToDelete);
                db.SaveChanges();
                ClientMessage.Text = "Клиент удалён!";
                RefreshLists();
            }
        }
        catch (Exception ex) { ClientMessage.Text = $"Ошибка при удалении (возможно, у клиента есть заезды): {ex.Message}"; }
    }

    private void KartsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (KartsList.SelectedItem is Kart kart)
        {
            KartModel.Text = kart.Model;
            KartEngine.Text = kart.EngineVolume.ToString();
            KartSpeed.Text = kart.MaxSpeed.ToString();
            KartPrice.Text = kart.PricePerRace.ToString();
        }
    }

    private void AddKart_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(KartModel.Text) ||
            !int.TryParse(KartEngine.Text, out int engine) ||
            !int.TryParse(KartSpeed.Text, out int speed) ||
            !int.TryParse(KartPrice.Text, out int price))
        {
            KartMessage.Text = "Ошибка: Проверьте числа и заполните модель!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Karts.Add(new Kart { Model = KartModel.Text, EngineVolume = engine, MaxSpeed = speed, PricePerRace = price });
            db.SaveChanges();
            KartMessage.Text = "Карт добавлен!";
            RefreshLists();
        }
        catch (Exception ex) { KartMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SaveKart_Click(object? sender, RoutedEventArgs e)
    {
        if (!(KartsList.SelectedItem is Kart selected))
        {
            KartMessage.Text = "Ошибка: Выберите карт в списке!";
            return;
        }
        if (string.IsNullOrWhiteSpace(KartModel.Text) ||
            !int.TryParse(KartEngine.Text, out int engine) ||
            !int.TryParse(KartSpeed.Text, out int speed) ||
            !int.TryParse(KartPrice.Text, out int price))
        {
            KartMessage.Text = "Ошибка: Проверьте числа и заполните модель!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var kartToUpdate = db.Karts.Find(selected.Id);
            if (kartToUpdate != null)
            {
                kartToUpdate.Model = KartModel.Text;
                kartToUpdate.EngineVolume = engine;
                kartToUpdate.MaxSpeed = speed;
                kartToUpdate.PricePerRace = price;
                db.SaveChanges();
                KartMessage.Text = "Карт обновлен!";
                RefreshLists();
            }
        }
        catch (Exception ex) { KartMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeleteKart_Click(object? sender, RoutedEventArgs e)
    {
        if (!(KartsList.SelectedItem is Kart selected))
        {
            KartMessage.Text = "Ошибка: Выберите карт!";
            return;
        }

        using var db = new AppDbContext();

        bool hasRaces = db.Races.Any(r => r.KartId == selected.Id);
        if (hasRaces)
        {
            KartMessage.Text = "Ошибка: Нельзя удалить карт, у него есть заезды!";
            return;
        }

        try
        {
            var k = db.Karts.Find(selected.Id);
            if (k != null)
            {
                db.Karts.Remove(k);
                db.SaveChanges();
                KartMessage.Text = "Карт удалён!";
                RefreshLists();
            }
        }
        catch (Exception ex) { KartMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void RacesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (RacesList.SelectedItem is Race race)
        {
            RaceClientCombo.SelectedItem = RaceClientCombo.ItemsSource?.Cast<Client>().FirstOrDefault(c => c.Id == race.ClientId);
            RaceKartCombo.SelectedItem = RaceKartCombo.ItemsSource?.Cast<Kart>().FirstOrDefault(k => k.Id == race.KartId);

            RaceDatePicker.SelectedDate = race.StartTime.Date;
            RaceTimePicker.SelectedTime = race.StartTime.TimeOfDay;
            RaceLaps.Text = race.Laps.ToString();
            RaceBestLap.Text = race.BestLapMs.ToString();
            RacePrice.Text = race.TotalPrice.ToString();
            RaceIsFinished.IsChecked = race.IsFinished;
        }
    }

    private void AddRace_Click(object? sender, RoutedEventArgs e)
    {
        if (!(RaceClientCombo.SelectedItem is Client client) || !(RaceKartCombo.SelectedItem is Kart kart))
        {
            RaceMessage.Text = "Ошибка: Выберите клиента и карт!";
            return;
        }
        if (RaceDatePicker.SelectedDate == null)
        {
            RaceMessage.Text = "Ошибка: Выберите дату!";
            return;
        }
        if (!int.TryParse(RaceLaps.Text, out int laps) ||
            !int.TryParse(RaceBestLap.Text, out int bestLap) ||
            !int.TryParse(RacePrice.Text, out int price))
        {
            RaceMessage.Text = "Ошибка: Проверьте числа!";
            return;
        }

        try
        {
            using var db = new AppDbContext();

            var date = RaceDatePicker.SelectedDate.Value.Date;
            var time = RaceTimePicker.SelectedTime ?? TimeSpan.Zero;

            var race = new Race
            {
                ClientId = client.Id,
                KartId = kart.Id,   
                StartTime = date + time,
                Laps = laps,
                BestLapMs = bestLap,
                TotalPrice = price,
                IsFinished = RaceIsFinished.IsChecked ?? false
            };

            db.Races.Add(race);
            db.SaveChanges();
            RaceMessage.Text = "Заезд добавлен!";
            RefreshLists();
        }
        catch (Exception ex) { RaceMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void SaveRace_Click(object? sender, RoutedEventArgs e)
    {
        if (!(RacesList.SelectedItem is Race selected))
        {
            RaceMessage.Text = "Ошибка: Выберите заезд в списке!";
            return;
        }
        if (!(RaceClientCombo.SelectedItem is Client client) || !(RaceKartCombo.SelectedItem is Kart kart) || RaceDatePicker.SelectedDate == null)
        {
            RaceMessage.Text = "Ошибка: Выберите клиента, карт и дату!";
            return;
        }
        if (!int.TryParse(RaceLaps.Text, out int laps) || !int.TryParse(RaceBestLap.Text, out int bestLap) || !int.TryParse(RacePrice.Text, out int price))
        {
            RaceMessage.Text = "Ошибка: Проверьте числа!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var raceToUpdate = db.Races.Find(selected.Id);
            if (raceToUpdate != null)
            {
                raceToUpdate.ClientId = client.Id;
                raceToUpdate.KartId = kart.Id;
                raceToUpdate.StartTime = RaceDatePicker.SelectedDate.Value.Date + (RaceTimePicker.SelectedTime ?? TimeSpan.Zero);
                raceToUpdate.Laps = laps;
                raceToUpdate.BestLapMs = bestLap;
                raceToUpdate.TotalPrice = price;
                raceToUpdate.IsFinished = RaceIsFinished.IsChecked ?? false;
                db.SaveChanges();
                RaceMessage.Text = "Заезд обновлен!";
                RefreshLists();
            }
        }
        catch (Exception ex) { RaceMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }

    private void DeleteRace_Click(object? sender, RoutedEventArgs e)
    {
        if (!(RacesList.SelectedItem is Race selected))
        {
            RaceMessage.Text = "Ошибка: Выберите заезд!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var race = db.Races.Find(selected.Id);
            if (race != null)
            {
                db.Races.Remove(race);
                db.SaveChanges();
                RaceMessage.Text = "Заезд удалён!";
                RefreshLists();
            }
        }
        catch (Exception ex) { RaceMessage.Text = $"Ошибка БД: {ex.Message}"; }
    }
}