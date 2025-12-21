namespace Svarozhich.Models.ECS.Components;

public enum ComponentType
{
    Transform,
}

public interface IComponent
{
    ComponentType Type { get; }
}

public abstract record ComponentBase(ComponentType Type) : IComponent;