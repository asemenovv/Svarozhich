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

    public void CreateFolders(ProjectFileNode projectHomePath)
    {
        foreach (var folder in Folders)
        {
            Directory.CreateDirectory(Path.Combine(projectHomePath.FullPath, folder));
        }

        var editorSystemPath = new DirectoryInfo(Path.Combine(projectHomePath.FullPath, ".Svarozhich"));
        editorSystemPath.Attributes |= FileAttributes.Hidden;
        if (PreviewImagePath != null)
            File.Copy(PreviewImagePath, Path.Combine(editorSystemPath.FullName, "preview.png"));
    }

    public void LoadPreviewImage()
    {
        if (PreviewImagePath != null) PreviewImage = new Bitmap(PreviewImagePath);
    }
}