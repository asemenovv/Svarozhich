using System.Collections.ObjectModel;
using System.Diagnostics;
using ReactiveUI;
using Svarozhich.ViewModels;

namespace Svarozhich.Models;

public class Scene : ViewModelBase
{
    private string _name;

    public Scene(ProjectViewModel projectViewModel, string name)
    {
        Debug.Assert(projectViewModel != null);
        ProjectViewModel = projectViewModel;
        _name = name;
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    public ProjectViewModel ProjectViewModel { get; private set; }
    
    public ObservableCollection<SceneNode> Entities { get; } = [];

    public SceneNode AddEntity(string title)
    {
        var entity = new SceneNode(title);
        Entities.Add(entity);
        return entity;
    }
}