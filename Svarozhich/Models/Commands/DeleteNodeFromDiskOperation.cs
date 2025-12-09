using System;
using System.IO;

namespace Svarozhich.Models.Commands;

public class DeleteNodeFromDiskOperation : IUndoableOperation
{
    private readonly ProjectFileNode _node;
    private readonly string _trashPath;
    private readonly string _originalPath;
    
    public string Name  => "Remove File / Folder from Disk";

    public DeleteNodeFromDiskOperation(ProjectFileNode node, string trashFolder)
    {
        _node = node;
        _originalPath = node.FullPath;
        _trashPath = Path.Combine(trashFolder, $"{_node.Name}_{Guid.NewGuid()}");
    }

    public void Do()
    {
        _node.MoveTo(_trashPath);
    }

    public void Undo()
    {
        _node.MoveTo(_originalPath);
    }
}