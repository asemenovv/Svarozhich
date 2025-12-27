using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;

namespace Svarozhich.Models;

[DataContract(Name = "ProjectTemplate")]
public class ProjectTemplate
{
    [DataMember]
    public required string ProjectType { get; set; }
    [DataMember]
    public required string ProjectFile { get; set; }
    [DataMember]
    public required List<string> Folders { get; set; }
    public string? PreviewImagePath { get; set; }
    public Bitmap? PreviewImage { get; set; }

    public void LoadPreviewImage()
    {
        if (PreviewImagePath != null) PreviewImage = new Bitmap(PreviewImagePath);
    }
}