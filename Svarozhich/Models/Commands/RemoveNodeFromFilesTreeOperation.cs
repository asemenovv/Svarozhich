using System;

namespace Svarozhich.Models.Commands;

public class RemoveNodeFromFilesTreeOperation : IUndoableOperation
{
    private readonly ProjectFileNode _node;
    private readonly ProjectFileNode _parent;
    private readonly int _index;

    public string Name => "Remove Filesystem Node";

    public RemoveNodeFromFilesTreeOperation(ProjectFileNode node)
    {
        _node = node;
        _parent = _node.Parent ?? throw new InvalidOperationException("Root node cannot be removed.");
        _index = _parent.Children.IndexOf(_node);
        if (_index < 0)
            throw new InvalidOperationException("Node is not in parent's collection.");
    }

    public void Do()
    {
        _parent.Children.Remove(_node);
    }

    public void Undo()
    {
        _parent.Children.Insert(_index, _node);
    }
}