using System;
using System.Collections.Generic;
using System.IO;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class ProjectTemplatesService
{
    private static readonly string TemplatePath =
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";
    
    public IReadOnlyList<ProjectTemplate> LoadTemplates()
    {
        var templateFiles = Directory.GetFiles(TemplatePath, "template.xml", SearchOption.AllDirectories);
        List<ProjectTemplate> templates = [];
        foreach (var templateFile in templateFiles)
        {
            var template = new XmlSerializer<ProjectTemplate>().FromFile(templateFile);
            if (template == null) continue;
            template.PreviewImagePath = Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(), "preview.png");
            templates.Add(template);
        }
        return templates.AsReadOnly();
    }

    public void ApplyTemplate(ProjectTemplate template, ProjectFileNode targetFolder)
    {
        template.CreateFolders(targetFolder);
    }
}