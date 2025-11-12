using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using Svarozhich.Messages;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public partial class NewProjectViewModel : ViewModelBase
{
    private readonly string _templatePath =
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";
    
    private string _name = "New Project";
    public string ProjectName
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    // private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Svarozhich/";
    private string _path = "/Users/alexeysemenov/RiderProjects/Svarozhich/Svarozhich/Projects/";
    public string ProjectPath
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }
    
    private readonly ObservableCollection<ProjectTemplateBinding> _templates = [];
    public ReadOnlyObservableCollection<ProjectTemplateBinding> ProjectTemplates { get; }
    private ProjectTemplateBinding _selectedTemplate;
    public ProjectTemplateBinding SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    
    public void CloseDialog()
    {
        WeakReferenceMessenger.Default.Send(new CloseProjectExploreDialogMessage());
    }
    
    public ReactiveCommand<Unit, Unit> CreateCommand { get; }

    private void CreateProject()
    {
        try
        {
            var projectHomePath = Path.Combine(_path, _name);
            if (!Directory.Exists(projectHomePath))
            {
                Directory.CreateDirectory(projectHomePath);
            }

            _selectedTemplate.CreateFolders(projectHomePath);
            var project = new ProjectViewModel(_name, projectHomePath);
            var scene = project.CreateScene("Default Scene");
            XmlSerializer.ToFile(project.ToModel(), Path.Combine(projectHomePath, _selectedTemplate.ProjectFile));
        } catch (Exception e)
        {
            //TODO: Log exception
            Console.WriteLine(e);
            throw;
        }
    }

    public NewProjectViewModel()
    {
        ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplateBinding>(_templates);
        this.ValidationRule(vm => vm.ProjectName,
            name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 3,
            "Name should be at least 3 characters long.");
        this.ValidationRule(vm => vm.ProjectName,
            name => !string.IsNullOrWhiteSpace(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1,
            "Project name should be a valid file name.");
        this.ValidationRule(vm => vm.ProjectName,
                                name => !string.IsNullOrWhiteSpace(name)
                                        && !string.IsNullOrWhiteSpace(_path)
                                        && !Directory.Exists(_path + name),
            "Project already exists.");
        
        this.ValidationRule(vm => vm.ProjectPath,
            name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 0,
            "Project folder not be empty.");
        this.ValidationRule(vm => vm.ProjectPath,
            name => name != null && name.IndexOfAny(Path.GetInvalidPathChars()) == -1,
            "Project folder should be a valid path.");
        
        CreateCommand = ReactiveCommand.Create(CreateProject, ValidationContext.Valid);
        LoadProjectTemplates();
    }

    private void LoadProjectTemplates()
    {
        try
        {
            var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
            Debug.Assert(templateFiles.Length != 0);
            foreach (var templateFile in templateFiles)
            {
                var template = XmlSerializer.FromFile<ProjectTemplateBinding>(templateFile);
                if (template == null) continue;
                template.PreviewImagePath = Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(), "preview.png");
                template.PreviewImage = new Bitmap(template.PreviewImagePath);
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