using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using NodifyM.Avalonia.ViewModelBase;
using Svarozhich.Models.Nodes;
using Svarozhich.Services;

namespace Svarozhich.ViewModels.Controls.Editors;

public class NodeEditorViewModel : NodifyEditorViewModelBase
{
    private readonly ProjectsAppService _projectsAppService;

    public NodeEditorViewModel(ProjectsAppService projectsAppService)
    {
        _projectsAppService = projectsAppService;
        // Console.WriteLine(projectsService.CurrentProject.Name);
    }

    private void LoadGraph(NodeGraph nodeGraph)
    {
        Nodes.Clear();
        Connections.Clear();
        var ports = new Dictionary<Guid, ConnectorViewModelBase>();
        foreach (var node in nodeGraph.Nodes)
        {
            ObservableCollection<object> inputs = [];
            ObservableCollection<object> outputs = [];
            foreach (var input in node.Inputs)
            {
                var port = new ConnectorViewModelBase
                {
                    Title = input.Name,
                    Flow = ConnectorViewModelBase.ConnectorFlow.Input
                };
                inputs.Add(port);
                ports.Add(input.Id, port);
            }
            foreach (var output in node.Outputs)
            {
                var port = new ConnectorViewModelBase
                {
                    Title = output.Name,
                    Flow = ConnectorViewModelBase.ConnectorFlow.Output
                };
                outputs.Add(port);
                ports.Add(output.Id, port);
            }
            var nodeViewModel = new NodeViewModelBase()
            {
                Location = new Point(400, 200),
                Title = node.Title,
                Input = inputs,
                Output = outputs
            };
            Nodes.Add(nodeViewModel);
        }

        foreach (var connection in nodeGraph.Connections)
        {
            var from = ports[connection.From.Id];
            var to = ports[connection.To.Id];
            from.IsConnected = true;
            to.IsConnected = true;
            Connections.Add(new ConnectionViewModelBase(this, from, to, connection.Title));
        }
    }
}