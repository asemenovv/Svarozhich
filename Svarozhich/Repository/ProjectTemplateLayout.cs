using System;
using System.IO;
using Svarozhich.Models;

namespace Svarozhich.Repository;

public class ProjectTemplateLayout
{
    public string PreviewImage(string templateFile)
    {
        return Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(),
            "preview.png");
    }
}