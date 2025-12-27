using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class RecentProjectsService
{
    private readonly ISerializer<OpenedProjectData> _serializer;
    private readonly InstallationFolderLayout _installationFolderLayout;

    private readonly OpenedProjectData _openedProjects;
    
    public RecentProjectsService(ISerializer<OpenedProjectData> serializer, ProjectLayout layout,
        InstallationFolderLayout installationFolderLayout)
    {
        _serializer = serializer;
        _installationFolderLayout = installationFolderLayout;

        _openedProjects = _serializer.FromFile(_installationFolderLayout.RecentsProjectsFile()) ?? new OpenedProjectData();
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => ValidateProjectPath(new ProjectFileNode(p.Path)))
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages(layout.PreviewImage(p.Path)));
    }

    public void MarkOpened(Project project)
    {
        _openedProjects.MarkOpened(project);
        Save();
    }
    
    public IReadOnlyList<ProjectData> GetRecentProjects()
    {
        return _openedProjects.Projects
            .OrderByDescending(x => x.LastOpenDate)
            .ToList()
            .AsReadOnly();
    }

    private bool ValidateProjectPath(ProjectFileNode projectPath)
    {
        if (!Directory.Exists(projectPath.FullPath))
        {
            return false;
        }

        return projectPath.LookupFiles(ProjectFileNodeType.ProjectFile)
            .Count != 0;
    }

    private void Save()
    {
        if (_openedProjects.IsDirty)
        {
            _serializer.ToFile(_openedProjects, _installationFolderLayout.RecentsProjectsFile());
        }
        _openedProjects.MarkClean();
    }
}