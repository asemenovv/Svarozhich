using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Services;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel(ProjectsService projectsService) : ViewModelBase
{
    [Reactive]
    public ObservableCollection<ProjectData> Projects { get; set; } = new(projectsService.PreviouslyOpenedProjects());

    [Reactive]
    public ProjectData? SelectedProject { get; set; }
    public Interaction<ProjectExploreResult, Unit> CloseDialogInteraction { get; } = new();

    public async Task CloseDialog()
    {
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Exit));
    }
}