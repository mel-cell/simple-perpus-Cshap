using CommunityToolkit.Mvvm.ComponentModel;

namespace pr.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string? _errorMessage;
}

