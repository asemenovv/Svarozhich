using System.IO;
using Svarozhich.Models;
using Svarozhich.Models.Project;

namespace Svarozhich.Repository;

public class ProjectLayout
{
    private static string ScenesFolderName => "Scenes";

    public string ProjectFilePath(string projectPath, string projectName)
    {
        var projectFilename = $"{projectName}{ProjectFileNodeType.ProjectFile.GetExtension()}";
        return Path.Combine(projectPath, projectFilename);
    }

    public string ScenesFolder(string projectPath)
        => Path.Combine(projectPath, ScenesFolderName);

    public string SceneFileRelativePath(string sceneName)
    {
        var safe = MakeSafeFileName(sceneName);
        return Path.Combine(ScenesFolderName, safe + ProjectFileNodeType.Scene.GetExtension());
    }

    public string SceneFileAbsolutePath(string projectPath, string sceneName)
        => Path.Combine(projectPath, SceneFileRelativePath(sceneName));

    public string PreviewImage(string projectPath)
        => Path.Combine(AppFolder(projectPath), "preview.png");

    public string AppFolder(string projectPath)
        => Path.Combine(projectPath, ".Svarozhich");

    public string TrashFolder(string projectPath)
        => Path.Combine(AppFolder(projectPath), "Trash");

    private static string MakeSafeFileName(string name)
    {
        name = name.Trim();
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return string.IsNullOrWhiteSpace(name) ? "Scene" : name;
    }
}