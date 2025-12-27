using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;

namespace Svarozhich.Utils;

public class InstallationFolderLayout(IOptions<AppSettings> options)
{
    private readonly AppSettings _settings = options.Value;

    private string RootFolder()
    {
        var appsFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appsFolder, "Svarozhich");
    }

    private string SubFolder(string path)
    {
        var relativePath = Path.Combine(RootFolder(), path);
        return Path.IsPathRooted(path) ? path : relativePath;
    }

    public string RecentsProjectsFile()
    {
        var applicationDataPath = RootFolder();
        if (!Directory.Exists(applicationDataPath))
        {
            Directory.CreateDirectory(applicationDataPath);
        }
        return Path.Combine(applicationDataPath, "Projects.xml");
    }
    
    public IEnumerable<string> LookupTemplateFiles()
    {
        var templatesPath = SubFolder(_settings.TemplatesPath);
        return Directory.GetFiles(templatesPath, "template.xml", SearchOption.AllDirectories);
    }
}