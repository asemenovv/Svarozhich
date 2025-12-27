namespace Svarozhich.Models.Commands;

public sealed class RenameProjectOperation : IUndoableOperation
{
    private readonly Project.Project _project;
    private readonly string _oldName;
    private readonly string _newName;
    
    public string Name => "Rename Project";

    public RenameProjectOperation(Project.Project project, string newName)
    {
        _project = project;
        _oldName = project.Name;
        _newName = newName;
    }

    public void Do()
    {
        _project.Rename(_newName);
    }

    public void Undo()
    {
        _project.Rename(_oldName);
    }
}