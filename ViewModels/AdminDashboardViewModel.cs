using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Services;
using pr.Data;
using pr.Models;
using System;
using Avalonia.Controls;
using pr.Views;

namespace pr.ViewModels;

public partial class AdminDashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage = string.Empty;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    private readonly ISessionService _sessionService;
    private readonly PerpusDbContext _dbContext;
    private readonly Action _onLogout;

    public AdminDashboardViewModel(ISessionService sessionService, Action onLogout)
    {
        _sessionService = sessionService;
        _onLogout = onLogout;
        _dbContext = new PerpusDbContext(); // Or inject if needed
        var user = _sessionService.CurrentUser;
        WelcomeMessage = $"Welcome Admin, {user?.UserName}";
        ShowDashboard();
    }

    [RelayCommand]
public void ShowDashboard()
    {
        CurrentView = new AdminDashboardContentViewModel();
    }

    [RelayCommand]
    public void ShowMasterBuku()
    {
        WelcomeMessage = "Master Buku clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new BukuViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Buku",
                Content = new BukuView { DataContext = vm },
                Width = 1200,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Buku window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowMasterPenulis()
    {
        WelcomeMessage = "Master Penulis clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new PenulisViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Penulis",
                Content = new PenulisView { DataContext = vm },
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Penulis window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowMasterPenerbit()
    {
        WelcomeMessage = "Master Penerbit clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new PenerbitViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Penerbit",
                Content = new PenerbitView { DataContext = vm },
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Penerbit window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowMasterRak()
    {
        WelcomeMessage = "Master Rak clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new RakViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Rak",
                Content = new RakView { DataContext = vm },
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Rak window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowMasterSiswa()
    {
        WelcomeMessage = "Master Siswa clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new SiswaViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Siswa",
                Content = new SiswaView { DataContext = vm },
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Siswa window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowKategori()
    {
        WelcomeMessage = "Kategori clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new KategoriViewModel(_dbContext);
            var window = new Window
            {
                Title = "Master Kategori",
                Content = new KategoriView { DataContext = vm },
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Kategori window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowPeminjaman()
    {
        WelcomeMessage = "Peminjaman clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new PeminjamanViewModel(_dbContext);
            var window = new Window
            {
                Title = "Peminjaman Buku",
                Content = new PeminjamanView { DataContext = vm },
                Width = 1200,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Peminjaman window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void ShowPengembalian()
    {
        WelcomeMessage = "Pengembalian clicked";
        ErrorMessage = string.Empty;
        try
        {
            var vm = new PengembalianViewModel(_dbContext);
            var window = new Window
            {
                Title = "Pengembalian Buku",
                Content = new PengembalianView { DataContext = vm },
                Width = 1200,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error opening Pengembalian window: {ex.Message}";
        }
    }

    [RelayCommand]
    public void Logout()
    {
        _sessionService.ClearCurrentUser();
        // Navigation handled by MainViewModel
    }
}
