using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Avalonia.Media.Imaging;
using ReactiveUI;
using Svarozhich.Utils;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class ProjectTemplate
{
    public required string ProjectType { get; set; }
    public required string ProjectFile { get; set; }
    public required List<string> Folders { get; set; }
    public Bitmap? PreviewImage { get; set; }
}

public class NewProject : ViewModelBase
{
    private readonly string _templatePath =
        "/Users/alexeysemenov/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";

    private string _name = "New Project";
    public string ProjectName
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Svarozhich/";
    public string ProjectPath
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    private readonly ObservableCollection<ProjectTemplate> _templates = [];
    public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }
    private ProjectTemplate? _selectedTemplate;
    public ProjectTemplate? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }

    public NewProject()
    {
        ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_templates);
        try
        {
            var templateFiles = Directory.GetFiles(_templatePath, "template.yaml", SearchOption.AllDirectories);
            Debug.Assert(templateFiles.Length != 0);
            foreach (var templateFile in templateFiles)
            {
                var template = YamlSerializer.FromFile<ProjectTemplate>(templateFile);
                var previewImage = Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(), "preview.png");
                template.PreviewImage = new Bitmap(previewImage);
                _templates.Add(template);
            }
        }
        catch (Exception e)
        {
            // TODO: Log error
            Console.WriteLine(e);
            throw;
        }
    }
}