using System.Reactive;
using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Svarozhich.ViewModels.ProjectsExplorer;

namespace Svarozhich.Views.ProjectsExplorer;

public partial class ProjectsExploreDialog : ReactiveWindow<ProjectsExploreDialogViewModel>
{
    public ProjectsExploreDialog(ProjectsExploreDialogViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        if (Design.IsDesignMode)
            return;
        
        this.WhenActivated(disposables =>
        {
            var newProjectVm = (NewProjectView.DataContext as NewProjectViewModel);
            var openProjectVm = (OpenProjectView.DataContext as OpenProjectViewModel);
            viewModel.ShowOpenProjectViewInteraction.RegisterHandler(_ => ShowOpenProjectView());
            viewModel.ShowCreateProjectViewInteraction.RegisterHandler(_ => ShowCreateProjectView());
            newProjectVm?.CloseDialogInteraction.RegisterHandler(result =>
                {
                    Close(result.Input);
                })
                .DisposeWith(disposables);
            openProjectVm?.CloseDialogInteraction.RegisterHandler(result =>
                {
                    Close(result.Input);
                })
                .DisposeWith(disposables);
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