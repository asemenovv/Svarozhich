using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Svarozhich.Models.Commands;
using Svarozhich.Services;
using Svarozhich.ViewModels;
using Svarozhich.ViewModels.Controls.Editors;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich;

public partial class App : Application
{
    private static IServiceProvider Services { get; set; } = null!;
    
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
        services.AddSingleton<FilesExplorerViewModel>();
        services.AddSingleton<SceneBrowserViewModel>();
        
        services.AddSingleton<ProjectsExploreDialogViewModel>();
        services.AddSingleton<ProjectsExploreDialog>();
        services.AddSingleton<NewProjectViewModel>();
        services.AddSingleton<OpenProjectViewModel>();

        services.AddSingleton<NodeEditorViewModel>();
        
        services.AddSingleton<ProjectsService>();
        services.AddSingleton<UndoRedoService>();
        
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(App).Assembly);
        });
        services.AddLogging(builder => builder.AddConsole());
    }
}