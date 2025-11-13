using System.IO;
using Svarozhich.Utils;

namespace Svarozhich.Models;

public class Scene : PersistedEntity
{
    private const string Extension = ".xml";
    private readonly Project _project;
    private readonly string _projectLocalFolder;
    public string Name { get; private set; }

    internal Scene(Project project, string name, string projectLocalFolder = "Scenes/")
    {
        _project = project ?? throw new System.ArgumentNullException(nameof(project));
        _projectLocalFolder = projectLocalFolder.Trim() ?? throw new System.ArgumentNullException(nameof(projectLocalFolder));
        Name = name.Trim() ?? throw new System.ArgumentNullException(nameof(name));
        MarkDirty();
    }

    public void Save(ISerializer serializer)
    {
        if (IsDirty)
        {
            serializer.ToFile(ToDto(), _project.AbsolutePath(SceneFileLocalPath()));
        }
        MarkClean();
    }

    public SceneRefDto ToRefDto()
    {
        return new SceneRefDto
        {
            Name = Name,
            Path = SceneFileLocalPath()
        };
    }

    private SceneDto ToDto()
    {
        return new SceneDto
        {
            Name = Name,
            Path = SceneFileLocalPath()
        };
    }

    private string SceneFileLocalPath()
    {
        return Path.Combine(_projectLocalFolder, $"{Name}{Extension}");
    }
}