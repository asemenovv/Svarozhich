using System;

namespace Svarozhich.Models.ECS;

public readonly record struct SceneId(Guid Value)
{
    public static SceneId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}

public sealed record SceneRef(SceneId Id, string Name, string RelativePath);

public class Scene : PersistedEntity<SceneDto>
{
    public SceneId Id { get; }
    public string Name { get; private set; }
    public Entity Root { get; }

    public Scene(SceneId id, string name)
    {
        Id = id;
        Name = name;
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