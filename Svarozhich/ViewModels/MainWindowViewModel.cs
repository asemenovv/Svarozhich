using System.Collections.ObjectModel;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string FahrenheitValue { get; } = "0.0";
    public string CelsiusValue { get; } = "0.0";
    public SceneViewModel Scene { get; } = new SceneViewModel(new ProjectViewModel("", ""), "Room", "");

    public MainWindowViewModel()
    {
        var roomNode = Scene.AddEntity("Room");
        roomNode.AddSubNode("Vase");
        roomNode.AddSubNode("Camera");
        var lightsNode = roomNode.AddSubNode("Lights");
        lightsNode.AddSubNode("Point Light");
        lightsNode.AddSubNode("Spot Light");
    }
}