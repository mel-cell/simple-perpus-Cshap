using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Services;
using System;

namespace pr.ViewModels;

public partial class MemberDashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage = string.Empty;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    private readonly ISessionService _sessionService;
    private readonly Action _onLogout;

    public MemberDashboardViewModel(ISessionService sessionService, Action onLogout)
    {
        _sessionService = sessionService;
        _onLogout = onLogout;
        var user = _sessionService.CurrentUser;
        WelcomeMessage = $"Welcome Anggota, {user?.UserName}";
        ShowDashboard();
    }

    [RelayCommand]
    private void ShowDashboard()
    {
        CurrentView = new MemberDashboardContentViewModel(_sessionService);
    }

    [RelayCommand]
    private void Logout()
    {
        _sessionService.ClearCurrentUser();
        // Navigation handled by MainViewModel
    }
}
