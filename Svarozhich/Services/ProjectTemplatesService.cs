using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using Svarozhich.Models;
using Svarozhich.Repository;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class ProjectTemplatesService(ISerializer<ProjectTemplate> serializer,
    ProjectLayout projectLayout, ProjectTemplateLayout projectTemplateLayout, InstallationFolderLayout installationFolderLayout)
{
    public IReadOnlyList<ProjectTemplate> LoadTemplates()
    {
        var templateFiles = installationFolderLayout.LookupTemplateFiles();
        List<ProjectTemplate> templates = [];
        foreach (var templateFile in templateFiles)
        {
            var template = serializer.FromFile(templateFile);
            if (template == null) continue;
            template.PreviewImagePath = projectTemplateLayout.PreviewImage(templateFile);
            templates.Add(template);
        }
        return templates.AsReadOnly();
    }

    public void ApplyTemplate(ProjectTemplate template, ProjectFileNode targetFolder)
    {
        foreach (var folder in template.Folders)
        {
            Directory.CreateDirectory(projectTemplateLayout.ChildFolder(targetFolder, folder));
        }

        var editorSystemPath = new DirectoryInfo(projectLayout.AppFolder(targetFolder.FullPath));
        editorSystemPath.Attributes |= FileAttributes.Hidden;
        if (template.PreviewImagePath != null)
            File.Copy(template.PreviewImagePath, projectLayout.PreviewImage(targetFolder.FullPath));
    }
}