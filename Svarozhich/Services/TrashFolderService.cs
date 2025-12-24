using Svarozhich.Models;

namespace Svarozhich.Services;

public class TrashFolderService
{
    public string TrashFolder(Project project)
    {
        return $"{project.RootProjectFolder.FullPath}/.Svarozhich/Trash";
    }
}