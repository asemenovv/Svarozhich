using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Project;

namespace Svarozhich.Services;

public class WorkspaceService(ProjectTreeBuilder treeBuilder) : ReactiveObject
{
    [Reactive]
    public Project? CurrentProject { get; private set; }
    [Reactive]
    public ProjectFileNode? ProjectTreeRoot { get; private set; }
    
    public void OpenProject(Project project, ProjectFileNode projectFileNode)
    {
        CurrentProject = project;
        ProjectTreeRoot = projectFileNode;
    }
    
    public void CloseProject()
    {
        CurrentProject = null;
        ProjectTreeRoot = null;
    }

    public void Refresh()
    {
        if (ProjectTreeRoot != null)
        {
            treeBuilder.Refresh(ProjectTreeRoot);
        }
    }
}