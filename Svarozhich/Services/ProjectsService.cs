using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class ProjectsService
{
    private static readonly string TemplatePath =
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";
    
    private readonly OpenedProjectData _openedProjects;
    private readonly XmlSerializer _serializer;

    public ProjectsService(XmlSerializer serializer)
    {
        _serializer  = serializer;
        _openedProjects = OpenedProjectData.Load(serializer);
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => new Project(p.Name, p.Path).Validate())
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages());
    }

    public Project Create(string name, string path, ProjectTemplate? template = null)
    {
        var project = new Project(name, path);
        project.CreateScene("Default Scene");
        if (template != null)
        {
            project.InitFromTemplate(template);
        }
        else
        {
            throw new ArgumentException("Project template is not defined.");
        }
        project.Save(_serializer);
        return project;
    }

    public Project Open(string path)
    {
        var project = Project.OpenFolder(path, _serializer);
        _openedProjects.MarkOpened(project);
        _openedProjects.Save(_serializer);
        return project;
    }

    public List<ProjectTemplate> LoadProjectTemplates()
    {
        var templateFiles = Directory.GetFiles(TemplatePath, "template.xml", SearchOption.AllDirectories);
        Debug.Assert(templateFiles.Length != 0);
        List<ProjectTemplate> templates = [];
        foreach (var templateFile in templateFiles)
        {
            var template = _serializer.FromFile<ProjectTemplate>(templateFile);
            if (template == null) continue;
            template.PreviewImagePath = Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(), "preview.png");
            templates.Add(template);
        }
        return templates;
    }

    public List<ProjectData> PreviouslyOpenedProjects()
    {
        return _openedProjects.Projects.OrderByDescending(x => x.LastOpenDate).ToList();
    }
}