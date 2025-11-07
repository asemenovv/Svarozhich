using System;
using CommunityToolkit.Mvvm.Messaging;
using Svarozhich.Messages;

namespace Svarozhich.ViewModels.ProjectsExplorer;

public class OpenProjectViewModel : ViewModelBase
{
    public void CloseDialog()
    {
        Console.WriteLine("Closing dialog!!!");
        WeakReferenceMessenger.Default.Send(new CloseProjectExploreDialogMessage());
    }
}