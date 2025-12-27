using System;
using Svarozhich.Models;

namespace Svarozhich.Services;

public class TrashFolderService(ProjectLayout layout, WorkspaceService workspaceService)
{
    public string TrashFolder()
    {
        return layout.TrashFolder(
            workspaceService.CurrentProject ?? throw new ArgumentException("Project not opened")
        );
    }

    public string TrashFolder(Project project)
    {
        return layout.TrashFolder(project);
    }
}