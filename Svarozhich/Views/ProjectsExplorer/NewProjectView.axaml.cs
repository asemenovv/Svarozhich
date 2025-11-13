using System.Reactive.Disposables.Fluent;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Svarozhich.ViewModels.ProjectsExplorer;

namespace Svarozhich.Views.ProjectsExplorer;

public partial class NewProjectView : ReactiveUserControl<NewProjectViewModel>
{
    public NewProjectView()
    {
        InitializeComponent();
        
        this.WhenActivated(disposables =>
        {
            var vm = (NewProjectViewModel)DataContext!;
            vm.PickFolderInteraction.RegisterHandler(
                    async interaction => 
                    { 
                        var topLevel = TopLevel.GetTopLevel(this);
                        var files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
                        {
                            Title = "Project Location",
                            AllowMultiple = false
                        });
                        if (files.Count >= 1)
                        {
                            interaction.SetOutput(files[0].Path.AbsolutePath);
                        }
                    }).DisposeWith(disposables);
        });
    }
}