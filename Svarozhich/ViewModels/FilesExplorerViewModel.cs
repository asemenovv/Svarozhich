using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
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

    public ReactiveCommand<ProjectFileNode, Unit> OpenFolderInFinderCommand { get; }

    public ReactiveCommand<ProjectFileNode, Unit> CreateFolderCommand { get; }

    public ReactiveCommand<ProjectFileNode, Unit> DeleteNodeCommand { get; }

    public FilesExplorerViewModel(UndoRedoService undoRedoService)
    {
        _undoRedoService = undoRedoService;
        OpenFolderInFinderCommand = ReactiveCommand.Create<ProjectFileNode>(OpenFolderInFinder);
        CreateFolderCommand = ReactiveCommand.Create<ProjectFileNode>(CreateFolder);
        DeleteNodeCommand = ReactiveCommand.Create<ProjectFileNode>(DeleteNode);
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

    private void CreateFolder(ProjectFileNode node)
    {
        Console.WriteLine($"Create Folder in {node.FullPath}");
    }

    private async void DeleteNode(ProjectFileNode node)
    {
        var confirmation = await MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams()
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new() { Name = "Yes", },
                new() { Name = "No", }
            },
            ContentTitle = "Confirmation",
            ContentMessage = $"Are you sure you would like to delete: \"{node.RelativePath()}\"?",
            Icon = Icon.Question,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 500,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = true,
        }).ShowAsync();
        if (confirmation != "Yes") return;
        var delete = new CompositeOperation($"Delete {(node.IsFolder ? "Folder" : "File")}", [
                new RemoveNodeFromFilesTreeOperation(node),
                new DeleteNodeFromDiskOperation(node, Project!.TrashFolder())
            ]);
        _undoRedoService.Do(delete);
    }
}