using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using pr.ViewModels;
using pr.Views;
using pr.Data;
using pr.Services;
using Microsoft.Extensions.DependencyInjection;

namespace pr;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            // Set up DI container
            var services = new ServiceCollection();
            services.AddSingleton<PerpusDbContext>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<MemberDashboardViewModel>();
            services.AddTransient<CrudViewModel>();
            services.AddTransient<AdminDashboardContentViewModel>();
            services.AddTransient<MemberDashboardContentViewModel>();

            var serviceProvider = services.BuildServiceProvider();

            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
            var mainView = new MainView { DataContext = mainViewModel };
            desktop.MainWindow = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
