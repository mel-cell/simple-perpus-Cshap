using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace pr.Views;

public partial class PenerbitView : UserControl
{
    public PenerbitView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
