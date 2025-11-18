using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace pr.Views;

public partial class BukuView : UserControl
{
    public BukuView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
