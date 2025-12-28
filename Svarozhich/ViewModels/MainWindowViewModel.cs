using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.Commands;
using Svarozhich.Services;
using Svarozhich.ViewModels.Controls.Editors;
using Unit = System.Reactive.Unit;

namespace Svarozhich.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly CommandsFactory _commandsFactory;
    public WorkspaceService WorkspaceService { get; private set; }
    public FilesExplorerViewModel FilesExplorerViewModel { get; private set; }
    public SceneBrowserViewModel SceneBrowserViewModel { get; private set; }
    [Reactive]
    public UndoRedoService UndoRedo { get; private set; }
    public ReactiveCommand<Unit, Unit> UndoCommand { get; }
    public ReactiveCommand<Unit, Unit> RedoCommand { get; }

    [ObservableAsProperty]
    public string WindowTitle { get; } = "Svarozhich";

    public NodeEditorViewModel NodeEditorViewModel { get; }

    public MainWindowViewModel(NodeEditorViewModel nodeEditorViewModel, FilesExplorerViewModel filesExplorerViewModel,
        SceneBrowserViewModel sceneBrowserViewModel, UndoRedoService undoRedoService, WorkspaceService workspaceService,
        CommandsFactory commandsFactory)
    {
        _commandsFactory = commandsFactory;
        WorkspaceService = workspaceService;
        FilesExplorerViewModel = filesExplorerViewModel;
        SceneBrowserViewModel = sceneBrowserViewModel;
        UndoRedo = undoRedoService;
        NodeEditorViewModel = nodeEditorViewModel;
        this.WhenAnyValue(vm => vm.WorkspaceService.CurrentProject)
            .Select(p => p is null
                ? Observable.Return("Svarozhich")
                : p.WhenAnyValue(x => x.Name).Select(name => $"Svarozhich - {name}"))
            .Switch()
            .ToPropertyEx(this, vm => vm.WindowTitle);
        var canUndo = this.WhenAnyValue(vm => vm.UndoRedo.CanUndo);
        var canRedo = this.WhenAnyValue(vm => vm.UndoRedo.CanRedo);
        UndoCommand = ReactiveCommand.Create(() => UndoRedo.Undo(), canUndo);
        RedoCommand = ReactiveCommand.Create(() => UndoRedo.Redo(), canRedo);
    }

    public void RenameProject(string newName)
    {
        _commandsFactory.RenameCurrentProject(newName);
    }
}