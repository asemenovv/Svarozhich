using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class ProjectViewModel : ViewModelBase
{
    private Project _model;

    public string Name
    {
        get => _model.Name;
        private set { }
    }

    public string Path { get; private set; }
    public ReadOnlyObservableCollection<SceneViewModel> Scenes { get; private set; }

    public ProjectViewModel(Project model)
    {
        _model = model;
    }
}