using System.Collections.ObjectModel;
using System.Diagnostics;
using ReactiveUI;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class SceneViewModel : ViewModelBase
{
    private string _name;
    private string _path;

    public SceneViewModel(ProjectViewModel projectViewModel, string name, string path)
    {
        Debug.Assert(projectViewModel != null);
        ProjectViewModel = projectViewModel;
        _name = name;
        _path = path;
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    public string Path {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }
    
    public string FullPath {
        get => System.IO.Path.Combine(_path, $"{_name}.xml");
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }
    
    public SceneRefDto ToModel() => new()
    {
        Name = Name,
        Path = FullPath,
    };
    
    public ProjectViewModel ProjectViewModel { get; private set; }
    
    public ObservableCollection<SceneNode> Entities { get; } = [];

    public SceneNode AddEntity(string title)
    {
        var entity = new SceneNode(title);
        Entities.Add(entity);
        return entity;
    }
}