using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pr.Services;
using pr.Data;
using pr.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using BCrypt;

namespace pr.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    public Action<string>? OnLoginSuccess;
    public Action? OnNavigateToRegister;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly PerpusDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public LoginViewModel(PerpusDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username dan password harus diisi.";
            return;
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserUsername == Username);

        if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.UserPassword))
        {
            _sessionService.SetCurrentUser(user);
            OnLoginSuccess?.Invoke(user.UserLevel.ToString());
        }
        else
        {
            ErrorMessage = "Username atau password salah.";
        }
    }

    [RelayCommand]
    private void NavigateToRegister()
    {
        OnNavigateToRegister?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        // Close the application or handle cancel
    }
}
