using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models.Commands;
using Svarozhich.Models.ECS;
using Svarozhich.Services;

namespace Svarozhich.ViewModels;

public class SceneBrowserViewModel : ViewModelBase
{
    [Reactive]
    public Scene? ActiveScene { get; set; }

    public ReactiveCommand<Unit, Unit> CreateSceneCommand { get; }

    public SceneBrowserViewModel(WorkspaceService workspace, UndoRedoService undoRedoService, CommandsFactory commandsFactory)
    {
        workspace.WhenAnyValue(w => w.ActiveScene)
            .BindTo(this, vm => vm.ActiveScene);

        var canCreate = workspace.WhenAnyValue(w => w.CurrentProject)
            .Select(p => p is not null);

        CreateSceneCommand = ReactiveCommand.Create(() =>
        {
            commandsFactory.CreateScene("New Scene");
        }, canCreate);
    }
}