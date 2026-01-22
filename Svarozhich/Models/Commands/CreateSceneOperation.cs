using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Models.Commands;

public class CreateSceneOperation(ProjectFileNode parentNode, string name, Project.Project project, FilesystemRepository filesystem) : IUndoableOperation
{
    public string Name => "Create Scene";
    
    public void Do()
    {
        project.AddScene(name, parentNode.);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}