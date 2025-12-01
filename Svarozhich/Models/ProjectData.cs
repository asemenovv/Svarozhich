using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;
using Svarozhich.Utils;

namespace Svarozhich.Models;

[DataContract]
public class ProjectData
{
    [DataMember(Name = "Name")]
    public required string Name { get; set; }
    [DataMember(Name = "Path")]
    public required string Path { get; set; }
    [DataMember(Name = "LastOpened")]
    public DateTime LastOpenDate { get; set; }
    [IgnoreDataMember]
    public Bitmap? PreviewImage { get; set; }

    public void LoadImages()
    {
        PreviewImage = new Bitmap(System.IO.Path.Combine(Path, ".Svarozhich", "preview.png"));
    }
}

[DataContract]
public class OpenedProjectData : PersistedEntity<OpenedProjectData>
{
    [DataMember(Name = "Projects")]
    public List<ProjectData> Projects { get; set; } = [];
    private static readonly string ApplicationDataPath = Path.Combine(
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}", "Svarozhich");
    // /Users/alexeysemenov/Library/Application Support/Svarozhich/Projects.xml
    private static readonly string ProjectsDataPath;

    static OpenedProjectData()
    {
        if (!Directory.Exists(ApplicationDataPath))
        {
            Directory.CreateDirectory(ApplicationDataPath);
        }
        ProjectsDataPath = Path.Combine(ApplicationDataPath, "Projects.xml");
    }

    public void MarkOpened(Project project)
    {
        if (Projects.All(p => p.Path != project.RootFolder))
        {
            Projects.Add(new ProjectData()
            {
                Path = project.RootFolder,
                Name = project.Name,
                LastOpenDate = DateTime.Now
            });
        }
        else
        {
            Projects.First(p => p.Path == project.RootFolder).LastOpenDate = DateTime.Now;
        }
        MarkDirty();
    }

    protected override OpenedProjectData ToDto()
    {
        return this;
    }

    protected override string FilePath()
    {
        return ProjectsDataPath;
    }

    public static OpenedProjectData Load(ISerializer<OpenedProjectData> serializer)
    {
        return serializer.FromFile(ProjectsDataPath) ?? new OpenedProjectData();
    }
}