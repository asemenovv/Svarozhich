using System;

namespace Svarozhich.Models.Nodes;

public class ExpScalarNode : Node
{
    public Port InputScalarPort { get; }
    public Port OutputScalarPort { get; }
    private readonly float _expBase;

    public ExpScalarNode(float expBase = 1.0f) : base($"Exp{{{expBase}}}")
    {
        InputScalarPort = AddInput("Input", PortDataType.Float);
        OutputScalarPort = AddOutput("Output", PortDataType.Float);
        _expBase = expBase;
    }

    public override object? EvaluateOutput(string outputPortName, EvaluationContext context)
    {
        if (outputPortName != OutputScalarPort.Name)
            throw new ArgumentException($"Unknown output port: {outputPortName}", nameof(outputPortName));
        
        if (context.TryGetCached(this, outputPortName, out var cached))
            return cached;
        
        var scalar = GetInputScalar(InputScalarPort, context);
        var result = Math.Pow(_expBase, scalar);

        context.SetCached(this, outputPortName, result);
        return result;
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