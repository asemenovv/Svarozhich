using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel : ViewModelBase
{
    public Interaction<ProjectExploreResult, Unit> CloseDialogInteraction { get; } = new();

    public async Task CloseDialog()
    {
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Exit));
    }
}