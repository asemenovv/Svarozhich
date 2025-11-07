using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class ProjectsExploreDialogViewModel : ViewModelBase
{
    public void ShowOpenProjectView()
    {
        WeakReferenceMessenger.Default.Send(new ShowOpenProjectsViewInProjectExploreDialogMessage());
    }
    
    public void ShowCreateProjectView()
    {
        WeakReferenceMessenger.Default.Send(new ShowCreateProjectsViewInProjectExploreDialogMessage());
    }
}