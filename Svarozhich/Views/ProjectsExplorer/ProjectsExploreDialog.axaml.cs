using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;
using Svarozhich.ViewModels;
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

public partial class ProjectsExploreDialog : Window
{
    private readonly MessageHandler<ProjectsExploreDialog, CloseProjectExploreDialogMessage> _closeDialogHandler =
        static (dialog, message) => { dialog.Close(); };
    private readonly MessageHandler<ProjectsExploreDialog, ShowOpenProjectsViewInProjectExploreDialogMessage> _showOpenProjectViewHandler =
        static (dialog, message) =>
        {
            dialog.OpenProjectView.IsVisible = true;
            dialog.OpenProjectButton.IsChecked = true;
            dialog.NewProjectView.IsVisible = false;
            dialog.CreateProjectButton.IsChecked = false;
        };
    private readonly MessageHandler<ProjectsExploreDialog, ShowCreateProjectsViewInProjectExploreDialogMessage> _showCreateProjectViewHandler =
        static (dialog, message) =>
        {
            dialog.NewProjectView.IsVisible = true;
            dialog.CreateProjectButton.IsChecked = true;
            dialog.OpenProjectView.IsVisible = false;
            dialog.OpenProjectButton.IsChecked = false;
        };

    public ProjectsExploreDialog()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        WeakReferenceMessenger.Default.Register(this, _closeDialogHandler);
        WeakReferenceMessenger.Default.Register(this, _showOpenProjectViewHandler);
        WeakReferenceMessenger.Default.Register(this, _showCreateProjectViewHandler);
    }
}