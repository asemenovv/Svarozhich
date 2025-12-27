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
public class FileExtensionsAttribute(bool deletable = false, bool canBeRenamed = true, params string[] extensions) : Attribute
{
    public string[] Extensions { get; } = extensions;

    public bool IsDeletable { get; set; } = deletable;
    
    public bool CanBeRenamed { get; set; } = canBeRenamed;
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
    
    public static bool IsDeletable(this ProjectFileNodeType type)
    {
        var field = type.GetType().GetField(type.ToString());
        if (field == null) return false;
        var attr = field.GetCustomAttribute<FileExtensionsAttribute>();
        return attr?.IsDeletable ?? false;
    }
    
    public static bool CanBeRenamed(this ProjectFileNodeType type)
    {
        var field = type.GetType().GetField(type.ToString());
        if (field == null) return false;
        var attr = field.GetCustomAttribute<FileExtensionsAttribute>();
        return attr?.CanBeRenamed ?? false;
    }
}

public enum ProjectFileNodeType
{
    [FileExtensions(false, false)]
    RootFolder,
    [FileExtensions(true)]
    Folder,
    [FileExtensions(false, false, ".svch")]
    ProjectFile,
    [FileExtensions(true, false, ".xml")]
    Scene,
    [FileExtensions(true, true, ".png", ".jpg", ".jpeg", ".tga", ".bmp")]
    Texture,
    [FileExtensions(true, true, ".obj")]
    Mesh,
    [FileExtensions(true, true, ".vert", ".frag", ".glsl", ".hlsl")]
    Shader,
    [FileExtensions(true, true, ".cs", ".lua")]
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
    }

    public ProjectFileNode Child(string name)
    {
        return Children.First(c => c.Name == name);
    }

    public string RelativePath()
    {
        if (Parent == null) return "rs://" + Name;
        return Parent.RelativePath() + "/" + Name;
    }

    public void Delete()
    {
        Directory.Delete(FullPath, true);
        Parent?.Children.Remove(this);
    }

    public void MoveTo(string destinationPath)
    {
        if (!Directory.Exists(FullPath)) return;
        Directory.Move(FullPath, destinationPath);
        FullPath = destinationPath;
    }

    public ProjectFileNode CreateChildFolder(string name)
    {
        var path = Path.Combine(FullPath, name);
        Directory.CreateDirectory(path);
        var node = new ProjectFileNode(path, this, ProjectFileNodeType.Folder);
        Children.Add(node);
        return node;
    }
}