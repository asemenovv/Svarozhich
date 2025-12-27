using Svarozhich.Models;

namespace Svarozhich.Services;

public class TrashFolderService
{
    private readonly ProjectLayout _layout;

    public TrashFolderService(ProjectLayout layout)
    {
        _layout = layout;
    }

    public string TrashFolder(Project project)
    {
        return _layout.TrashFolder(project);
    }
}