using System;
using Svarozhich.Models.Nodes;
using Svarozhich.Models.ProgramGraph.Nodes;

namespace Svarozhich.Services;

public class ProgramGraphsService
{
    public ProgramGraphsService()
    {
        var nodeGraph = GetNodeGraph();
        // var value = nodeGraph.EvaluateColor(colorScalarMultiplyNode, colorScalarMultiplyNode.OutputColorPort.Name);
        // Console.WriteLine(value);
    }

    public NodeGraph GetNodeGraph()
    {
        var nodeGraph = new NodeGraph();
        
        var inColorNode = new ConstantColorNode("Red-ish Color", new ColorRgb(1.0f, 0.1f, 0.1f));
        var scalarNode = new ConstantScalarNode("Exposure", 2.0f);
        var colorScalarMultiplyNode = new ColorScalarMultiplyNode();
        
        nodeGraph.AddNode(inColorNode);
        nodeGraph.AddNode(scalarNode);
        nodeGraph.AddNode(colorScalarMultiplyNode);
        
        nodeGraph.AddConnection(new Connection(inColorNode.OutputColorPort, colorScalarMultiplyNode.InputColorPort));
        nodeGraph.AddConnection(new Connection(scalarNode.OutputScalarPort, colorScalarMultiplyNode.InputScalar));
        return nodeGraph;
    }
}