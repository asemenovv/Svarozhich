using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.Project;
using Svarozhich.Services;
using Unit = System.Reactive.Unit;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel(ProjectsAppService projectsAppService, RecentProjectsService recentProjectsService,
    WorkspaceService workspaceService) : ViewModelBase
{
    [Reactive]
    public ObservableCollection<ProjectData> Projects { get; set; } = new(recentProjectsService.GetRecentProjects());

    [Reactive]
    public ProjectData? SelectedProject { get; set; }
    public Interaction<ProjectExploreResult, Unit> CloseDialogInteraction { get; } = new();

    public async Task CloseDialog()
    {
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Exit));
    }

    public async Task OpenProject()
    {
        projectsAppService.LoadFromFolder(SelectedProject!.Path);
        await CloseDialogInteraction.Handle(new ProjectExploreResult(ProjectExploreResultMode.Open));
    }
}