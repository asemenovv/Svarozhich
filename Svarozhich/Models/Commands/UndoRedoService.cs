using System.Collections.Generic;

namespace Svarozhich.Models.Commands;

public class UndoRedoService
{
    private readonly Stack<IUndoableOperation> _undoStack = new();
    private readonly Stack<IUndoableOperation> _redoStack = new();
    
    public bool CanUndo { get; private set; }

    public bool CanRedo { get; private set; }

    public void Do(IUndoableOperation op)
    {
        op.Do();
        _undoStack.Push(op);
        _redoStack.Clear();
        UpdateFlags();
    }

    public void Undo()
    {
        if (_undoStack.Count == 0) return;
        
        var op = _undoStack.Pop();
        op.Undo();
        _redoStack.Push(op);
        UpdateFlags();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0) return;
        var op = _redoStack.Pop();
        op.Do();
        _undoStack.Push(op);
        UpdateFlags();
    }
    
    private void UpdateFlags()
    {
        CanUndo = _undoStack.Count > 0;
        CanRedo = _redoStack.Count > 0;
    }
}