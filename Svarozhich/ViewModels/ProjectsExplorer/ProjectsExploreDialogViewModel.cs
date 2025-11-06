using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public partial class ProjectsExploreDialogViewModel : ViewModelBase
{
    [RelayCommand]
    private static async Task CloseDialog()
    {
        Console.WriteLine("Closing dialog");
        await WeakReferenceMessenger.Default.Send(new CloseProjectExploreDialogMessage());
    }
}