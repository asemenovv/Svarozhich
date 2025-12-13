using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Commands;
using Svarozhich.Services;

namespace Svarozhich.ViewModels;

public class FilesExplorerViewModel : ViewModelBase
{
    private readonly UndoRedoService _undoRedoService;
    [Reactive] public Project? Project { get; set; }
    [Reactive] public ProjectFileNode? SelectedNode { get; set; }
    public Interaction<ProjectFileNode, bool> DeleteConfirmationInteraction { get; }
    public Interaction<Unit, string?> FolderNameDialogInteraction { get; }

    public ReactiveCommand<ProjectFileNode, Unit> OpenFolderInFinderCommand { get; }
    
    public ReactiveCommand<Unit, Unit> DeleteSelectedNodeCommand { get; }
    
    public ReactiveCommand<Unit, Unit> CreateFolderInSelectedFolderCommand { get; }
    
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    public FilesExplorerViewModel(UndoRedoService undoRedoService)
    {
        _undoRedoService = undoRedoService;
        DeleteConfirmationInteraction = new Interaction<ProjectFileNode, bool>();
        FolderNameDialogInteraction = new Interaction<Unit, string?>();
        OpenFolderInFinderCommand = ReactiveCommand.Create<ProjectFileNode>(OpenFolderInFinder);
        
        var isNodeSelected = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node != null);
        var isFolderSelected = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node is { IsFolder: true });
        DeleteSelectedNodeCommand = ReactiveCommand.CreateFromTask(DeleteSelectedNode, isNodeSelected);
        CreateFolderInSelectedFolderCommand = ReactiveCommand.CreateFromTask(CreateFolderInSelectedNode, isFolderSelected);
        RefreshCommand = ReactiveCommand.Create(() => Project?.RootProjectFolder.Refresh());
    }

    public async Task DeleteNode(ProjectFileNode node)
    {
        var deleteConfirmed = await DeleteConfirmationInteraction.Handle(node);
        if (deleteConfirmed)
        {
            var delete = new CompositeOperation($"Delete {(node.IsFolder ? "Folder" : "File")}", [
                new RemoveNodeFromFilesTreeOperation(node),
                new DeleteNodeFromDiskOperation(node, Project!.TrashFolder())
            ]);
            _undoRedoService.Do(delete);
        }
    }

    public async Task CreateFolderIn(ProjectFileNode node)
    {
        var folderName = await FolderNameDialogInteraction.Handle(Unit.Default);
        if (string.IsNullOrEmpty(folderName)) return;
        var operation = new CreateFolderOperation(node, folderName);
        _undoRedoService.Do(operation);
        Console.Write($"Creating '{folderName}' folder in '{node.RelativePath()}'");
    }

    private async Task DeleteSelectedNode()
    {
        if (SelectedNode == null)  return;
        await DeleteNode(SelectedNode);
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
    }

    private async Task CreateFolderInSelectedNode()
    {
        if (SelectedNode == null)  return;
        await CreateFolderIn(SelectedNode);
    }
}