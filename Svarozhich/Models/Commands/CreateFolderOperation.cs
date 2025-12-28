using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Models.Commands;

public class CreateFolderOperation(ProjectFileNode parentNode, string name, FilesystemRepository filesystem) : IUndoableOperation
{
    public string Name => "Create Folder";
    private ProjectFileNode? _newFolderNode;

    public void Do()
    {
        filesystem.CreateFolder(false, parentNode.FullPath, name);
        _newFolderNode = parentNode.CreateChildFolder(name);
    }

    public void Undo()
    {
        if (_newFolderNode == null)  return;
        filesystem.DeleteFolder(_newFolderNode.FullPath);
        _newFolderNode.Delete();
        _newFolderNode = null;
    }
}