using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Avalonia.Media.Imaging;

namespace Svarozhich.Models;

[DataContract(Name = "ProjectTemplate")]
public class ProjectTemplateBinding
{
    [DataMember]
    public required string ProjectType { get; set; }
    [DataMember]
    public required string ProjectFile { get; set; }
    [DataMember]
    public required List<string> Folders { get; set; }
    public string? PreviewImagePath { get; set; }
    public Bitmap? PreviewImage { get; set; }

    public void CreateFolders(string projectHomePath)
    {
        foreach (var folder in Folders)
        {
            Directory.CreateDirectory(Path.Combine(projectHomePath, folder));
        }

        var editorSystemPath = new DirectoryInfo(Path.Combine(projectHomePath, ".Svarozhich"));
        editorSystemPath.Attributes |= FileAttributes.Hidden;
        if (PreviewImagePath != null)
            File.Copy(PreviewImagePath, Path.Combine(editorSystemPath.FullName, "preview.png"));
    }
}