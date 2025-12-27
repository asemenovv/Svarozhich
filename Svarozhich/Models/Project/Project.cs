using System;
using ReactiveUI.Fody.Helpers;

namespace Svarozhich.Models.Project;

public class Project : PersistedEntity<ProjectBinding>
{
    [Reactive]
    public string Name { get; private set; }
    
    public Project(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
        
        Name = name.Trim();
        MarkDirty();
    }

    public void Rename(string newName)
    {
        Name = newName.Trim();
        MarkDirty();
    }

    public Scene CreateScene(string name, string projectFolder = "Scenes/")
    {
        return new Scene(this, name, projectFolder);
    }
}