using System.Collections.Generic;

namespace Svarozhich.Models.Nodes;

public abstract class Node(string title)
{
    private readonly List<Port> _inputs = new();
    private readonly List<Port> _outputs = new();

    public IReadOnlyList<Port> Inputs => _inputs;
    public IReadOnlyList<Port> Outputs => _outputs;

    public string Title { get; } = title;
    protected NodeGraph _nodeGraph;

    protected Port AddInput(string name, PortDataType type)
    {
        var port = new Port(this, name, PortDirection.Input, type);
        _inputs.Add(port);
        return port;
    }

    protected Port AddOutput(string name, PortDataType type)
    {
        var port = new Port(this, name, PortDirection.Output, type);
        _outputs.Add(port);
        return port;
    }
    
    public abstract object? EvaluateOutput(string outputPortName, EvaluationContext context);

    public void SetNodeGraph(NodeGraph nodeGraph) => _nodeGraph = nodeGraph;
}