using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.Commands;

namespace Svarozhich.Services;

public class UndoRedoService
{
    private readonly Stack<IUndoableOperation> _undoStack = new();
    private readonly Stack<IUndoableOperation> _redoStack = new();

    [Reactive]
    public bool CanUndo { get; private set; }
    [Reactive]
    public bool CanRedo { get; private set; }

    private Guid _id = Guid.NewGuid();

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