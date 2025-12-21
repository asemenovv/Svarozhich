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
using Svarozhich.Views.Controls.Dialogs;

namespace Svarozhich.ViewModels;

public class FilesExplorerViewModel : ViewModelBase
{
    private readonly UndoRedoService _undoRedoService;
    [Reactive] public Project? Project { get; set; }
    [Reactive] public ProjectFileNode? SelectedNode { get; set; }
    public Interaction<ProjectFileNode, bool> DeleteConfirmationInteraction { get; }
    public Interaction<ProjectFileNode, InputDialogResponse?> FolderNameDialogInteraction { get; }
    
    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    
    public ReactiveCommand<Unit, Unit> CreateFolderCommand { get; }
    
    public ReactiveCommand<Unit, Unit> CreateSceneInSelectedFolderCommand { get; }
    
    public ReactiveCommand<Unit, Unit> RenameCommand { get; }

    public ReactiveCommand<ProjectFileNode, Unit> OpenFolderInFinderCommand { get; }
    
    public ReactiveCommand<Unit, Unit> DeleteSelectedNodeCommand { get; }
    
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    public FilesExplorerViewModel(UndoRedoService undoRedoService)
    {
        _undoRedoService = undoRedoService;
        DeleteConfirmationInteraction = new Interaction<ProjectFileNode, bool>();
        FolderNameDialogInteraction = new Interaction<ProjectFileNode, InputDialogResponse?>();
        OpenFolderInFinderCommand = ReactiveCommand.Create<ProjectFileNode>(OpenFolderInFinder);
        
        // var isNodeSelected = this.WhenAnyValue(x => x.SelectedNode)
            // .Select(node => node != null);
        var isFolderSelected = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node is { IsFolder: true });
        var isFileToOpenSelected = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node is { NodeType: ProjectFileNodeType.Scene });
        var isDeletable = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node != null && node.NodeType.IsDeletable());
        var canBeRenamed = this.WhenAnyValue(x => x.SelectedNode)
            .Select(node => node != null && node.NodeType.CanBeRenamed());
        OpenCommand = ReactiveCommand.CreateFromTask(OpenSelectedNode, isFileToOpenSelected);
        CreateFolderCommand = ReactiveCommand.CreateFromTask(CreateFolderInSelectedNode);
        CreateSceneInSelectedFolderCommand = ReactiveCommand.CreateFromTask(CreateSceneInSelectedNode, isFolderSelected);
        RenameCommand = ReactiveCommand.CreateFromTask(RenameSelectedNode, canBeRenamed);
        DeleteSelectedNodeCommand = ReactiveCommand.CreateFromTask(DeleteSelectedNode, isDeletable);
        RefreshCommand = ReactiveCommand.Create(() => Project?.RootProjectFolder.Refresh());
    }

    public async Task CreateFolderIn(ProjectFileNode parentFolder)
    {
        var response = await FolderNameDialogInteraction.Handle(parentFolder);
        if (string.IsNullOrEmpty(response?.Text)) return;
        if (response.FlagChecked != null && response.FlagChecked.Value)
        {
            parentFolder = Project!.RootProjectFolder;
        }
        
        var operation = new CreateFolderOperation(parentFolder, response.Text);
        _undoRedoService.Do(operation);
        Console.Write($"Creating '{response}' folder in '{parentFolder.RelativePath()}'");
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

    private async Task OpenSelectedNode()
    {
        // SelectedNode;
        Console.WriteLine($"Opening '{SelectedNode.RelativePath()}'");
    }

    private async Task CreateFolderInSelectedNode()
    {
        if (SelectedNode is not { NodeType: ProjectFileNodeType.Folder })
        {
            await CreateFolderIn(Project?.RootProjectFolder!);
        }
        else
        {
            await CreateFolderIn(SelectedNode);
        }
    }

    private async Task CreateSceneInSelectedNode()
    {
        if (SelectedNode == null) return;
    }
    
    private async Task RenameSelectedNode()
    {
        if (SelectedNode == null) return;
    }
}