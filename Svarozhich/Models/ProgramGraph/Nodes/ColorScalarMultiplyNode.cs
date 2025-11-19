using System;

namespace Svarozhich.Models.Nodes;

public class ColorScalarMultiplyNode : Node
{
    public Port InputColorPort { get; }
    public Port InputScalar { get; }
    public Port OutputColorPort { get; }

    public ColorScalarMultiplyNode() : base("Multiply")
    {
        InputColorPort = AddInput("Color", PortDataType.Color);
        InputScalar = AddInput("Scalar", PortDataType.Float);
        OutputColorPort = AddOutput("Color", PortDataType.Color);
    }

    public override object? EvaluateOutput(string outputPortName, EvaluationContext context)
    {
        if (outputPortName != OutputColorPort.Name)
            throw new ArgumentException($"Unknown output port: {outputPortName}", nameof(outputPortName));
        
        if (context.TryGetCached(this, outputPortName, out var cached))
            return cached;
        
        var inColor = GetInputColor(InputColorPort, context);
        var scalar = GetInputScalar(InputScalar, context);
        var result = inColor * scalar;

        context.SetCached(this, outputPortName, result);
        return result;
    }

    private ColorRgb GetInputColor(Port port, EvaluationContext context)
    {
        var connection = _nodeGraph.FindConnectionTo(port);
        if (connection is null)
            return new ColorRgb(0, 0, 0);

        var sourceNode = connection.From.Owner;
        var value = sourceNode.EvaluateOutput(connection.From.Name, context);

        if (value is ColorRgb c)
            return c;

        throw new InvalidOperationException("Expected ColorRgb on input");
    }

    private float GetInputScalar(Port port, EvaluationContext context)
    {
        var connection = _nodeGraph.FindConnectionTo(port);
        if (connection is null)
            return 1.0f;

        var sourceNode = connection.From.Owner;
        var value = sourceNode.EvaluateOutput(connection.From.Name, context);

        if (value is float f)
            return f;

        throw new InvalidOperationException("Expected float on exposure input");
    }
}