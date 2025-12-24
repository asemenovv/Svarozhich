using System;
using System.Collections.Generic;
using System.IO;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.ECS;
using Svarozhich.Utils;

namespace Svarozhich.Models;

public class Scene : PersistedEntity<SceneDto>
{
    public Entity Root { get; }
    
    private const string Extension = ".xml";
    private readonly Project? _project;
    private readonly string _projectLocalFolder;

    internal Scene(Project? project, string name, string projectLocalFolder = "Scenes/")
    {
        _project = project;// ?? throw new System.ArgumentNullException(nameof(project));
        _projectLocalFolder = projectLocalFolder.Trim() ?? throw new System.ArgumentNullException(nameof(projectLocalFolder));
        Root = new Entity(EntityKind.Folder, name);
        MarkDirty();
    }
    
    public Entity CreateFolder(string name, Entity? parent = null)
        => Create(EntityKind.Folder, name, parent);
    
    public Entity CreateEntity(string name, Entity? parent = null)
        => Create(EntityKind.Entity, name, parent);
    
    private Entity Create(EntityKind kind, string name, Entity? parent)
    {
        var e = new Entity(kind, name);
        Attach(parent ?? Root, e);
        return e;
    }
    
    public void Attach(Entity parent, Entity child)
    {
        if (!parent.IsFolder)
            throw new InvalidOperationException("Only Folder entities can have children.");

        // detach from old parent
        if (child.Parent != null)
        {
            var oldParent = child.Parent;
            oldParent.RemoveChild(child);
        }

        child.SetParent(parent);
        parent.AddChild(child);
    }
    
    public void Detach(Entity child)
    {
        if (child.Parent == null) return;
        
        child.Parent.RemoveChild(child);
        child.SetParent(null);
    }
    
    public void DeleteSubtree(Entity entity)
    {
        if (entity.Id == Root.Id)
            throw new InvalidOperationException("Cannot delete root.");
        
        foreach (var c in entity.Children)
            DeleteSubtree(c);
        
        Detach(entity);
    }
}