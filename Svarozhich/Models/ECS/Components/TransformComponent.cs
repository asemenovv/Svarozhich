namespace Svarozhich.Models.ECS.Components;

public sealed record TransformComponent(float X, float Y, float Z) : ComponentBase(ComponentType.Transform);
