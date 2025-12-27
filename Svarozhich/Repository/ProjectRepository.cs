using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Models.Project;
using Svarozhich.Services;
using Svarozhich.Utils;

namespace Svarozhich.Repository;

public class ProjectRepository
{
    private readonly ISerializer<ProjectBinding> _serializer;
    private readonly ProjectLayout _layout;

    public ProjectRepository(ISerializer<ProjectBinding> serializer, ProjectLayout layout)
    {
        _serializer = serializer;
        _layout = layout;
    }

    public Project LoadFromFolder(string rootFolder)
    {
        var projectFiles = LookupFiles(rootFolder, ProjectFileNodeType.ProjectFile);
        switch (projectFiles.Count)
        {
            case 0:
                throw new ArgumentException($"Folder {rootFolder} is not valid.");
            case > 1:
                throw new ArgumentException($"Folder {rootFolder} contains more than one project file.");
        }

        var projectBinding = _serializer.FromFile(projectFiles[0]);
        if (projectBinding == null)
        {
            throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        }
        var project = new Project(projectBinding.Name);
        project.MarkClean();
        return project;
    }

    public void Save(string projectFolder, Project project)
    {
        var projectBinding = new ProjectBinding { Name = project.Name };
        var projectFile = _layout.ProjectFilePath(projectFolder, projectBinding.Name);
        _serializer.ToFile(projectBinding, projectFile);
    }

    public bool IsProjectPath(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            return false;
        }
        return LookupFiles(projectPath, ProjectFileNodeType.ProjectFile)
            .Count == 1;
    }
    
    public List<string> LookupFiles(string path, ProjectFileNodeType type)
    {
        var files = new List<string>();
        foreach (var extension in type.GetExtensions())
        {
            files.AddRange(Directory.GetFiles(path, $"*{extension}", SearchOption.TopDirectoryOnly));
        }
        return files;
    }
}