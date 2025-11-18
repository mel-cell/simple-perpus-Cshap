using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace pr.Views;

public partial class SiswaView : UserControl
{
    public SiswaView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
