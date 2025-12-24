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
            .Where(p => ValidateProject(new Project(p.Name, new ProjectFileNode(p.Path))))
            .ToList();
        _openedProjects.Projects.ForEach(p => p.LoadImages());
    }

    public Project Create(string name, string path, ProjectTemplate? template = null)
    {
        var rootFolder = new ProjectFileNode(path);
        rootFolder.CreateIfNotExist();
        var project = new Project(name, rootFolder);
        project.CreateScene("Default Scene");
        if (template != null)
        {
            template.CreateFolders(project.RootProjectFolder.FullPath);
        }
        else
        {
            throw new ArgumentException("Project template is not defined.");
        }
        SaveProject(project);
        return project;
    }

    public Project LoadFromFolder(string path)
    {
        var rootFolder = new ProjectFileNode(path);
        var projectFiles = rootFolder.LookupFiles(ProjectFileNodeType.ProjectFile);
        switch (projectFiles.Count)
        {
            case 0:
                throw new ArgumentException($"Folder {rootFolder} is not valid.");
            case > 1:
                throw new ArgumentException($"Folder {rootFolder} contains more than one project file.");
        }

        var projectBinding = new XmlSerializer<ProjectBinding>()
            .FromFile(projectFiles[0].FullPath);
        if (projectBinding == null)
        {
            throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        }
        var project = new Project(projectBinding.Name, rootFolder);
        project.MarkClean();
        _openedProjects.MarkOpened(project);
        _openedProjects.Save(new XmlSerializer<OpenedProjectData>());
        CurrentProject  = project;
        return project;
    }

    public bool ValidateProject(Project project)
    {
        if (!Directory.Exists(project.RootProjectFolder.FullPath))
        {
            return false;
        }

        return project.RootProjectFolder
            .LookupFiles(ProjectFileNodeType.ProjectFile)
            .Count != 0;
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
    
    public void SaveProject(Project project)
    {
        var serializer = new XmlSerializer<ProjectBinding>();
        var projectBinding = new ProjectBinding
        {
            Name = project.Name
        };
        var projectFilename = $"{project.Name}{ProjectFileNodeType.ProjectFile.GetExtension()}";
        var projectFile = project.RootProjectFolder.FilePath(projectFilename);
        serializer.ToFile(projectBinding, projectFile);
    }
}