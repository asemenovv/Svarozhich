using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia.Media.Imaging;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class ProjectsService
{
    private static readonly string ApplicationDataPath = Path.Combine(
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}", "Svarozhich");
    private static readonly string TemplatePath =
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";

    private readonly string _projectsDataPath;
    private ProjectDataList _projects = new();
    private readonly XmlSerializer _serializer;

    public ProjectsService(XmlSerializer serializer)
    {
        if (!Directory.Exists(ApplicationDataPath))
        {
            Directory.CreateDirectory(ApplicationDataPath);
        }
        _projectsDataPath = Path.Combine(ApplicationDataPath, "Projects.xml");
        _serializer  = serializer;
    }

    public Project Create(string name, string path)
    {
        var project = new Project(name, path);
        project.CreateScene("Default Scene");
        project.Save(_serializer);
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
}