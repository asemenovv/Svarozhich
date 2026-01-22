using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.ECS;
using Svarozhich.Services;

namespace Svarozhich.ViewModels;

public class SceneBrowserViewModel : ViewModelBase
{
    private readonly UndoRedoService _undoRedoService;
    [Reactive]
    public Scene ActiveScene { get; set; }

    public SceneBrowserViewModel(UndoRedoService undoRedoService)
    {
        _undoRedoService = undoRedoService;
        ActiveScene = new Scene(null, "Default Scene");
        var global = ActiveScene.CreateFolder("Global");
        var room = ActiveScene.CreateFolder("Room");
        ActiveScene.CreateEntity("Camera", global);
        ActiveScene.CreateEntity("Player", global);
        ActiveScene.CreateEntity("Floor", room);
        ActiveScene.CreateEntity("Vase", room);
        ActiveScene.CreateEntity("Point Light", room);
    }
}