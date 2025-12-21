using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using Svarozhich.Models.ECS.Components;

namespace Svarozhich.Models.ECS;

public readonly record struct EntityId(Guid Value)
{
    public static EntityId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}

public enum EntityKind
{
    Folder,
    Entity
}

public sealed class Entity : ReactiveObject
{
    public EntityId Id { get; }
    public EntityKind Kind { get; }
    public string Name { get; private set; }
    public Entity? Parent { get; private set; }
    public ObservableCollection<Entity> Children { get; } = [];
    
    private readonly Dictionary<ComponentType, IComponent> _components = new();
    public IReadOnlyDictionary<ComponentType, IComponent> Components => _components;
    
    public bool IsFolder => Kind == EntityKind.Folder;
    
    public Entity(EntityKind kind, string? name = null, EntityId? id = null)
    {
        Kind = kind;
        Id = id ?? EntityId.New();
        Name = Normalize(name, kind == EntityKind.Folder ? "Folder" : "Entity");
    }
    
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Entity name cannot be empty.", nameof(newName));

        Name = newName.Trim();
    }
    
    internal void SetParent(Entity? parent) => Parent = parent;

    internal void AddChild(Entity child) => Children.Add(child);
    
    internal bool RemoveChild(Entity child)
    {
        return Children.Remove(child);
    }

    public void AddComponent(IComponent component)
    {
        if (IsFolder)
            throw new InvalidOperationException("Folder entities cannot have components.");

        _components[component.Type] = component;
    }

    public bool RemoveComponent(ComponentType type)
    {
        if (IsFolder) return false;
        return _components.Remove(type);
    }

    public bool TryGetComponent<T>(ComponentType type, out T? component) where T : class, IComponent
    {
        if (_components.TryGetValue(type, out var c) && c is T typed)
        {
            component = typed;
            return true;
        }
        component = null;
        return false;
    }
    
    private static string Normalize(string? name, string fallback)
        => string.IsNullOrWhiteSpace(name) ? fallback : name.Trim();
}