using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Svarozhich.Models;
using Svarozhich.Utils;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class NewProjectViewModel : ViewModelBase
{
    private readonly string _templatePath =
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/InstallationFiles/Templates";

    [Reactive]
    public string ProjectName { get; set; } = "New Project";
    [Reactive]
    public string ProjectPath {  get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/RiderProjects/Svarozhich/Svarozhich/Projects/";
    [Reactive]
    public ProjectTemplateDto? SelectedTemplate {  get; set; }
    
    public Interaction<Unit, string?> PickFolderInteraction { get; }
    public Interaction<ProjectExploreResult, Unit> CloseDialogInteraction { get; }
    public ReactiveCommand<Unit, Task> CreateCommand { get; }
    public ObservableCollection<ProjectTemplateDto> ProjectTemplates { get; } = [];

    public NewProjectViewModel()
    {
        PickFolderInteraction = new Interaction<Unit, string?>();
        CloseDialogInteraction = new Interaction<ProjectExploreResult, Unit>();
        this.ValidationRule(vm => vm.ProjectName,
            name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 3,
            "Name should be at least 3 characters long.");
        this.ValidationRule(vm => vm.ProjectName,
            name => !string.IsNullOrWhiteSpace(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1,
            "Project name should be a valid file name.");
        this.ValidationRule(vm => vm.ProjectName,
            name => !string.IsNullOrWhiteSpace(name)
                    && !string.IsNullOrWhiteSpace(ProjectPath)
                    && !Directory.Exists(ProjectPath + name),
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
    
    public async Task PickFolderAsync()
    {
        var path = await PickFolderInteraction.Handle(Unit.Default);
        if (path != null)
            ProjectPath = path;
    }
    
    public async Task CloseDialog()
    {
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Exit));
    }

    private async Task CreateProject()
    {
        try
        {
            var projectHomePath = Path.Combine(ProjectPath, ProjectName);
            if (!Directory.Exists(projectHomePath))
            {
                Directory.CreateDirectory(projectHomePath);
            }

            SelectedTemplate?.CreateFolders(projectHomePath);
            var project = new Project(ProjectName, projectHomePath);
            project.CreateScene("Default Scene");
            project.Save(new XmlSerializer());
            await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Create));
        } catch (Exception e)
        {
            //TODO: Log exception
            Console.WriteLine(e);
            throw;
        }
    }

    private void LoadProjectTemplates()
    {
        try
        {
            var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
            Debug.Assert(templateFiles.Length != 0);
            foreach (var templateFile in templateFiles)
            {
                var serializer = new XmlSerializer();
                var template = serializer.FromFile<ProjectTemplateDto>(templateFile);
                if (template == null) continue;
                template.PreviewImagePath = Path.Combine(Path.GetDirectoryName(templateFile) ?? throw new InvalidOperationException(), "preview.png");
                template.PreviewImage = new Bitmap(template.PreviewImagePath);
                ProjectTemplates.Add(template);
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