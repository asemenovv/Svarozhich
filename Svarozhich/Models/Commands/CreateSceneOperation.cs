using System;
using Svarozhich.Models.ECS;
using Svarozhich.Services;

namespace Svarozhich.Models.Commands;

public class CreateSceneOperation(string sceneName, Project.Project project, SceneService sceneService) : IUndoableOperation
{
    public string Name => $"Create Scene '{sceneName}'";

    private SceneRef? _created;

    public void Do()
    {
        _created = _created is null
            ? sceneService.CreateScene(project, sceneName)
            : sceneService.CreateScene(project, _created.Name, _created.Id, _created.RelativePath);
    }

    public void Undo()
    {
        if (_created is null) return;
        sceneService.DeleteScene(project, _created);
    }
}