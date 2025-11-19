using System.Collections.Generic;

namespace Svarozhich.Models.Nodes;

public class EvaluationContext
{
    private readonly Dictionary<(Node node, string portName), object?> _cache = new();

    public bool TryGetCached(Node node, string portName, out object? value)
        => _cache.TryGetValue((node, portName), out value);

    public void SetCached(Node node, string portName, object? value)
        => _cache[(node, portName)] = value;
}