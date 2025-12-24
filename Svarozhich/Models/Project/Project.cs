using System;
using ReactiveUI.Fody.Helpers;

namespace Svarozhich.Models;

public class Project : PersistedEntity<ProjectBinding>
{
    [Reactive]
    public string Name { get; private set; }
    [Reactive]
    public ProjectFileNode RootProjectFolder { get; private set; }
    
    public Project(string name, ProjectFileNode rootFolder)
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