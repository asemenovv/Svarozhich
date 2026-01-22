using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Svarozhich.Models;
using Svarozhich.Models.DTO;
using Svarozhich.Models.ECS;
using Svarozhich.Models.Project;
using Svarozhich.Services;
using Svarozhich.Utils;
using SceneDto = Svarozhich.Models.DTO.SceneDto;
using SceneRefDto = Svarozhich.Models.DTO.SceneRefDto;

namespace Svarozhich.Repository;

public class ProjectRepository
{
    private readonly ISerializer<ProjectDto> _serializer;
    private readonly ProjectLayout _layout;

    public ProjectRepository(ISerializer<ProjectDto> serializer, ProjectLayout layout)
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

        var projectBinding = _serializer.FromFile(projectFiles[0])
            ?? throw new InvalidOperationException($"Project {projectFiles[0]} can not be loaded.");
        var project = new Project(projectBinding.Name, rootFolder);
        foreach (var s in projectBinding.Scenes)
        {
            project.AddScene(new SceneRef(
                new SceneId(Guid.Parse(s.Id)),
                s.Name,
                s.Path
            ));
        }
        project.MarkClean();
        return project;
    }

    public void Save(string projectFolder, Project project)
    {
        var projectFile = _layout.ProjectFilePath(projectFolder, project.Name);
        var projectBinding = new ProjectDto
        {
            Name = project.Name,
            Scenes = project.Scenes
                .Select(s => new SceneRefDto()
                {
                    Id = s.Id.Value.ToString("D"),
                    Name = s.Name,
                    Path = s.RelativePath
                }).ToList()
        };
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