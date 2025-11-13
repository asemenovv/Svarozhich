using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Avalonia;
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
    public ProjectsExploreDialog()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;
        
        this.WhenActivated(disposables =>
        {
            var vm = (ProjectsExploreDialogViewModel)DataContext!;
            var newProjectVm = (NewProjectView.DataContext as NewProjectViewModel);
            var openProjectVm = (OpenProjectView.DataContext as OpenProjectViewModel);
            vm.ShowOpenProjectViewInteraction.RegisterHandler(_ => ShowOpenProjectView());
            vm.ShowCreateProjectViewInteraction.RegisterHandler(_ => ShowCreateProjectView());
            newProjectVm?.CloseDialogInteraction.RegisterHandler(_ => Close()).DisposeWith(disposables);
            openProjectVm?.CloseDialogInteraction.RegisterHandler(_ => Close()).DisposeWith(disposables);
        });
    }

    private void ShowCreateProjectView()
    {
        NewProjectView.IsVisible = true;
        CreateProjectButton.IsChecked = true;
        OpenProjectView.IsVisible = false;
        OpenProjectButton.IsChecked = false;
    }

    private void ShowOpenProjectView()
    {
        OpenProjectView.IsVisible = true;
        OpenProjectButton.IsChecked = true;
        NewProjectView.IsVisible = false;
        CreateProjectButton.IsChecked = false;
    }
}