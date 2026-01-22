using System;
using Svarozhich.Models;
using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Services;

public class ProjectsAppService(
    ProjectRepository repository,
    ProjectTreeBuilder treeBuilder,
    RecentProjectsService recentsService,
    WorkspaceService workspace,
    ProjectTemplatesService templatesService,
    FilesystemRepository filesystemRepository)
{
    public ProjectFileNode CreateProjectFromTemplate(string name, string projectsPath, ProjectTemplate template)
    {
        var folderResult = filesystemRepository.CreateFolder(false, projectsPath, name);
        templatesService.ApplyTemplate(template, folderResult.FullPath);
        var project = new Project(name);
        project.AddScene("Default Scene");
        repository.Save(folderResult.FullPath, project);
        var filesTree = treeBuilder.Build(folderResult.FullPath);
        workspace.OpenProject(project, filesTree);
        recentsService.MarkOpened(project, filesTree);
        return filesTree;
    }

    public Project LoadFromFolder(string path)
    {
        var project = repository.LoadFromFolder(path);
        var filesTree = treeBuilder.Build(path);
        recentsService.MarkOpened(project, filesTree);
        workspace.OpenProject(project, filesTree);
        return project;
    }
    
    public void SaveCurrent()
    {
        var project = workspace.CurrentProject;
        var path = workspace.ProjectTreeRoot?.FullPath;
        if (project == null || path == null) return;
        repository.Save(path, project);
    }
    
    public void CloseProject()
    {
        workspace.CloseProject();
    }
}