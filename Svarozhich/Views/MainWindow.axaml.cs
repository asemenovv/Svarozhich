using System;
using Avalonia.Controls;
using MediatR;
using Svarozhich.Models.Events;
using Svarozhich.ViewModels;
using Svarozhich.ViewModels.ProjectsExplorer;
using Svarozhich.Views.ProjectsExplorer;

namespace Svarozhich.Views;

public partial class MainWindow : Window
{
    private readonly ProjectsExploreDialog _projectsExploreDialog;
    private readonly IMediator _mediator;

    public MainWindow(MainWindowViewModel viewModel, ProjectsExploreDialog projectsExploreDialog, IMediator mediator)
    {
        _projectsExploreDialog = projectsExploreDialog;
        _mediator = mediator;
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