using ReactiveUI.Fody.Helpers;
using Svarozhich.ViewModels.Controls.Editors;

namespace Svarozhich.ViewModels;

public class MainWindowViewModel(NodeEditorViewModel nodeEditorViewModel) : ViewModelBase
{
    public NodeEditorViewModel NodeEditorViewModel  { get; } = nodeEditorViewModel;
}