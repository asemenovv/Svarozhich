using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class ProjectsExploreDialogViewModel : ViewModelBase
{
    public Interaction<Unit, Unit> ShowOpenProjectViewInteraction { get; } = new();
    public Interaction<Unit, Unit> ShowCreateProjectViewInteraction { get; } = new();

    public async Task ShowOpenProjectView()
    {
        await ShowOpenProjectViewInteraction.Handle(Unit.Default);
    }
    
    public async Task ShowCreateProjectView()
    {
        await ShowCreateProjectViewInteraction.Handle(Unit.Default);
    }
}