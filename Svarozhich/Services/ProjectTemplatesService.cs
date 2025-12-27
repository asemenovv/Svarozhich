using System.Collections.Generic;
using System.IO;
using Svarozhich.Models;
using Svarozhich.Repository;
using Svarozhich.Utils;

namespace Svarozhich.Services;

public class ProjectTemplatesService(ISerializer<ProjectTemplate> serializer, ProjectLayout projectLayout,
    ProjectTemplateLayout projectTemplateLayout, InstallationFolderLayout installationFolderLayout,
    FilesystemRepository filesystemRepository)
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

    public void ApplyTemplate(ProjectTemplate template, string targetFolder)
    {
        foreach (var folder in template.Folders)
        {
            var isAppFolder = folder == projectLayout.AppFolder(targetFolder);
            filesystemRepository.CreateFolder(isAppFolder, targetFolder, folder);
        }

        if (template.PreviewImagePath != null)
            filesystemRepository.Copy(template.PreviewImagePath, projectLayout.PreviewImage(targetFolder));
    }
}