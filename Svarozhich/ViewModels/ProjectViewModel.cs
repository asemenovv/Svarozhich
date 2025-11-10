using System.Collections.ObjectModel;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class ProjectViewModel : ViewModelBase
{
    private static string Extension { get; } = ".svch";

    public string Name { get; private set; }

    public string Path { get; private set; }

    public string FullPath => System.IO.Path.Combine(Path, $"/{Name}{Extension}");

    private readonly ObservableCollection<Scene> _scenes = [];
    public ReadOnlyObservableCollection<Scene> Scenes { get; private set; }

    public ProjectViewModel(string name, string path)
    {
        Name = name;
        Path = path;
        Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
    }

    public Project ToModel() => new()
    {
        Name = Name,
        Path = Path
    };

    public Scene CreateScene(string name)
    {
        var scene = new Scene(this, name);
        _scenes.Add(scene);
        return scene;
    }
}