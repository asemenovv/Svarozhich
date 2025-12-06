using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Commands;
using Svarozhich.Models.Events;
using Svarozhich.ViewModels.Controls.Editors;
using Unit = System.Reactive.Unit;

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
    public UndoRedoService UndoRedo { get; private set; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }

    [Reactive]
    public Project? Project { get; set; }

    [ObservableAsProperty]
    public string WindowTitle  { get; }

    public NodeEditorViewModel NodeEditorViewModel { get; }

    public MainWindowViewModel(NodeEditorViewModel nodeEditorViewModel, UndoRedoService undoRedoService)
    {
        UndoRedo = undoRedoService;
        NodeEditorViewModel = nodeEditorViewModel;
        this.WhenAnyValue(vm => vm.Project)
            .Select(p => p is null ? "Svarozhich" : $"Svarozhich - {p.Name}")
            .ToPropertyEx(this, vm => vm.WindowTitle);
        UndoCommand = ReactiveCommand.Create(
            () => UndoRedo.Undo(),
            this.WhenAnyValue(vm => vm.UndoRedo.CanUndo)
        );
        RedoCommand = ReactiveCommand.Create(
            () => UndoRedo.Redo(),
            this.WhenAnyValue(vm => vm.UndoRedo.CanRedo)
        );
    }

    public void RenameProject(string newName)
    {
        if (Project is null) return;

        var op = new RenameProjectOperation(Project, newName);
        UndoRedo.Do(op);
        this.RaisePropertyChanged(nameof(WindowTitle));
    }
}