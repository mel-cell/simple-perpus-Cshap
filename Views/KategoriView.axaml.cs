using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace pr.Views;

public partial class KategoriView : UserControl
{
    public KategoriView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
