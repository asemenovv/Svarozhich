using Svarozhich.Models;
using Svarozhich.Repository;

namespace Svarozhich.Services;

public class ProjectsAppService(
    ProjectRepository repository,
    RecentProjectsService recentsService,
    WorkspaceService workspace,
    ProjectTemplatesService templatesService)
{
    public Project CreateProjectFromTemplate(string name, string path, ProjectTemplate template)
    {
        var rootFolder = new ProjectFileNode(path);
        rootFolder.CreateIfNotExist();
        var project = new Project(name, rootFolder);
        project.CreateScene("Default Scene");
        templatesService.ApplyTemplate(template, rootFolder);
        repository.Save(project);
        workspace.SetCurrentProject(project);
        recentsService.MarkOpened(project);
        return project;
    }

    public Project LoadFromFolder(string path)
    {
        var rootFolder = new ProjectFileNode(path);
        var project = repository.LoadFromFolder(rootFolder);
        recentsService.MarkOpened(project);
        workspace.SetCurrentProject(project);
        return project;
    }
    
    public void SaveCurrent()
    {
        var p = workspace.CurrentProject;
        if (p is null) return;
        repository.Save(p);
    }
    
    public void CloseProject()
    {
        workspace.CloseProject();
    }
}