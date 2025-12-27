using System;
using Svarozhich.Models;
using Svarozhich.Services;
using Svarozhich.Utils;

namespace Svarozhich.Repository;

public class ProjectRepository
{
    private readonly ISerializer<ProjectBinding> _serializer;
    private readonly ProjectLayout _layout;

    public ProjectRepository(ISerializer<ProjectBinding> serializer, ProjectLayout layout)
    {
        _serializer = serializer;
        _layout = layout;
    }

    public Project LoadFromFolder(ProjectFileNode rootFolder)
    {
        var projectFiles = rootFolder.LookupFiles(ProjectFileNodeType.ProjectFile);
        switch (projectFiles.Count)
        {
            case 0:
                throw new ArgumentException($"Folder {rootFolder} is not valid.");
            case > 1:
                throw new ArgumentException($"Folder {rootFolder} contains more than one project file.");
        }

        var projectBinding = _serializer.FromFile(projectFiles[0].FullPath);
        if (projectBinding == null)
        {
            throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        }
        var project = new Project(projectBinding.Name, rootFolder);
        project.MarkClean();
        return project;
    }

    public void Save(Project project)
    {
        var projectBinding = new ProjectBinding { Name = project.Name };
        var projectFile = _layout.ProjectFilePath(project);
        _serializer.ToFile(projectBinding, projectFile);
    }
}