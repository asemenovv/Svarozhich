using System;
using System.Linq;
using DynamicData;
using Svarozhich.Models;
using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Services;

public class ProjectTreeBuilder(FilesystemRepository filesystem)
{
    public ProjectFileNode Build(string projectRootPath)
    {
        var root = new ProjectFileNode(projectRootPath);
        BuildChildren(root);
        return root;
    }

    public void Refresh(ProjectFileNode root)
    {
        root.Children.Clear();
        BuildChildren(root);
    }

    private void BuildChildren(ProjectFileNode folderNode)
    {
        foreach (var dir in filesystem.EnumerateDirectories(folderNode.FullPath))
        {
            var childFolder = new ProjectFileNode(dir, folderNode, ProjectFileNodeType.Folder);
            folderNode.Children.Add(childFolder);
            BuildChildren(childFolder);
        }
        foreach (var file in filesystem.EnumerateFiles(folderNode.FullPath))
        {
            var type = ProjectFileNodeTypeExtensions.FromPath(file);
            if (type is null) continue;
            folderNode.Children.Add(new ProjectFileNode(file, folderNode, type.Value));
        }
        var sortedFiles = folderNode.Children
            .OrderByDescending(n => n.IsFolder)
            .ThenBy(n => n.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
        folderNode.Children.Clear();
        folderNode.Children.AddRange(sortedFiles);
    }
}