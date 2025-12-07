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
    public Project CurrentProject { get; private set; }

    public ProjectsService()
    {
        _openedProjects = OpenedProjectData.Load(new XmlSerializer<OpenedProjectData>());
        _openedProjects.Projects = _openedProjects.Projects
            .Where(p => new Project(p.Name, new ProjectFileNode(p.Path)).Validate())
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages());
    }

    public Project Create(string name, string path, ProjectTemplate? template = null)
    {
        var project = new Project(name, new  ProjectFileNode(path));
        project.CreateScene("Default Scene");
        if (template != null)
        {
            project.InitFromTemplate(template);
        }
        else
        {
            throw new ArgumentException("Project template is not defined.");
        }
        project.Save(new XmlSerializer<ProjectBinding>());
        foreach (var scene in project.Scenes)
        {
            scene.Save(new XmlSerializer<SceneDto>());
        }
        return project;
    }

    public Project Open(string path)
    {
        var rootFolder = new ProjectFileNode(path);
        var project = Project.OpenFolder(rootFolder, new XmlSerializer<ProjectBinding>());
        _openedProjects.MarkOpened(project);
        _openedProjects.Save(new XmlSerializer<OpenedProjectData>());
        CurrentProject  = project;
        return project;
    }

    public List<ProjectTemplate> LoadProjectTemplates()
    {
        var templateFiles = Directory.GetFiles(TemplatePath, "template.xml", SearchOption.AllDirectories);
        Debug.Assert(templateFiles.Length != 0);
        List<ProjectTemplate> templates = [];
        foreach (var templateFile in templateFiles)
        {
            var template = new XmlSerializer<ProjectTemplate>().FromFile(templateFile);
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