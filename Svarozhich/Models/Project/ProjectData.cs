using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;

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

    public void MarkOpened(Project project)
    {
        if (Projects.All(p => p.Path != project.RootProjectFolder.FullPath))
        {
            Projects.Add(new ProjectData()
            {
                Path = project.RootProjectFolder.FullPath,
                Name = project.Name,
                LastOpenDate = DateTime.Now
            });
        }
        else
        {
            Projects.First(p => p.Path == project.RootProjectFolder.FullPath).LastOpenDate = DateTime.Now;
        }
        MarkDirty();
    }
}