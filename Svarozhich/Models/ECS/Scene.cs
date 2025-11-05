using System.Collections.ObjectModel;

namespace Svarozhich.Models;

public class Scene
{
    public ObservableCollection<SceneNode> Entities { get; } = [];

    public SceneNode AddEntity(string title)
    {
        var entity = new SceneNode(title);
        Entities.Add(entity);
        return entity;
    }
}