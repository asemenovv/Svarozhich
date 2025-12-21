using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Svarozhich.Models;
using Svarozhich.ViewModels;
using Svarozhich.Views.Controls.Dialogs;

namespace Svarozhich.Views.Docks.Files;

public partial class FileSystemPanel : ReactiveUserControl<FilesExplorerViewModel>
{
    public FileSystemPanel()
    {
        InitializeComponent();
        this.WhenActivated(OnViewActivation);
    }

    private async Task AskForFolderNameHandler(IInteractionContext<ProjectFileNode, InputDialogResponse?> context)
    {
        var parentFolder = context.Input;
        var window = TopLevel.GetTopLevel(this) as Window;
        if (window == null)
        {
            context.SetOutput(null);
            return;
        }

        var folderNameDialog = new InputDialogConfigs("New Folder", "Name:", "New Folder",
            true, "Root folder",
            parentFolder.NodeType == ProjectFileNodeType.RootFolder,
            parentFolder.NodeType == ProjectFileNodeType.RootFolder);
        var inputDialogView = new InputDialogView(folderNameDialog);
        var dialogResponse = await inputDialogView.ShowDialog<InputDialogResponse?>(window);
        context.SetOutput(dialogResponse);
    }

    private async Task DeleteConfirmationHandler(IInteractionContext<ProjectFileNode, bool> context)
    {
        var confirmation = await MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams()
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new() { Name = "Yes", },
                new() { Name = "No", }
            },
            ContentTitle = "Confirmation",
            ContentMessage = $"Are you sure you would like to delete: \"{context.Input.RelativePath()}\"?",
            Icon = Icon.Question,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 500,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = true,
        }).ShowAsync();
        context.SetOutput(confirmation == "Yes");
    }

    private void OnViewActivation(CompositeDisposable disposables)
    {
        var vm = (FilesExplorerViewModel)DataContext!;
        vm.DeleteConfirmationInteraction.RegisterHandler(DeleteConfirmationHandler)
            .DisposeWith(disposables);
        vm.FolderNameDialogInteraction.RegisterHandler(AskForFolderNameHandler)
            .DisposeWith(disposables);
    }
}