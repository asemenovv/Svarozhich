namespace Svarozhich.Models.Commands;

public interface IUndoableOperation
{
    string Name { get; }

    void Do();

    void Undo();
}

public class NopOperation : IUndoableOperation
{
    public string Name => "NOP";
    public void Do() {}
    public void Undo() {}
}