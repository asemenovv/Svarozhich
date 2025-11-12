using System.Collections.ObjectModel;
using System.Linq;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class ProjectViewModel : ViewModelBase
{
    private static string Extension { get; } = ".svch";

    public string Name { get; private set; }

    public string Path { get; private set; }

    public string FullPath => System.IO.Path.Combine(Path, $"/{Name}{Extension}");

    private readonly ObservableCollection<SceneViewModel> _scenes = [];
    public ReadOnlyObservableCollection<SceneViewModel> Scenes { get; private set; }

    public ProjectViewModel(string name, string path)
    {
        Name = name;
        Path = path;
        Scenes = new ReadOnlyObservableCollection<SceneViewModel>(_scenes);
    }

    public ProjectBinding ToModel() => new()
    {
        Name = Name,
        Path = Path,
        Scenes = Scenes.Select(s => s.ToModel()).ToList()
    };

    public SceneViewModel CreateScene(string name)
    {
        var scene = new SceneViewModel(this, name, "Scenes/");
        _scenes.Add(scene);
        return scene;
    }
}