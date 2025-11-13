using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Svarozhich.Messages;
using Svarozhich.ViewModels.ProjectsExplorer;

namespace Svarozhich.Views.ProjectsExplorer;

public enum ProjectExploreResultMode
{
    Open,
    Create
}

public readonly struct ProjectExploreResult(ProjectExploreResultMode mode)
{
    private ProjectExploreResultMode Mode { get; } = mode;
}

public partial class ProjectsExploreDialog : ReactiveWindow<ProjectsExploreDialogViewModel>
{
    private static void _showOpenProjectViewHandler(ProjectsExploreDialog dialog,
        ShowOpenProjectsViewInProjectExploreDialogMessage message)
    {
        dialog.OpenProjectView.IsVisible = true;
        dialog.OpenProjectButton.IsChecked = true;
        dialog.NewProjectView.IsVisible = false;
        dialog.CreateProjectButton.IsChecked = false;
    }

    private static void _showCreateProjectViewHandler(ProjectsExploreDialog dialog,
        ShowCreateProjectsViewInProjectExploreDialogMessage message)
    {
        dialog.NewProjectView.IsVisible = true;
        dialog.CreateProjectButton.IsChecked = true;
        dialog.OpenProjectView.IsVisible = false;
        dialog.OpenProjectButton.IsChecked = false;
    }

    public ProjectsExploreDialog()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;
        
        this.WhenActivated(disposables =>
        {
            var newProjectVm = (NewProjectView.DataContext as NewProjectViewModel);
            var openProjectVm = (OpenProjectView.DataContext as OpenProjectViewModel);
            newProjectVm?.CloseDialogInteraction.RegisterHandler(_ => Close()).DisposeWith(disposables);
            openProjectVm?.CloseDialogInteraction.RegisterHandler(_ => Close()).DisposeWith(disposables);
        });
        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog,
            ShowOpenProjectsViewInProjectExploreDialogMessage>(this, _showOpenProjectViewHandler);
        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog,
            ShowCreateProjectsViewInProjectExploreDialogMessage>(this, _showCreateProjectViewHandler);
    }
}