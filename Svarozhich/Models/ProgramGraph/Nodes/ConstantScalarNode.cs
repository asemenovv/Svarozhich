using System;
using Svarozhich.Models.Nodes;

namespace Svarozhich.Models.ProgramGraph.Nodes;

public class ConstantScalarNode : Node
{
    private readonly float _value;
    public Port OutputScalarPort { get; }

    public ConstantScalarNode(string name, float value) : base($"{name} := {value}")
    {
        _value = value;
        OutputScalarPort = AddOutput("Out", PortDataType.Float);
    }

    public override object? EvaluateOutput(string outputPortName, EvaluationContext context)
    {
        if (outputPortName != OutputScalarPort.Name)
            throw new ArgumentException($"Unknown output port: {outputPortName}", nameof(outputPortName));
        
        if (context.TryGetCached(this, outputPortName, out var cached))
            return cached;
        
        context.SetCached(this, outputPortName, _value);
        return _value;
    }
}