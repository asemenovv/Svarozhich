using System;
using System.IO;
using Svarozhich.Models.ECS;
using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Services;

public class SceneService(
    ProjectLayout layout,
    SceneRepository sceneRepository,
    ProjectRepository projectRepository,
    FilesystemRepository filesystem,
    WorkspaceService workspace)
{
    public SceneRef CreateScene(Project project, string sceneName, SceneId? id = null, string? relativePath = null)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
            throw new ArgumentException("Scene name is empty.", nameof(sceneName));

        var rel = relativePath ?? layout.SceneFileRelativePath(sceneName);
        var abs = Path.Combine(project.RootPath, rel);

        filesystem.CreateFolder(false, layout.ScenesFolder(project.RootPath));

        var scene = new Scene(id ?? SceneId.New(), sceneName);
        sceneRepository.Save(abs, scene);

        var sceneRef = new SceneRef(scene.Id, scene.Name, rel);
        project.AddScene(sceneRef);
        projectRepository.Save(project.RootPath, project);

        // обновить дерево
        if (workspace.ProjectTreeRoot != null)
        {
            var scenesNode = FindOrCreateScenesNode(workspace.ProjectTreeRoot, layout.ScenesFolder(project.RootPath));
            scenesNode.Children.Add(new ProjectFileNode(abs, scenesNode, ProjectFileNodeType.Scene));
        }

        return sceneRef;
    }

    public void DeleteScene(Project project, SceneRef sceneRef)
    {
        var abs = Path.Combine(project.RootPath, sceneRef.RelativePath);

        project.RemoveScene(sceneRef.Id);
        projectRepository.Save(project.RootPath, project);

        if (File.Exists(abs))
            filesystem.Delete(abs);

        if (workspace.ProjectTreeRoot != null)
            RemoveNodeByPath(workspace.ProjectTreeRoot, abs);
    }

    private static ProjectFileNode FindOrCreateScenesNode(ProjectFileNode root, string scenesPath)
    {
        foreach (var child in root.Children)
            if (child.IsFolder && child.FullPath == scenesPath)
                return child;

        // папка уже создана на диске, просто добавляем ноду
        var node = new ProjectFileNode(scenesPath, root, ProjectFileNodeType.Folder);
        root.Children.Add(node);
        return node;
    }

    private static void RemoveNodeByPath(ProjectFileNode root, string fullPath)
    {
        foreach (var child in root.Children)
        {
            if (child.FullPath == fullPath)
            {
                child.Delete();
                return;
            }
            if (child.IsFolder)
                RemoveNodeByPath(child, fullPath);
        }
    }
}
