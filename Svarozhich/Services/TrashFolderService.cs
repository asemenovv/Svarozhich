using System;
using Svarozhich.Models;
using Svarozhich.Models.Project;
using Svarozhich.Repository;

namespace Svarozhich.Services;

public class TrashFolderService(ProjectLayout layout, WorkspaceService workspaceService)
{
    public string TrashFolder()
    {
        return layout.TrashFolder(
            workspaceService.ProjectTreeRoot?.FullPath ?? throw new ArgumentException("Project not opened")
        );
    }
}