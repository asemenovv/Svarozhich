using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Events;
using Svarozhich.Services;
using Unit = System.Reactive.Unit;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel(ProjectsService projectsService, IMediator mediator) : ViewModelBase
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

    public async Task OpenProject()
    {
        var project = projectsService.LoadFromFolder(SelectedProject!.Path);
        await mediator.Publish(new ProjectOpenedEvent(project));
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Open));
    }
}