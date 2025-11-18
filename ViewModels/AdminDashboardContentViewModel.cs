using CommunityToolkit.Mvvm.ComponentModel;

namespace pr.ViewModels;

public partial class AdminDashboardContentViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _dashboardTitle = "Dashboard Admin";

    [ObservableProperty]
    private string _welcomeText = "Selamat datang di Sistem Perpustakaan";
}
