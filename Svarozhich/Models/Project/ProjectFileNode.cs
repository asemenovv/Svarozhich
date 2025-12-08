using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Svarozhich.Models;

[AttributeUsage(AttributeTargets.Field)]
public class FileExtensionsAttribute(params string[] extensions) : Attribute
{
    public string[] Extensions { get; } = extensions;
}

public static class ProjectFileNodeTypeExtensions
{
    public static IReadOnlyList<string> GetExtensions(this ProjectFileNodeType type)
    {
        var field = type.GetType().GetField(type.ToString());
        if (field == null) return [];
        var attr = field.GetCustomAttribute<FileExtensionsAttribute>();
        return attr?.Extensions ?? Array.Empty<string>();
    }
    
    public static string? GetExtension(this ProjectFileNodeType type)
    {
        var extensions = type.GetExtensions();
        return extensions.Count == 0 ? null : extensions[0];
    }
}

public enum ProjectFileNodeType
{
    [FileExtensions]
    RootFolder,
    [FileExtensions]
    Folder,
    [FileExtensions(".svch")]
    ProjectFile,
    [FileExtensions(".xml")]
    Scene,
    [FileExtensions(".png", ".jpg", ".jpeg", ".tga", ".bmp")]
    Texture,
    [FileExtensions(".obj")]
    Mesh,
    [FileExtensions(".vert", ".frag", ".glsl", ".hlsl")]
    Shader,
    Script
}

public class ProjectFileNode : ReactiveObject
{
    public ProjectFileNode? Parent { get; }

    [Reactive]
    public string Name { get; private set; }

    [Reactive]
    public string FullPath { get; private set; }

    public ProjectFileNodeType NodeType { get; }

    public ObservableCollection<ProjectFileNode> Children { get; } = [];

    public bool IsFolder => NodeType is ProjectFileNodeType.Folder or ProjectFileNodeType.RootFolder;

    public ProjectFileNode(string fullPath, ProjectFileNode? parent = null,
        ProjectFileNodeType nodeType = ProjectFileNodeType.RootFolder)
    {
        Parent = parent;
        Name = Path.GetFileName(fullPath);
        FullPath = fullPath;
        NodeType = nodeType;
        if (nodeType == ProjectFileNodeType.Folder || nodeType == ProjectFileNodeType.RootFolder)
        {
            ScanFolder(this);
        }
    }

    private void ScanFolder(ProjectFileNode folder)
    {
        if (!IsFolder || IsHidden()) return;
        folder.Children.Clear();
        folder.Children.AddRange(folder.LookupFolders());
        foreach (var type in Enum.GetValues(typeof(ProjectFileNodeType)).Cast<ProjectFileNodeType>())
        {
            folder.Children.AddRange(folder.LookupFiles(type));
        }
    }

    private bool IsHidden()
    {
        var directoryInfo = new DirectoryInfo(FullPath);
        return directoryInfo.Attributes.HasFlag(FileAttributes.Hidden);
    }

    public ProjectFileNode Child(string name)
    {
        return Children.First(c => c.Name == name);
    }

    public List<ProjectFileNode> LookupFiles(ProjectFileNodeType type)
    {
        var files = new List<string>();
        foreach (var extension in type.GetExtensions())
        {
            files.AddRange(Directory.GetFiles(FullPath, $"*{extension}", SearchOption.TopDirectoryOnly));
        }
        return files.Select(
            file => new ProjectFileNode(file, this, type)
        ).ToList();
    }

    public List<ProjectFileNode> LookupFolders()
    {
        return Directory.GetDirectories(FullPath)
            .Select(d => new ProjectFileNode(d, this, ProjectFileNodeType.Folder))
            .Where(d => !d.IsHidden())
            .ToList();
    }

    public bool Valid()
    {
        return Directory.Exists(FullPath);
    }
}