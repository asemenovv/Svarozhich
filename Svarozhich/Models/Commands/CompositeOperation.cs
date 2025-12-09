using System.Collections.Generic;

namespace Svarozhich.Models.Commands;

public class CompositeOperation : IUndoableOperation
{
    private readonly IReadOnlyList<IUndoableOperation> _operations;
    public string Name { get; }

    public CompositeOperation(string name, IReadOnlyList<IUndoableOperation> operations)
    {
        Name = name;
        _operations = operations;
    }

    public void Do()
    {
        foreach (var operation in _operations)
            operation.Do();
    }

    public void Undo()
    {
        for (var i = _operations.Count - 1; i >= 0; i--)
            _operations[i].Undo();
    }
}