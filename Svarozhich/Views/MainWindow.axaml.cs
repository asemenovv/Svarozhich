using System;
using Avalonia.Controls;
using Svarozhich.ViewModels;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich.Views;

public partial class MainWindow : Window
{
    private readonly ProjectsExploreDialog _projectsExploreDialog;

    public MainWindow(MainWindowViewModel viewModel, ProjectsExploreDialog projectsExploreDialog)
    {
        _projectsExploreDialog = projectsExploreDialog;
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void Window_OnOpened(object? sender, EventArgs eventArgs)
    {
        try
        {
            var result = await _projectsExploreDialog.ShowDialog<ProjectExploreResult?>(this);
            if (result is { Mode: ProjectExploreResultMode.Exit })
            {
                //Close();
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            Close();
        }
    }
}