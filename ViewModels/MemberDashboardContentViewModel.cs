using CommunityToolkit.Mvvm.ComponentModel;
using pr.Services;

namespace pr.ViewModels;

public partial class MemberDashboardContentViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _dashboardTitle = "Dashboard Anggota";

    [ObservableProperty]
    private string _welcomeText = string.Empty;

    public MemberDashboardContentViewModel(ISessionService sessionService)
    {
        var user = sessionService.CurrentUser;
        WelcomeText = $"Selamat datang, {user?.UserName}!";
    }
}
