using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using Svarozhich.Models;
using Svarozhich.Models.Events;
using Svarozhich.ViewModels.Controls.Editors;

namespace Svarozhich.ViewModels;

public class MainWindowViewModel : ViewModelBase, INotificationHandler<ProjectOpenedEvent>
{
    private readonly ILogger<MainWindowViewModel> _logger;

    [Reactive]
    public Project Project { get; set; }
    [Reactive]
    public string WindowTitle { get; set; } = "Svarozhich???";
    
    public Guid InstanceId { get; } = Guid.NewGuid();
    
    public NodeEditorViewModel NodeEditorViewModel { get; }

    public MainWindowViewModel(NodeEditorViewModel nodeEditorViewModel, ILogger<MainWindowViewModel> logger)
    {
        NodeEditorViewModel = nodeEditorViewModel;
        _logger = logger;
        _logger.LogInformation($"{InstanceId} Created");
    }

    public Task Handle(ProjectOpenedEvent notification, CancellationToken cancellationToken)
    {
        Project = notification.Project;
        WindowTitle = $"Svarozhich - {Project.Name}";
        _logger.LogInformation("{InstanceId} Project {ProjectName} Opened", InstanceId.ToString(), notification.Project.Name);
        return Task.CompletedTask;
    }
}