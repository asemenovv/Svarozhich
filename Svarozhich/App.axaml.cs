using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Svarozhich.Services;
using Svarozhich.Utils;
using Svarozhich.ViewModels;
using Svarozhich.ViewModels.Controls.Editors;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        ConfigureServices(collection);
        Services = collection.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        
        services.AddSingleton<ProjectsExploreDialogViewModel>();
        services.AddSingleton<ProjectsExploreDialog>();
        services.AddSingleton<NewProjectViewModel>();
        services.AddSingleton<OpenProjectViewModel>();

        services.AddSingleton<NodeEditorViewModel>();

        services.AddSingleton<XmlSerializer>();
        services.AddSingleton<ProjectsService>();
        services.AddSingleton<ProgramGraphsService>();
    }
}