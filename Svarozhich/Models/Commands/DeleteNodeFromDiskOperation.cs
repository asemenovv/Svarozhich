using System;
using System.IO;
using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Models.Commands;

public class DeleteNodeFromDiskOperation : IUndoableOperation
{
    private readonly ProjectFileNode _node;
    private readonly FilesystemRepository _filesystem;
    private readonly string _trashPath;
    private readonly string _originalPath;
    
    public string Name  => "Remove File / Folder from Disk";

    public DeleteNodeFromDiskOperation(ProjectFileNode node, string trashFolder, FilesystemRepository filesystem)
    {
        _node = node;
        _filesystem = filesystem;
        _originalPath = node.FullPath;
        _trashPath = Path.Combine(trashFolder, $"{_node.Name}_{Guid.NewGuid()}");
    }

    public void Do()
    {
        _filesystem.Move(_node.FullPath, _trashPath);
        _node.MoveTo(_trashPath);
    }

    public void Undo()
    {
        _filesystem.Move(_node.FullPath, _originalPath);
        _node.MoveTo(_originalPath);
    }
}