using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;

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

public partial class ProjectsExploreDialog : Window
{
    private static void _closeDialogHandler(ProjectsExploreDialog dialog, CloseProjectExploreDialogMessage message)
    {
        dialog.Close();
    }

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

        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog,
            CloseProjectExploreDialogMessage>(this, _closeDialogHandler);
        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog,
            ShowOpenProjectsViewInProjectExploreDialogMessage>(this, _showOpenProjectViewHandler);
        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog,
            ShowCreateProjectsViewInProjectExploreDialogMessage>(this, _showCreateProjectViewHandler);
    }
}