using System;
using System.Collections.Generic;
using System.Linq;
using Svarozhich.Utils;

namespace Svarozhich.Models.Nodes;

public class NodeGraph : PersistedEntity<NodeGraphBinding>
{
    private readonly Project _project;
    private readonly List<Node> _nodes = new();
    private readonly List<Connection> _connections = new();

    public NodeGraph(Project project)
    {
        _project = project;
    }

    public IReadOnlyList<Node> Nodes => _nodes;
    public IReadOnlyList<Connection> Connections => _connections;
    
    public void AddNode(Node node)
    {
        node.SetNodeGraph(this);
        _nodes.Add(node);
    }

    public void AddConnection(Connection connection) => _connections.Add(connection);
    
    public ColorRgb EvaluateColor(Node outputNode, string outputPortName)
    {
        var context = new EvaluationContext();
        var value = outputNode.EvaluateOutput(outputPortName, context);

        if (value is ColorRgb color)
            return color;

        throw new InvalidOperationException("Expected ColorRgb as output.");
    }
    
    public Connection? FindConnectionTo(Port inputPort)
    {
        return _connections.FirstOrDefault(c => c.To == inputPort);
    }

    protected override NodeGraphBinding ToDto()
    {
        throw new NotImplementedException();
    }

    protected override string FilePath()
    {
        throw new NotImplementedException();
    }
}
