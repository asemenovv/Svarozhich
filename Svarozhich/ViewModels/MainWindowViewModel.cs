using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Events;
using Svarozhich.ViewModels.Controls.Editors;

namespace Svarozhich.ViewModels;

public class ProjectOpenedHandler(MainWindowViewModel viewModel, ILogger<ProjectOpenedHandler> logger) : INotificationHandler<ProjectOpenedEvent>
{
    public Task Handle(ProjectOpenedEvent notification, CancellationToken cancellationToken)
    {
        viewModel.Project = notification.Project;
        logger.LogInformation("Project {ProjectName} Opened", notification.Project.Name);
        return Task.CompletedTask;
    }
}

public class MainWindowViewModel : ViewModelBase
{
    [Reactive]
    public Project? Project { get; set; }

    [ObservableAsProperty]
    public string WindowTitle  { get; }

    public NodeEditorViewModel NodeEditorViewModel { get; }

    public MainWindowViewModel(NodeEditorViewModel nodeEditorViewModel)
    {
        NodeEditorViewModel = nodeEditorViewModel;
        this.WhenAnyValue(vm => vm.Project)
            .Select(p => p is null ? "Svarozhich" : $"Svarozhich - {p.Name}")
            .ToPropertyEx(this, vm => vm.WindowTitle);
    }

    public void RenameProject(string newName)
    {
        if (Project is null) return;

        Project.Rename(newName);
        this.RaisePropertyChanged(nameof(WindowTitle));
    }
}