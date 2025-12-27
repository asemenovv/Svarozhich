using System.IO;
using Svarozhich.Models;

namespace Svarozhich.Services;

public class ProjectLayout
{
    public string RootFolder(Project project)
    {
        return project.RootProjectFolder.FullPath;
    }

    public string ProjectFilePath(string projectPath, string projectName)
    {
        var projectFilename = $"{projectName}{ProjectFileNodeType.ProjectFile.GetExtension()}";
        return Path.Combine(projectPath, projectFilename);
    }

    public string ProjectFilePath(Project project)
    {
        return ProjectFilePath(RootFolder(project), project.Name);
    }

    public string PreviewImage(string projectPath)
    {
        return Path.Combine(AppFolder(projectPath), "preview.png");
    }

    public string PreviewImage(Project project)
    {
        return PreviewImage(RootFolder(project));
    }

    public string AppFolder(string projectPath)
    {
        return Path.Combine(projectPath, ".Svarozhich");
    }

    public string AppFolder(Project project)
    {
        return AppFolder(RootFolder(project));
    }

    public string TrashFolder(string projectPath)
    {
        return Path.Combine(AppFolder(projectPath), "Trash");
    }

    public string TrashFolder(Project project)
    {
        return TrashFolder(RootFolder(project));
    }

    public string NewProjectFolder(string projectsPath, string projectName)
    {
        return Path.Combine(projectsPath, projectName);
    }
}