using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using NodifyM.Avalonia.ViewModelBase;
using Svarozhich.Services;

namespace Svarozhich.ViewModels.Controls.Editors;

public class NodeEditorViewModel : NodifyEditorViewModelBase
{
    public NodeEditorViewModel(ProgramGraphsService programGraphsService)
    {
        var nodeGraph = programGraphsService.GetNodeGraph();
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
        // var knot1 = new KnotNodeViewModel()
        // {
        //     Location = new Point(300, 100)
        // };
        // var input1 = new ConnectorViewModelBase()
        // {
        //     Title = "AS 1",
        //     Flow = ConnectorViewModelBase.ConnectorFlow.Input
        // };
        // var output1 = new ConnectorViewModelBase()
        // {
        //     Title = "B 1",
        //     Flow = ConnectorViewModelBase.ConnectorFlow.Output
        // };
        // Connections.Add(new ConnectionViewModelBase(this, output1, knot1.Connector, "Test"));
        // Connections.Add(new ConnectionViewModelBase(this, knot1.Connector, input1));
        // var node1 = new NodeViewModelBase()
        // {
        //     Location = new Point(400, 200),
        //     Title = "Node 1",
        //     Input =
        //     [
        //         input1,
        //         new ComboBox()
        //         {
        //             ItemsSource = new ObservableCollection<object>
        //             {
        //                 "Item 1",
        //                 "Item 2",
        //                 "Item 3"
        //             }
        //         }
        //     ],
        //     Output =
        //     [
        //         new ConnectorViewModelBase()
        //         {
        //             Title = "Output 2",
        //             Flow = ConnectorViewModelBase.ConnectorFlow.Output
        //         }
        //     ]
        // };
        // var sumNode = new NodeViewModelBase()
        // {
        //     Title = "SUM",
        //     Location = new Point(-100, -100),
        //     Input = new ObservableCollection<object>
        //     {
        //         new ConnectorViewModelBase()
        //         {
        //             Title = "A",
        //             Flow = ConnectorViewModelBase.ConnectorFlow.Input
        //         },
        //         new ConnectorViewModelBase()
        //         {
        //             Title = "B",
        //             Flow = ConnectorViewModelBase.ConnectorFlow.Input
        //         }
        //     },
        //     Output = new ObservableCollection<object>
        //     {
        //         output1,
        //         new ConnectorViewModelBase()
        //         {
        //             Flow = ConnectorViewModelBase.ConnectorFlow.Output,
        //             Title = "Output 1"
        //         },
        //         new ConnectorViewModelBase()
        //         {
        //             Flow = ConnectorViewModelBase.ConnectorFlow.Output,
        //             Title = "Output 2"
        //         }
        //     }
        // };
        // Nodes = [node1, sumNode, knot1];
        // knot1.Connector.IsConnected = true;
        // output1.IsConnected = true;
        // input1.IsConnected = true;
    }

    public override void Connect(ConnectorViewModelBase source, ConnectorViewModelBase target)
    {
        base.Connect(source, target);
    }

    public override void DisconnectConnector(ConnectorViewModelBase connector)
    {
        base.DisconnectConnector(connector);
    }
}