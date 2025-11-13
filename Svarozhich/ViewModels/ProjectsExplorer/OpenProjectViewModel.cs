using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel : ViewModelBase
{
    public Interaction<Unit, Unit> CloseDialogInteraction { get; }

    public OpenProjectViewModel()
    {
        CloseDialogInteraction = new Interaction<Unit, Unit>();
    }

    public async Task CloseDialog()
    {
        await CloseDialogInteraction.Handle(Unit.Default);
    }
}