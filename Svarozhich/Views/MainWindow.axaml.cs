using System;
using Avalonia.Controls;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Window_OnOpened(object? sender, EventArgs eventArgs)
    {
        try
        {
            var projectsExploreDialog = new ProjectsExploreDialog()
            {
                DataContext = new ProjectsExploreDialogViewModel()
            };
            var result = await projectsExploreDialog.ShowDialog<ProjectExploreResult?>(this);
            if (result is { Mode: ProjectExploreResultMode.Exit })
            {
                Close();
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            Close();
        }
    }
}