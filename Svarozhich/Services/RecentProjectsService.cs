using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class RecentProjectsService
{
    private readonly ISerializer<OpenedProjectData> _serializer;
    private readonly ProjectLayout _projectLayout;
    private readonly InstallationFolderLayout _installationFolderLayout;

    private OpenedProjectData _openedProjects;
    
    public RecentProjectsService(ISerializer<OpenedProjectData> serializer, ProjectLayout projectLayout,
        InstallationFolderLayout installationFolderLayout)
    {
        _serializer = serializer;
        _projectLayout = projectLayout;
        _installationFolderLayout = installationFolderLayout;
        Load();
    }

    private void Load()
    {
        _openedProjects = _serializer.FromFile(_installationFolderLayout.RecentsProjectsFile()) ?? new OpenedProjectData();
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => ValidateProjectPath(p.Path))
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages(_projectLayout.PreviewImage(p.Path)));
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

    private bool ValidateProjectPath(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            return false;
        }
        return new ProjectFileNode(projectPath)
            .LookupFiles(ProjectFileNodeType.ProjectFile)
            .Count == 1;
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