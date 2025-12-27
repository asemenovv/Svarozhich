using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Svarozhich.Models;
using Svarozhich.Repository;
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<FilesExplorerViewModel>();
        services.AddSingleton<SceneBrowserViewModel>();
        
        services.AddSingleton<ProjectsExploreDialogViewModel>();
        services.AddSingleton<ProjectsExploreDialog>();
        services.AddSingleton<NewProjectViewModel>();
        services.AddSingleton<OpenProjectViewModel>();

        services.AddSingleton<NodeEditorViewModel>();
        
        services.AddSingleton<ProjectsAppService>();
        services.AddSingleton<RecentProjectsService>();
        services.AddSingleton<WorkspaceService>();
        services.AddSingleton<ProjectTemplatesService>();
        services.AddSingleton<UndoRedoService>();
        services.AddSingleton<TrashFolderService>();

        services.AddSingleton<ProjectRepository>();
        services.AddSingleton<ProjectLayout>();
        services.AddSingleton<ProjectTemplateLayout>();
        services.AddSingleton<InstallationFolderLayout>();
        
        services.AddSingleton<ISerializer<ProjectBinding>>(new XmlSerializer<ProjectBinding>());
        services.AddSingleton<ISerializer<ProjectTemplate>>(new XmlSerializer<ProjectTemplate>());
        services.AddSingleton<ISerializer<OpenedProjectData>>(new XmlSerializer<OpenedProjectData>());
        
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(App).Assembly);
        });
        services.AddLogging(builder => builder.AddConsole());
    }
}