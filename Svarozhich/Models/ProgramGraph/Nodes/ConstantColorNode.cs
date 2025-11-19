using System;
using Svarozhich.Models.Nodes;

namespace Svarozhich.Models.ProgramGraph.Nodes;

public class ConstantColorNode : Node
{
    private readonly ColorRgb _value;
    public Port OutputColorPort { get; }

    public ConstantColorNode(string name, ColorRgb value) : base($"{name} := {value}")
    {
        _value = value;
        OutputColorPort = AddOutput("Out", PortDataType.Color);
    }

    public override object? EvaluateOutput(string outputPortName, EvaluationContext context)
    {
        if (outputPortName != OutputColorPort.Name)
            throw new ArgumentException($"Unknown output port: {outputPortName}", nameof(outputPortName));
        
        if (context.TryGetCached(this, outputPortName, out var cached))
            return cached;
        
        context.SetCached(this, outputPortName, _value);
        return _value;
    }
}