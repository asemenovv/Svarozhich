using System;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Repository;

public class ProjectRepository
{
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

        var projectBinding = new XmlSerializer<ProjectBinding>()
            .FromFile(projectFiles[0].FullPath);
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
        var serializer = new XmlSerializer<ProjectBinding>();
        var projectBinding = new ProjectBinding
        {
            Name = project.Name
        };
        var projectFilename = $"{project.Name}{ProjectFileNodeType.ProjectFile.GetExtension()}";
        var projectFile = project.RootProjectFolder.FilePath(projectFilename);
        serializer.ToFile(projectBinding, projectFile);
    }
}