using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Models.Project;
using Svarozhich.Repository;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class RecentProjectsService
{
    private readonly ISerializer<OpenedProjectData> _serializer;
    private readonly ProjectLayout _projectLayout;
    private readonly InstallationFolderLayout _installationFolderLayout;
    private readonly ProjectRepository _projectRepository;

    private OpenedProjectData _openedProjects;
    
    public RecentProjectsService(ISerializer<OpenedProjectData> serializer, ProjectLayout projectLayout,
        InstallationFolderLayout installationFolderLayout, ProjectRepository projectRepository)
    {
        _serializer = serializer;
        _projectLayout = projectLayout;
        _installationFolderLayout = installationFolderLayout;
        _projectRepository = projectRepository;
        Load();
    }

    private void Load()
    {
        _openedProjects = _serializer.FromFile(_installationFolderLayout.RecentsProjectsFile()) ?? new OpenedProjectData();
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => _projectRepository.IsProjectPath(p.Path))
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages(_projectLayout.PreviewImage(p.Path)));
    }

    public void MarkOpened(Project project, ProjectFileNode projectPath)
    {
        _openedProjects.MarkOpened(project, projectPath.FullPath);
        Save();
    }
    
    public IReadOnlyList<ProjectData> GetRecentProjects()
    {
        return _openedProjects.Projects
            .OrderByDescending(x => x.LastOpenDate)
            .ToList()
            .AsReadOnly();
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