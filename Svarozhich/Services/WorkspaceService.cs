using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;

namespace Svarozhich.Services;

public class WorkspaceService : ReactiveObject
{
    [Reactive]
    public Project? CurrentProject { get; private set; }
    
    public void SetCurrentProject(Project project)
    {
        CurrentProject = project;
    }
    
    public void CloseProject()
    {
        CurrentProject = null;
    }
}