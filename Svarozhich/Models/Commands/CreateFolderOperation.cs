namespace Svarozhich.Models.Commands;

public class CreateFolderOperation(ProjectFileNode parentNode, string name) : IUndoableOperation
{
    public string Name => "Create Folder";
    private ProjectFileNode? _newFolderNode;

    public void Do()
    {
        _newFolderNode = parentNode.CreateChildFolder(name);
    }

    public void Undo()
    {
        if (_newFolderNode == null)  return;
        _newFolderNode.Delete();
        _newFolderNode = null;
    }
}