using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Services;
using pr.Data;
using pr.Models;
using System;

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
    private void ShowDashboard()
    {
        CurrentView = new AdminDashboardContentViewModel();
    }

    [RelayCommand]
    private void ShowMasterBuku()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Buku), "BukuId");
    }

    [RelayCommand]
    private void ShowMasterPenulis()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Penulis), "PenulisId");
    }

    [RelayCommand]
    private void ShowMasterPenerbit()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Penerbit), "PenerbitId");
    }

    [RelayCommand]
    private void ShowMasterRak()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Rak), "RakId");
    }

    [RelayCommand]
    private void ShowMasterSiswa()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Siswa), "SiswaId");
    }

    [RelayCommand]
    private void ShowKategori()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Kategori), "KategoriId");
    }

    [RelayCommand]
    private void ShowPeminjaman()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Peminjaman), "PeminjamanId");
    }

    [RelayCommand]
    private void ShowPengembalian()
    {
        CurrentView = new CrudViewModel(_dbContext, typeof(Pengembalian), "PengembalianId");
    }

    [RelayCommand]
    private void Logout()
    {
        _sessionService.ClearCurrentUser();
        // Navigation handled by MainViewModel
    }
}
