namespace Svarozhich.Models;

public class PersistedEntity
{
    protected bool IsDirty { get; private set; }

    protected void MarkDirty()
    {
        IsDirty = true;
    }

    protected void MarkClean()
    {
        IsDirty = false;
    }
}