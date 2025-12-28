using Svarozhich.Models.Project;
using Svarozhich.Repository;
using Svarozhich.Services;

namespace Svarozhich.Models.Commands;

public class CommandsFactory(FilesystemRepository filesystem, UndoRedoService undoRedoService,
    TrashFolderService trashFolderService, WorkspaceService workspaceService)
{
    public IUndoableOperation CreateFolder(ProjectFileNode parentFolder, string name)
    {
        var operation = new CreateFolderOperation(parentFolder, name, filesystem);
        undoRedoService.Do(operation);
        return operation;
    }

    public IUndoableOperation DeleteFilesystemNode(ProjectFileNode node)
    {
        var operation = new CompositeOperation($"Delete {(node.IsFolder ? "Folder" : "File")}", [
            new RemoveNodeFromFilesTreeOperation(node),
            new DeleteNodeFromDiskOperation(node, trashFolderService.TrashFolder(), filesystem)
        ]);
        undoRedoService.Do(operation);
        return operation;
    }

    public IUndoableOperation RenameCurrentProject(string newName)
    {
        if (workspaceService.CurrentProject is null) return new NopOperation();

        var operation = new RenameProjectOperation(workspaceService.CurrentProject, newName);
        undoRedoService.Do(operation);
        return operation;
    }
}