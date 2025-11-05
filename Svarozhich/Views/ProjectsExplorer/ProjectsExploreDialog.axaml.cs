using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;
using Svarozhich.ViewModels;

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
    public ProjectsExploreDialog()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode)
            return;
        
        WeakReferenceMessenger.Default.Register<ProjectsExploreDialog, CloseProjectExploreDialogMessage>(this,
            static (w, m) =>
            {
                w.Close();
            });
    }

    private void ProjectButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, OpenProjectButton))
        {
            OpenProjectView.IsVisible = true;
            OpenProjectButton.IsChecked = true;
            CreateProjectView.IsVisible = false;
            CreateProjectButton.IsChecked = false;
        }
        else
        {
            CreateProjectView.IsVisible = true;
            CreateProjectButton.IsChecked = true;
            OpenProjectView.IsVisible = false;
            OpenProjectButton.IsChecked = false;
        }
    }
}