using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Utils;

namespace Svarozhich.Models;

public class Project
{
    private static readonly string Extension = ".svch";
    private bool _isDirty;
    private readonly List<Scene> _scenes = [];
    public string Name { get; private set; }
    private string RootFolder { get; set; }

    public IReadOnlyList<Scene> Scenes => _scenes.AsReadOnly();
    
    public Project(string name, string rootFolder)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(rootFolder))
        {
            throw new ArgumentException("Root folder cannot be null or whitespace.", nameof(rootFolder));
        }
        Name = name.Trim();
        RootFolder = rootFolder.Trim();
    }

    public Scene CreateScene(string name, string projectFolder = "Scenes/")
    {
        if (_scenes.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"Scene with name {name} already exists in Project {Name}.", nameof(name));
        }

        var scene = new Scene(this, name, projectFolder);
        _scenes.Add(scene);
        MarkDirty();
        return scene;
    }

    public void Save(ISerializer serializer)
    {
        if (_isDirty)
        {
            serializer.ToFile(ToDto(), FullPath());
        }
        _isDirty =  false;
    }
    
    private string FullPath()
    {
        return Path.Combine(RootFolder, $"{Name}{Extension}");
    }

    private ProjectBinding ToDto()
    {
        var dto = new ProjectBinding()
        {
            Name = Name,
            RootFolder = RootFolder
        };
        return dto;
    }

    private void MarkDirty()
    {
        _isDirty = true;
    }
}