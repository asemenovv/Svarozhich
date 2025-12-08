using System;
using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;

namespace Svarozhich.ViewModels;

public class FilesExplorerViewModel : ViewModelBase
{
    [Reactive]
    public Project? Project { get; set; }
    [Reactive]
    public ProjectFileNode? SelectedNode { get; set; }

    public ReactiveCommand<ProjectFileNode, Unit> OpenFolderInFinderCommand { get; }

    public FilesExplorerViewModel()
    {
        OpenFolderInFinderCommand = ReactiveCommand.Create<ProjectFileNode>(OpenFolderInFinder);
    }

    private void OpenFolderInFinder(ProjectFileNode node)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "open",
            Arguments = $"\"{node.FullPath}\"",
            UseShellExecute = false
        };

        Process.Start(psi);
        Console.WriteLine("открыть папку в Finder/Explorer");
    }
}