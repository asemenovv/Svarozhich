using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;

namespace Svarozhich.Models.Project;

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

    public void LoadImages(string previewImagePath)
    {
        PreviewImage = new Bitmap(previewImagePath);
    }
}

[DataContract]
public class OpenedProjectData : PersistedEntity<OpenedProjectData>
{
    [DataMember(Name = "Projects")]
    public List<ProjectData> Projects { get; set; } = [];

    public void MarkOpened(Models.Project.Project project, string projectPath)
    {
        if (Projects.All(p => p.Path != projectPath))
        {
            Projects.Add(new ProjectData()
            {
                Path = projectPath,
                Name = project.Name,
                LastOpenDate = DateTime.Now
            });
        }
        else
        {
            Projects.First(p => p.Path == projectPath).LastOpenDate = DateTime.Now;
        }
        MarkDirty();
    }
}