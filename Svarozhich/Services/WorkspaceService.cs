using Svarozhich.Models;

namespace Svarozhich.Services;

public class WorkspaceService
{
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