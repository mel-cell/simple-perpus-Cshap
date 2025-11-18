using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace pr.Views;

public partial class PeminjamanView : UserControl
{
    public PeminjamanView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
