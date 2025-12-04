namespace Svarozhich.Models.Commands;

public interface IUndoableOperation
{
    string Name { get; }

    void Do();

    void Undo();
}