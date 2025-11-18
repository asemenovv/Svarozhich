using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Utils;

namespace Svarozhich.Models;

public class Project : PersistedEntity
{
    private const string Extension = ".svch";
    private readonly List<Scene> _scenes = [];
    public string Name { get; private set; }
    public string RootFolder { get; set; }

    public IReadOnlyList<Scene> Scenes => _scenes.AsReadOnly();
    
    public Project(string name, string rootFolder, List<KeyValuePair<string, string>>? scenes = null)
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
        if (scenes != null) _scenes = scenes.Select(s => new Scene(this, s.Key, s.Value)).ToList();
        MarkDirty();
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
        if (IsDirty)
        {
            serializer.ToFile(ToDto(), ProjectFilePath());
            foreach (var scene in Scenes)
            {
                scene.Save(serializer);
            }
        }
        MarkClean();
    }

    public string AbsolutePath(string projectLocalPath)
    {
        return Path.Combine(RootFolder, projectLocalPath);
    }

    private string ProjectFilePath()
    {
        return Path.Combine(RootFolder, $"{Name}{Extension}");
    }

    private ProjectBinding ToDto()
    {
        var dto = new ProjectBinding
        {
            Name = Name,
            RootFolder = RootFolder,
            Scenes = _scenes.Select(s => s.ToRefDto()).ToList()
        };
        return dto;
    }

    public void InitFromTemplate(ProjectTemplate selectedTemplate)
    {
        if (!Directory.Exists(RootFolder))
        {
            Directory.CreateDirectory(RootFolder);
        }
        else
        {
            throw new InvalidOperationException("Project has been already initialized.");
        }
        selectedTemplate.CreateFolders(RootFolder);
    }

    public static Project OpenFolder(string path, ISerializer serializer)
    {
        var projectFiles = Directory.GetFiles(path, $"*{Extension}", SearchOption.TopDirectoryOnly);
        switch (projectFiles.Length)
        {
            case 0:
                throw new ArgumentException($"Folder {path} is not valid.");
            case > 1:
                throw new ArgumentException($"Folder {path} contains more than one project file.");
        }

        var projectBinding = serializer.FromFile<ProjectBinding>(projectFiles[0]);
        if (projectBinding == null)
        {
            throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        }
        var project = new Project(projectBinding.Name, projectBinding.RootFolder,
            projectBinding.Scenes.Select(s => new KeyValuePair<string, string>(s.Name, s.Path)).ToList());
        project.MarkClean();
        return project;
    }

    public bool Validate()
    {
        if (!Directory.Exists(RootFolder))
        {
            return false;
        }

        if (!File.Exists(ProjectFilePath()))
        {
            return false;
        }
        return true;
    }
}