using CommunityToolkit.Mvvm.ComponentModel;
using pr.Services;
using pr.Data;
using System;

namespace pr.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;

    private readonly PerpusDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public MainViewModel(PerpusDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;

        // Create ViewModels
        var loginVM = new LoginViewModel(_dbContext, _sessionService);
        var registerVM = new RegisterViewModel(_dbContext);

        // Navigation from Login to Register
        loginVM.OnNavigateToRegister = () => CurrentViewModel = (ViewModelBase)registerVM;

        // Navigation from Register back to Login
        registerVM.OnNavigateToLogin = () => CurrentViewModel = (ViewModelBase)loginVM;

        // Navigation from Login to Dashboard on success
        loginVM.OnLoginSuccess = (userLevel) =>
        {
            Action onLogout = () => CurrentViewModel = (ViewModelBase)loginVM;
            if (userLevel == "admin")
            {
                CurrentViewModel = new AdminDashboardViewModel(_sessionService, onLogout);
            }
            else if (userLevel == "anggota")
            {
                CurrentViewModel = new MemberDashboardViewModel(_sessionService, onLogout);
            }
        };

        // Start with LoginViewModel
        CurrentViewModel = (ViewModelBase)loginVM;
    }
}
