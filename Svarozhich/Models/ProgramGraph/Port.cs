using System;

namespace Svarozhich.Models.Nodes;

public enum PortDirection
{
    Input,
    Output
}

public enum PortDataType
{
    Color,
    Float
}

public sealed class Port
{
    public Guid Id { get; }
    public string Name { get; }
    public PortDirection Direction { get; }
    public PortDataType DataType { get; }

    public Node Owner { get; }

    internal Port(Node owner, string name, PortDirection direction, PortDataType dataType, Guid? id = null)
    {
        Owner = owner;
        Name = name;
        Direction = direction;
        DataType = dataType;
        Id = id ?? Guid.NewGuid();
    }
}