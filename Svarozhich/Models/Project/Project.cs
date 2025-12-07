using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.Nodes;
using Svarozhich.Models.ProgramGraph.Nodes;
using Svarozhich.Utils;

namespace Svarozhich.Models;

public class Project : PersistedEntity<ProjectBinding>
{
    [Reactive]
    public string Name { get; private set; }
    [Reactive]
    public ProjectFileNode RootProjectFolder { get; private set; }
    
    public ObservableCollection<Scene> Scenes { get; } = [];
    
    public Project(string name, ProjectFileNode rootFolder, List<KeyValuePair<string, string>>? scenes = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }

        if (!rootFolder.Valid())
        {
            throw new ArgumentException("Root folder should exist.", nameof(rootFolder));
        }
        Name = name.Trim();
        RootProjectFolder = rootFolder;
        if (scenes != null) Scenes = new ObservableCollection<Scene>(scenes
            .Select(s => new Scene(this, s.Key, s.Value))
        );
        MarkDirty();
    }

    public Scene CreateScene(string name, string projectFolder = "Scenes/")
    {
        if (Scenes.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"Scene with name {name} already exists in Project {Name}.", nameof(name));
        }

        var scene = new Scene(this, name, projectFolder);
        Scenes.Add(scene);
        MarkDirty();
        return scene;
    }

    protected override string FilePath()
    {
        return RootProjectFolder.Child($"{Name}{ProjectFileNodeType.ProjectFile.GetExtension()}").FullPath;
    }

    public string AbsolutePath(string projectLocalPath)
    {
        return Path.Combine(RootProjectFolder.FullPath, projectLocalPath);
    }

    public NodeGraph GetNodeGraph()
    {
        var nodeGraph = new NodeGraph(this);
        
        var inColorNode = new ConstantColorNode("Red-ish Color", new ColorRgb(1.0f, 0.1f, 0.1f));
        var scalarNode = new ConstantScalarNode("Exposure", 2.0f);
        var expScalarNode = new ExpScalarNode(2.2f);
        var colorScalarMultiplyNode = new ColorScalarMultiplyNode();
        
        nodeGraph.AddNode(inColorNode);
        nodeGraph.AddNode(scalarNode);
        nodeGraph.AddNode(expScalarNode);
        nodeGraph.AddNode(colorScalarMultiplyNode);
        
        nodeGraph.AddConnection(new Connection(inColorNode.OutputColorPort, colorScalarMultiplyNode.InputColorPort));
        nodeGraph.AddConnection(new Connection(scalarNode.OutputScalarPort, expScalarNode.InputScalarPort));
        nodeGraph.AddConnection(new Connection(expScalarNode.OutputScalarPort, colorScalarMultiplyNode.InputScalarPort));
        return nodeGraph;
    }

    protected override ProjectBinding ToDto()
    {
        var dto = new ProjectBinding
        {
            Name = Name,
            RootFolder = RootProjectFolder.FullPath,
            Scenes = Scenes.Select(s => s.ToRefDto()).ToList()
        };
        return dto;
    }

    public void InitFromTemplate(ProjectTemplate selectedTemplate)
    {
        if (!Directory.Exists(RootProjectFolder.FullPath))
        {
            Directory.CreateDirectory(RootProjectFolder.FullPath);
        }
        else
        {
            throw new InvalidOperationException("Project has been already initialized.");
        }
        selectedTemplate.CreateFolders(RootProjectFolder.FullPath);
    }

    public static Project OpenFolder(ProjectFileNode rootFolder, ISerializer<ProjectBinding> serializer)
    {
        var projectFiles = rootFolder.LookupFiles(ProjectFileNodeType.ProjectFile);
        switch (projectFiles.Count)
        {
            case 0:
                throw new ArgumentException($"Folder {rootFolder} is not valid.");
            case > 1:
                throw new ArgumentException($"Folder {rootFolder} contains more than one project file.");
        }

        var projectBinding = serializer.FromFile(projectFiles[0].FullPath);
        if (projectBinding == null)
        {
            throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        }
        var project = new Project(projectBinding.Name, rootFolder,
            projectBinding.Scenes.Select(s => new KeyValuePair<string, string>(s.Name, s.Path)).ToList());
        project.MarkClean();
        return project;
    }

    public bool Validate()
    {
        if (!Directory.Exists(RootProjectFolder.FullPath))
        {
            return false;
        }

        if (!File.Exists(FilePath()))
        {
            return false;
        }
        return true;
    }

    public void Rename(string newName)
    {
        Name = newName.Trim();
        MarkDirty();
    }
}