using System.IO;
using Svarozhich.Models;

namespace Svarozhich.Repository;

public class ProjectLayout
{
    public string ProjectFilePath(string projectPath, string projectName)
    {
        var projectFilename = $"{projectName}{ProjectFileNodeType.ProjectFile.GetExtension()}";
        return Path.Combine(projectPath, projectFilename);
    }

    public string PreviewImage(string projectPath)
    {
        return Path.Combine(AppFolder(projectPath), "preview.png");
    }

    public string AppFolder(string projectPath)
    {
        return Path.Combine(projectPath, ".Svarozhich");
    }

    public string TrashFolder(string projectPath)
    {
        return Path.Combine(AppFolder(projectPath), "Trash");
    }
}