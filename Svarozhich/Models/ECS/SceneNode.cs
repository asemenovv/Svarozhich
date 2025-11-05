using System.Collections.ObjectModel;

namespace Svarozhich.Models;

public class SceneNode
{
    public ObservableCollection<SceneNode> SubNodes { get; } = [];
    public string Title { get; }
    
    public SceneNode(string title)
    {
        Title = title;
    }

    public SceneNode AddSubNode(string title)
    {
        var child = new SceneNode(title);
        SubNodes.Add(child);
        return child;
    }
}