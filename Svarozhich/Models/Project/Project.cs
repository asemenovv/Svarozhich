using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.DTO;
using Svarozhich.Models.ECS;

namespace Svarozhich.Models.Project;

public class Project : PersistedEntity<ProjectDto>
{
    [Reactive]
    public string Name { get; private set; }
    public string RootPath { get; }

    public ObservableCollection<SceneRef> Scenes { get; } = [];
    
    public Project(string name, string rootPath)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
        
        Name = name.Trim();
        RootPath = rootPath;
        MarkDirty();
    }

    public void Rename(string newName)
    {
        Name = newName.Trim();
        MarkDirty();
    }

    public void AddScene(SceneRef scene)
    {
        if (Scenes.Any(s => s.Id == scene.Id))
            throw new ArgumentException("Scene cannot be added twice.", nameof(scene));
        Scenes.Add(scene);
    }

    public bool RemoveScene(SceneId id)
    {
        var existing = Scenes.FirstOrDefault(s => s.Id == id);
        return existing != null && Scenes.Remove(existing);
    }
}