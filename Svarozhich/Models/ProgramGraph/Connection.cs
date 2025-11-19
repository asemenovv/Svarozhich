using System;

namespace Svarozhich.Models.Nodes;

public class Connection
{
    public Port From { get; }
    public Port To { get; }
    public string Title { get; }

    public Connection(Port from, Port to,  string title = "")
    {
        if (from.Direction != PortDirection.Output)
            throw new ArgumentException("From must be output port", nameof(from));
        if (to.Direction != PortDirection.Input)
            throw new ArgumentException("To must be input port", nameof(to));
        if (from.DataType != to.DataType)
            throw new ArgumentException("Port types must match");

        From = from;
        To = to;
        Title = title;
    }
}