using System;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;

namespace Svarozhich.ViewModels;

public partial class ProjectsExploreDialogViewModel : ViewModelBase
{
    [RelayCommand]
    private static async Task CloseDialog()
    {
        Console.WriteLine("Closing dialog");
        await WeakReferenceMessenger.Default.Send(new CloseProjectExploreDialogMessage());
    }
}