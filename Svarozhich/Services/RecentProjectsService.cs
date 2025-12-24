using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class RecentProjectsService
{
    private static readonly string ProjectsDataPath;
    
    private readonly OpenedProjectData _openedProjects;

    static RecentProjectsService()
    {
        var applicationDataPath = Path.Combine(
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}", "Svarozhich");
        if (!Directory.Exists(applicationDataPath))
        {
            Directory.CreateDirectory(applicationDataPath);
        }
        ProjectsDataPath = Path.Combine(applicationDataPath, "Projects.xml");
    }
    
    public RecentProjectsService()
    {
        _openedProjects = new XmlSerializer<OpenedProjectData>()
            .FromFile(ProjectsDataPath) ?? new OpenedProjectData();
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => ValidateProjectPath(new ProjectFileNode(p.Path)))
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages());
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
            new XmlSerializer<OpenedProjectData>()
                .ToFile(_openedProjects, ProjectsDataPath);
        }
        _openedProjects.MarkClean();
    }
}