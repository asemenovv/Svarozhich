using System;
using System.Linq;
using DynamicData;
using Svarozhich.Models;
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
        foreach (var type in Enum.GetValues(typeof(ProjectFileNodeType)).Cast<ProjectFileNodeType>())
        {
            foreach (var extension in type.GetExtensions())
            {
                folderNode.Children.AddRange(
                    filesystem.EnumerateFiles(folderNode.FullPath, extension)
                        .Select(f  => new ProjectFileNode(f, folderNode, type))
                        .ToList()
                );
            }
        }
    }
}